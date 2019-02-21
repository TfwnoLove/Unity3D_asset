using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Realtime : MonoBehaviour {
	public Transform DirectionLight;
	void Update()
	{
		System.DateTime time = System.DateTime.Now;
		Vector3 newRotation = DirectionLight.localEulerAngles;
		newRotation.y = 360.0f/12.0f*time.Hour + 360.0f/12.0f/60.0f*time.Minute;
		DirectionLight.localEulerAngles = newRotation;
	}
}
