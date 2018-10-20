using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent((typeof(SteamVR_Behaviour_Pose)))]
public class ThrowObject : MonoBehaviour
{
	SteamVR_Behaviour_Pose rightControllerBehaviour;

	Rigidbody attachedObjectRigidbody = null;
	FixedJoint fixedJoint = null;
	bool isThrowing = false;
	// Use this for initialization
	void Start()
	{
		rightControllerBehaviour = transform.GetComponent<SteamVR_Behaviour_Pose>();
		fixedJoint = transform.GetComponent<FixedJoint>();
	}

	// Update is called once per frame
	void Update()
	{

		if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(rightControllerBehaviour.inputSource))
		{
			Debug.Log("Test");
			//SteamVR_Input.
			//Debug.Log(rightControllerBehaviour.GetVelocity());
			PickupObject();
		}

		else if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(rightControllerBehaviour.inputSource))
		{
			Debug.Log("Test");
			//SteamVR_Input.
			//Debug.Log(rightControllerBehaviour.GetVelocity());
			Throw();
		}

	}

	private void FixedUpdate()
	{
		if(isThrowing)
		{
			isThrowing = false;
			if (attachedObjectRigidbody)
			{
				attachedObjectRigidbody.velocity = rightControllerBehaviour.GetVelocity();
				attachedObjectRigidbody.angularVelocity = rightControllerBehaviour.GetAngularVelocity();
			}
		}
	}

	void PickupObject()
	{
		if (attachedObjectRigidbody)
			fixedJoint.connectedBody = attachedObjectRigidbody;
		
	}

	void Throw()
	{
		fixedJoint.connectedBody = null;
		isThrowing = true;
	}

	private void OnTriggerEnter(Collider iOther)
	{
		if (iOther.tag == "Throwable")
		{
			attachedObjectRigidbody = iOther.gameObject.GetComponent<Rigidbody>();
		}
	}

	private void OnTriggerExit(Collider iOther)
	{
		if (iOther.tag == "Throwable")
		{
			attachedObjectRigidbody = null;
		}
	}


}
