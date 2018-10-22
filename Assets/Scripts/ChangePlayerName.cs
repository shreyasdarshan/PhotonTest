using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Change Player Name
public class ChangePlayerName : Photon.MonoBehaviour
{
	[SerializeField] Text playerNameText;
	GameObject photonManager;

    void Start()
	{
        //Send RPC from current players to other players to reflect name change
		if (photonView.isMine)
		{
            //Get a photon manager reference. This occurs only once per game
			photonManager = GameObject.Find("PhotonNetworkManager");
			if (photonManager)
			{
				PlayerConnect playerConnect = photonManager.GetComponent<PlayerConnect>();
				playerNameText.text = playerConnect.playerName;
                //Send buffered RPC to reflect name change for the other players as well as the ones who join later
				photonView.RPC("ChangePlayerNameRPC", PhotonTargets.OthersBuffered, playerConnect.playerName);
			}
		}

	}

    //RPC to change name
	[PunRPC]
	void ChangePlayerNameRPC(string iName)
	{
		playerNameText.text = iName;
	}
}
