using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

//Script to handle pick up and throw of an object in VR
[RequireComponent((typeof(SteamVR_Behaviour_Pose)))]
public class ThrowObject : MonoBehaviour
{
    //Controller behaviour ref
	SteamVR_Behaviour_Pose controllerBehaviour;
    //Physics refs
	Rigidbody attachedObjectRigidbody = null;
	GameObject attachedGameObject;
	FixedJoint fixedJoint = null;
	bool isThrowing = false;

    void Start()
	{
		controllerBehaviour = transform.GetComponent<SteamVR_Behaviour_Pose>();
        //Fixed joint used to attach rigidbody to this object's rigidbody with physics
        fixedJoint = transform.GetComponent<FixedJoint>();
	}

	void Update()
	{
        //Code for debugging only
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameObject ball = GameObject.Find("Ball");
			var otherPhotonView = ball.GetComponent<PhotonView>();
			if (otherPhotonView.ownerId != PhotonNetwork.player.ID)
				otherPhotonView.TransferOwnership(PhotonNetwork.player.ID);
		}

        //Check for trigger down then pick up and throw
		if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(controllerBehaviour.inputSource))
			PickupObject();
		else if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(controllerBehaviour.inputSource))
			Throw();
	}

	private void FixedUpdate()
	{
        //Release object with the controllers' velocity when released 
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
        //Attach the other rigidbody to ours
		if (attachedObjectRigidbody)
		{
            //Check for ownership of the networked object, if not ours take the ownership to control network movement
			var otherPhotonView = attachedObjectRigidbody.gameObject.GetComponent<PhotonView>();
			if (otherPhotonView.ownerId != PhotonNetwork.player.ID)
				otherPhotonView.TransferOwnership(PhotonNetwork.player.ID);

			fixedJoint.connectedBody = attachedObjectRigidbody;
		}
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
