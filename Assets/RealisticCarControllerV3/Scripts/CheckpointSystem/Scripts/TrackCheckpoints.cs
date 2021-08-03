using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;
    public event Action<string, bool> OnPlayerLastCheckpoint;

    public Transform[] raceStartPoints;

    public List<CheckpointSingle> checkpointSingleList;

    public RCC_CarControllerV3 carController;
    public List<RCC_CarControllerV3> carControllerList;
    private List<int> nextCheckpointSingleIndexList;

    public bool isMine;
    private void Awake() {
        foreach (var checkpointSingle in checkpointSingleList)
            checkpointSingle.SetTrackCheckpoints(this);
    }

    public void RefreshCarList() {
        carControllerList = FindObjectsOfType<RCC_CarControllerV3>().ToList();
        nextCheckpointSingleIndexList = new List<int>();
        for (int i = 0; i < carControllerList.Count; i++) {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, RCC_CarControllerV3 carController) {
        isMine = carController.GetComponent<RCC_PhotonNetwork>().isMine;
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carControllerList.IndexOf(carController)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            // Correct checkpoint
            Debug.Log("Correct");

            if (checkpointSingle == checkpointSingleList.Last()) {
                string playerName = "";
                if (isMine)
                    playerName = PhotonNetwork.NickName;
                else
                    foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                        if (player.NickName != PhotonNetwork.NickName)
                            playerName = player.NickName;

                EndRace(playerName, isMine);
                return;
            }

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            if(isMine)
                correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[carControllerList.IndexOf(carController)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else {
            // Wrong checkpoint
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }
    }

    private void EndRace(string playerName, bool isMine) {
        print("EndRace");
        OnPlayerLastCheckpoint?.Invoke(playerName, isMine);
    }
}
