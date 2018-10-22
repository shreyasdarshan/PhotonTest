using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script to allow smooth player movement without any rigidbody
public class NetworkPlayerMovement : Photon.MonoBehaviour
{
    //Smooth out movement
	[SerializeField] float positionLerpSpeed = 8f;
	[SerializeField] float rotationLerpSpeed = 8f;

	Transform camAttachPoint;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	private void Start()
	{
        //Get reference point based on vive headset camera
        camAttachPoint = Camera.main.transform.GetChild(0);
	}

	private void Update()
	{
        //If local player, then set the transform data, else get it from the networked players
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

    //Photon callback method for sending and receiving data stream
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
        //Check for errors and send or receive data
		if (stream.isWriting)
		{
			if (!float.IsNaN(transform.position.x) && !float.IsNaN(transform.position.y) && !float.IsNaN(transform.position.z))
				stream.SendNext(transform.position);

			if (!float.IsNaN(transform.rotation.x) && !float.IsNaN(transform.rotation.y) && !float.IsNaN(transform.rotation.z) && !float.IsNaN(transform.rotation.w))
				stream.SendNext(transform.rotation);

		}
		else
		{
			Vector3 receiveingPos = (Vector3)stream.ReceiveNext();
			Quaternion receiveingRot = (Quaternion)stream.ReceiveNext();

			if (!float.IsNaN(receiveingPos.x) && !float.IsNaN(receiveingPos.y) && !float.IsNaN(receiveingPos.z))
				correctPlayerPos = receiveingPos;

			if (!float.IsNaN(receiveingRot.x) && !float.IsNaN(receiveingRot.y) && !float.IsNaN(receiveingRot.z) && !float.IsNaN(receiveingRot.w))
				correctPlayerRot = receiveingRot;
		}
	}
}
