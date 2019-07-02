using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

	public enum collectPowerup {shield, hammer, redPendant, bluePendant, bluePotion, redPotion, pot, scroll};
	public collectPowerup powerupType;
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
			powerupSound.Play();
			Destroy(gameObject);
		}
	}
}
