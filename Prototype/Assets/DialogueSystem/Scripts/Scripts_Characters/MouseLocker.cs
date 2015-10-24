using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class MouseLocker : MonoBehaviour {
	public string ToggleMouseKey = "";
	private bool IsMouseEnabled = true;
	private float MovementSpeed;
	
	// Use this for initialization
	void Start () {
		MovementSpeed = gameObject.GetComponent<RigidbodyFirstPersonController> ().movementSettings.ForwardSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (ToggleMouseKey != "") {
			KeyCode MyKey =  (KeyCode) System.Enum.Parse(typeof(KeyCode), ToggleMouseKey); 
			if (Input.GetKeyDown(MyKey)) {
				IsMouseEnabled = !IsMouseEnabled;
			}
		}
		if (IsMouseEnabled) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			gameObject.GetComponent<RigidbodyFirstPersonController>().mouseLook.XSensitivity = 2;
			gameObject.GetComponent<RigidbodyFirstPersonController>().mouseLook.YSensitivity = 2;
			gameObject.GetComponent<RigidbodyFirstPersonController> ().movementSettings.ForwardSpeed = MovementSpeed;
			gameObject.GetComponent<RigidbodyFirstPersonController> ().movementSettings.BackwardSpeed = MovementSpeed/2f;
			gameObject.GetComponent<RigidbodyFirstPersonController> ().movementSettings.StrafeSpeed = MovementSpeed/2f;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			gameObject.GetComponent<RigidbodyFirstPersonController>().mouseLook.XSensitivity = 0;
			gameObject.GetComponent<RigidbodyFirstPersonController>().mouseLook.YSensitivity = 0;
			gameObject.GetComponent<RigidbodyFirstPersonController> ().movementSettings.ForwardSpeed = 0;
			gameObject.GetComponent<RigidbodyFirstPersonController> ().movementSettings.BackwardSpeed = 0;
			gameObject.GetComponent<RigidbodyFirstPersonController> ().movementSettings.StrafeSpeed = 0;
		}
	}
}
