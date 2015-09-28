using UnityEngine;
using System.Collections;

public enum Emotions{Sad,Happy,Anger,Fear,Anxious,Confused};

public class BasicAI : MonoBehaviour {

	public Transform[] waypoint;        // The amount of Waypoint you want
	public float patrolSpeed = 3f;       // The walking speed between Waypoints
	public bool  loop = true;       // Do you want to keep repeating the Waypoints
	public float dampingLook= 6.0f;          // How slowly to turn
	public float pauseDuration = 0;   // How long to pause at a Waypoint
	public GameObject PlayerObj;
	public GameObject slot;
	public Emotions emotion;

	private float curTime;
	private int currentWaypoint = 0;
	private CharacterController character;
	public bool Speaking = false;

	public GameObject SlotItem{
		get{
			if(slot.transform.childCount>0){
				return slot.transform.GetChild (0).gameObject;
			}
			return null;
		}
	}

	void  Start (){
		
		character = GetComponent<CharacterController>();
		currentWaypoint = Random.Range(0, waypoint.Length);
	}
	
	void  Update (){
		float dist = Vector3.Distance (transform.position, PlayerObj.transform.position);
		if (dist < 3) {
			Speaking = true;
			Debug.Log ("Hit Player");
		} else {
			Speaking = false;
		}


		if (!Speaking) {
			if (currentWaypoint < waypoint.Length) {
				patrol ();
			} else {    
				if (loop) {
					currentWaypoint = 0;
				} 
			}
		}
		State ();
		switch (emotion) {
		case Emotions.Anger:
			GetComponent<Renderer>().material.color = Color.red;
			break;
		case Emotions.Anxious:
			GetComponent<Renderer>().material.color = Color.cyan;
			break;
		case Emotions.Fear:
			GetComponent<Renderer>().material.color = Color.magenta;
			break;
		case Emotions.Happy:
			GetComponent<Renderer>().material.color = Color.green;
			break;
		case Emotions.Sad:
			GetComponent<Renderer>().material.color = Color.black;
			break;
		case Emotions.Confused:
			GetComponent<Renderer>().material.color = Color.yellow;
			break;

		}
	}

	void State(){


		switch (SlotItem.name) {
		case "AnxiousMemory":
			emotion = Emotions.Anxious;
			break;
		case "FearMemory":
			emotion = Emotions.Fear;
			break;
		case "JoyfulMemory":
			emotion = Emotions.Happy;
			break;
		case "AngryMemory":
			emotion = Emotions.Anger;
			break;
		case "SadMemory":
			emotion = Emotions.Sad;
			break;
		case "ConfusedMemory":
			emotion = Emotions.Confused;
			break;

		}

	}


	
	void  patrol (){

		Vector3 target = waypoint[currentWaypoint].position;
		target.y = transform.position.y; // Keep waypoint at character's height
		Vector3 moveDirection = target - transform.position;
		
		if(moveDirection.magnitude < 0.5f){
			if (curTime == 0)
				curTime = Time.time; // Pause over the Waypoint
			if ((Time.time - curTime) >= pauseDuration){
				currentWaypoint = Random.Range(0, waypoint.Length);
				curTime = 0;
			}
		}else{        
			var rotation= Quaternion.LookRotation(target - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
			character.Move(moveDirection.normalized * patrolSpeed * Time.deltaTime);
		}  
	}



		
}
