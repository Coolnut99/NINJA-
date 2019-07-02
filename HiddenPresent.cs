using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPresent : MonoBehaviour {

	[SerializeField]
	private GameObject ninja;

	// Use this for initialization
	void Start () {
		if (ninja.GetComponent<Ninja>().powerupType == Ninja.collectPowerup.scroll)
		{
			GetComponent<Animator>().Play("Present visible");
		} else {
			GetComponent<Animator>().Play("Present invisible");
		}
	}
	
	void FixedUpdate() {
		if (ninja.GetComponent<Ninja>().powerupType == Ninja.collectPowerup.scroll)
		{
			GetComponent<Animator>().Play("Present visible");
		} else {
			GetComponent<Animator>().Play("Present invisible");
		}
	}
}
