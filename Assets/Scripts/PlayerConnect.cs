using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerConnect : MonoBehaviour {

	[SerializeField] GameObject playerPrefab;
	[SerializeField] Text statusText;
	[SerializeField] InputField playerNameField;
	public string playerName { get; set; } 
	string version = "0.1";
	string initialStatusString;


	private void Awake()
	{
		DontDestroyOnLoad(transform);
		PhotonNetwork.sendRate = 30;
		PhotonNetwork.sendRateOnSerialize = 20;

		PhotonNetwork.ConnectUsingSettings(version);
		Debug.Log("Init connect");
		initialStatusString = statusText.text;
		statusText.text = initialStatusString + "Init connect";		
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += HandleSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= HandleSceneLoaded;
	}

	private void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby(TypedLobby.Default);
		Debug.Log("Master connect");
		statusText.text = initialStatusString + "Master connect";
	}

	private void OnJoinedLobby()
	{
		Debug.Log("Lobby connect");
		statusText.text = initialStatusString + "Lobby connect, Please join or create game";
	}

	private void OnJoinedRoom()
	{
		if (playerNameField.text == "")
			playerNameField.text = "No Name";
		playerName = playerNameField.text;
		PhotonNetwork.LoadLevel("Main");
	}

	private void HandleSceneLoaded(Scene iSceneName, LoadSceneMode iMode)
	{
		if (iSceneName.name == "Main")
		{
			Debug.Log("sd");
			PhotonNetwork.Instantiate(playerPrefab.name, Camera.main.transform.position, Camera.main.transform.rotation, 0);
		}
	}	
}
