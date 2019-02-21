using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Characters/status")]

public class CharacterStatus : ScriptableObject {

	public bool isAiming;
	public bool isGunplay;
	public bool isSprint;
	public bool isSmash;	
	public bool isReload;
	public bool isKnee;
	public bool isJumpOverCover;
}
