using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaHealth : MonoBehaviour {

	[SerializeField]
	private GameObject [] healthBar;

	// Use this for initialization
	void Start () {
		//SetHealth(12);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void SetHealth (int health) {
		for (int i = 0; i < healthBar.Length; i++) {
			SpriteRenderer[] a = healthBar[i].GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer s in a) {
				if (health > i) {
					if(s.gameObject.name == "Red Health") {
						s.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
						}
					if (s.gameObject.name == "Blue Health") {
						s.gameObject.GetComponent<SpriteRenderer>().color = new Color (0f, 0f, 0f, 0f);
					}
				} else {
					if(s.gameObject.name == "Red Health") {
						s.gameObject.GetComponent<SpriteRenderer>().color = new Color (0f, 0f, 0f, 0f);
					}
					if (s.gameObject.name == "Blue Health") {
						s.gameObject.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
					}
				}
			}
		}
	}
}
