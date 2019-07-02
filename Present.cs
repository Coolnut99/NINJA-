using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour {

	[SerializeField]
	private GameObject item;
	GameObject itemClone;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Sword"){
			GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 25f);
			animator.Play("Present open");
		}
	}

	void OpenPresent() {
		itemClone = Instantiate(item, transform.position, Quaternion.identity) as GameObject;
		itemClone.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 1f);
		Destroy(gameObject);
	}

}
