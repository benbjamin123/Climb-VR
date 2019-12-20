using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour {

	public Rigidbody Body;
	public PickFunctionality left;
	public PickFunctionality right;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {

		bool isResting = left.isResting || right.isResting;

		if (isResting) {
			if (left.isResting) {
				Body.useGravity = false;
				Body.isKinematic = true;
				Body.transform.position += (left.prevPos - left.transform.localPosition);
			} 


			if (right.isResting) {
				Body.useGravity = false;
				Body.isKinematic = true;
				Body.transform.position += (right.prevPos - right.transform.localPosition);
			} 



		}
		else {
			Body.useGravity = true;
			Body.isKinematic = false;
		}



		left.prevPos = left.transform.localPosition;
		right.prevPos = right.transform.localPosition;

	}
}
