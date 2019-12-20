using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	// Stores object trigger is colliding with
	private GameObject collidingObject; 
	// Reference to object currently being held
	private GameObject objectInHand;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void SetCollidingObject(Collider col)
	{
		// Doesn't make the target a potential grab target if the player is already holding something or the target has no rigidbody
		if (collidingObject || !col.GetComponent<Rigidbody>() || !col.CompareTag("Grabbable"))
		{
			return;
		}
		// Assigns the object as a potential grab target
		collidingObject = col.gameObject;
	}
	// When trigger collider enters another, sets up the other collider as a potential grab target
	public void OnTriggerEnter(Collider other)
	{
		SetCollidingObject(other);
		if (collidingObject) {
			collidingObject.GetComponent<Renderer> ().material.SetColor ("_SpecColor", Color.white);
			SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(500);
		}
	}

	// Similar, but different as it ensures the target is set when the player holds a controller over an object for a while
	public void OnTriggerStay(Collider other)
	{
		SetCollidingObject(other);
		if (collidingObject) {
			collidingObject.GetComponent<Renderer> ().material.SetColor ("_SpecColor", Color.white);
		}
	}

	// When the collider exits an object, this removes its target
	public void OnTriggerExit(Collider other)
	{
		if (!collidingObject)
		{
			return;
		}
		collidingObject.GetComponent<Renderer> ().material.SetColor("_SpecColor",Color.black);
		SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(500);
		collidingObject = null;
	}
	private void GrabObject()
	{
		// Moves the GameObject inside the players hand and removes it from the collidingObject variable
		objectInHand = collidingObject;
		collidingObject = null;
		// Adds a new joint that connects the controller to the object using the AddFixedJoint() method
		var joint = AddFixedJoint();
		joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
	}

	// Makes a new fixed joint, adds it to the controller, then sets it up so it doesn't break easily.
	private FixedJoint AddFixedJoint()
	{
		FixedJoint fx = gameObject.AddComponent<FixedJoint>();
		fx.breakForce = 20000;
		fx.breakTorque = 20000;
		return fx;
	}
	private void ReleaseObject()
	{
		// Makes sure there's a fixed joint attached to the controller
		if (GetComponent<FixedJoint>())
		{
			// Removes the connection to the object held by the joint and destroys the joint
			GetComponent<FixedJoint>().connectedBody = null;
			Destroy(GetComponent<FixedJoint>());
			// Adds the speed and rotation of the controller when the player releases the object, so the result is a realistic arc
			objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
			objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
			objectInHand.GetComponent<Renderer> ().material.SetColor("_SpecColor",Color.black);
		}
		// removes the reference to the previously attached object
		objectInHand = null;
	}
	// Update is called once per frame
	void Update () {
		// When the player squeezes the trigger and there's a potential grab target, this grabs it
		if (Controller.GetHairTriggerDown ()) {
			if (collidingObject) {
				GrabObject ();
			}
		}

		// If the player relaeases the trigger and there's an object attached to the controller, this releases it.
		if (Controller.GetHairTriggerUp())
		{
			if (objectInHand)
			{
				ReleaseObject();
			}
		}
	}
}
