using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerName : Photon.MonoBehaviour
{
	//Change Player Name
	[SerializeField] Text playerNameText;
	GameObject photonManager;
	PhotonView photonView;
	// Use this for initialization
	void Start()
	{
		photonView = transform.GetComponent<PhotonView>();
		if (photonView.isMine)
		{
			photonManager = GameObject.Find("PhotonNetworkManager");
			if (photonManager)
			{
				PlayerConnect playerConnect = photonManager.GetComponent<PlayerConnect>();
				playerNameText.text = playerConnect.playerName;
				photonView.RPC("ChangePlayerNameRPC", PhotonTargets.OthersBuffered, playerConnect.playerName);
			}
		}

	}

	[PunRPC]
	void ChangePlayerNameRPC(string iName)
	{
		playerNameText.text = iName;
	}
}
