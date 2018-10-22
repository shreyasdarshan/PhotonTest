using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class to handle create and join game buttons
public class CreateAndJoin : MonoBehaviour {
	[SerializeField] InputField createGameName;
	[SerializeField] InputField joinGameName;

    //Create room on click 
	public void OnCreateGame()
	{
		PhotonNetwork.CreateRoom(createGameName.text);
	}

    //Join room on click 
    public void OnJoinGame()
	{
		PhotonNetwork.JoinRoom(joinGameName.text);
	}
}
