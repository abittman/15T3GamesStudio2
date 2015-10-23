using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
	Stores data of the dialogue lines
	Has a function to convert command lines into dialogue data
*/


namespace DialogueSystem {
	
	// ramblings
	// if character has completed a condition, move to MyNext 0, if not move to 1
	
	[System.Serializable]
	public class DialogueLine {
		public List<int> MyNext = new List<int>();		// index for each response
		public int IsFirstMyNext;
		public string SpeechDialogue = "";
		public List<string> ReverseDialogueLines = new List<string>();
		public bool IsQuestGive = false;
		public bool IsQuestCheck = false;
		public string QuestName = "";
		public bool IsExitChat = false;
		public bool bIsFirst = false;	// makes an alternative speech option
		public bool bIsRandomizePath = false;
		public string ComponentName = "";	// the component to activate the function off
		public string FunctionName = "";	// a function to activate
		
		public DialogueLine() {
			
		}
		
		public string GetTotalReverseText() {
			string Blarg = "";
			for (int i = 0; i < ReverseDialogueLines.Count; i++) {
				Blarg += "[" + (i+1) + "] " + ReverseDialogueLines[i] + "\n";
			}
			return Blarg;
		}
		public string GetReverseSpeechDialogue(bool IsFirst) {
			if (ReverseDialogueLines.Count == 0)
				return "";
			return ReverseDialogueLines[0];
			//return ReverseSpeechDialogue;
			/*
			if (bIsFirst) {
				if (!IsFirst && ReverseSpeechDialogue.Count >= 2)
					return ReverseSpeechDialogue[1];
			}
			return ReverseSpeechDialogue[0];*/
		}

		// if no speech!
		public string GetSpeechDialogue(bool IsFirst) {
			return SpeechDialogue;
		}

		public static List<int> GetInts(string MyIntsString) {
			//Debug.LogError (MyIntsString);
			string[] MyInts = MyIntsString.Split(' ');
			//for (int i = 0; i < MyInts.Length; i++)
			//	Debug.LogError (MyInts[i]);
			List<int> NewInts = new List<int> ();
			if (MyInts != null)
			for (int j = 0; j < MyInts.Length; j++) {
				if (MyInts[j].Length > 0) {
					if (MyInts[j].Contains(","))
						MyInts[j] = MyInts[j].Remove(MyInts[j].IndexOf(","));
					//Debug.LogError("Trying to read:" + MyInts[j]);
					int IsInt = -1;
					try {
						IsInt = int.Parse(MyInts[j]);
						NewInts.Add(IsInt);
					} catch(System.FormatException e) {

					}
					//if (MyInts[j] != "" && MyInts[j] != " ")
				}
			}
			return NewInts;
		}
		
		public bool IsReverseSpeech() {
			if (ReverseDialogueLines.Count == 0)
				return false;
			if (ReverseDialogueLines.Count == 1)
				return true;
			return false;
		}
		
		public static string RemoveCommand(string FullString, string CommandCode) {
			int IndexToDelete = FullString.IndexOf(CommandCode);
			FullString = FullString.Remove(0,IndexToDelete+CommandCode.Length);
			return FullString;
		}
		public bool ListContains(List<string> MyList, string Blarg) {
			for (int i = 0; i < MyList.Count; i++) {
				if (MyList[i] == Blarg) return true;
			}
			return false;
		}
		public int GetNextLine(bool IsFirst, int Value, List<string> QuestsGiven, List<string> QuestsCompleted) {
			
			// if first time talking condition
			if (bIsFirst && IsFirst) 
			{
				return IsFirstMyNext;
			}
			// for this condition, use 3 variables for next
			if (IsQuestCheck) {
				if (ListContains(QuestsCompleted,QuestName)) // if has completed quest
				{
					if (MyNext.Count >= 1)
						return MyNext[2];
				} else if (ListContains(QuestsGiven,QuestName)) {
					if (MyNext.Count >= 2)
						return MyNext[1];
				} else {
					if (MyNext.Count >= 1)
						return MyNext[0];
					else
						return 0;
				}
			}
			
			// Activate a function
			if (FunctionName != "") {
				// ie disable bot - kill bot etc - or Animate.smile kinda thing
			}
			if (Value >= 0 && Value < MyNext.Count)
			return MyNext[Value];	// value is the option chosen - default is 0
			
			Debug.LogError ("Problem with MyNext List!");
			Debug.LogError ("Value: " + Value + " And MyNext Size: " + MyNext.Count);
			return 0;
		}
		
