using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {
	//public UnityEngine.Events.UnityEvent OnOpenDoor = null;
	//public UnityEngine.Events.UnityEvent OnClickLockedDoor = null;
	public GameObject MyRaycastObject;
	
	// Use this for initialization
	void Awake () {
		if (MyRaycastObject == null)
			MyRaycastObject = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit MyHit;
			if (Physics.Raycast (MyRaycastObject.transform.position, MyRaycastObject.transform.forward, out MyHit)) {
				Door MyDoor = MyHit.collider.gameObject.GetComponent<Door>();
				if (MyDoor) {
					Debug.Log("Toggling door!");
					MyDoor.ToggleDoor();
					/*if (MyDoor.IsLocked) {
						Debug.Log("Oh noes door is locked!!");
						//if (OnClickLockedDoor != null)
						//	OnClickLockedDoor.Invoke();
					} else {
						Debug.Log("Door is unlocked!");
						MyDoor.ToggleDoor();
						//if (OnOpenDoor != null)
						//	OnOpenDoor.Invoke();
					}*/
				}
			}
		}
	}
}
