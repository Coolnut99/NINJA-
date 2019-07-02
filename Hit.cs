using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	private float hitTimer = 0.2f;

	// Use this for initialization
	void Start () {
		StartCoroutine(HitDestroy());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator HitDestroy() {
		yield return new WaitForSeconds(hitTimer);
		Destroy(gameObject);
	}
}
