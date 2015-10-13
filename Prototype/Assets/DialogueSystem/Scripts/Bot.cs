using UnityEngine;
using System.Collections;

// use this in Bot class, similar to player but for npcs
// make a basic bot class that can be used for things like animations etc

namespace DialogueSystem {
	public enum MovementState {
		Waiting,
		WalkToPosition,
		Wander,
		Chase,
		Flee
	};

	public class Bot : MonoBehaviour {
		public MovementState MyMovementState = MovementState.Waiting;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}