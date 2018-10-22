using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Main script for photon networking 
public class PlayerConnect : MonoBehaviour {

	[SerializeField] GameObject playerPrefab;
	[SerializeField] Text statusText;
	[SerializeField] InputField playerNameField;
	public string playerName { get; set; } 
	string version = "0.1";
	string initialStatusString;

	private void Awake()
	{
        //Dont destroy this object on load
		DontDestroyOnLoad(transform);
        //Set network properties for fast data transfer for realtime syncing
		PhotonNetwork.sendRate = 30;
		PhotonNetwork.sendRateOnSerialize = 20;
        //Connect to the game
		PhotonNetwork.ConnectUsingSettings(version);		
		initialStatusString = statusText.text;
		statusText.text = initialStatusString + "Initial connection established";		
	}

	private void OnEnable()
	{
        //Subscribe to event on scene load
		SceneManager.sceneLoaded += HandleSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= HandleSceneLoaded;
	}

    //Callback for master server connection
	private void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby(TypedLobby.Default);
		statusText.text = initialStatusString + "Connected to Photon master server";
	}

    //Callback for lobby connection
    private void OnJoinedLobby()
	{
		statusText.text = initialStatusString + "Connected to Lobby, Please create or join a game";
	}

    //Callback for room connection
    private void OnJoinedRoom()
	{
        //If player name is empty set it as a default name
		if (playerNameField.text == "")
			playerNameField.text = "No Name";
		playerName = playerNameField.text;
        //Load the main level
		PhotonNetwork.LoadLevel("Main");
	}

    //Instantiate photon player on scene load
	private void HandleSceneLoaded(Scene iSceneName, LoadSceneMode iMode)
	{
		if (iSceneName.name == "Main")
		{
			PhotonNetwork.Instantiate(playerPrefab.name, Camera.main.transform.position, Camera.main.transform.rotation, 0);
		}
	}	
}
