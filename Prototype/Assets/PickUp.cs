using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (transform.position, GameObject.FindWithTag ("NPC").transform.position);

		if (dist < 3) {

			Destroy(gameObject);

			print ("Pickup");
		}
	}
}
