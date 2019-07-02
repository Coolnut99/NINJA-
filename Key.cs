using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	public enum collectKey {bronze, silver, gold};
	public collectKey keyType;
	private AudioSource keyCollect;

	// Use this for initialization
	void Start () {
		keyCollect = GameObject.Find("Item Collect").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider != null && collider.gameObject.tag == "Player") {
			keyCollect.Play();
			Destroy(gameObject);
		}
	}
}
