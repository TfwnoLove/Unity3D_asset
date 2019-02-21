using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class RagdollController : MonoBehaviour {

Vector2 rotation = new Vector2 (0, 0);
[Tooltip("Horizontal rotation sensitivity")]
public float HorizontalRotationSpeed = 3; //Horizontal rotation sensitivity

[Tooltip("Control key")]
public KeyCode Slider, Smash, Jump, Camera, Knee, Sprint, RollForward;//Control key
[Tooltip("CharacterController is used to influence gravity on a player")]
public CharacterController controller; //CharacterController is used to influence gravity on a player
public float gravity = 1.5f;
public Animator animator;  
public Camera cam3rd,cam1st;
public bool camSwitch = false;
[Tooltip("The CharacterInventory stores weapons prefabs")]
public CharacterInventory CharacterInventory;
[Tooltip("The point at which the character aims")]
public Transform targetLook;
[Tooltip("The point for holding the left hand")]
public Transform l_Hand_Target;
Transform shouldres, hips, l_Hand, r_Hand, aimPivot;
Quaternion lh_rot, rh_rot;

public float rh_Weight; //Weight variable values

public float lh_Weight;
[Tooltip("CharacterStatus provides a link between the script and the animator")]
public CharacterStatus characterStatus;
[Tooltip("The character center from which the check beams are issued")]
public Transform PlayerCenter;    
[Tooltip("The length of the ray check")]
public float distRayCheck, OverheadRayCheck;
[Tooltip("Text management displays character health")]
public Text Health;
[Tooltip("The amount of health in whole units")]
public int health;
public Image BloodyScreen;
public float a =0;

void  Awake (){
//Set Cursor to not be visible
Cursor.visible = false;
Cursor.lockState = CursorLockMode.None;;
Health.text = "";
//animator = GetComponent<Animator>(); //Fill the field animator
shouldres = animator.GetBoneTransform(HumanBodyBones.RightShoulder).transform; //Fill the field shouldres
hips = animator.GetBoneTransform(HumanBodyBones.Hips).transform; //Fill the field hips
aimPivot = new GameObject().transform;
aimPivot.name = "aim Pivot";
aimPivot.transform.parent = transform;
r_Hand = new GameObject().transform;
r_Hand.name = "r_Hand";
r_Hand.transform.parent = aimPivot;		
l_Hand = new GameObject().transform;
l_Hand.name = "l_Hand";
l_Hand.transform.parent = aimPivot;	
r_Hand.localPosition = CharacterInventory.firstWeapon.rHandPos;
Quaternion rotRight = Quaternion.Euler(CharacterInventory.firstWeapon.rHandRot.x,CharacterInventory.firstWeapon.rHandRot.y,CharacterInventory.firstWeapon.rHandRot.z);
r_Hand.localRotation = rotRight;
//audio = GetComponent<AudioSource>(); //Fill the field audio
}
 
void LateUpdate()
{
	//These 2 lines are required to rotate the character horizontally with the mouse.
	rotation.y += Input.GetAxis ("Mouse X");
	transform.eulerAngles = (Vector2)rotation * HorizontalRotationSpeed;	
	
}

void FixedUpdate () { 
	StartCoroutine(BloodScreen());
	//Change hands weight
		lh_Weight = Mathf.Clamp(lh_Weight, 0, 1);
		rh_Weight = Mathf.Clamp(rh_Weight, 0, 1);		
	//These 4 lines are required to play the reload animation.
	AnimatorStateInfo curentState = animator.GetCurrentAnimatorStateInfo(0);
	if (curentState.IsName("Smash")){		
	characterStatus.isAiming = false;	
	animator.SetBool("aiming",characterStatus.isAiming);}

	//This line defines the center of the character’s body.
	controller.center = new Vector3(hips.transform.localPosition.x, hips.transform.localPosition.y +0.15f, hips.transform.localPosition.z);	 
    
	var Vertical = Input.GetAxis("Vertical");
	var Horizontal = Input.GetAxis("Horizontal"); 
	animator.SetFloat("Vertical", Vertical, 0.0f, Time.smoothDeltaTime);	
	animator.SetFloat("Horizontal", Horizontal, 0.0f, Time.smoothDeltaTime);	    
	
	lh_rot = l_Hand_Target.rotation;
	l_Hand.position = l_Hand_Target.position;
	
	
	//Camera switching function
	if (Input.GetKeyDown(Camera)){     
	camSwitch = !camSwitch;
    cam1st.gameObject.SetActive(camSwitch);
    cam3rd.gameObject.SetActive(!camSwitch);
	}  
		

	if (Input.GetKey(Sprint)){
	characterStatus.isAiming = false;
	animator.SetBool("aiming",characterStatus.isAiming);
	characterStatus.isSprint = true;
	animator.SetBool("isSprint",characterStatus.isSprint);
	}
	else characterStatus.isSprint = false;
	animator.SetBool("isSprint",characterStatus.isSprint);
	if (Input.GetKey(Sprint) && characterStatus.isGunplay){
	characterStatus.isAiming = true;
	animator.SetBool("aiming",characterStatus.isAiming);
	characterStatus.isSprint = false;
	animator.SetBool("isSprint",characterStatus.isSprint);
	}
	int Mask = 1 << 10;
	Mask = ~Mask;
	RaycastHit hits;	
 	if (Input.GetKey(Knee)){	
		animator.SetBool("knee", true);	
		characterStatus.isKnee = true;										
		}			
	else{
		animator.SetBool("knee", false);
		characterStatus.isKnee = false;								
		Debug.DrawRay(PlayerCenter.transform.position, transform.TransformDirection(Vector3.up) * 1000, Color.white);		
		}
	if(characterStatus.isKnee == false && Physics.Raycast(PlayerCenter.transform.position,Vector3.up, out hits,OverheadRayCheck,Mask)){
		animator.SetBool("knee", true);	
		characterStatus.isKnee = true;
		Debug.DrawRay(PlayerCenter.transform.position, transform.TransformDirection(Vector3.up) * hits.distance, Color.red);
        Debug.Log("Collision with "+ hits.collider.gameObject.name);		
	}
	if (characterStatus.isKnee == true && Input.GetKeyDown(Knee)){
		//audio.PlayOneShot(sfx);  
		FindObjectOfType<AudioManager>().Play("Sit down");  
	}
		
		
	if (Input.GetKeyDown(Smash)){
	characterStatus.isSmash = true;	
	animator.SetBool("Smash", characterStatus.isSmash);}
	else characterStatus.isSmash = false;
	animator.SetBool("Smash", characterStatus.isSmash);
	
	
	if (Input.GetKeyDown(Slider) && Input.GetKey(Sprint))
	animator.SetBool("Slider", true);
	else animator.SetBool("Slider", false);
	

	if (Input.GetKeyDown(RollForward) && Input.GetKey(Sprint))
	animator.SetBool("RollForward", true);
	else animator.SetBool("RollForward", false);

	 
	if (Input.GetKeyDown(Jump) && characterStatus.isSprint == false ){
	animator.SetBool("jump", true); 
	}	
	else animator.SetBool("jump", false);
	Health.text = health + "%";
	

	int layerMask = 1 << 10;	
	layerMask = ~layerMask;
	RaycastHit hit;
	if (Physics.Raycast(PlayerCenter.transform.position,Vector3.down, out hit,distRayCheck,layerMask)) {
		Debug.DrawRay(PlayerCenter.transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.red);
        //Debug.Log("Collision with "+ hit.collider.gameObject.name);						
		transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
		}
    else{
        Debug.DrawRay(PlayerCenter.transform.position, transform.TransformDirection(-Vector3.up) * 1000, Color.white);
		Vector3 velocity = new Vector3(0.0f, gravity, 0.0f);
		transform.position -= velocity * Time.deltaTime;
		}		
}	    
	
    void OnAnimatorIK()
	{	aimPivot.position = shouldres.position;	
		aimPivot.LookAt(targetLook);
		animator.SetLookAtWeight(1f,0.1f,1f);	
		animator.SetLookAtPosition(targetLook.position);
		
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,lh_Weight += Time.deltaTime *1.3f);
		animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,lh_Weight += Time.deltaTime *1.3f);
		animator.SetIKPosition(AvatarIKGoal.LeftHand,l_Hand.position);
		animator.SetIKRotation(AvatarIKGoal.LeftHand,lh_rot);

		if (characterStatus.isAiming) {
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand,rh_Weight += Time.deltaTime *1.3f);
		animator.SetIKRotationWeight(AvatarIKGoal.RightHand,rh_Weight += Time.deltaTime *1.3f);
		animator.SetIKPosition(AvatarIKGoal.RightHand,r_Hand.position);
		animator.SetIKRotation(AvatarIKGoal.RightHand,r_Hand.rotation);		
		animator.SetBool("aiming",characterStatus.isAiming);
		}
		else if (!characterStatus.isAiming){
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand,rh_Weight -= Time.deltaTime *1.3f);
		animator.SetIKRotationWeight(AvatarIKGoal.RightHand,rh_Weight -= Time.deltaTime *1.3f);
		animator.SetIKPosition(AvatarIKGoal.RightHand,r_Hand.position);
		animator.SetIKRotation(AvatarIKGoal.RightHand,r_Hand.rotation);		
		animator.SetBool("aiming",characterStatus.isAiming);
		}
		
		if(characterStatus.isSprint){
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand,rh_Weight -= Time.deltaTime *2.0f);
		animator.SetIKRotationWeight(AvatarIKGoal.RightHand,rh_Weight -= Time.deltaTime *2.3f);
		animator.SetIKPosition(AvatarIKGoal.RightHand,r_Hand.position);
		animator.SetIKRotation(AvatarIKGoal.RightHand,r_Hand.rotation); 
		}
		

		if(characterStatus.isReload){		
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand,rh_Weight -= Time.deltaTime *1.3f);
		animator.SetIKRotationWeight(AvatarIKGoal.RightHand,rh_Weight -= Time.deltaTime *1.3f);
		animator.SetIKPosition(AvatarIKGoal.RightHand,r_Hand.position);
		animator.SetIKRotation(AvatarIKGoal.RightHand,r_Hand.rotation); 

		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
		animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0);
		}		
	}
   public void Damage (){ 
		health = health-DamageSingleton.SharedInstance.bulletDamage;		
		if (health <= 0) 
		{
        GetComponent<Animator>().enabled = false;       		
        Debug.Log("Game over");   		  
		}  
    }

    public IEnumerator BloodScreen()
    {	if (health < 30){
		BloodyScreen.color = new Color(1,1,1,a);        
		a = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time, 1));
		if(a >=0.95f ){
		FindObjectOfType<AudioManager>().Play("Low_health");}			
		}
		else{
			BloodyScreen.color = new Color(1,1,1,0);
			a=0;
			FindObjectOfType<AudioManager>().Stop("Low_health");
		}	
		yield return null;
    }   

}