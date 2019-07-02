using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHead : MonoBehaviour {

	private AudioSource slashHit;

	int health;
	public float moveTime = 3.0f;
	public float idleTime = 1.0f;
	public float speed = 10f;
	public float flySpeed = 10f;
	public bool isAlive;
	public bool canMove;
	public bool moveRight;
	public bool canAttack;
	public bool CleaningUp;
	public bool SwitchAI;
	public bool isAttacking;
	public bool changeFlyingDirection;
	private Animator animator;
	private float score;
	private float distanceTraveled;

	// Use this for initialization
	void Awake () {
		health = GetComponent<EnemyHealth>().health;
		isAlive = true;
		canMove = true;
		canAttack = true;
		CleaningUp = false;
		SwitchAI = true;
		isAttacking = false;
		changeFlyingDirection = true;
		animator = GetComponent<Animator>();
		slashHit = GameObject.Find("Loud Squish").GetComponent<AudioSource>();
		MoveTowardsPlayer();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAlive) {
			CheckForDeath();
			float tempX = transform.position.x + speed * Time.deltaTime;
			distanceTraveled += (tempX - transform.position.x);
			float tempY = transform.position.y + flySpeed * Time.deltaTime * Mathf.Cos(distanceTraveled / Mathf.PI);
			transform.position = new Vector2(tempX, tempY);
		}

		if (!isAlive && CleaningUp == false){
			Debug.Log("Skull defeated");
			animator.StopPlayback();
			StartCoroutine(CleanUp());
		}
	}


	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Kunai" || collider.gameObject.tag == "Sword") {
			slashHit.Play();
			if (GetComponent<EnemyHealth>().health > 0) {
				StartCoroutine(Injured());
			}
			CheckForDeath();
		} 
	}

	public void EnemyDead() {
		Destroy(gameObject);
	}

	void CheckForDeath() {
		if (GetComponent<EnemyHealth>().health <= 0)  {
			GameObject.Find("Ninja").GetComponent<Ninja>().score += GetComponent<EnemyHealth>().value * GameObject.Find("Ninja").GetComponent<Ninja>().pendantBonus();
			GameObject.Find("Ninja").GetComponent<Ninja>().UpdateScoreText();
			isAlive = false;
			animator.StopPlayback();
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	void MoveTowardsPlayer() {
		Vector3 temp = GameObject.Find("Ninja").transform.position;
		if (temp.x < transform.position.x) {
			speed = -speed;
			transform.localScale = new Vector3(-2f, 2f, 2f);
		} else {
			transform.localScale = new Vector3(2f, 2f, 2f);
		}
		GetComponent<SpriteRenderer>().flipX = true;
	}


	IEnumerator Injured() {
		StopCoroutine("EnemyIsAttacking");
		Color color = gameObject.GetComponent<SpriteRenderer>().color;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		canMove = false;
		//animator.Play("Injured");
		renderer.color = new Color(1f, 0f, 0f, 0.2f);
		yield return new WaitForSeconds(0.25f);
		renderer.color = color;
		canMove = true;
	}

	IEnumerator CleanUp() {
		CleaningUp = true;
		yield return new WaitForSeconds(0.1f);
		animator.Play("Death");
		yield return new WaitForSeconds(8f);
		Destroy(gameObject);
	}
}
