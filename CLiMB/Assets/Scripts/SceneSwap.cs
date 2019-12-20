using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour {

	// Use this for initialization
	public void OnClick(string level){
		SceneManager.LoadSceneAsync (level);
		SceneManager.UnloadSceneAsync (0);
	}
}
