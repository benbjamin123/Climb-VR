using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTouchpadMove : MonoBehaviour {
	[SerializeField]
	private Transform rig;

	private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

	private SteamVR_TrackedObject trackedObj;
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input ((int)trackedObj.index); } }

	private Vector2 axis = Vector2.zero;
	bool canGrip = false;


	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	void OnTriggerEnter(){
		canGrip = true;
	}
	void OnTriggerExit(){
		canGrip = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (controller == null) {
			Debug.Log ("Controller not initialized");
			return;
		}

		var device = SteamVR_Controller.Input ((int)trackedObj.index);

		if(controller.GetPress(touchpad) && canGrip == false)
		{
			axis = device.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0);

			if (rig!=null)
			{
				rig.position += (transform.right * axis.x + transform.forward * axis.y) * Time.deltaTime;
				rig.position = new Vector3 (rig.position.x, rig.position.y, rig.position.z);
			}
		}
	}

}
	