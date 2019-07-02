using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public float coinTimer = 5f;
	public float coinCritical = 5f;
	public float value;
	private AudioSource coin;

	[SerializeField]
	private GameObject scoreText;
	GameObject scoreTextClone;

	// Use this for initialization
	void Awake () {
		coin = GameObject.Find("Coin").GetComponent<AudioSource>();
		StartCoroutine(destroyCoin());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player") {
			coin.Play();
			scoreTextClone = Instantiate (scoreText, transform.position, Quaternion.identity) as GameObject;

			Destroy(gameObject);
		}
	}

	IEnumerator destroyCoin() {
		Color color = gameObject.GetComponent<SpriteRenderer>().color;
		yield return new WaitForSeconds(coinTimer);
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.color = new Color(0.5f, 0.5f, 0.5f, 0.67f);
		yield return new WaitForSeconds(coinCritical);
		Destroy(gameObject);

	}
}
