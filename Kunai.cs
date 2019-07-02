using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour {

	public float kunaiTimer = 3f;
	public int damage = 1;

	[SerializeField]
	private GameObject explosionKunai;

	GameObject explosionClone;

	private int localFlipSprite;


	// Use this for initialization
	void Start () {
		StartCoroutine(destroyKunai());
		localFlipSprite = GameObject.Find("Ninja").GetComponent<Ninja>().flipSprite();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Enemy" && GameObject.Find("Ninja").GetComponent<Ninja>().powerupType == Ninja.collectPowerup.hammer) {
			if (collider.gameObject.GetComponent<EnemyHealth>() != null) {
				collider.gameObject.GetComponent<EnemyHealth>().health -= (damage + 1);
				Debug.Log("You have hit the enemy with the hammer powerup");
			}
			explosionClone = Instantiate(explosionKunai, transform.position, Quaternion.identity) as GameObject;
			explosionClone.transform.Translate(new Vector3 (localFlipSprite * 1.5f, 0f, 0f));
			Destroy(gameObject);
		} else if (collider.gameObject.tag == "Enemy" && GameObject.Find("Ninja").GetComponent<Ninja>().powerupType != Ninja.collectPowerup.hammer) {
			if (collider.gameObject.GetComponent<EnemyHealth>() != null) {
				collider.gameObject.GetComponent<EnemyHealth>().health -= damage;
				Debug.Log("You have hit the enemy");
			}
			explosionClone = Instantiate(explosionKunai, transform.position, Quaternion.identity) as GameObject;
			explosionClone.transform.Translate(new Vector3 (localFlipSprite * 1.5f, 0f, 0f));
			Destroy(gameObject);
		}
	}

	IEnumerator destroyKunai() {
		yield return new WaitForSeconds(kunaiTimer);
		Destroy(gameObject);
	}
}
