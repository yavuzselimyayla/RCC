//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2019 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Connects to Photon Server, registers the player, and activates player UI panel when connected.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Scene Manager")]
public class RCC_PhotonManager : Photon.Pun.MonoBehaviourPunCallbacks {

	public InputField playerName;
	public RCC_PhotonRace photonRace;

	void Start () {

		ConnectToServer();
	
	}

	void ConnectToServer () {

		print("Connecting to photon server");
		RCC_InfoLabel.Instance.ShowInfo ("Connecting to photon server");

		if (!Photon.Pun.PhotonNetwork.IsConnectedAndReady) {

			playerName.gameObject.SetActive(false);
			RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType (RCC_UIDashboardDisplay.DisplayType.Off);
			
			Photon.Pun.PhotonNetwork.ConnectUsingSettings ();

		}

		if (Photon.Pun.PhotonNetwork.IsConnectedAndReady) {

			playerName.gameObject.SetActive(false);
			RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType (RCC_UIDashboardDisplay.DisplayType.Full);

		}
	
	}

	public override void OnConnectedToMaster(){

		print ("Connected to master server");
		Photon.Pun.PhotonNetwork.JoinLobby ();
	}

	void OnGUI(){

		if(!Photon.Pun.PhotonNetwork.IsConnectedAndReady)
			GUI.color = Color.red;
		
		GUILayout.Label("State: " + PhotonNetwork.NetworkClientState.ToString());
		GUI.color = Color.white;
		GUILayout.Label("Total Player Count: " + Photon.Pun.PhotonNetwork.PlayerList.Length.ToString());
		GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString());

	}

	public override void OnJoinedLobby(){

		print("Joined lobby");
		RCC_InfoLabel.Instance.ShowInfo ("Joined Lobby");
		playerName.gameObject.SetActive(true);

	}

    public void QuickMatch() {
		PhotonNetwork.JoinRandomOrCreateRoom();
	}

	public override void OnJoinRandomFailed(short a, string b){

		print("Joining to random room has failed!, Creating new room...");
		RCC_InfoLabel.Instance.ShowInfo ("Joining to random room has failed!, Creating new room...");
		PhotonNetwork.CreateRoom(null);
	} 

	public override void OnJoinedRoom(){
		print("Joined room");
		RCC_InfoLabel.Instance.ShowInfo ("Joined Room. Waiting for opponents");
		StartCoroutine(WaitForPlayers());
	}

	public IEnumerator WaitForPlayers() {
        while (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
			yield return null;
		
		photonRace.ReadyRace();
	}

	public void SetPlayerName(string name){
		PhotonNetwork.NickName = name;
		playerName.gameObject.SetActive(false);
		RCC_SceneManager.Instance.activePlayerCanvas.SetDisplayType (RCC_UIDashboardDisplay.DisplayType.Full);
	}
}
