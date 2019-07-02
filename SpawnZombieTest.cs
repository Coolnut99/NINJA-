using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombieTest : MonoBehaviour {

	public GameObject zombie;
	GameObject zombieClone;
	public float spawnTime = 5f;
	private bool canSpawnZombie;

	// Use this for initialization
	void Start () {
		canSpawnZombie = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(canSpawnZombie == true) {
			StartCoroutine(SpawnZombie());
		}
	}

	IEnumerator SpawnZombie() {
		canSpawnZombie = false;
		zombieClone = Instantiate(zombie, transform.position, Quaternion.identity) as GameObject;
		zombieClone.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		yield return new WaitForSeconds(spawnTime);
		Debug.Log ("Zombie spawned");
		canSpawnZombie = true;
	}
}
