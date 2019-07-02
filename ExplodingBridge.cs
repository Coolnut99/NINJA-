using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBridge : MonoBehaviour {

	[SerializeField]
	private GameObject explosion;
	GameObject explosionClone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DestroyBridge() {
		StartCoroutine(Detonator());	
	}

	IEnumerator Detonator() {
		float timer = 1.0f;
		while (timer > 0) {
			float tempX = Random.Range(-3f, 3f);
			float tempY = Random.Range(-2f, 2f);
			explosionClone = Instantiate(explosion, new Vector3(transform.position.x + tempX, transform.position.y + tempY), Quaternion.identity) as GameObject;
			explosionClone.transform.parent = transform;
			yield return new WaitForSeconds (0.06f);
			timer -= 0.06f;
		}
		Destroy(this.gameObject);

	}
}
