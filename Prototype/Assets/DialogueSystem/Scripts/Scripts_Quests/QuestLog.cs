using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem {
	public class QuestLog : MonoBehaviour {
		[Header("Quests")]
		
		[Tooltip("Limits the amount of uncompleted quests you can do. Set to -1 for unlimited.")]
		public int QuestLimit = -1;
		[Tooltip("These quests can be given to other characters to complete.")]
		public List<Quest> MyQuests = new List<Quest>();
		[Tooltip("This should be linked to the object that has a QuestLogGuiHandler class attached. Keep empty if the character has no gui.")]
		public QuestLogGuiHandler MyQuestLogGui;

		[Header("Sounds")]
		private AudioSource MySource;
		[Tooltip("Sound Plays when completing a quest")]
		public AudioClip OnCompleteQuestSound;
		[Tooltip("Sound Plays when handing in a quest")]
		public AudioClip OnHandInQuestSound;
		[Tooltip("Sound Plays when Beginning in a quest")]
		public AudioClip BeginQuestSound;

		// Use this for initialization
		void Start () {
			MySource = gameObject.GetComponent<AudioSource> ();
			if (MySource == null)
				MySource = gameObject.AddComponent<AudioSource> ();
		}

		public Quest GetQuest(string QuestName) {
			for (int i = 0; i < MyQuests.Count; i++) {
				if (MyQuests [i].Name == QuestName) {
					return MyQuests[i];
				}
			}
			return null;
		}
		// Quest Stuff
		public bool CompleteQuest(string QuestName) {
			Quest MyQuest = GetQuest (QuestName);
			if (MyQuest != null
			    && !MyQuest.IsCompleted && !MyQuest.IsHandedIn)
				{
					MyQuest.CompleteQuest();
					RefreshQuestsGui();		// refresh gui
					
					if (OnCompleteQuestSound != null)
						MySource.PlayOneShot (OnCompleteQuestSound);
					return true;
				}
			return false;
		}

		// when loading quests
		public void AddQuest(Quest NewQuest) {
			NewQuest.QuestGiver = this;
			if (!MyQuests.Contains(NewQuest))
				MyQuests.Add (NewQuest);
		}
		public int GetUncompletedQuests() {
			int UncompletedQuestsCount = 0;
			for (int i = 0; i < MyQuests.Count; i++) {
				if (!MyQuests[i].IsHandedIn) {
					UncompletedQuestsCount++;
				}
			}
			return UncompletedQuestsCount;
		}

		// when another quest holder is issueing the quest out
		public string GiveCharacterQuest(string QuestName, QuestLog MyQuestTaker) {
			if (GetUncompletedQuests () >= QuestLimit && QuestLimit != -1) {
				return "";
			}

			int QuestIndex = -1;
			for (int i = 0; i < MyQuests.Count; i++) {
				if (MyQuests[i].Name == QuestName) {
					QuestIndex = i;
					i = MyQuests.Count;
				}
			}
			if (QuestIndex == -1)
				return "!";
			
			if (QuestIndex < MyQuests.Count && QuestIndex >= 0) {
				Quest NewQuest = MyQuests [QuestIndex];
				if (!MyQuestTaker.MyQuests.Contains (NewQuest)) 
				{
					MyQuestTaker.MyQuests.Add (NewQuest);
					if (MyQuestTaker.BeginQuestSound != null) {
						MySource.PlayOneShot (MyQuestTaker.BeginQuestSound);
						//Debug.LogError("PLaying sound");
					}
					//Debug.LogError("Adding Quest");
					//Debug.LogError("Adding quest: " + MyCharacter.MyQuests[QuestIndex].Name);
					if (MyQuestTaker.MyQuestLogGui) {
						MyQuestTaker.MyQuestLogGui.UpdateQuestGuis();
					}
					NewQuest.QuestGiver = this;
					NewQuest.QuestTaker = MyQuestTaker;
					MyQuestTaker.RefreshQuestsGui();

					return NewQuest.Name;
				}
			}
			return "";
		}

		public void LeaveZone(string ZoneName) {
			//Debug.LogError ("Handing Leaving of: " + ZoneName);
			for (int i = 0; i < MyQuests.Count; i++) {
				if (MyQuests[i].IsLeaveZone && MyQuests[i].ZoneName == ZoneName) {
					CompleteQuest(MyQuests[i].Name);
				}
			}
		}
		// when player is handing in a quest
		public int RemoveQuest(string QuestName, Character MyQuestGiver) {
			int QuestIndex = -1;
			for (int i = 0; i < MyQuests.Count; i++) {
				if (MyQuests[i].Name == QuestName) {
					QuestIndex = i;
					i = MyQuests.Count;
				}
			}
			// if no quest of same name found
			if (QuestIndex == -1)
				return -1;
			Quest MyQuest = MyQuests[QuestIndex];
			if (MyQuest.IsCompleted && !MyQuest.IsHandedIn) {
				//Debug.LogError (name + " Removeing quest: " + QuestIndex);
				MyQuest.HandIn();
				RefreshQuestsGui();		// refresh gui
				//MyQuests.RemoveAt (QuestIndex);
				//MyQuestLogGui.RemoveAt(QuestIndex);
				
				if (OnHandInQuestSound != null)
					MySource.PlayOneShot (OnHandInQuestSound);
				return QuestIndex;
			}
			return -1;
		}
		public void RefreshQuestsGui() {
			for (int i = 0; i < MyQuests.Count; i++) {
				MyQuests[i].CheckCompleted(this);
			}
			if (MyQuestLogGui)
				MyQuestLogGui.CheckQuestCompletitions();
		}

		public bool DoesHaveQuest(string QuestName) {
			Quest MyQuest = GetQuest (QuestName);
			if (MyQuest == null) {
				return false;
			} else {
				if (MyQuest.IsCompleted){
					return false;
				}
				return true;
			}
		}
	}
}