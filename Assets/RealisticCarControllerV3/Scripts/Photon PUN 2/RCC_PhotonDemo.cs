//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2019 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Pun;

/// <summary>
/// A simple manager script for photon demo scene. It has an array of networked spawnable player vehicles, public methods, restart, and quit application.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Demo Manager")]
public class RCC_PhotonDemo : Photon.Pun.MonoBehaviourPun {

	public RCC_CarControllerV3[] selectableVehicles;

	public int selectedCarIndex = 0;
	public int selectedBehaviorIndex = 0;

	public Transform spawnPoint;

	public void Spawn () {

		Vector3 lastKnownPos = Vector3.zero;
		Quaternion lastKnownRot = Quaternion.identity;

		RCC_CarControllerV3 newVehicle;

		if(RCC_SceneManager.Instance.activePlayerVehicle){

			lastKnownPos = RCC_SceneManager.Instance.activePlayerVehicle.transform.position;
			lastKnownRot = RCC_SceneManager.Instance.activePlayerVehicle.transform.rotation;

		}

		if (lastKnownPos == Vector3.zero) {
			
			lastKnownPos = spawnPoint.position;
			lastKnownRot = spawnPoint.rotation;

		}

		lastKnownRot.x = 0f;
		lastKnownRot.z = 0f;

		if(RCC_SceneManager.Instance.activePlayerVehicle)
			PhotonNetwork.Destroy(RCC_SceneManager.Instance.activePlayerVehicle.gameObject);
			
		newVehicle = PhotonNetwork.Instantiate("Photon Vehicles/" + selectableVehicles[selectedCarIndex].gameObject.name, lastKnownPos + (Vector3.up), lastKnownRot, 0).GetComponent<RCC_CarControllerV3>();

		RCC.RegisterPlayerVehicle (newVehicle);
		RCC.SetControl (newVehicle, true);

		if (RCC_SceneManager.Instance.activePlayerCamera)
			RCC_SceneManager.Instance.activePlayerCamera.SetTarget (newVehicle.gameObject);
		
	}

	public void SelectVehicle (int index) {

		selectedCarIndex = index;

	}

	public void SelectBehavior(int index){

		selectedBehaviorIndex = index;

	}

	public void RestartScene(){

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	}

	public void Quit(){

		Application.Quit();

	}

}
