using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	private Transform cameraTransform;
	private float distance = 3f;

	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector2(cameraTransform.position.x * 0.80f, (cameraTransform.position.y * 0.80f) + 7f);
	}
}
