using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeExplode : MonoBehaviour {

	[SerializeField]
	private GameObject bridge1, bridge2, bridge3, bridge4;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Player") {
			StartCoroutine(DestroyBridges());
			GetComponent<BoxCollider2D>().size = new Vector2(0f, 0f);
		}	
	}

	IEnumerator DestroyBridges() {
		//Detonate bridge4, then 3, then 2, then 1
		bridge4.GetComponent<ExplodingBridge>().DestroyBridge();
		yield return new WaitForSeconds(1.1f);
		bridge3.GetComponent<ExplodingBridge>().DestroyBridge();
		yield return new WaitForSeconds(1.1f);
		bridge2.GetComponent<ExplodingBridge>().DestroyBridge();
		yield return new WaitForSeconds(1.1f);
		bridge1.GetComponent<ExplodingBridge>().DestroyBridge();
		yield return new WaitForSeconds(1.1f);

	}
}
