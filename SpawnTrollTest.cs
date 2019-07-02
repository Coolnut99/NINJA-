using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrollTest : MonoBehaviour {

	public GameObject troll;
	GameObject trollClone;
	public float spawnTime = 5f;
	private bool canSpawnTroll;

	// Use this for initialization
	void Start () {
		canSpawnTroll = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(canSpawnTroll == true) {
			StartCoroutine(SpawnTroll());
		}
	}

	IEnumerator SpawnTroll() {
		canSpawnTroll = false;
		trollClone = Instantiate(troll, transform.position, Quaternion.identity) as GameObject;
		trollClone.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		yield return new WaitForSeconds(spawnTime);
		Debug.Log("Troll spawned");
		canSpawnTroll = true;
	}

}
