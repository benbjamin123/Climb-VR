using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour {

	public bool gripping = false;
	public AudioSource grip;
	// Use this for initialization
	void Start () {
	}

	public void setGripping(bool newVal){
		
		this.gripping = newVal;
	}

	public bool getGripping(){
		return this.gripping;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
