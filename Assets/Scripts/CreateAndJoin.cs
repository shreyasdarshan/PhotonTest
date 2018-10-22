using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoin : MonoBehaviour {
	[SerializeField] InputField createGameName;
	[SerializeField] InputField joinGameName;

	public void OnCreateGame()
	{
		PhotonNetwork.CreateRoom(createGameName.text);
	}

	public void OnJoinGame()
	{
		PhotonNetwork.JoinRoom(joinGameName.text);
	}
}
