using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraKunai : MonoBehaviour {

	private AudioSource powerupSound;

	// Use this for initialization
	void Start () {
		powerupSound = GameObject.Find("Powerup Collect").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider != null && collider.gameObject.tag == "Player") {
			//Add 10 kunai to total
			powerupSound.Play();
			Destroy(gameObject);
		}
	}
}
