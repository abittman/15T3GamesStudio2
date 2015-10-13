using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
		public bool IsRayTrace = false;
		public Transform MyRayObject = null;
		public List<Quest> MyQuests = new List<Quest>();
		public SpeechHandler MySpeechBubble;
		public QuestLogGuiHandler MyQuestLogGui;

		// Use this for initialization
		void Awake () {
			if (MySpeechBubble != null) {
				MySpeechBubble.gameObject.transform.parent.gameObject.SetActive (false);
				MySpeechBubble.SetCharacter(this);
				name = MySpeechBubble.MyFile;
			}
			Transform MyCharacterLabel = MySpeechBubble.transform.parent.parent.FindChild ("Label");
			if (MyCharacterLabel != null) {
				MyCharacterLabel.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
			}
		}
		public void AddQuest(Quest NewQuest) {
			NewQuest.QuestGiver = this;
			if (!MyQuests.Contains(NewQuest))
				MyQuests.Add (NewQuest);
		}
		public int GiveCharacterQuest(int QuestIndex, Character MyQuestTaker) {
			if (QuestIndex < MyQuests.Count && QuestIndex >= 0) {
				Quest NewQuest = MyQuests [QuestIndex];
				if (!MyQuestTaker.MyQuests.Contains (NewQuest)) {
					MyQuestTaker.MyQuests.Add (NewQuest);
					//Debug.LogError("Adding quest: " + MyCharacter.MyQuests[QuestIndex].Name);
					if (MyQuestTaker.MyQuestLogGui) {
						MyQuestTaker.MyQuestLogGui.AddQuestGui (NewQuest.Name);
					}
					NewQuest.QuestGiver = this;
					NewQuest.QuestTaker = MyQuestTaker;
					MyQuestTaker.CheckQuestComplete();
					return QuestIndex;
				}
			}
			return -1;
		}

		public int RemoveQuest(int QuestIndex, Character MyQuestGiver) {
			string QuestName = MyQuestGiver.MyQuests [QuestIndex].Name;

			if (QuestIndex < MyQuests.Count && QuestIndex >= 0) {
				QuestIndex = -1;
				for (int i = 0; i < MyQuests.Count; i++) {
					if (MyQuests[i].Name == QuestName) {
						QuestIndex = i;
					}
				}
				// if no quest of same name found
				if (QuestIndex == -1)
					return -1;

				Quest NewQuest = MyQuests[QuestIndex];
				if (NewQuest.IsCompleted) {
					//Debug.LogError (name + " Removeing quest: " + QuestIndex);
					MyQuests.RemoveAt (QuestIndex);
					MyQuestLogGui.RemoveQuest(QuestIndex);
					return QuestIndex;
				}
			}
			return -1;
		}

		public void BeginDialogue(Character ConversationStarter) {
			// Now have speech bubbles pop up
			if (!MySpeechBubble.IsActive) {
				if (gameObject.GetComponent<Wander>()) {
					gameObject.GetComponent<Wander>().ToggleWander(false);
					gameObject.GetComponent<Movement>().LookAt(ConversationStarter.gameObject);
				}
				ConversationStarter.MySpeechBubble.SetCharacter2(this);
				ConversationStarter.MySpeechBubble.SetSecondaryTalker();
				MySpeechBubble.SetCharacter2(ConversationStarter);
				MySpeechBubble.ToggleSpeech (true, false);
				MySpeechBubble.SetMainTalker();
				MySpeechBubble.Activate ();
				if (gameObject.GetComponent<FaceUpdate>()) 
				{
					gameObject.GetComponent<FaceUpdate>().SetAnimation(3);
				}
			}
		}

		public void OnEndDialogue() {
			if (gameObject.GetComponent<Wander>()) {
				gameObject.GetComponent<Wander>().ToggleWander(true);
			}
		}


		public void EndTalk(Character ConversationStarter) {
			MySpeechBubble.ToggleSpeech (false);
		}
		// Raytraces, checks for character or item
		public bool RayTrace() {
			//Debug.LogError ("RayTracing");
			if (MyRayObject == null)
				MyRayObject = gameObject.transform;
			RaycastHit MyHit;
			if (Physics.Raycast (MyRayObject.position, MyRayObject.forward, out MyHit, 5)) {
				//Debug.LogError("RayHit");
				Character HitCharacter = MyHit.collider.gameObject.GetComponent<Character>();
				if (HitCharacter != null) {
					//Debug.LogError("HitCharacter!");
					// now initialize that characters dialogue system with (MainCharacter->Lotus dialogue file)
					if (!MySpeechBubble.IsTalking()) {
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
					CheckQuestComplete();
					return true;
				}
			}
			return false;
		}
		public void CheckQuestComplete() {
			for (int i = 0; i < MyQuests.Count; i++) {
				MyQuests[i].CheckCompleted(this);
			}
			if (MyQuestLogGui)
				MyQuestLogGui.CheckQuestCompletitions();
		}
	}
}
