using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* Dialogue system
	Once initialized, a certain player will start the talking
	MainCharacter -> Lotus etc
	(the idea is that characters will initialize their own dialogue with anyone)
	One of the Speech Handlers are used as the client, and the other is the server
	
*/
namespace DialogueSystem {
	public class SpeechHandler : MonoBehaviour {
		private bool HasLoaded = false;	// used to check if the file has loaded - io
		// all the dialogue data
		public int DialogueIndex = 0;
		public List<DialogueLine> MyDialogues = new List<DialogueLine> ();
		// animates the text
		private SpeechAnimator MySpeechAnimator;
		// true = text is animating - false = text is not animating
		private bool CanUpdateDialogue = true;	
		// text file name! without the .txt extension
		public string MyFile = "";
		// the 2 characters involved in the conversation! (more in the future)
		private Character MyCharacter;
		private Character MyCharacter2;
		public bool IsActive = false;
		// used to understand what speech handler is dictating the conversation - one initiator and one reciever
		private bool IsFirstTalker = false;
		// counts how many times someone has talked to the reciever
		private int ChattedCount = 0;

		public bool IsTalking() {
			return IsActive;
		}
		public Character GetMainTalker() {
			return MyCharacter;
		}
		public void SetMainTalker() {
			IsFirstTalker = true;
		}
		public void SetSecondaryTalker() {
			IsFirstTalker = false;
		}
		public void SetCharacter(Character MyCharacter_) {
			MyCharacter = MyCharacter_;
		}
		public void SetCharacter2(Character MyCharacter2_) {
			MyCharacter2 = MyCharacter2_;
		}

		// Use this for initialization
		void Awake () {
			if (MyFile != "") {
				//SpeechData.PrintText("Loading from: " + MyFile);
				HasLoaded = SpeechData.ReadDialogue(MyFile, this);
			}
		}

