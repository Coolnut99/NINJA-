using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private GameObject ninja;

	// Use this for initialization
	void Start () {
		ninja = GameObject.Find("Ninja");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = transform.position;
		if (temp.y < 7f) {
			temp.y = 7f;
		}
		transform.position = new Vector3(ninja.transform.position.x, temp.y, -10);

	}
}
