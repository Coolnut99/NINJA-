using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusController : MonoBehaviour {

	[SerializeField]
	private Text vitalityText, vitalityTextBonus, timeText, timeTextBonus, keyTextBonus, totalBonusText, totalBonusTextBonus, goToNextStage;

	[SerializeField]
	private SpriteRenderer bronzeKey, silverKey, goldKey;

	private bool enterNextStage;

	private float vitalityBonus, timeBonus, bronzeKeyBonus, silverKeyBonus, goldKeyBonus, totalBonus;

	Color blank = new Color (0f, 0f, 0f, 0f);
	Color reveal = new Color (1f, 1f, 0f, 1f);

	void Awake() {
		Time.timeScale = 1f;
		vitalityText.color = blank;
		vitalityTextBonus.color = blank;
		timeText.color = blank;
		timeTextBonus.color = blank;
		bronzeKey.color = blank;
		silverKey.color = blank;
		goldKey.color = blank;
		keyTextBonus.color = blank;
		totalBonusText.color = blank;
		totalBonusTextBonus.color = blank;
		goToNextStage.color = blank;
		enterNextStage = false;
	}

	// Use this for initialization
	void Start () {
		GameObject.Find("Music Manager").GetComponent<MusicManagerInstance>().StopAllSounds();
		GameObject.Find("Music Manager").GetComponent<MusicManagerInstance>().PlayTrack("Victory");
		vitalityBonus =  (float)GameObject.Find("Game Controller").GetComponent<GameController>().vitalityLeft * 640f;
		timeBonus = (float)GameObject.Find("Game Controller").GetComponent<GameController>().timeLeft * 10f;
		bronzeKeyBonus = (float)GameObject.Find("Game Controller").GetComponent<GameController>().bronzeKeys * 800f;
		silverKeyBonus = (float)GameObject.Find("Game Controller").GetComponent<GameController>().silverKeys * 2000f;
		goldKeyBonus = (float)GameObject.Find("Game Controller").GetComponent<GameController>().goldKeys * 4000f;
		totalBonus = vitalityBonus + timeBonus + bronzeKeyBonus + silverKeyBonus + goldKeyBonus;
		StartCoroutine("ShowBonus");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown == true && enterNextStage == false) {
			StopCoroutine("ShowBonus");
			RevealAll();
			Debug.Log("Key pressed!");
			enterNextStage = true;
		} else if (Input.anyKeyDown == true && enterNextStage == true) {
			GameObject.Find("Game Controller").GetComponent<GameController>().AddScore(totalBonus);
			Debug.Log(GameObject.Find("Game Controller").GetComponent<GameController>().GetScore());
			Application.LoadLevel(GameObject.Find("Game Controller").GetComponent<GameController>().nextLevel);
		}
	}

	IEnumerator ShowBonus() {
		vitalityText.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		vitalityTextBonus.text = GameObject.Find("Game Controller").GetComponent<GameController>().vitalityLeft.ToString() + " x 640 = " + vitalityBonus.ToString();
		vitalityTextBonus.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		timeText.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		timeTextBonus.text = GameObject.Find("Game Controller").GetComponent<GameController>().timeLeft.ToString() + " x 10 = " + timeBonus.ToString();
		timeTextBonus.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		bronzeKey.color = reveal;
		silverKey.color = reveal;
		goldKey.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		keyTextBonus.text = bronzeKeyBonus.ToString() + " + " + silverKeyBonus.ToString() + " + " + goldKeyBonus.ToString();
		keyTextBonus.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		totalBonusText.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		totalBonusTextBonus.text = totalBonus.ToString();
		totalBonusTextBonus.color = reveal;
		yield return new WaitForSecondsRealtime(0.5f);
		goToNextStage.color = reveal;
		Debug.Log("OK to enter next stage!");
		enterNextStage = true;
	}

	void RevealAll() {
		vitalityText.color = reveal;
		vitalityTextBonus.text = GameObject.Find("Game Controller").GetComponent<GameController>().vitalityLeft.ToString() + " x 640 = " + vitalityBonus.ToString();
		vitalityTextBonus.color = reveal;
		timeText.color = reveal;
		timeTextBonus.text = GameObject.Find("Game Controller").GetComponent<GameController>().timeLeft.ToString() + " x 10 = " + timeBonus.ToString();
		timeTextBonus.color = reveal;
		bronzeKey.color = reveal;
		silverKey.color = reveal;
		goldKey.color = reveal;
		keyTextBonus.text = bronzeKeyBonus.ToString() + " + " + silverKeyBonus.ToString() + " + " + goldKeyBonus.ToString();
		keyTextBonus.color = reveal;
		totalBonusText.color = reveal;
		totalBonusTextBonus.text = totalBonus.ToString();
		totalBonusTextBonus.color = reveal;
		goToNextStage.color = reveal;
	}
}
