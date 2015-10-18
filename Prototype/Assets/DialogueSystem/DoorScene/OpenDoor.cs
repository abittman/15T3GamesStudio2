using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
	public Canvas minigame;
	Canvas clone;
	bool canOpen = true;
	Door MyDoor;
	// Use this for initialization
	void Start ()
	{
		minigame.GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canOpen) {
			//hackDoor();
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit MyHit;
				if (Physics.Raycast (transform.position, transform.forward, out MyHit)) {
					MyDoor = MyHit.collider.gameObject.GetComponent<Door> ();
					if (MyDoor) {
						hackDoor();
					}
				}
			}
		}
	}

	private void hackDoor ()
	{
		canOpen = false;
		clone = Instantiate (minigame);

		clone.transform.position = new Vector3 (0, 0, 0);

	}

	public void gameWon ()
	{
		clone = null;
		Debug.Log ("Game Won. Toggle Door!");
		MyDoor.ToggleDoor ();
		canOpen = true;
	}

	public void gameLost ()
	{
		clone = null;
		Debug.Log ("Game lost");
		canOpen = true;
	}
}
