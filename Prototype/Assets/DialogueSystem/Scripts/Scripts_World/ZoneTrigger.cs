using UnityEngine;
using System.Collections;

namespace DialogueSystem {
	public class ZoneTrigger : MonoBehaviour {
		public QuestLog MyCharacter;

		void OnTriggerExit(Collider other) {
			//Debug.LogError ("I have gotten away from seth! " + other.gameObject.name);
			QuestLog MyLeavingCharacter = other.gameObject.GetComponent<QuestLog> ();
			if (MyLeavingCharacter) {
				MyLeavingCharacter.LeaveZone(gameObject.name);
				/*for (int i = 0; i < MyCharacter.MyQuests.Count; i++) 
				{
					//Debug.LogError("Completing: " + MyCharacter.MyQuests[i].Name);
					MyLeavingCharacter.CompleteQuest(MyCharacter.MyQuests[i].Name);
				}*/
			}
		}
	}
}