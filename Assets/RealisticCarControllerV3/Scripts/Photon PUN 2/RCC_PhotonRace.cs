using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RCC_PhotonRace : MonoBehaviourPun {
    public TrackCheckpoints trackCheckpoints;
    public RCC_CarControllerV3[] selectableVehicles;
    public int selectedCarIndex = 0;
    public int selectedBehaviorIndex = 0;
    public Transform[] spawnPoints;

    public void SelectVehicle(int index) => selectedCarIndex = index;

    public void Spawn() {

        Vector3 lastKnownPos = Vector3.zero;
        Quaternion lastKnownRot = Quaternion.identity;

        RCC_CarControllerV3 newVehicle;

        if (RCC_SceneManager.Instance.activePlayerVehicle) {
            lastKnownPos = RCC_SceneManager.Instance.activePlayerVehicle.transform.position;
            lastKnownRot = RCC_SceneManager.Instance.activePlayerVehicle.transform.rotation;
        }

        if (lastKnownPos == Vector3.zero) {
            lastKnownPos = spawnPoints[PhotonNetwork.CountOfPlayers - 1].position;
            lastKnownRot = spawnPoints[PhotonNetwork.CountOfPlayers - 1].rotation;
        }


        lastKnownRot.x = 0f;
        lastKnownRot.z = 0f;

        if (RCC_SceneManager.Instance.activePlayerVehicle)
            PhotonNetwork.Destroy(RCC_SceneManager.Instance.activePlayerVehicle.gameObject);

        newVehicle = PhotonNetwork.Instantiate("Photon Vehicles/" + selectableVehicles[selectedCarIndex].gameObject.name, lastKnownPos + (Vector3.up), lastKnownRot, 0).GetComponent<RCC_CarControllerV3>();

        RCC.RegisterPlayerVehicle(newVehicle);
        RCC.SetControl(newVehicle, true);

        if (RCC_SceneManager.Instance.activePlayerCamera)
            RCC_SceneManager.Instance.activePlayerCamera.SetTarget(newVehicle.gameObject);

    }


    public void BeginRace() {
        Spawn();
        trackCheckpoints.RefreshCarList();
        trackCheckpoints.OnPlayerLastCheckpoint += TrackCheckpoints_OnPlayerLastCheckpoint1; ;
    }

    private void TrackCheckpoints_OnPlayerLastCheckpoint1(bool isMe) {
        if (isMe)
            Debug.Log("You Win");
        else
            Debug.Log("You Lost");
    }

    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void Quit() => Application.Quit();
}

/*
Maç Seçenekleri
-Maç Ara-
-Maç Bul-
Maçı Başlat, bağlan,araba oluştur
*/