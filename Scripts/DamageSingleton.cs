using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSingleton : MonoBehaviour {

	private static DamageSingleton instance = null;
    public static DamageSingleton SharedInstance {
        get {
            if (instance == null) {
                instance = new DamageSingleton ();
            }
            return instance;
        }
    }	
    public int bulletDamage = 1;
}   
