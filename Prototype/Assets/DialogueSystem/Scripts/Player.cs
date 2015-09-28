using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;



public class Player : MonoBehaviour {
	bool IsMouseEnabled = true;
	public bool CanJump = false;
	public bool CanRun = false;
	Animation anim;
	bool openInv = false;
	// Use this for initialization
	void Awake () {
		if (!CanJump) {
			gameObject.GetComponent<RigidbodyFirstPersonController>().movementSettings.JumpForce = 0;
		}
		if (!CanRun) {
			gameObject.GetComponent<RigidbodyFirstPersonController>().movementSettings.RunMultiplier = 1;
		}
	}
	void Start(){
		anim = transform.GetComponent<Animation> ();
	}
	// Update is called once per frame
	void Update () {
		if (IsMouseEnabled) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			IsMouseEnabled = !IsMouseEnabled;
		}
		if (Input.GetKeyDown (KeyCode.O)) {
			openInv = !openInv;
			if(openInv){
				anim.Play ("ArmAnimation");

			}else{
				anim.Play ("ArmAnimationDown");

			}
		}
	}
}
