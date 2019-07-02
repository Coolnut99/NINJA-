using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("Game Controller").GetComponent<GameController>().ResetScore();
		GameObject.Find("Game Controller").GetComponent<GameController>().ResetKeys();
		GameObject.Find("Game Controller").GetComponent<GameController>().gameStarted = false;
		GameObject.Find("Game Controller").GetComponent<GameController>().keepPowerup = false;
		GameObject.Find("Game Controller").GetComponent<GameController>().nextLevel = 1;
		GameObject.Find("Game Controller").GetComponent<GameController>().kunaiCount = 20;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S)) {
			Application.LoadLevel(GameObject.Find("Game Controller").GetComponent<GameController>().nextLevel);
		}

		if(Input.GetKeyDown(KeyCode.Q)) {
			Application.Quit();
		}

		if(Input.GetKeyDown(KeyCode.A)) {
			Application.LoadLevel("About");
		}

		// This one is just a test for enemies and other stuff
		if(Input.GetKeyDown(KeyCode.D)) {
			Application.LoadLevel("Dojo");
		}
	}
}
