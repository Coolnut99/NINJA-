using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavySlash : MonoBehaviour {

	private GameObject hitSword;
	GameObject hitSwordClone;

	public float swordTimer = 0.5f;
	private int flipExplosion;

	public int damage = 5;

	// Use this for initialization
	void Start () {
		StartCoroutine(destroySword());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	IEnumerator destroySword() {
		yield return new WaitForSeconds(swordTimer);
		Destroy(gameObject);
	}
}
