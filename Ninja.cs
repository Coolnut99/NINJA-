using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ninja : MonoBehaviour {

	public GameObject kunai;
	public GameObject sword;
	public GameObject slide;
	public GameObject heavySlash;
	GameObject kunaiClone;
	GameObject swordClone;
	GameObject slideClone;
	GameObject heavySlashClone;

	[SerializeField]
	private SpriteRenderer gameOver, gameOverBackground, gamePaused;

	[SerializeField]
	private AudioSource stepWood, throwKunaiSound, jumpSound, injuredSound1, injuredSound2, kiai, doorOpen;

	[SerializeField]
	private AudioSource [] slashSound;

	private Animator animator;
	private Animator weaponChargeAnimator;

	[SerializeField]
	private Text timeText, timeTextShadow;		// UI timer text

	[SerializeField]
	private Text scoreText, scoreTextShadow;

	[SerializeField]
	private Text bronzeKeyText, silverKeyText, goldKeyText;

	[SerializeField]
	private Text kunaiText;

	public bool isAttacking;
	private bool attackLimiter;
	private bool throwingLimiter;
	public float ninjaAttackCooldown = 0.333f; 	// Time allowed between attacks
	public bool moveRight; 						// true = moving right; false = moving left
	public bool inTheAir; 						// Do not animate running motion if character is in the air
	public bool canMove; 						// Can you move the ninja? (You can't for a brief moment if, say, you are attacking or jumping.)
	public bool isSliding;						// Is the ninja sliding?
	public bool canSlide;
	public bool isAlive;
	public bool airJump;
	public bool isInvulnerable;
	public bool notStunned;
	public bool isCrouching;
	public bool gameIsOver;
	public bool onSolidGround;					// Ninja is on a "solid" platform 
	public int health;
	public int timeLeft;
	public int kunaiCount;
	private float canMoveCooldown; 				// Length of time before you can move (usually 0.1 or 0.2 seconds)
	private float speed;
	private float fastSpeed;
	public float score;
	private Rigidbody2D myRigidBody;
	private BoxCollider2D boxCollider2D;
	private NinjaHealth ninjaHealth;

	private Slider weaponCharge;

	public enum collectPowerup {none, shield, hammer, redPendant, bluePendant, bluePotion, redPotion, pot, scroll};
	public collectPowerup powerupType;

	void Awake() {
		score = GameObject.Find("Game Controller").GetComponent<GameController>().GetScore();
		bronzeKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().bronzeKeys.ToString();
		silverKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().silverKeys.ToString();
		goldKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().goldKeys.ToString();
		GameObject.Find("Game Controller").GetComponent<GameController>().gameStarted = true;
	}

	// Use this for initialization
	void Start () {
		timeLeft = 999;
		kunaiCount = GameObject.Find("Game Controller").GetComponent<GameController>().kunaiCount;
		animator = GetComponent<Animator>();
		weaponChargeAnimator = GameObject.Find("Weapon Charge Bar").GetComponent<Animator>();
		attackLimiter = true;
		throwingLimiter = true;
		moveRight = true;
		canMove = true;
		inTheAir = false;
		isSliding = false;
		canSlide = true;
		isAlive = true;
		airJump = true;
		isInvulnerable = false;
		notStunned = true;
		isCrouching = false;
		gameIsOver = false;
		onSolidGround = true;
		gameOver.gameObject.SetActive(false);
		gameOverBackground.gameObject.SetActive(false);
		gamePaused.gameObject.SetActive(false);
		if (GameObject.Find("Game Controller").GetComponent<GameController>().keepPowerup == true) {
			GameObject.Find("Game Controller").GetComponent<GameController>().keepPowerup = false;
		} else {
			GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
		}
		speed = 0.2f;
		fastSpeed = 0.5f;
		myRigidBody = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		ninjaHealth = GetComponent<NinjaHealth>();
		ninjaHealth.SetHealth(health);
		weaponCharge = GameObject.Find("Weapon Charge").GetComponent<Slider>();
		weaponCharge.value = 0f;
		scoreText.text = score.ToString();
		scoreTextShadow.text = score.ToString();
		StartCoroutine(TickingTime());
		if (GameObject.Find("Game Controller").GetComponent<GameController>().nextLevel == 1) {
			GameObject.Find("Music Manager").GetComponent<MusicManagerInstance>().PlayTrack("Level 1 revised");
		} else {
			GameObject.Find("Music Manager").GetComponent<MusicManagerInstance>().PlayTrack("Level 2");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Quits app
		if(Input.GetKeyDown(KeyCode.Q)) {
			Application.Quit();
		}

		//Pauses app
		if(Input.GetKeyDown(KeyCode.P)) {
			if(Time.timeScale == 1f) {
				gameOverBackground.gameObject.SetActive(true);
				gamePaused.gameObject.SetActive(true);
				Time.timeScale = 0f;
			} else if (Time.timeScale == 0f) {
				gameOverBackground.gameObject.SetActive(false);
				gamePaused.gameObject.SetActive(false);
				Time.timeScale = 1f;
			}
		}

		if(Input.GetAxis("Vertical") <= -0.2f) {
			isCrouching = true;
		} else {
			isCrouching = false;
		}
		/*
		if(inTheAir == true) {
			GetComponent<Rigidbody2D>().isKinematic = true;
		} else {
			GetComponent<Rigidbody2D>().isKinematic = false;
		}*/

		float h = Input.GetAxis("Horizontal");
		if (animator.GetBool("isRunning") == false && h == 0) {
			animator.StopPlayback();
		}
		if (isAlive && notStunned) {
		//Moving -- Only run if player is on the ground.
		if (canMove && isCrouching == false) {
			if (h > 0 && inTheAir == false) {
				moveRight = true;
				animator.gameObject.transform.localScale = new Vector3(flipSprite(), 1, 1);
				animator.SetBool("isRunning", true);
				animator.gameObject.transform.position += new Vector3(speed * flipSprite() * Time.timeScale, 0, 0);
			} else if (h < 0 && inTheAir == false) {
				moveRight = false;
				animator.gameObject.transform.localScale = new Vector3(flipSprite(), 1, 1);
				animator.SetBool("isRunning", true);
				animator.gameObject.transform.position += new Vector3(speed * flipSprite() * Time.timeScale, 0, 0);
			} else if (h > 0 && inTheAir == true) {
				moveRight = true;
				animator.gameObject.transform.localScale = new Vector3(flipSprite(), 1, 1);
				animator.SetBool("isRunning", false);
				animator.gameObject.transform.position += new Vector3(speed * flipSprite() * Time.timeScale, 0, 0);
			} else if (h < 0 && inTheAir == true) {
				moveRight = false;
				animator.gameObject.transform.localScale = new Vector3(flipSprite(), 1, 1);
				animator.SetBool("isRunning", false);
				animator.gameObject.transform.position += new Vector3(speed * flipSprite() * Time.timeScale, 0, 0);
			} else {
				animator.SetBool("isRunning", false);
			}
		}


		//Attacking
		if (Input.GetKeyDown(KeyCode.Return) && attackLimiter == true && isCrouching == false && weaponCharge.value < 1) {
			animator.StopPlayback();
			animator.SetBool("isRunning", false);
			animator.SetTrigger("isAttacking trigger");
			weaponCharge.value = 0;
			StartCoroutine("AttackLimiter");
		}

		//Charged Attack -- only use when attack meter is fully charged
			if (Input.GetKeyDown(KeyCode.Return) && attackLimiter == true && isCrouching == false && weaponCharge.value >= 1) {
			animator.StopPlayback();
			animator.SetBool("isRunning", false);
			animator.SetTrigger("isAttacking trigger");
			weaponCharge.value = 0;
			StartCoroutine("PowerAttackLimiter");
		}

		//Death (fake death?)
		if(Input.GetKeyDown(KeyCode.X)) {
			animator.SetBool("isRunning", false);
			animator.SetTrigger("isDead trigger");
			isAlive = false;
			Debug.Log("YOU HAVE DIED! Press 'R' to revive.");
		}

		//Jumping
			if (Input.GetAxis("Vertical") >= -0.2f && Input.GetAxis("Vertical") <= 0.2f && Input.GetKeyDown(KeyCode.Space) && inTheAir == false && airJump == false) {
			animator.SetBool("isRunning", false);
			animator.SetTrigger("Jump trigger");
			inTheAir = true;
			onSolidGround = false; // TEST -- this seems to be working
			myRigidBody.velocity = new Vector2(0, 20f);
			jumpSound.Play();
			StartCoroutine(SingleToDoubleJumpWait());
			Debug.Log("Jumping");
		}

		// High Jumping
		if (Input.GetAxis("Vertical") > 0.2f && Input.GetKeyDown(KeyCode.Space) && inTheAir == false && airJump == false) {
			animator.SetBool("isRunning", false);
			animator.SetTrigger("Jump trigger");
			inTheAir = true;
			myRigidBody.velocity = new Vector2(0, 30f);
			jumpSound.Play();
			StartCoroutine(SingleToDoubleJumpWait());
			Debug.Log("High Jumping");
		}

		// Double Jumping
		if (Input.GetAxis("Vertical") >= -0.2f && Input.GetKeyDown(KeyCode.Space) && airJump == true) {
			animator.SetTrigger("Jump trigger");
			inTheAir = true;
			airJump = false;
			myRigidBody.velocity = new Vector2(0, 20f);
			jumpSound.Play();
			Debug.Log("Double Jumping");
		}

		//Lay down/crouching
		if (Input.GetAxis("Vertical") < -0.2f && inTheAir == false && canSlide == true) {
			animator.SetBool("isRunning", false);
			animator.Play("Slide Idle");
		}

		//Jumping down -- temporary for now
		if(Input.GetKeyDown(KeyCode.Z) && inTheAir == false && onSolidGround == false) {
			GetComponent<BoxCollider2D>().isTrigger = true;
		}


		//Sliding
		if (Input.GetAxis("Vertical") < -0.2f && Input.GetKeyDown(KeyCode.Space) && inTheAir == false && canSlide == true) {
			animator.SetBool("isRunning", false);
			animator.SetTrigger("Sliding trigger");
			StartCoroutine("Sliding");
		}

		//Throwing
			if (Input.GetKeyDown(KeyCode.T) && kunaiCount > 0 && isCrouching == false && throwingLimiter == true) {
			kunaiCount--;
			UpdateKunaiText();
			animator.SetBool("isRunning", false);
			animator.SetTrigger("Throwing trigger");
			StartCoroutine(ThrowingLimiter());
		}
		} // End of isAlive functions


		//Revives player
		if (Input.GetKeyDown(KeyCode.R) && isAlive == false) {
			inTheAir = false;
			isAlive = true;
			health = 5;
			animator.Play("Idle");
		}

		if (weaponCharge.value < 1) {
			weaponCharge.value += Time.deltaTime / chargeTime();
			weaponChargeAnimator.Play("sword fill idle");
		} else {
			weaponChargeAnimator.Play("sword fill anim");
		}

	}

	public int flipSprite () {
		if (moveRight == true) {
			return 1;
		} else {
			return -1;
		}
	}

	public float pendantBonus () {
		if (powerupType == Ninja.collectPowerup.bluePendant) {
			return 2.0f;
		} else {
			return 1.0f;
		}
	}

	public float chargeTime() {
		if (powerupType == Ninja.collectPowerup.bluePotion) {
			return 1.1f;
		} else {
			return 1.7f;
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "Ground" || collider.tag == "Solid Ground") {
				if(animator.GetBool("isRunning") == false && isSliding == false && isAlive == true) {
				Debug.Log("Player is on the ground.");
				inTheAir = false;
				airJump = false;
				animator.Play("Idle");
			}
		}

		if (collider.tag == "Enemy Attack" && isInvulnerable == false && isAlive == true) {
			Debug.Log("Collision");
			StartCoroutine(isInjured());
			StartCoroutine(StunPlayer(0.4f));
			if (collider.gameObject.GetComponent<EnemyDamage>() != null) {
				health -= collider.gameObject.GetComponent<EnemyDamage>().damage;
				int r = Random.Range(0, 2);
				if(r == 0) {
					injuredSound1.Play();
				} else if (r == 1) {
					injuredSound2.Play();
				}
			} else {
				Debug.LogError("Enemy damage not found.");
			}
			ninjaHealth.SetHealth(health);

			if (health <= 0) {
				PlayerDead();
				StartCoroutine(GameOverFool());
			}
		}

		if (collider.tag == "Bonus") {
			if (collider.gameObject.GetComponent<Coin>() != null) {
				score += collider.gameObject.GetComponent<Coin>().value * pendantBonus();
				UpdateScoreText();
			}
		}

		if (collider.tag == "Solid Ground") {
			onSolidGround = true;
			GetComponent<BoxCollider2D>().isTrigger = false;
		}

		if (collider.tag == "Ground") {
			onSolidGround = false;
		}

		if (collider.tag == "Item") {
			if(collider.gameObject.GetComponent<Key>() != null) {
				if(collider.gameObject.GetComponent<Key>().keyType == Key.collectKey.bronze) {
					GameObject.Find("Game Controller").GetComponent<GameController>().bronzeKeys++;
					bronzeKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().bronzeKeys.ToString();
				}

				if(collider.gameObject.GetComponent<Key>().keyType == Key.collectKey.silver) {
					GameObject.Find("Game Controller").GetComponent<GameController>().silverKeys++;
					silverKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().silverKeys.ToString();
				}

				if(collider.gameObject.GetComponent<Key>().keyType == Key.collectKey.gold) {
					GameObject.Find("Game Controller").GetComponent<GameController>().goldKeys++;
					goldKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().goldKeys.ToString();
				}
			} 

		}

		if (collider.tag == "Powerup") {
			GameObject.Find("Game Controller").GetComponent<GameController>().keepPowerup = true;
			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.shield) {
				// Set shield to true and all others false
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Shield Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.shield;
			}

			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.bluePendant) {
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Blue Crystal Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.bluePendant;
			}

			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.redPendant) {
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Red Crystal Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.redPendant;
			}

			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.hammer) {
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Hammer Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.hammer;
			}

			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.bluePotion) {
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Blue Potion Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.bluePotion;
			}

			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.redPotion) {
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Red Potion Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.redPotion;
			}

			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.scroll) {
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Scroll Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.scroll;
			}

			if(collider.gameObject.GetComponent<Powerup>().powerupType == Powerup.collectPowerup.pot) {
				GameObject.Find("Game Controller").GetComponent<GameController>().ResetPowerups();
				GameObject.Find("Pot Powerup").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				powerupType = collectPowerup.pot;
			}
		}

		if (collider.tag == "Extra Kunai") {
			kunaiCount += 20;
			UpdateKunaiText();
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Enemy" && isInvulnerable == true) {
			Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.collider);
		}
	}

	void OnCollisionStay2D(Collision2D collision) {
		if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Solid Ground") {
			inTheAir = false;
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Solid Ground") {
			inTheAir = true;
			onSolidGround = false;
			Debug.Log("In the air.");
		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		if (collider.gameObject.tag == "Door" && Input.GetKey(KeyCode.UpArrow)) {
			if(collider.gameObject.GetComponent<DoorLock>().doorLockType == DoorLock.doorLock.bronze && GameObject.Find("Game Controller").GetComponent<GameController>().bronzeKeys >= 1)	{
				collider.gameObject.GetComponent<Animator>().Play("Door open");
				Debug.Log("Bronze Door is opened");
				GameObject.Find("Game Controller").GetComponent<GameController>().bronzeKeys--;
				bronzeKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().bronzeKeys.ToString();
				collider.GetComponent<DoorLock>().DoorPrize();
				doorOpen.Play();
				collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0f, 0f);
			} else if (collider.gameObject.GetComponent<DoorLock>().doorLockType == DoorLock.doorLock.silver && GameObject.Find("Game Controller").GetComponent<GameController>().silverKeys >= 1) {
				collider.gameObject.GetComponent<Animator>().Play("Door open");
				Debug.Log("Silver Door is opened");
				GameObject.Find("Game Controller").GetComponent<GameController>().silverKeys--;
				silverKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().silverKeys.ToString();
				collider.GetComponent<DoorLock>().DoorPrize();
				doorOpen.Play();
				collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0f, 0f);
			} else if (collider.gameObject.GetComponent<DoorLock>().doorLockType == DoorLock.doorLock.gold && GameObject.Find("Game Controller").GetComponent<GameController>().goldKeys >= 1) {
				collider.gameObject.GetComponent<Animator>().Play("Door open");
				Debug.Log("Gold Door is opened");
				GameObject.Find("Game Controller").GetComponent<GameController>().goldKeys--;
				goldKeyText.text = GameObject.Find("Game Controller").GetComponent<GameController>().goldKeys.ToString();
				collider.GetComponent<DoorLock>().DoorPrize();
				doorOpen.Play();
				collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0f, 0f);
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		GetComponent<BoxCollider2D>().isTrigger = false;
	}

	void PlayerDead() {
		animator.SetBool("isRunning", false);
		animator.SetTrigger("isDead trigger");
		isAlive = false;
		Debug.Log("YOU HAVE DIED! Press 'R' to revive.");
	}

	void Step() {
		stepWood.Play();
	}

	public void UpdateScoreText() {
		scoreText.text = score.ToString();
		scoreTextShadow.text = score.ToString();
	}

	public void UpdateKunaiText() {
		kunaiText.text = kunaiCount.ToString();
	}

	IEnumerator AttackLimiter() {
		canMove = false;
		attackLimiter = false;
		swordClone = Instantiate(sword, new Vector3((transform.position.x + (flipSprite() * 3f)), transform.position.y + 1.75f, 0f), Quaternion.identity) as GameObject;
		swordClone.transform.localScale = new Vector3(flipSprite(), 1f);
		swordClone.transform.parent = transform;
		swordClone.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -20f);
		int x = Random.Range(0, slashSound.Length);
		slashSound[x].Play();
		yield return new WaitForSeconds(ninjaAttackCooldown);
		attackLimiter = true;
		canMove = true;
	}

	IEnumerator PowerAttackLimiter() {
		canMove = false;
		attackLimiter = false;
		heavySlashClone = Instantiate(heavySlash, new Vector3((transform.position.x + (flipSprite() * 3f)), transform.position.y, 0f), Quaternion.identity) as GameObject;
		heavySlashClone.transform.localScale = new Vector3(flipSprite()*2f, 3f);
		heavySlashClone.transform.parent = transform;
		heavySlashClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(flipSprite() * 3000f, 0));
		int x = Random.Range(0, slashSound.Length);
		slashSound[x].Play();
		kiai.Play();
		yield return new WaitForSeconds(ninjaAttackCooldown);
		attackLimiter = true;
		canMove = true;
	}

	IEnumerator CannotMove(float s) {
		canMove = false;
		yield return new WaitForSeconds(s);
		canMove = true;
	}

	IEnumerator StunPlayer(float s) {
		canMove = false;
		notStunned = false;
		yield return new WaitForSeconds(s);
		notStunned = true;
		canMove = true;
	}

	IEnumerator ThrowingLimiter() {
		canMove = false;
		throwingLimiter = false;
		yield return new WaitForSeconds(0.1f);
		throwKunaiSound.Play();
		kunaiClone = Instantiate(kunai, transform.position, Quaternion.identity) as GameObject;
		kunaiClone.transform.localScale = new Vector3(flipSprite() * 2, 2);
		kunaiClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(flipSprite() * 2000f, 0));
		yield return new WaitForSeconds(0.1f);
		throwingLimiter = true;
		canMove = true;
	}

	IEnumerator Sliding() {
		canMove = false;
		canSlide = false;
		isSliding = true;
		animator.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * flipSprite() *180f, 0);
		slideClone = Instantiate(slide, new Vector3((transform.position.x + (flipSprite() * 0.6f)), transform.position.y - 1.85f, 0f), Quaternion.identity) as GameObject;
		slideClone.transform.localScale = new Vector3(flipSprite()*1, 1);
		slideClone.transform.parent = transform;
		yield return new WaitForSeconds (0.25f);
		animator.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
		animator.Play("Idle");
		canMove = true;
		canSlide = true;
		isSliding = false;
	}

	IEnumerator SingleToDoubleJumpWait() {
		airJump = false;
		GetComponent<BoxCollider2D>().isTrigger = true; // TEST -- this seems to be working
		yield return new WaitForSeconds(0.5f);
		airJump = true;
	}

	IEnumerator isInjured() {
		Color color = gameObject.GetComponent<SpriteRenderer>().color;
		isInvulnerable = true;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.color = new Color(1f, 0f, 0f, 0.2f);
		yield return new WaitForSeconds(1f);
		renderer.color = color;
		isInvulnerable = false;
	}

	IEnumerator TickingTime() {
		while(timeLeft > 0) {
			timeLeft --;
			timeText.text = timeLeft.ToString();
			timeTextShadow.text = timeLeft.ToString();
			if (timeLeft < 100) {
				timeText.color = new Color(1f, 0f, 0f, 1f);
			}
			yield return new WaitForSeconds(0.17f);
		}
		while (health > 0) {
			health--;
			ninjaHealth.SetHealth(health);
			if (health <= 0) {
				PlayerDead();
				StartCoroutine(GameOverFool());
			}
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator GameOverFool() {
		gameOver.gameObject.SetActive(true);
		gameOverBackground.gameObject.SetActive(true);
		yield return new WaitForSeconds(3f);
		Application.LoadLevel(0);
	}
}
