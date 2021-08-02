using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;
    public event Action<bool> OnPlayerLastCheckpoint;

    public List<CheckpointSingle> checkpointSingleList;

    public List<RCC_CarControllerV3> carControllerList;
    private List<int> nextCheckpointSingleIndexList;

    private void Awake() {
        foreach (var checkpointSingle in checkpointSingleList)
            checkpointSingle.SetTrackCheckpoints(this);
        
        RefreshCarList();
    }

    // TODO: Daha temiz yazılmalı
    public void RefreshCarList() {
        carControllerList = FindObjectsOfType<RCC_CarControllerV3>().ToList();

        nextCheckpointSingleIndexList = new List<int>();
        foreach (var carController in carControllerList)
            nextCheckpointSingleIndexList.Add(0);
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, RCC_CarControllerV3 carController) {        
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carControllerList.IndexOf(carController)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            // Correct checkpoint
            Debug.Log("Correct");

            if (checkpointSingle == checkpointSingleList.Last()) {
                EndRace(carController.GetComponent<RCC_PhotonNetwork>().isMine);
                return;
            }

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[carControllerList.IndexOf(carController)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        } else {
            // Wrong checkpoint
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }
    }

    private void EndRace(bool isMe) {
        print("EndRace");
        OnPlayerLastCheckpoint?.Invoke(isMe);

    }
}
