using UnityEngine;
using System.Collections;

public class TradeScreen : MonoBehaviour {

	public string MyToggleKey = " ";
	public float DisableDistance = 20;
	public GameObject Player;
	public Vector3 MyDirection;
	public float MyDisplayDistance = 2f;
	public float Speed = 3f;
	public float SpinSpeed = 0.5f;
	float TimeCount;
	float LastSpunTime = 0f;
	float TimeDifference = 0f;
	bool IsSpinning = false;
	
	
	// Update is called once per frame
	void Update () {
		if (Player == null) {
			float DistanceToCam = Vector3.Distance (transform.position, Camera.main.transform.position);
			if (DistanceToCam > DisableDistance)
				Toggle (false);
			else
				Toggle (true);
		} else {	// is orbitor
			if (MyToggleKey != " ") {
				KeyCode MyKey =  (KeyCode) System.Enum.Parse(typeof(KeyCode), MyToggleKey); 
				if (Input.GetKeyDown(MyKey)) {
					Toggle ();
				}
			}
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
			Vector3 TemporaryDirection = new Vector3(Mathf.Cos(SpinAngle+MyDirection.normalized.x), 0, Mathf.Sin(SpinAngle+MyDirection.normalized.z)).normalized;
			
			Vector3 NewPoint = Player.transform.position + new Vector3(-3f,0.5f,0f) + TemporaryDirection.normalized*MyDisplayDistance;
			//transform.position = Vector3.Lerp(transform.position, Player.transform.position + new Vector3(0,0.5f,0) + (Player.transform.forward *1.3f), Time.deltaTime*Speed);
			transform.position = Vector3.Lerp(transform.position,NewPoint, Time.deltaTime*Speed);
		}
		
		transform.LookAt (Camera.main.transform.position);
		//transform.eulerAngles = transform.eulerAngles + new Vector3 (180, 0, 180);
	}
	void OnTriggerEnter(Collider other) {
		Toggle (false);
		
	}
	
	void OnTriggerExit(Collider other) {
		Toggle (true);
	}
	public void Toggle() {
		Toggle (!transform.Find ("Main").gameObject.activeSelf);
	}
	public void Toggle(bool IsEnabled) {
		transform.Find ("Main").gameObject.SetActive (IsEnabled);
	}
}
