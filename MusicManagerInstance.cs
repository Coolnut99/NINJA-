using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerInstance : MonoBehaviour {

	public static MusicManagerInstance instance;

	// Use this for initialization
	void Awake () {
		MakeSingleton();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void MakeSingleton() {
		if(instance != null) {
			Destroy(gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void StopAllSounds() {
		AudioSource[] temp = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach(AudioSource s in temp) {
			s.Stop();
		}
	}

	public void StopTrack(string track) {
		GameObject.Find(track).GetComponent<AudioSource>().Stop();
	}

	public void PlayTrack(string track) {
		GameObject.Find(track).GetComponent<AudioSource>().Play();
	}
}