		public bool IsEmptyChat() {
			if (SpeechDialogue == "" && ReverseDialogueLines.Count == 0)
				return true;
			return false;
		}
		// need to add a variable for each response
		// or a unity function reference here, so one can be like, 'i would like to trade'->open trade window

		public bool HasOptions() {
			if (ReverseDialogueLines.Count > 1)
				return true;
			else
				return false;
		}
		
		// String reading shit!!
		public DialogueLine(List<string> SavedData, int NextCount, string CharacterName) {
			MyNext.Add (NextCount);	// default pointer to next dialogue line
			for (int i = 0; i < SavedData.Count; i++) {
				//Debug.LogError("Reading: " + SavedData[i]);
				if (SavedData[i].Contains ("/"+CharacterName)) {
					SavedData[i] = RemoveCommand(SavedData[i], "/"+CharacterName);
					// if (SpeechDialogue != "") MyDialogueGroup.CreateNewDialogueLine();
					SpeechDialogue = SavedData[i];
				}
				// gives a quest to the player
				else if (SavedData[i].Contains ("/givequest "))
				{
					if (QuestName == "") 
					{
						SavedData[i] = RemoveCommand(SavedData[i], "/givequest ");
						IsQuestGive = true;
						QuestName = SavedData[i];
					}
				}  
				// if player has finished a quest, rewards them and removes the quest here!
				else if (SavedData[i].Contains ("/checkquest "))
				{
					if (QuestName == "") 
					{
						SavedData[i] = RemoveCommand(SavedData[i], "/checkquest ");
						IsQuestCheck = true;
						QuestName = SavedData[i];
						//QuestIndex = int.Parse(SavedData[i])-1;
					}
				}  
				else if (SavedData[i].Contains ("/execute "))
				{
					if (FunctionName == "") 
					{
						SavedData[i] = RemoveCommand(SavedData[i], "/execute ");
						// seperate this like - ComponentName.FunctionName !!
						FunctionName = SavedData[i];
						//QuestIndex = int.Parse(SavedData[i])-1;
					}
				}
				// can specify a different route for the dialogue tree
				else if (SavedData[i].Contains ("/next "))
				{
					//Debug.LogError("Dat: " + SavedData[i]);
					SavedData[i] = RemoveCommand(SavedData[i], "/next ");
					List<int> MyInts = GetInts(SavedData[i]);
					
					if (MyInts.Count == 1) 
					{
						MyNext[0] = MyInts[0]-1;
					}
					else {
						if (MyInts.Count > 1)
							MyNext.Clear();
						for (int j = 0; j < MyInts.Count; j++) {
							MyNext.Add (MyInts[j]-1);
							//Debug.LogError("Dat$: " + MyInts[j]);
						}
					}
				}
				// ends the dialogue
				else if (SavedData[i].Contains("/exit"))
				{
					IsExitChat = true;
				}
				// if the first time talking to the npc, it will give a different route
				else if (SavedData[i].Contains("/first"))
				{
					SavedData[i] = RemoveCommand(SavedData[i], "/first");
					IsFirstMyNext = int.Parse(SavedData[i])-1;
					//if (SavedData[i] != "")
					//	MyNext.Add (int.Parse(SavedData[i])-1);
					bIsFirst = true;
				}
				// randomizes the next dialogue
				else if (SavedData[i].Contains("/randomize")) 
				{
					bIsRandomizePath = true;
				}
				
				else 
				{
					if (!SpeechFileReader.IsEmptyLine(SavedData[i]))
					{
						ReverseDialogueLines.Add(SavedData[i]);
						//if (MyResponseLines.Count == 4)
						//	Debug.LogError("Count: " + SavedData[i].Length + " Type: " + (int)(SavedData[i][0]));
					}
				}
				//Debug.LogError("Finished: " + SavedData[i]);
			}
		}
	}
}