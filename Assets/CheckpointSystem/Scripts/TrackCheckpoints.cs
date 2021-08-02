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

    public List<RCC_CarControllerV3> carControllerList;
    private List<int> nextCheckpointSingleIndexList;

    private void Awake() {
        foreach (var checkpointSingle in checkpointSingleList)
            checkpointSingle.SetTrackCheckpoints(this);
    }

    // TODO: Daha temiz yazılmalı
    [ContextMenu("Refresh")]
    public void RefreshCarList() {
        carControllerList = FindObjectsOfType<RCC_CarControllerV3>().ToList();

        nextCheckpointSingleIndexList = new List<int>();
        for (int i = 0; i < carControllerList.Count; i++) {
            carControllerList[i].transform.position = raceStartPoints[i].position;
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, RCC_CarControllerV3 carController) {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carControllerList.IndexOf(carController)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            // Correct checkpoint
            Debug.Log("Correct");

            if (checkpointSingle == checkpointSingleList.Last()) {
                var isMe = carController.GetComponent<RCC_PhotonNetwork>().isMine;
                string playerName = "";
                if (isMe)
                    playerName = PhotonNetwork.NickName;
                else
                    foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
                        if (player.NickName != PhotonNetwork.NickName)
                            playerName = player.NickName;

                EndRace(playerName, isMe);
                return;
            }

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
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

    private void EndRace(string playerName, bool isMe) {
        print("EndRace");
        OnPlayerLastCheckpoint?.Invoke(playerName, isMe);
    }
}
