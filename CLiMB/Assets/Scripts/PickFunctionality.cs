using UnityEngine;
using System.Collections;

public class PickFunctionality : MonoBehaviour
{

	public SteamVR_TrackedObject controller;

	[HideInInspector]
	public Vector3 prevPos;

	public bool isResting;

	// Use this for initialization
	void Start () {
		prevPos = controller.transform.localPosition;

	}

	void OnTriggerEnter(){
		SteamVR_Controller.Input((int)controller.index).TriggerHapticPulse(500);
		isResting = true;
	}
	void OnTriggerExit(){
		isResting = false;
	}

}