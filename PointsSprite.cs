using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(DestroyPoints());
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 temp = transform.position;
		transform.position = new Vector3(temp.x, temp.y + (4f * Time.deltaTime));
	}

	IEnumerator DestroyPoints() {
		yield return new WaitForSeconds(1.0f);
		Destroy(gameObject);
	}
}
