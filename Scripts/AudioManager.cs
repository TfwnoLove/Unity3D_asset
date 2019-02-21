using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	[Tooltip("Squat sound")]
	public AudioClip SitDown;
	public AudioClip LowHealth;
	//new AudioSource audio;
	 AudioSource audioData;
	void Awake()
	{
		audioData = GetComponent<AudioSource>(); //Fill the field audio
	}
	public void Play (string name){
		switch(name){
			case "Sit down":
			audioData.PlayOneShot(SitDown);
			break;
			case "Low_health":			
			audioData.loop= true;
			audioData.clip = LowHealth;
			audioData.Play();
			break;
		}
	}
	public void Stop (string name){
		switch(name){
			case "Sit down":
			audioData.PlayOneShot(SitDown);
			break;
			case "Low_health":
			audioData.clip = LowHealth;
			audioData.Stop();
			break;
		}
	}
}
