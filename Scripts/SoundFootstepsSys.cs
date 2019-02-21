using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFootstepsSys : MonoBehaviour {

public Animator animator;
public AudioSource audioLeftSource, audioRightSource;
public float CheckRay = 1.0f; 
[Header("Beam sending point")]
public Transform LeftFoot;
public Transform RightFoot;
[Header("Position correction")]
public float footprintOffset = 0.05f;
[Header("Pitch effects")]
public GameObject sandFootstepEffect;
public GameObject l_sandFootprintEffect;
public GameObject r_sandFootprintEffect;

public GameObject waterLeakEffect;
[Header("Sounds of steps for the left leg")]
public AudioClip l_sandFootstepSound;	
public AudioClip l_metalFootstepSound;
public AudioClip l_stoneFootstepSound;
public AudioClip l_waterFootstepSound;
public AudioClip l_woodFootstepSound;	

[Header("Sounds of steps for the right leg")]
public AudioClip r_sandFootstepSound;	
public AudioClip r_metalFootstepSound;
public AudioClip r_stoneFootstepSound;
public AudioClip r_waterFootstepSound;
public AudioClip r_woodFootstepSound;	

	public	void LeftFootstepSand()
		{
		Vector3 rayOrigin = LeftFoot.position;
			RaycastHit hit;
			if (Physics.Raycast(rayOrigin, LeftFoot.up, out hit, CheckRay))
			{
				//Debug.DrawLine(LeftFoot.transform.position,hit.point, Color.red);				
				if(hit.collider.sharedMaterial != null){
					string materialName = hit.collider.sharedMaterial.name;
					switch(materialName)
					{
						case "Metal":
							audioLeftSource.PlayOneShot(l_metalFootstepSound);
							break;
						case "Sand":
							SpawnDecal(hit, sandFootstepEffect);
							SpawnDecal(hit, l_sandFootprintEffect);
							audioLeftSource.PlayOneShot(l_sandFootstepSound);
							break;
						case  "Stone":
							audioLeftSource.PlayOneShot(l_stoneFootstepSound);
							break;
						case "WaterFilled":
							SpawnDecal(hit, waterLeakEffect);
							audioLeftSource.PlayOneShot(l_waterFootstepSound);
							break;
						case "Wood":
							audioLeftSource.PlayOneShot(l_woodFootstepSound);
							break;
						case "Meat":
							//audioLeftSource.PlayOneShot(meatFootstepSound);
							break;
					}
				}
			}
		}
	public	void RightFootstepSand()
		{
		Vector3 rayOrigin = RightFoot.position;
			RaycastHit hit;
			if (Physics.Raycast(rayOrigin, RightFoot.up, out hit, CheckRay))
			{
				//Debug.DrawLine(RightFoot.transform.position,hit.point, Color.red);				
				if(hit.collider.sharedMaterial != null){
					string materialName = hit.collider.sharedMaterial.name;
					switch(materialName)
					{
						case "Metal":
							audioRightSource.PlayOneShot(r_metalFootstepSound);
							break;
						case "Sand":
							SpawnDecal(hit, sandFootstepEffect);
							SpawnDecal(hit, r_sandFootprintEffect);
							audioRightSource.PlayOneShot(r_sandFootstepSound);
							break;
						case  "Stone":
							audioRightSource.PlayOneShot(r_stoneFootstepSound);
							break;
						case "WaterFilled":
							SpawnDecal(hit, waterLeakEffect);
							audioRightSource.PlayOneShot(r_waterFootstepSound);
							break;
						case "Wood":
							audioRightSource.PlayOneShot(r_woodFootstepSound);
							break;
						case "Meat":
							//audioLeftSource.PlayOneShot(meatFootstepSound);
							break;
					}
				}
			}
		}
	void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point+hit.normal*footprintOffset, Quaternion.LookRotation(hit.normal));
		//spawnedDecal.transform.SetParent(hit.collider.transform);
	}

}
