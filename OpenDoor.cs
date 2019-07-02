using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

	[SerializeField]
	private Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D collider) {
		if(Input.GetKey(KeyCode.UpArrow) && collider.gameObject.tag == "Player") {
			StartCoroutine(ExitLevel());
		}
	}

	IEnumerator ExitLevel() {
		Time.timeScale = 0f;
		animator.Play("level end fade out");
		GameObject.Find("Music Manager").GetComponent<MusicManagerInstance>().StopAllSounds();
		GameObject.Find("Game Controller").GetComponent<GameController>().timeLeft = GameObject.Find("Ninja").GetComponent<Ninja>().timeLeft;
		GameObject.Find("Game Controller").GetComponent<GameController>().vitalityLeft = GameObject.Find("Ninja").GetComponent<Ninja>().health;
		GameObject.Find("Game Controller").GetComponent<GameController>().SetScore(GameObject.Find("Ninja").GetComponent<Ninja>().score);
		GameObject.Find("Game Controller").GetComponent<GameController>().nextLevel++;
		yield return new WaitForSecondsRealtime(2.0f);
		Application.LoadLevel("Clear Level");
	}
}
