using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent((typeof(SteamVR_Behaviour_Pose)))]
public class ThrowObject : MonoBehaviour
{
	SteamVR_Behaviour_Pose controllerBehaviour;

	Rigidbody attachedObjectRigidbody = null;
	GameObject attachedGameObject;
	FixedJoint fixedJoint = null;
	bool isThrowing = false;
	// Use this for initialization
	void Start()
	{
		controllerBehaviour = transform.GetComponent<SteamVR_Behaviour_Pose>();
		fixedJoint = transform.GetComponent<FixedJoint>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameObject ball = GameObject.Find("Ball");
			var otherPhotonView = ball.GetComponent<PhotonView>();
			if (otherPhotonView.ownerId != PhotonNetwork.player.ID)
				otherPhotonView.TransferOwnership(PhotonNetwork.player.ID);
		}

		if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(controllerBehaviour.inputSource))
			PickupObject();
		else if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(controllerBehaviour.inputSource))
			Throw();


	}

	private void FixedUpdate()
	{
		if (isThrowing)
		{
			isThrowing = false;
			fixedJoint.connectedBody = null;
			if (attachedObjectRigidbody)
			{
				attachedObjectRigidbody.velocity = controllerBehaviour.GetVelocity();
				attachedObjectRigidbody.angularVelocity = controllerBehaviour.GetAngularVelocity();
				attachedObjectRigidbody = null;
			}
		}
	}

	void PickupObject()
	{
		var otherPhotonView = attachedObjectRigidbody.gameObject.GetComponent<PhotonView>();
		if (otherPhotonView.ownerId != PhotonNetwork.player.ID)
			otherPhotonView.TransferOwnership(PhotonNetwork.player.ID);					

		if (attachedObjectRigidbody)
			fixedJoint.connectedBody = attachedObjectRigidbody;

	}

	void Throw()
	{		
		isThrowing = true;
	}

	private void OnTriggerEnter(Collider iOther)
	{
		//only pickup object if the controller doesn't have any already attached
		if (iOther.tag == "Throwable" && fixedJoint.connectedBody == null)
		{
			attachedObjectRigidbody = iOther.gameObject.GetComponent<Rigidbody>();
		}
	}

	private void OnTriggerExit(Collider iOther)
	{
		//only discard object if the controller doesn't have any already attached
		if (iOther.tag == "Throwable"  && fixedJoint.connectedBody == null)
		{
			attachedObjectRigidbody = null;
		}
	}


}
