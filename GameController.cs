using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController instance;

	private float score;

	public int bronzeKeys, silverKeys, goldKeys;

	public int kunaiCount;

	public int timeLeft;

	public int vitalityLeft;

	public int nextLevel;

	public bool gameStarted;
	public bool keepPowerup;

	// Use this for initialization
	void Awake () {
		MakeSingleton();
		ResetScore();
		GameObject.Find("Music Manager").GetComponent<MusicManagerInstance>().StopAllSounds();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MakeSingleton() {
		if(instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void AddScore(float s) {
		score += s;
	}

	public float GetScore() {
		return score;
	}

	public void ResetScore() {
		score = 0;
	}

	public void SetScore(float s) {
		score = s;
	}

	public void ResetKeys() {
		bronzeKeys = 0;
		silverKeys = 0;
		goldKeys = 0;
	}

	public void ResetPowerups() {
		//Add other powerups and make them false if necessary
		GameObject.Find("Shield Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		GameObject.Find("Hammer Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		GameObject.Find("Red Crystal Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		GameObject.Find("Blue Crystal Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		GameObject.Find("Blue Potion Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		GameObject.Find("Red Potion Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		GameObject.Find("Pot Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		GameObject.Find("Scroll Powerup").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);

		GameObject.Find("Ninja").GetComponent<Ninja>().powerupType = Ninja.collectPowerup.none;
	}

}
