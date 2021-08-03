using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RCC_PhotonRace : MonoBehaviourPun {
    public TrackCheckpoints trackCheckpoints;
    public RCC_CarControllerV3[] selectableVehicles;
    public int selectedCarIndex = 0;
    public int selectedBehaviorIndex = 0;
    public Transform spawnPoint;

    public int countTime = 3;
    public List<string> rankings;
    public GameObject endPanel;
    public Text rankingText;

    public void SelectVehicle(int index) => selectedCarIndex = index;

    public void ReadyRace() {
        Spawn();
        trackCheckpoints.OnPlayerLastCheckpoint += TrackCheckpoints_OnPlayerLastCheckpoint;
        StartCoroutine(Count());
    }

    [PunRPC]
    public void BeginRace() {
        foreach (var car in trackCheckpoints.carControllerList) {
            if (car.GetComponent<RCC_PhotonNetwork>().isMine)
                car.SetCanControl(true);
        }
    }

    private IEnumerator Count() {
        yield return new WaitForSeconds(1f);
        trackCheckpoints.RefreshCarList();

        foreach (var car in trackCheckpoints.carControllerList)
            car.SetCanControl(false);

        for (int i = countTime; i > 0; i--) {
            Debug.Log(i.ToString());
            RCC_InfoLabel.Instance.ShowInfo(i.ToString());

            yield return new WaitForSeconds(1);
        }
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("BeginRace", RpcTarget.All);
    }

    public void Spawn() {

        Vector3 lastKnownPos = Vector3.zero;
        Quaternion lastKnownRot = Quaternion.identity;

        RCC_CarControllerV3 newVehicle;

        if (RCC_SceneManager.Instance.activePlayerVehicle) {
            lastKnownPos = RCC_SceneManager.Instance.activePlayerVehicle.transform.position;
            lastKnownRot = RCC_SceneManager.Instance.activePlayerVehicle.transform.rotation;
        }

        if (lastKnownPos == Vector3.zero) {
            var index = PhotonNetwork.IsMasterClient ? 0 : 1;
            lastKnownPos = trackCheckpoints.raceStartPoints[index].position;
            lastKnownRot = trackCheckpoints.raceStartPoints[index].rotation;
        }

        lastKnownRot.x = 0f;
        lastKnownRot.z = 0f;

        if (RCC_SceneManager.Instance.activePlayerVehicle)
            PhotonNetwork.Destroy(RCC_SceneManager.Instance.activePlayerVehicle.gameObject);

        newVehicle = PhotonNetwork.Instantiate("Photon Vehicles/" + selectableVehicles[selectedCarIndex].gameObject.name, lastKnownPos + (Vector3.up), lastKnownRot, 0).GetComponent<RCC_CarControllerV3>();

        RCC.RegisterPlayerVehicle(newVehicle);
        RCC.SetControl(newVehicle, false);
        newVehicle.SetCanControl(false);

        if (RCC_SceneManager.Instance.activePlayerCamera)
            RCC_SceneManager.Instance.activePlayerCamera.SetTarget(newVehicle.gameObject);
    }

    public void TrackCheckpoints_OnPlayerLastCheckpoint(string playerName, bool isMe) {
        rankings.Add($"{rankings.Count + 1}. {playerName}");
        string rankList = "";
        foreach (var rank in rankings) {
            rankList += rank + "\n";
        }
        if (isMe) {
            endPanel.SetActive(true);
            rankingText.text = rankList;

            foreach (var car in trackCheckpoints.carControllerList)
                if (car.GetComponent<RCC_PhotonNetwork>().isMine)
                    car.SetCanControl(false);
        }
        else
            rankingText.text = rankList;

    }

    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void Quit() => Application.Quit();
}