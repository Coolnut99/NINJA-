using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaSlide : MonoBehaviour {

	[SerializeField]
	private GameObject hitSlide;

	GameObject hitSlideClone;

	public float slideTimer;

	public int damage;
	// Use this for initialization
	void Start () {
		StartCoroutine(DestroySlide());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Enemy" && GameObject.Find("Ninja").GetComponent<Ninja>().powerupType == Ninja.collectPowerup.hammer) {
			if (collider.gameObject.GetComponent<EnemyHealth>() != null) {
				collider.gameObject.GetComponent<EnemyHealth>().health -= damage + 1;
			}
			hitSlideClone = Instantiate(hitSlide, transform.position, Quaternion.identity) as GameObject;
			hitSlideClone.transform.Translate(new Vector3 (GameObject.Find("Ninja").GetComponent<Ninja>().flipSprite()* 1.5f, 0f, 0f));
			Destroy(gameObject);
		} else if (collider.gameObject.tag == "Enemy" && GameObject.Find("Ninja").GetComponent<Ninja>().powerupType != Ninja.collectPowerup.hammer) {
			if (collider.gameObject.GetComponent<EnemyHealth>() != null) {
				collider.gameObject.GetComponent<EnemyHealth>().health -= damage;
			}
			hitSlideClone = Instantiate(hitSlide, transform.position, Quaternion.identity) as GameObject;
			hitSlideClone.transform.Translate(new Vector3 (GameObject.Find("Ninja").GetComponent<Ninja>().flipSprite()* 1.5f, 0f, 0f));
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collider) {
		
	}

	IEnumerator DestroySlide() {
		yield return new WaitForSeconds (slideTimer);
		Destroy (gameObject);
	}
}
