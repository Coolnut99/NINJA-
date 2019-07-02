using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour {

	public enum doorLock {bronze, silver, gold};
	public doorLock doorLockType;

	[SerializeField]
	private GameObject [] commonItems, rareItems;
	GameObject itemSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Get an item for opening the door. either a commonItems item or, if you have the red pendant, a rareItems item.
	// Note that these can instantiate traps, too, so be careful!
	public void DoorPrize() {
		if (GameObject.Find("Ninja").GetComponent<Ninja>().powerupType == Ninja.collectPowerup.redPendant) {
			//Rare item
			int x = Random.Range(0, rareItems.Length);
			itemSpawn = Instantiate (rareItems[x], new Vector3(transform.position.x, transform.position.y + 10f, 0f), Quaternion.identity) as GameObject;
			//itemSpawn.transform.parent = transform;
		} else {
			//Common item
			int x = Random.Range(0, commonItems.Length);
			itemSpawn = Instantiate (commonItems[x], new Vector3(transform.position.x, transform.position.y + 10f, 0f), Quaternion.identity) as GameObject;
			//itemSpawn.transform.parent = transform;
		}
	}
}
