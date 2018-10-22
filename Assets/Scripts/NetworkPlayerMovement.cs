using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerMovement : Photon.MonoBehaviour
{
	[SerializeField] float positionLerpSpeed = 8f;
	[SerializeField] float rotationLerpSpeed = 8f;

	Transform camAttachPoint;
	PhotonView photonView;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	private void Start()
	{
		photonView = transform.GetComponent<PhotonView>();
		camAttachPoint = Camera.main.transform.GetChild(0);
	}

	private void Update()
	{
		if (photonView.isMine)
		{
			transform.position = camAttachPoint.position;
			transform.rotation = camAttachPoint.rotation;
		}
		else
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * positionLerpSpeed);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * rotationLerpSpeed);
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
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}
}
