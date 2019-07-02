using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public GameObject coin;
	GameObject coinClone;

	private AudioSource slashHit;

	int health;
	public float moveTime = 3.0f;
	public float idleTime = 1.0f;
	public float speed = 10f;
	public bool isAlive;
	public bool canMove;
	public bool moveRight;
	public bool canAttack;
	public bool CleaningUp;
	public bool SwitchAI;
	public bool isAttacking;
	private Animator animator;
	private float score;

	// Use this for initialization
	void Awake () {
		health = GetComponent<EnemyHealth>().health;
		isAlive = true;
		canMove = true;
		canAttack = true;
		CleaningUp = false;
		SwitchAI = true;
		isAttacking = false;
		animator = GetComponent<Animator>();
		animator.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 0f));
		StartCoroutine(EnemyIsAttacking());
		slashHit = GameObject.Find("Loud Squish").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAlive) {
			CheckForDeath();
		}

		if (isAlive && canMove && !isAttacking) {
			if(SwitchAI) {
				SwitchAI = false;
				LeftOrRight();
				transform.localScale = new Vector3(flipSprite() * 1f, 1f, 1f);
				StartCoroutine(ResetAI(moveTime));
				Debug.Log("Changing direction");
				}
			//animator.SetBool("isWalkingBool", true);
			animator.Play("Troll walk");
			animator.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * speed, 0f);
			
		} else if (!isAlive && CleaningUp == false){
			Debug.Log("Troll defeated");
			animator.StopPlayback();
			StartCoroutine(CleanUp());
		}

		if(isAlive && canAttack) {
			MakeAttack();

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
		if(collider.gameObject.tag == "Border") {
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
		}
	}

	public void EnemyDead() {
		Destroy(gameObject);
	}

	public int flipSprite () {
		if (moveRight == true) {
			return 1;
		} else {
			return -1;
		}
	}

	public void ReturnToIdle() {
		animator.Play("Troll idle");
	}

	void LeftOrRight() {
		int r = Random.Range(0, 2);
		if (r == 0) {
			moveRight = false;
		} else {
			moveRight = true;
		}
	}


	void MakeAttack() {
		Vector2 player = GameObject.FindGameObjectWithTag("Player").transform.position;
		if (Mathf.Abs(player.x - transform.position.x) <= 5) {
			FacePlayer();
			animator.Play("Troll attack");
			StartCoroutine(EnemyIsAttacking());
		}
	}

	void FacePlayer() {
		Vector2 player = GameObject.FindGameObjectWithTag("Player").transform.position;
		if (player.x < transform.position.x) {
			moveRight = false;
		} else {
			moveRight = true;
		}
		transform.localScale = new Vector3(flipSprite() * 1f, 1f, 1f);
	}

	void CheckForDeath() {
		if (GetComponent<EnemyHealth>().health <= 0)  {
			GameObject.Find("Ninja").GetComponent<Ninja>().score += GetComponent<EnemyHealth>().value * GameObject.Find("Ninja").GetComponent<Ninja>().pendantBonus();
			GameObject.Find("Ninja").GetComponent<Ninja>().UpdateScoreText();
			isAlive = false;
			StopAllCoroutines();
			animator.StopPlayback();
			GetComponent<BoxCollider2D>().enabled = false;
			coinClone = Instantiate(coin, new Vector3(transform.position.x, transform.position.y + 2f), Quaternion.identity) as GameObject;
			Destroy(gameObject.GetComponent<Rigidbody2D>());
		}
	}

	IEnumerator CannotMove(float s) {
		yield return new WaitForSeconds(s);
		canMove = true;
	}

	IEnumerator Injured() {
		StopCoroutine("EnemyIsAttacking");
		Color color = gameObject.GetComponent<SpriteRenderer>().color;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		canMove = false;
		animator.Play("Injured");
		renderer.color = new Color(1f, 0f, 0f, 0.2f);
		yield return new WaitForSeconds(0.25f);
		renderer.color = color;
		canMove = true;
	}

	IEnumerator KnockBack() {
		if (gameObject.GetComponent<Rigidbody2D>() != null) {
			canAttack = false;
			//animator.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * 2000f, 0f);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(flipSprite() * 2000f, 200f));
			yield return new WaitForSeconds(1f);
			GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
			canAttack = true;
		}
	}

	IEnumerator ResetAI(float x) {
		yield return new WaitForSeconds(x);
		SwitchAI = true;
	}

	IEnumerator EnemyIsAttacking() {
		isAttacking = true;
		canAttack = false;
		Debug.Log ("Troll is attacking");
		yield return new WaitForSeconds(1f);
		canAttack = true;
		isAttacking = false;
	}

	IEnumerator CleanUp() {
		CleaningUp = true;
		yield return new WaitForSeconds(0.1f);
		animator.Play("Death");
		yield return new WaitForSeconds(8f);
		Destroy(gameObject);
	}

}
