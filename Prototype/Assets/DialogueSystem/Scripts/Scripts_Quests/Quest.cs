using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem {
	// what makes the quest true or not
	[System.Serializable]
	public class Condition {
		public bool IsInventory;
		public int ItemQuantity;
		public string ItemName;
		public bool bIsTakeItems;
	}

	[System.Serializable]
	public class Quest {
		public string Name;
		public string Description;		// 
		//public string Condition;		// a command code for the quests condition
		public Condition DecodedCondition = new Condition();

		public bool IsCompleted = false;
		public float TimeCompleted = 0f;
		public QuestLog QuestGiver = null;
		public QuestLog QuestTaker = null;
		public bool IsHandedIn = false;
		public float TimeFinished = 0f;

		// conditions
		public bool IsLeaveZone = false;
		public string ZoneName = "";

		public void HandIn() {
			IsHandedIn = true;
			TimeFinished = Time.time;
		}
		public void CompleteQuest() {
			IsCompleted = true;
			TimeCompleted = Time.time;
		}

		public Quest() {}

		public bool HasGivenToSomeone() {
			return (QuestTaker != null);
		}

		public bool HasCompleted() {
			return IsCompleted;
		}


		public bool CheckCompleted(QuestLog MyCharacter) {
			if (IsHandedIn)
				return true;
			//IsCompleted = false;
			Inventory MyInventory = MyCharacter.gameObject.GetComponent<Inventory> ();
			if (IsConditionTrue(MyCharacter))
				IsCompleted = true;
			else 
				IsCompleted = false;

			return IsCompleted;
		}

		// checks conditions
		// uses statistics, inventory classes
		public bool IsConditionTrue(QuestLog MyCharacter) {
			// inventory condition checks
			if (DecodedCondition.IsInventory) {
				Inventory MyInventory = MyCharacter.gameObject.GetComponent<Inventory> ();
				//if (MyInventory.MyItems.Count > 0) return true;
				for (int i = 0; i < MyInventory.MyItems.Count; i++) {
					if (MyInventory.MyItems [i].Name == DecodedCondition.ItemName) {
						if (MyInventory.MyItems [i].Quantity >= DecodedCondition.ItemQuantity)
							return true;
					}
				}
			}
			else // if not inventory, check if it's already been completed
			{
				if (IsCompleted)
					return IsCompleted;
			}
			// area in condition

			// 

			return false;
		}

		//reading/writing section
		
		public Quest(List<string> SavedData) {
			for (int i = 0; i < SavedData.Count; i++) {
				//Debug.LogError("Reading: " + SavedData[i]);
				if (SavedData[i].Contains ("/quest")) {
					int IndexToDelete = SavedData[i].IndexOf("/quest");
					SavedData[i] = SavedData[i].Remove(0,IndexToDelete+"/quest ".Length);
					Name = SavedData[i];
				} else if (SavedData[i].Contains ("/description")) {
					int IndexToDelete = SavedData[i].IndexOf("/description");
					SavedData[i] = SavedData[i].Remove(0,IndexToDelete+"/description ".Length);
					Description =(SavedData[i]);
				}   else if (SavedData[i].Contains ("/zone ")) {
					int IndexToDelete = SavedData[i].IndexOf("/zone ");
					SavedData[i] = SavedData[i].Remove(0,IndexToDelete+"/zone ".Length);
					IsLeaveZone = true;
					ZoneName = SavedData[i];
				}  else if (SavedData[i].Contains ("/rewards")) {
					int IndexToDelete = SavedData[i].IndexOf("/rewards ");
					SavedData[i] = SavedData[i].Remove(0,IndexToDelete+"/rewards".Length);
					
				}  else if (SavedData[i].Contains ("/failure")) {
					int IndexToDelete = SavedData[i].IndexOf("/failure ");
					SavedData[i] = SavedData[i].Remove(0,IndexToDelete+"/failure".Length);
					
				}  else if (SavedData[i].Contains ("/items")) {
					int IndexToDelete = SavedData[i].IndexOf("/items ");
					SavedData[i] = SavedData[i].Remove(0,IndexToDelete+"/items ".Length);
					List<string> MyCommands = SpeechFileReader.SplitCommands(SavedData[i]);

					if (MyCommands.Count > 0) {
						DecodedCondition.IsInventory = true;
						try { 
							string QuantityString = MyCommands[MyCommands.Count-1];
							DecodedCondition.ItemQuantity  = int.Parse(QuantityString);
							SavedData[i] = SavedData[i].Remove(SavedData[i].IndexOf(QuantityString)-1, QuantityString.Length+1);
							DecodedCondition.ItemName = SavedData[i];
						} catch(System.FormatException e) {	
							DecodedCondition.ItemName = SavedData[i];
						}
					}
					if (MyCommands.Count == 2) {
					}
				}
			}
		}
	}
}
