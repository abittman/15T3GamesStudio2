using UnityEngine;
using System.Collections;

/*
	Main script for handling a 3d canvas
		- dialog speech bubble
		- characters name/title etc

	Functions:
		Orbits character - at specific position or orbit aroud
		Toggles canvas
		Lerps the movement to follow the position
		Auto Toggle if distance too great
*/

public class Billboard : MonoBehaviour {
	// input
	public string MyToggleKey = " ";
	public string MyFollowKey = " ";
	// options
	public bool IsOn = false;	// user can activate and disactivate their appearance
	public bool bCanBePaused = true;				// !
	public bool IsFollowUserAngle = false;			// rotates to position according to users transform.forward angle
	public bool IsFollowUserPosition = true;		// follows the character, otherwise it stays still
	public bool IsLookAtPlayer = true;				// will face the camera.main
	public bool IsAboveCharacterHead = false;		// will follow player and appear above their head
	public bool IsFollowUserAngleAddition = false;	// will follow camera rotation on an angle addition
	public bool IsFixedAtCameraAngle = false;		// changes Z rotation to look at camera
	public float DisableDistance = 20;
	public GameObject TargetCharacter;
	public Vector3 MyDirection;
	public float MyDisplayDistance = 2f;
	public float Speed = 3f;
	public float SpinSpeed = 0.5f;
	// magic variables
	float TimeCount;
	float LastSpunTime = 0f;
	float TimeDifference = 0f;
	bool IsSpinning = false;
	float TargetAngleX;
	float TargetAngleZ;
	float DistanceToCam;
	public Vector3 AboveCharacterHeadOffset = new Vector3(0,1.6f,0);

	void Awake() {
		IsOn = transform.Find ("Main").gameObject.activeSelf;
		//Debug.LogError (gameObject.name+": " + IsOn);
		MyDirection.Normalize();
	}
	// Update is called once per frame
	void Update () {
		HandleInput ();
		UpdateDistance ();
		UpdateOrbit ();
		UpdateBillboard ();
	}

	private void HandleInput() {
		if (IsKeyDown (MyFollowKey) && bCanBePaused) {
			IsFollowUserPosition = !IsFollowUserPosition;
		}
		if (IsKeyDown(MyToggleKey)) {
				IsOn = !IsOn;
				Toggle ();
		}
	}

	private bool IsKeyDown(string MyKey) {
		if (MyKey != " ") {
			KeyCode MyKeyCode =  (KeyCode) System.Enum.Parse(typeof(KeyCode), MyKey); 
			if (Input.GetKeyDown(MyKeyCode))
				return true;
		}
		return false;
	}
	private void UpdateBillboard() {
		if (IsLookAtPlayer) {
			transform.LookAt (Camera.main.transform.position);
			transform.eulerAngles = transform.eulerAngles + new Vector3 (180, 0, 180);
			if (IsFixedAtCameraAngle) {
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
			}
		} else {
			if (TargetCharacter != null) {
				transform.LookAt (TargetCharacter.transform.position);
				transform.eulerAngles = transform.eulerAngles + new Vector3 (180, 0, 180);
			}
		}
	}
	private void UpdateOrbit() {
		if (TargetCharacter != null) {	// is orbitor
			if (IsFollowUserPosition) {
				TargetAngleX = MyDirection.x;
				TargetAngleZ = MyDirection.z;
				OrbitPlayer ();
				Vector3 TemporaryDirection = new Vector3 ((TargetAngleX), 0, (TargetAngleZ)).normalized;
				//TemporaryDirection = TargetCharacter.transform.forward*TargetAngleZ + TargetCharacter.transform.right*TargetAngleX;
			//	Debug.LogError("Direction: " + TemporaryDirection.ToString());
				if (IsFollowUserAngle) 
				{
					TemporaryDirection = TargetCharacter.transform.forward;
					MyDirection = TargetCharacter.transform.forward;
				}
				if (IsFollowUserAngleAddition) {
					TemporaryDirection = TargetCharacter.transform.forward*TargetAngleZ + TargetCharacter.transform.right*TargetAngleX;
				}
				Vector3 TargetPosition = TemporaryDirection * MyDisplayDistance;
				TargetPosition += TargetCharacter.transform.position;
				if (IsAboveCharacterHead) {
					TargetPosition = TargetCharacter.transform.position + AboveCharacterHeadOffset;
				}
				transform.position = Vector3.Lerp (transform.position, TargetPosition, Time.deltaTime * Speed / (DistanceToCam+1));
			}
		}
	}

	// saves the distance, and turns it on or off depending on distance
	private void UpdateDistance() {
		if (IsLookAtPlayer)
			DistanceToCam = Vector3.Distance (transform.position, Camera.main.transform.position);
		else
			DistanceToCam = Vector3.Distance (transform.position, TargetCharacter.transform.position);
		if (IsOn) {
			if (DistanceToCam > DisableDistance)
				Toggle (false);
			else
				Toggle (true);
		}
	}
	// rotates around player using x and z angles
	private void OrbitPlayer() {
		if (IsSpinning) {
			TimeCount += Time.deltaTime;
		}
		float SpinAngle = (TimeCount -TimeDifference)*SpinSpeed;
		if (!IsSpinning) {
			SpinAngle = LastSpunTime;
		} 
		else {
			LastSpunTime = SpinAngle;
		}
		TargetAngleX += SpinAngle;
		TargetAngleZ += SpinAngle;
	}

	void OnTriggerEnter(Collider other) {
		if (IsOn)
			Toggle (false);
	}
	
	void OnTriggerExit(Collider other) {
		if (IsOn)
			Toggle (true);
	}

	public void Toggle() {
		Toggle (!transform.Find ("Main").gameObject.activeSelf);
	}
	public void Toggle(bool IsEnabled) {
		transform.Find ("Main").gameObject.SetActive (IsEnabled);
	}
}