		public void Activate() {
			DialogueIndex = 0;
			IsActive = true;
			gameObject.GetComponent<Text> ().text = "";
			CheckForEmptyDialogue();
			UpdateSpeech ();
		}
		void Update() {
			if (IsActive) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					NextLine ();
				}
				if (Input.GetKeyDown (KeyCode.E)) {
					ExitChat ();
				}
			}
		}
		public void BeginSpeech(SpeechHandler MySpeech2, bool IsFirstSpeaker) {

		}

		public void ExitChat() {
			IsActive = false;
			ToggleSpeech (false);
			ResetAll ();
			ChattedCount++;
			MyCharacter.OnEndDialogue();
			MyCharacter2.OnEndDialogue();
		}

		public void ResetAll() {
			DialogueIndex = 0;
			gameObject.GetComponent<Text> ().text = "";
		}
		public void NextLine() {
			NextLine (0, false);
		}
		public void NextLine(int Value) {
			NextLine (Value, true);
		}
		public void NextLine(int Value, bool IsOption) {
			//Debug.LogError ("Next line beg : " + IsActive + " : " + gameObject.name);
			if (!IsFirstTalker) {
				MyCharacter2.MySpeechBubble.CanUpdateDialogue = CanUpdateDialogue;
				MyCharacter2.MySpeechBubble.NextLineDo(Value, IsOption);
			} else {
				NextLineDo(Value, IsOption);
			}
		}
		public List<int> QuestsCompleted = new List<int>();	// indexes of quests completed
		public List<int> QuestsGiven = new List<int>();	// indexes of quests completed
		
		public void NextLineDo(int Value, bool IsOption) {
			if (CanUpdateDialogue)
			if (MyDialogues.Count > 0 && DialogueIndex < MyDialogues.Count) {
				{
					DialogueLine MyDialogueLine = GetCurrentDialogue ();
					if (MyDialogueLine.RespondType != ResponseType.Next && !IsOption)
						return;
					//Debug.LogError ("Nextline doing : " + DialogueIndex);
					// quests
					//Debug.LogError("Adding quest: " + QuestIndex);
					if (IsFirstTalker) 
					{
						// reciever gives the quest to the initiator - i.e. player recieves the quest off the npc
						if (MyDialogueLine.IsQuestGive) 
						{
							if (MyCharacter)
							{
								int QuestIndex = MyDialogueLine.QuestIndex;
								int MyQuestGivenIndex = MyCharacter.GiveCharacterQuest(QuestIndex, MyCharacter2);
								if (MyQuestGivenIndex != -1) 
								{
									QuestsGiven.Add(MyQuestGivenIndex);
								}
							}
						} 
						// checking from recieving speech handler - handing the quest in
						else if (MyDialogueLine.IsQuestCheck)
						{
							if (MyCharacter2) 
							{
								int QuestIndex = MyDialogueLine.QuestIndex;
								int MyQuestCompletedIndex = MyCharacter2.RemoveQuest(QuestIndex, MyCharacter);
								if (MyQuestCompletedIndex != -1) 
								{
									QuestsCompleted.Add(QuestIndex);
								}
							}
						}
						
						if (MyDialogueLine.IsExitChat) 
						{
							ExitChat ();
							return;
						}
					}
					DialogueIndex = MyDialogueLine.GetNextLine ((ChattedCount == 0), Value, QuestsGiven, QuestsCompleted);
					CheckForEmptyDialogue(Value);
					UpdateSpeech ();
				}
			}
		}

		void CheckForEmptyDialogue() {
			CheckForEmptyDialogue (0, 0);
		}
		void CheckForEmptyDialogue(int Value) {
			CheckForEmptyDialogue (Value, 0);
		}
		void CheckForEmptyDialogue(int Value, int ChecksNumber) {
			if (MyDialogues [DialogueIndex].IsEmptyChat ()) {
				Debug.Log ("Skipping Dialogue: " + DialogueIndex);
				DialogueIndex = MyDialogues [DialogueIndex].GetNextLine ((ChattedCount == 0), Value, QuestsGiven, QuestsCompleted);
				ChecksNumber++;
				if (ChecksNumber < 1000)
					CheckForEmptyDialogue();
			}
		}
		// 0 to 3, for the various options

		public DialogueLine GetCurrentDialogue() {
			if (DialogueIndex >= 0 && DialogueIndex < MyDialogues.Count)
				return MyDialogues [DialogueIndex];
			else
				return new DialogueLine ();
		}
		// uses the dialogue index, and updates the gui with dialogue data
		private void UpdateSpeech() {
			if (DialogueIndex >= 0 && DialogueIndex < MyDialogues.Count)
				UpdateDialogue (MyDialogues [DialogueIndex]);
		}


		private void UpdateDialogue(DialogueLine NewDialogue) {
			if (CanUpdateDialogue) {
				if (NewDialogue.IsReverseSpeech())
				{
					ToggleSpeech(false, true);
					MyCharacter2.MySpeechBubble.gameObject.GetComponent<SpeechAnimator> ().NewLine (NewDialogue.GetReverseSpeechDialogue(ChattedCount == 0));
					MyCharacter2.MySpeechBubble.DeactivateChildren ();
					MyCharacter2.MySpeechBubble.AddAnimationListener();
					MyCharacter2.MySpeechBubble.CanUpdateDialogue = false;
				} else {
					ToggleSpeech(true, false);
					gameObject.GetComponent<SpeechAnimator> ().NewLine (NewDialogue.GetSpeechDialogue(ChattedCount == 0));
					DeactivateChildren ();
					// this way the handler knowns when the text has finished animation - stops skipping to next line
					gameObject.GetComponent<SpeechAnimator> ().OnFinishedAnimationFunction.AddListener(UpdateRespondType);
					CanUpdateDialogue = false;
				}
			}
		}

		public void AddAnimationListener() {
			gameObject.GetComponent<SpeechAnimator> ().OnFinishedAnimationFunction.AddListener(UpdateRespondType);
		}

		public void ToggleSpeech(bool IsSpeech) {
			ToggleSpeech (IsSpeech, IsSpeech);
		}

		public void ToggleSpeech(bool IsSpeech, bool IsSpeech2) {
			gameObject.transform.parent.gameObject.SetActive (IsSpeech);
			Transform MyCharacterLabel = gameObject.transform.parent.parent.FindChild ("Label");
			if (MyCharacterLabel != null) {
				MyCharacterLabel.gameObject.SetActive(!IsSpeech);
			}
			IsActive = IsSpeech;
			
			if (MyCharacter2) {
				MyCharacter2.MySpeechBubble.IsActive = IsSpeech2;
				MyCharacter2.MySpeechBubble.MyCharacter2 = MyCharacter;
				MyCharacter2.MySpeechBubble.gameObject.transform.parent.gameObject.SetActive (IsSpeech2);
				Transform MyCharacterLabel2 = MyCharacter2.MySpeechBubble.transform.parent.parent.FindChild ("Label");
				if (MyCharacterLabel2 != null) {
					MyCharacterLabel2.gameObject.SetActive (!IsSpeech2);
				}
			}
		}

		private void UpdateRespondType() {
			//Debug.LogError ("Finished animating text.");
			UpdateRespondType(GetCurrentDialogue());
		}

		// handles the various setups for responses
		private void UpdateRespondType(DialogueLine NewDialogue) {
			DeactivateChildren ();
			switch (NewDialogue.RespondType){
			case(ResponseType.Next):
				transform.FindChild("NextButton").gameObject.SetActive(true);
				break;
			case(ResponseType.YesNo):
				transform.FindChild("YesButton").gameObject.SetActive(true);
				transform.FindChild("NoButton").gameObject.SetActive(true);
				break;
			case(ResponseType.Options4):
				for (int i = 0; i < NewDialogue.MyResponseLines.Count; i++) {
					transform.FindChild("Option"+(i+1)).gameObject.SetActive(true);
					transform.FindChild("Option"+(i+1)).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = NewDialogue.MyResponseLines[i];
				}
				break;
			}
			CanUpdateDialogue = true;
		}

		private void DeactivateChildren() {
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}