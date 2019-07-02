using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSummon : MonoBehaviour {

	[SerializeField]
	private GameObject boss, bossSummoningPoint; // Boss is one of the "enemies", bossSummoningPoint is just an empty GameObject with a transform
	GameObject bossClone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Player") {
			Debug.Log("WARNING! WARNING! A HUGE BOSS TROLL IS APPROACHING!!!!!");
			MusicManagerInstance.instance.StopAllSounds();
			MusicManagerInstance.instance.PlayTrack("Boss Battle");
			//instantiate a boss on the boss's summoning point
			bossClone = Instantiate(boss, bossSummoningPoint.transform.position, Quaternion.identity) as GameObject;

			//then turn off this GameObject
			GetComponent<BoxCollider2D>().gameObject.SetActive(false);
		}
	}
}
