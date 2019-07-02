using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingDummy : MonoBehaviour {

	[SerializeField]
	private GameObject explosion;
	GameObject explosionClone;

	private bool hit;

	// Use this for initialization
	void Start () {
		hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Kunai" || collider.gameObject.tag == "Sword") {
			if (hit == false) {
				StartCoroutine(ExplodeAndDestroy());
				hit = true;
			}
		}
	}

	IEnumerator ExplodeAndDestroy() {
		float timer = 3.0f;
		while (timer > 0) {
			float tempX = Random.Range(-3f, 3f);
			float tempY = Random.Range(-2f, 2f);
			explosionClone = Instantiate(explosion, new Vector3(transform.position.x + tempX, transform.position.y + tempY), Quaternion.identity) as GameObject;
			explosionClone.transform.parent = transform;
			yield return new WaitForSeconds (0.1f);
			timer -= 0.1f;
		}
		Destroy(gameObject);
	}
}
