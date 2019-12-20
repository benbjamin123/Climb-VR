using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winScript : MonoBehaviour {
	public Rigidbody Body;
	public Rigidbody WinPlatform;
	public GameObject WinText;

	public void OnTriggerEnter(Collider player){
		if(player.attachedRigidbody == Body){
			print("Win");
			WinText.SetActive (true);
		}
	
	}
	public void OnTriggerExit(Collider player){
		if (player.attachedRigidbody == Body) {
			print ("Lose");
			WinText.SetActive (false);
		}
	}
}