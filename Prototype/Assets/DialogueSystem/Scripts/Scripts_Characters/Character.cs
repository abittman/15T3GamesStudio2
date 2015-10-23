using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using AISystem;

/*
	NPC's class
		Contains movement State
		Raytrace for other characters and quest items
	Other npc classes:
		Stats (dynamic)
		Inventory
 */

namespace DialogueSystem {
	public class Character : MonoBehaviour {
		[Header("Raycasting")]
		[Tooltip("Used for interaction between character and other objects. Keep blank if using it's own transform.forward vector for ray casts.")]
		public Transform MyRayObject = null;
		[Tooltip("Chose the layers that you want to be able to interact with.")]
		public LayerMask MyLayer;
		private SpeechHandler MySpeechBubble;
		[Header("Speech")]
		[Tooltip("These functions are called when dialogue begins!")]
		public UnityEngine.Events.UnityEvent OnBeginTalking;
		[Tooltip("These functions are called when dialogue ends!")]
		public UnityEngine.Events.UnityEvent OnEndTalking;
		
		[Header("Sounds")]
		private AudioSource MySource;
		[Tooltip("Played when dialogue begins")]
		public AudioClip OnBeginTalkingSound;
		[Tooltip("Played when dialogue ends")]
		public AudioClip OnEndTalkingSound;

		// Use this for initialization
		void Awake () {
			MySource = gameObject.GetComponent<AudioSource> ();
			if (MySource == null)
				MySource = gameObject.AddComponent<AudioSource> ();
			// Grab my speech handler!
			MySpeechBubble = gameObject.GetComponent <SpeechHandler> ();
			if (MySpeechBubble != null) {
				MySpeechBubble.MyDialogueText.gameObject.transform.parent.gameObject.SetActive (false);
				MySpeechBubble.SetCharacter(this);
			}

			// Title gui
			Transform MyCharacterLabel = MySpeechBubble.MyDialogueText.transform.parent.parent.FindChild ("Label");
			if (MyCharacterLabel != null) {
				MyCharacterLabel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
			}
		}

		// Interfacing with dialogue/speech handler

		public SpeechHandler GetSpeechHandler() {
			return MySpeechBubble;
		}

		public void BeginDialogue(Character ConversationStarter) {
			// Now have speech bubbles pop up
			if (MySpeechBubble.CanTalk() && ConversationStarter.GetSpeechHandler().CanTalk()) 
			{
				/*if (gameObject.GetComponent<Wander>()) {
					gameObject.GetComponent<Wander>().ToggleWander(false);
					gameObject.GetComponent<Movement>().LookAt(ConversationStarter.gameObject);
				}*/
				MySpeechBubble.StartConversation(ConversationStarter, this);

				OnBeginTalking.Invoke();
				if (OnBeginTalkingSound != null)
					MySource.PlayOneShot (OnBeginTalkingSound);
			}
		}

		public void OnEndDialogue() {
			OnEndTalking.Invoke ();
			if (OnEndTalkingSound != null)
				MySource.PlayOneShot (OnEndTalkingSound);
		}


		public void EndTalk(Character ConversationStarter) {
			MySpeechBubble.ToggleSpeech (false);
		}


	// Ray trace stuff
		// Raytraces, checks for character or item
		public bool RayTrace() {
			//Debug.LogError ("RayTracing");
			if (MyRayObject == null)
				MyRayObject = gameObject.transform;

			//var layerMask = 1 << 2; layerMask = ~layerMask;

			RaycastHit MyHit;
			if (Physics.Raycast (MyRayObject.position, MyRayObject.forward, out MyHit, 5, MyLayer)) {
				//Debug.LogError("RayHit");
				Character HitCharacter = MyHit.collider.gameObject.GetComponent<Character>();
				if (HitCharacter != null) {
					//Debug.LogError("HitCharacter!");
					// now initialize that characters dialogue system with (MainCharacter->Lotus dialogue file)
					if (MySpeechBubble.CanTalk()) {
						HitCharacter.BeginDialogue(this);
					}
					return true;
				}
				
				ItemObject HitItemObject = MyHit.collider.gameObject.GetComponent<ItemObject>();
				if (HitItemObject != null) {
					//Debug.LogError("HitItem!");

					if (HitItemObject.IsItemPickup) {
						DialogueSystem.Inventory MyInventory = gameObject.GetComponent<DialogueSystem.Inventory>();
						if (MyInventory != null) {
							MyInventory.AddItem(HitItemObject.GetItem());
						}
					}

					HitItemObject.Pickup();	// does things like destroy, activates the special function
					// add a value to statistics! for things 
					if (gameObject.GetComponent<QuestLog>())
						gameObject.GetComponent<QuestLog>().RefreshQuestsGui();
					return true;
				}
			}
			return false;
		}
	}
}
