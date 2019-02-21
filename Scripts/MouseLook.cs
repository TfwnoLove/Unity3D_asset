using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
float vertical;
public float minY, maxY;
public Transform PivotTarget;
public float sensitivityCam =80;
    void FixedUpdate()
    {       
     vertical += Input.GetAxis("Mouse Y") * sensitivityCam * Time.deltaTime;
     vertical = Mathf.Clamp(vertical, minY, maxY);
     PivotTarget.localEulerAngles = new Vector3(-vertical,  PivotTarget.localEulerAngles.z);
	}
}
