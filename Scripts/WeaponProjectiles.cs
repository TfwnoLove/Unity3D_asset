using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponProjectiles : MonoBehaviour {
	public float fireRate = 0.5f;
	float lastShot = 0.0f;
	public int ammo = 30;
	public float distWallCheck = 0.14f;	
	public Transform nullPoint;
	public Transform firePoint;
	public Transform fireTarget;
	public GameObject bulletPref;	
	public KeyCode Shoot;
	public KeyCode Reload;
	public bool debugLine = false;
	bool canShoot = true;	
	public Animator animator; 	
	public CharacterStatus characterStatus;

	public AudioClip vois;
	new AudioSource audio;

	public ParticleSystem muzzleParticle;
	


	public Text Ammo;	
	void Awake()
	{
		Ammo.text = "";
		audio = GetComponent<AudioSource>();
	}
	void LateUpdate () {
		
		if(debugLine == true){
		Vector3 end = fireTarget.position;
		Debug.DrawLine(gameObject.transform.position,end, Color.black);}
		}
	void TargetPosition(){
		RaycastHit hit;
        if (Physics.Raycast(firePoint.transform.position, transform.TransformDirection(Vector3.up),out hit, 1000)) {
            fireTarget.position = hit.point;
            Debug.Log("Collision with "+ hit.collider.gameObject.name);
        }
	}
	
	void FixedUpdate()
	{
		Ammo.text = ammo +"";
		WallCheck();
		//TargetPosition();
		if (Input.GetKey(Shoot) && ammo !=0 &&  canShoot == true ){			
			Fire();			
		}
		if (Input.GetKeyUp(Shoot) || ammo ==0 ) {
		characterStatus.isGunplay = false;	
		animator.SetBool("Gunplay", characterStatus.isGunplay);
		}
		if (Input.GetKeyDown(Reload) || ammo ==0) {					
			StartCoroutine(waiting());		
		}		
	}	
	 void Fire(){
		if (Time.time > fireRate + lastShot){
		Instantiate(bulletPref, firePoint.position, transform.rotation);
		muzzleParticle.Play();
		audio.PlayOneShot(vois);		
		lastShot = Time.time;
		ammo -= 1;
		characterStatus.isGunplay = true;	
		animator.SetBool("Gunplay", characterStatus.isGunplay);				
		}	
	}			
	IEnumerator waiting()
    {   canShoot = false;
		characterStatus.isReload = true;
		animator.SetBool("Reload",characterStatus.isReload);
		characterStatus.isAiming = false;
		animator.SetBool("aiming",characterStatus.isAiming);	

		characterStatus.isGunplay = false;	
		animator.SetBool("Gunplay", characterStatus.isGunplay);
		ammo = 30;
        yield return new WaitForSeconds(4);		
		canShoot = true;
		characterStatus.isReload = false;
		animator.SetBool("Reload",characterStatus.isReload);
		characterStatus.isAiming = true;
		animator.SetBool("aiming",characterStatus.isAiming);
    }
	public void WallCheck(){		
		Vector3 end = firePoint.position;
		RaycastHit hit;
		Vector3 fwd = nullPoint.transform.TransformDirection(Vector3.forward); 
		if(Physics.Raycast(nullPoint.transform.position, fwd, out hit, distWallCheck) && hit.transform.tag == "Wall"){
			characterStatus.isAiming=false;
			canShoot = false;
			Debug.Log("Collision with "+ hit.collider.gameObject.tag);
			Debug.DrawLine(nullPoint.transform.position,end, Color.red);
			characterStatus.isGunplay = false;	
			animator.SetBool("Gunplay", characterStatus.isGunplay);
		}
		else if(characterStatus.isReload==true){
			characterStatus.isAiming=false;			
		}
		else if(Physics.Raycast(nullPoint.transform.position, fwd, out hit, distWallCheck) && hit.transform.tag == null){
			canShoot = true;
			Debug.DrawLine(nullPoint.transform.position,end, Color.green);
		}else{
			characterStatus.isAiming=true;
			canShoot = true;		
			Debug.DrawLine(nullPoint.transform.position,end, Color.green);
		}		
	}
}
