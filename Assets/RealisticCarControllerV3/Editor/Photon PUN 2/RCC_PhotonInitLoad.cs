//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2019 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;

public class RCC_PhotonInitLoad : MonoBehaviour {

	[InitializeOnLoad]
	public class InitOnLoad {

		static InitOnLoad(){

			if(!EditorPrefs.HasKey("Photon" + "V3.3" + "Installed")){
				
				EditorPrefs.SetInt("Photon" + "V3.3" + "Installed", 1);
				EditorUtility.DisplayDialog("Photon PUN 2 For Realistic Car Controller", "Be sure you have imported latest Photon PUN 2 to your project. Pass in your AppID to Photon, and run the RCC City Photon 2 demo scene. You can find more detailed info in documentation.", "Close");

			}

		}

	}

}
