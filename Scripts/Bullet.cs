using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	[Tooltip("Переменная урона хранится в скрипте DamageSingleton.cs")]
	public GameObject PlasmeBeam;
	[Range (0f, 500f)]
	public float bulletSpeed;
	Rigidbody bulletRB;
	public GameObject metalHitEffect;
	public GameObject sandHitEffect;
	public GameObject stoneHitEffect;
	public GameObject waterLeakEffect;
    public GameObject waterLeakExtinguishEffect;
	public GameObject[] fleshHitEffects;
	public GameObject woodHitEffect;
	 void Update() {
		bulletRB = PlasmeBeam.GetComponent<Rigidbody>();
		bulletRB.AddForce(transform.forward * bulletSpeed);
		Destroy(gameObject, 3);
	}   
	void OnCollisionEnter(Collision collisionInfo)
	{
	if (collisionInfo.gameObject.tag == "Player"){ 		
		GameObject.Find("solderT").SendMessage("Damage");
		Destroy(gameObject);
		Debug.Log("Player injured!");
	}

	if (collisionInfo.collider.sharedMaterial != null){		
		string materialName = collisionInfo.collider.sharedMaterial.name;
		ContactPoint contact = collisionInfo.contacts[0];
		Vector3 pos = contact.point;
		Quaternion rot = Quaternion.LookRotation(contact.normal);
		
		switch(materialName)
			{
				case "Metal":					
					Instantiate(metalHitEffect, pos, rot);
					Debug.Log("Decal metal");
					break;
				case "Sand":
					Instantiate(sandHitEffect,  pos, rot);
					Debug.Log("Decal sand");
					break;
				case  "Stone":
					Instantiate(stoneHitEffect,  pos, rot);
					break;
				case "WaterFilled":					
					Instantiate(waterLeakEffect,  pos, rot);					
					break;
				case "Wood":					
					Instantiate(woodHitEffect,  pos, rot);
					break;
				case "Meat":
					Instantiate(fleshHitEffects[Random.Range(0, fleshHitEffects.Length)],  pos, rot);					
					break;
				case "Character":
					Instantiate(fleshHitEffects[Random.Range(0, fleshHitEffects.Length)],  pos, rot);
					break;
                case "WaterFilledExtinguish":
					Instantiate(waterLeakExtinguishEffect,  pos, rot);
                    Instantiate(metalHitEffect,  pos, rot);
                    break;
            }
		Destroy(gameObject);
		}
		else
		{
			Destroy(gameObject,5);
		}
	}
}
