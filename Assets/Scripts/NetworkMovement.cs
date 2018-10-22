using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMovement : Photon.MonoBehaviour
{
	[SerializeField] float positionLerpSpeed = 8f;
	[SerializeField] float rotationLerpSpeed = 8f;

	PhotonView photonView;
	private Vector3 correctPos;
	private Quaternion correctRot;

	private void Start()
	{
		photonView = transform.GetComponent<PhotonView>();
	}

	private void Update()
	{
		if (photonView.isMine)
		{
			//transform.position = Camera.main.transform.position + localOffset;
			//transform.rotation = Camera.main.transform.rotation;
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPos, Time.deltaTime * positionLerpSpeed);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctRot, Time.deltaTime * rotationLerpSpeed);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

		}
		else
		{
			// Network player, receive data
			this.correctPos = (Vector3)stream.ReceiveNext();
			this.correctRot = (Quaternion)stream.ReceiveNext();
		}
	}
}
