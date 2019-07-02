using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkullTest : MonoBehaviour {

	public GameObject skull;
	GameObject skullClone;
	public float spawnTime = 3f;
	private bool canSpawnSkull;

	// Use this for initialization
	void Start () {
		canSpawnSkull = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(canSpawnSkull == true) {
			StartCoroutine(SpawnSkull());
		}
	}

	IEnumerator SpawnSkull() {
		canSpawnSkull = false;
		float tempTime = Random.Range(0.5f, spawnTime);
		float temp = GameObject.Find("Ninja").transform.position.y;
		transform.position = new Vector3(transform.position.x, temp);
		skullClone = Instantiate(skull, transform.position, Quaternion.identity) as GameObject;
		skullClone.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		yield return new WaitForSeconds(tempTime);
		canSpawnSkull = true;
	}
}
