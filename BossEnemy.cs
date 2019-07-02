using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

	private GameObject bossHealth;

	private int maxHealth;
	private int healthBars;

	private AudioSource slashHit;

	[SerializeField]
	private GameObject explosion;
	GameObject explosionClone;

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
	public bool isInjured;
	private Animator animator;
	private float score;

	// Use this for initialization
	void Awake () {
		bossHealth = GameObject.Find("Enemy UI");
		maxHealth = GetComponent<EnemyHealth>().health;
		SetHealthBars();
		bossHealth.GetComponent<BossPresence>().SetHealth(healthBars);
		isAlive = true;
		canMove = true;
		canAttack = true;
		CleaningUp = false;
		SwitchAI = true;
		isAttacking = false;
		isInjured = false;
		animator = GetComponent<Animator>();
		animator.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 0f));
		StartCoroutine(EnemyIsAttacking());
		slashHit = GameObject.Find("Loud Squish").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		SetHealthBars();
		bossHealth.GetComponent<BossPresence>().SetHealth(healthBars);
		if (GetComponent<EnemyHealth>().health <= 0) {
			bossHealth.GetComponent<BossPresence>().SetHealth(0);
		}

		// Boss does three things: move around and swing like a regular troll (but is much faster and stronger); drop a flame on the ground that goes in two directions,
		// and high jump toward the player, throwing 3 or so shockwaves at the player as he descends.
		// The swing does 4 and the other two deal 3 damage.
		// Zombies and fellow trolls still appear, so watch out for them!

		if (isAlive && canMove && !isAttacking) {
			if(SwitchAI) {
				//SwitchAI = false;
				FacePlayer();
				StartCoroutine(ResetAI(moveTime));
				}
			//animator.SetBool("isWalkingBool", true);
			//animator.SetTrigger("isWalking");
			animator.Play("Walk");
			animator.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * speed, 0f);
			Debug.Log ("Boss Troll is moving");

		}

		if (!isAlive && CleaningUp == false){
			Debug.Log("Boss Troll defeated");
			animator.StopPlayback();
			StartCoroutine(CleanUp());
		}

		if(isAlive && canAttack) {
			MakeAttack();
		}
	}


	void SetHealthBars() {
		healthBars = (GetComponent<EnemyHealth>().health * 16 / maxHealth) + 1;
	}


	void MakeAttack() {
		Vector2 player = GameObject.FindGameObjectWithTag("Player").transform.position;
		if (Mathf.Abs(player.x - transform.position.x) <= 7.5) {
			FacePlayer();
			animator.Play("Attack");
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
		transform.localScale = new Vector3(flipSprite() * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}



	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Kunai" || collider.gameObject.tag == "Sword") {
			slashHit.Play();
			Debug.Log(GetComponent<EnemyHealth>().health);
			Debug.Log(healthBars);
			Debug.Log("Max health is now: " + maxHealth);
			if (GetComponent<EnemyHealth>().health > 0 && isInjured == false) {
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
		animator.Play("Idle");
	}

	void LeftOrRight() {
		int r = Random.Range(0, 2);
		if (r == 0) {
			moveRight = false;
		} else {
			moveRight = true;
		}
	}


	public void Jump() {
		GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * speed * 3, 25);
	}


	void CheckForDeath() {
		if (GetComponent<EnemyHealth>().health <= 0)  {
			GameObject.Find("Ninja").GetComponent<Ninja>().score += GetComponent<EnemyHealth>().value * GameObject.Find("Ninja").GetComponent<Ninja>().pendantBonus();
			GameObject.Find("Ninja").GetComponent<Ninja>().UpdateScoreText();
			isAlive = false;
			StopAllCoroutines();
			animator.StopPlayback();
			GetComponent<BoxCollider2D>().enabled = false;
			//coinClone = Instantiate(coin, new Vector3(transform.position.x, transform.position.y + 2f), Quaternion.identity) as GameObject;
			Destroy(gameObject.GetComponent<Rigidbody2D>());
			StartCoroutine(ExplodeAndDestroy());
		}
	}

	IEnumerator CannotMove(float s) {
		yield return new WaitForSeconds(s);
		canMove = true;
	}

	IEnumerator Injured() {
		isInjured = true;
		canMove = false;
		//StopCoroutine("EnemyIsAttacking");
		Color color = GetComponent<SpriteRenderer>().color;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		animator.Play("Injured");
		renderer.color = new Color(1f, 0f, 0f, 0.2f);
		yield return new WaitForSeconds(0.34f);
		canMove = true;
		canAttack = true;
		isInjured = false;
		renderer.color = color;
		//animator.Play("Idle");
	}

	IEnumerator ExplodeAndDestroy() {
		float timer = 3.0f;
		while (timer > 0) {
			float tempX = Random.Range(-7f, 7f);
			float tempY = Random.Range(-7f, 7f);
			explosionClone = Instantiate(explosion, new Vector3(transform.position.x + tempX, transform.position.y + tempY), Quaternion.identity) as GameObject;
			explosionClone.transform.localScale = new Vector2(20f, 20f);
			explosionClone.transform.parent = transform;
			yield return new WaitForSeconds (0.1f);
			timer -= 0.1f;
		}
		//Destroy(gameObject);
	}

	IEnumerator KnockBack() {
		if (gameObject.GetComponent<Rigidbody2D>() != null) {
			canAttack = false;
			//animator.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * 2000f, 0f);
			//GetComponent<Rigidbody2D>().AddForce(new Vector2(flipSprite() * 2000f, 200f));
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
