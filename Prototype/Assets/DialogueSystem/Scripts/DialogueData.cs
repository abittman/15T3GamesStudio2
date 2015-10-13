﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
	Stores data of the dialogue lines
	Has a function to convert command lines into dialogue data
*/


namespace DialogueSystem {

	public enum ResponseType {
		Next,
		YesNo,
		Options4,
		Options2,
		End
	}

	// ramblings
	// if character has completed a condition, move to MyNext 0, if not move to 1

	[System.Serializable]
	public class DialogueLine {
		//public int NextDialogueLine = 0;
		public List<int> MyNext = new List<int>();		// index for each response
		//public string SpeechDialogue;
		//public List<string> SpeechDialogue = new List<string>();
		public string SpeechDialogue = "";
		public string ReverseSpeechDialogue = "";
		//public List<string> ReverseSpeechDialogue = new List<string>();
		public ResponseType RespondType;	// 0 is next, 1 is yes or no, 2 is multiple options
		public List<string> MyResponseLines = new List<string>();
		public bool IsQuestGive = false;
		public bool IsQuestCheck = false;
		public int QuestIndex = -1;
		public bool IsExitChat = false;
		public bool bIsFirst = false;	// makes an alternative speech option
		public bool bIsRandomizePath = false;

		public DialogueLine() {

		}
		
		public string GetReverseSpeechDialogue(bool IsFirst) {
			return ReverseSpeechDialogue;
			/*
			if (bIsFirst) {
				if (!IsFirst && ReverseSpeechDialogue.Count >= 2)
					return ReverseSpeechDialogue[1];
			}
			return ReverseSpeechDialogue[0];*/
		}
		public string GetSpeechDialogue(bool IsFirst) {
			// if no speech!
			return SpeechDialogue;
			/*if (SpeechDialogue.Count == 0)
				return "";
			
			if (!bIsFirst && !bIsRandomizePath) // if no conditions for dialogue
			{
				return SpeechDialogue [0];
			}

			if ((IsFirst && IsFirst) || SpeechDialogue.Count == 1) 
			{
				return SpeechDialogue [0];
			}

			else if (!IsFirst && IsFirst)) 
			{
				return SpeechDialogue[Random.Range(1, SpeechDialogue.Count)];
			} 
			else 
			{
				return SpeechDialogue[Random.Range(0, SpeechDialogue.Count)];
			}
			return SpeechDialogue[1];*/
		}

		public DialogueLine(List<string> SavedData, int NextCount, string CharacterName) {
			MyNext.Add (NextCount);	// default pointer to next dialogue line
			for (int i = 0; i < SavedData.Count; i++) {
				//Debug.LogError("Reading: " + SavedData[i]);
				if (SavedData[i].Contains ("/"+CharacterName)) {
					SavedData[i] = RemoveCommand(SavedData[i], "/"+CharacterName);
					// if (SpeechDialogue != "") MyDialogueGroup.CreateNewDialogueLine();
					SpeechDialogue = SavedData[i];

				} 
				// uses reverse dialogue
				else if (SavedData[i].Contains ("/me")) 
				{
					SavedData[i] = RemoveCommand(SavedData[i], "/me");
					ReverseSpeechDialogue = (SavedData[i]);
				} 
				// gives a quest to the player
				else if (SavedData[i].Contains ("/givequest"))
				{
					if (QuestIndex == -1) {
						SavedData[i] = RemoveCommand(SavedData[i], "/givequest");
						IsQuestGive = true;
						QuestIndex = int.Parse(SavedData[i])-1;
					}
				}  
				// if player has finished a quest, rewards them and removes the quest here!
				else if (SavedData[i].Contains ("/checkquest"))
				{
					if (QuestIndex == -1) {
						SavedData[i] = RemoveCommand(SavedData[i], "/checkquest");
						IsQuestCheck = true;
						QuestIndex = int.Parse(SavedData[i])-1;
					}
				}  
				// gives the player different response options
				else if (SavedData[i].Contains ("/options "))
				{
					SavedData[i] = RemoveCommand(SavedData[i], "/options ");

					if (int.Parse(SavedData[i]) == 2) {
						RespondType = ResponseType.YesNo;
					} else if (int.Parse(SavedData[i]) > 2) {
						RespondType = ResponseType.Options4;
					}
				}
				// can specify a different route for the dialogue tree
				else if (SavedData[i].Contains ("/next "))
				{
					SavedData[i] = RemoveCommand(SavedData[i], "/next ");
					//Debug.LogError("Dat: " + SavedData[i]);
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
					if (SavedData[i] != "")
						MyNext.Add (int.Parse(SavedData[i])-1);
					bIsFirst = true;
				}
				// randomizes the next dialogue
				else if (SavedData[i].Contains("/randomize")) 
				{
					bIsRandomizePath = true;
				}

				else 
				{
					if (!SpeechData.IsEmptyLine(SavedData[i]))
					{
						MyResponseLines.Add(SavedData[i]);
						//if (MyResponseLines.Count == 4)
						//	Debug.LogError("Count: " + SavedData[i].Length + " Type: " + (int)(SavedData[i][0]));
					}
				}
				//Debug.LogError("Finished: " + SavedData[i]);
			}
		}
		public List<int> GetInts(string MyIntsString) {
			string[] MyInts = MyIntsString.Split(' ');
			List<int> NewInts = new List<int> ();
			if (MyInts != null)
			for (int j = 0; j < MyInts.Length; j++) {
				if (MyInts[j].Length > 0) {
					if (MyInts[j].Contains(","))
					MyInts[j] = MyInts[j].Remove(MyInts[j].IndexOf(","));
					/*for (int k = MyInts[j].Length-1; k >= 0; k--) {
						if (MyInts[j][k] == ',')
							MyInts[j].Remove (k);
					}*/
					//Debug.LogError("Trying to read:" + MyInts[j]);
					if (MyInts[j] != "")
						NewInts.Add(int.Parse(MyInts[j]));
				}
			}
			return NewInts;
		}

		public bool IsReverseSpeech() {
			return (ReverseSpeechDialogue != "");
		}

		public string RemoveCommand(string FullString, string CommandCode) {
			int IndexToDelete = FullString.IndexOf(CommandCode);
			FullString = FullString.Remove(0,IndexToDelete+CommandCode.Length);
			return FullString;
		}

		public int GetNextLine(bool IsFirst, int Value, List<int> QuestsGiven, List<int> QuestsCompleted) {
			if (IsQuestCheck) {
				if (QuestsCompleted.Contains(QuestIndex)) // if has completed quest
				{
					if (MyNext.Count >= 1)
						return MyNext[2];
				} else if (QuestsGiven.Contains (QuestIndex)) {
					if (MyNext.Count >= 2)
						return MyNext[1];
				}
			}
			switch (RespondType) {
			case(ResponseType.Next):
				if (bIsFirst && IsFirst) {
					if (MyNext.Count > 1)
						return MyNext[1];
				}
				return MyNext[0];
			case(ResponseType.YesNo):
				if (Value >= 0 && Value < MyNext.Count)
					return MyNext[Value];
				break;
			case(ResponseType.Options4):
				if (Value >= 0 && Value < MyNext.Count)
					return MyNext[Value];
				break;
			}
			return 0;
		}
		public bool IsEmptyChat() {
			if (SpeechDialogue == "" && ReverseSpeechDialogue == "")
				return true;
			return false;
		}
		// need to add a variable for each response
		// or a unity function reference here, so one can be like, 'i would like to trade'->open trade window
	}
}