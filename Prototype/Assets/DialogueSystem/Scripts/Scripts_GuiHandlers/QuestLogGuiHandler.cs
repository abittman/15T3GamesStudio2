using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/*
	Handles just the gui's of quests
		
*/

namespace DialogueSystem {
	public class QuestLogGuiHandler : GuiListHandler {
		public Color32 MyQuestColour = new Color (0, 22, 66);
		public Color32 MyQuestCompleteColour = new Color (0, 55, 155);
		public Color32 MyHandedInQuestColour = new Color (0, 55, 155);
		public QuestLog MyCharacter;
		public bool IsCompletedOnly = false;
		public bool IsNonCompletedOnly = false;
		
		// Update is called once per frame
		void Update () {
			UpdateGuis ();
		}
		
		public void ResetFilters() {
			IsCompletedOnly = false;
			IsNonCompletedOnly = false;
			UpdateQuestGuis ();
			CheckQuestCompletitions ();
		}
		public void FilterNonCompletedOnly() {
			IsCompletedOnly = false;
			IsNonCompletedOnly = true;
			UpdateQuestGuis ();
			CheckQuestCompletitions ();
		}
		public void FilterCompletedOnly() {
			IsCompletedOnly = true;
			IsNonCompletedOnly = false;
			UpdateQuestGuis ();
			CheckQuestCompletitions ();
		}

		public void UpdateQuestGuis() {
			Debug.Log ("Refreshing Inventory Gui: " + Time.time);
			Clear ();
			for (int i = 0; i < MyCharacter.MyQuests.Count; i++) {
				if (IsRenderQuest(MyCharacter.MyQuests[i]))
				{
					TooltipData MyData = new TooltipData();
					MyData.LabelText = MyCharacter.MyQuests[i].Name;
					MyData.DescriptionText = MyCharacter.MyQuests[i].Description;
					AddGui(MyCharacter.MyQuests[i].Name, MyData);
				}
			}
		}
		public bool IsRenderQuest(Quest MyQuest) {
			if ((!IsCompletedOnly && !IsNonCompletedOnly) || 
			    (IsCompletedOnly && MyQuest.IsCompleted) || 
			    (IsNonCompletedOnly && !MyQuest.IsCompleted)) 
				return true;
			return false;
		}
		public void CheckQuestCompletitions() {
			if (MyCharacter) {
				int j = 0;
				for (int i = 0; i < MyCharacter.MyQuests.Count; i++) {
					if (IsRenderQuest(MyCharacter.MyQuests[i]))
					{
						if (MyCharacter.MyQuests [i].IsHandedIn) {
							MyGuis [j].GetComponent<RawImage> ().color = MyHandedInQuestColour;
						} 
						else if (MyCharacter.MyQuests [i].HasCompleted()) {
							MyGuis [j].GetComponent<RawImage> ().color = MyQuestColour;
						} else {
							MyGuis [j].GetComponent<RawImage> ().color = MyQuestCompleteColour;
						}
						j++;
					}
				}
			}
		}
	}
}