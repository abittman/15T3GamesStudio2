using UnityEngine;
using System.Collections;

public class PushAway : MonoBehaviour {
	public GameObject PlayerObj;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = PlayerObj.GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (transform.position, PlayerObj.transform.position);

		if (dist < 3) {
			Vector3 direction = PlayerObj.transform.position - transform.position;
			direction.Normalize();
			PlayerObj.transform.Translate(transform.forward* 300 * Time.deltaTime, Space.World);

		}
	}




}
