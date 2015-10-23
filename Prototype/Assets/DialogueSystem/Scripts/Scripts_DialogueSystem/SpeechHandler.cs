using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* Speech Handler - Attach to bot
 * Link it to the Speech Gameobject(with Text on it)
	Once initialized, a certain player will start the talking
	MainCharacter -> Lotus etc
	(the idea is that characters will initialize their own dialogue with anyone)
	One of the Speech Handlers are used as the client, and the other is the server

	Depends on scripts:
		Character
		SpeechAnimator	(GuiUtilities)
		DialogueLine	(Data Format)
		SpeechFileReader(Static Reader)
		List			(Unity)
*/

namespace DialogueSystem {
	public class SpeechHandler : MonoBehaviour {
		[Header("Speechness")]
		[Tooltip("Link to a gameObject with a Text Component. If blank, this script won't work.")]
		public Text MyDialogueText;		// used to display the text in the chat - and toggle it on/off
		public bool IsOpen = false;	// whether the dialogue is currently opened or not
		public bool HasLoaded = false;	// used to check if the file has loaded - io
		public int ChattedCount = 0;			// counts how many times someone has talked to the reciever

		// all the dialogue data
		private string MyFile = "";
		private int DialogueIndex = 0;
		public List<DialogueLine> MyDialogues = new List<DialogueLine> ();
		// animates the text
		private SpeechAnimator MySpeechAnimator;
		// true = text is animating - false = text is not animating
		private bool CanUpdateDialogue = true;	
		// text file name! without the .txt extension
		// the 2 characters involved in the conversation! (more in the future)
		private Character MyCharacter;	// set in /awake - attached to same gameobject as this class
		private Character MyCharacter2;

		private bool IsFirstTalker = false;		// used to understand what speech handler is dictating the conversation - one initiator and one reciever
		private bool IsOptions = false;	// used primarily in secondary speaker, to check if its options
		private bool IsTalking = false;

		[Header("Quests")]
		// Quests
		// names of quests completed and given
		public List<string> QuestsCompleted = new List<string>();	// indexes of quests completed
		public List<string> QuestsGiven = new List<string>();	// indexes of quests completed
		
		public QuestLog GetMainQuestLog() {
			return MyCharacter.gameObject.GetComponent<QuestLog>();
		}
		public QuestLog GetSecondaryQuestLog() {
			return MyCharacter2.gameObject.GetComponent<QuestLog>();
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
			//MyCharacter = MyCharacter_;
		}
		public void SetCharacter2(Character MyCharacter2_) {
			MyCharacter2 = MyCharacter2_;
		}

		// empties the data
		public void Clear() {
			MyDialogues.Clear ();
		}
		public void AddDialogue(DialogueLine NewDialogue) {
			MyDialogues.Add (NewDialogue);
		}
		public int DialogueSize() {
			return MyDialogues.Count;
		}

		public bool CanTalk() 
		{
			return !IsTalking;
		}

		// Use this for initialization
		void Awake () {
			MyFile = gameObject.name;
			MyCharacter = gameObject.GetComponent<Character> ();
			if (MyFile != "") {
				//SpeechData.PrintText("Loading from: " + MyFile);
				HasLoaded = SpeechFileReader.ReadDialogue(MyFile, this);
			}
		}

		// example: the player starts the conversation, second character is the npc
		public void StartConversation(Character FirstCharacter, Character SecondCharacter) {
			//Debug.LogError ("Inside of: " + name + " - at: " + Time.time);
			//Debug.LogError ("Char1: " + FirstCharacter.name + " - Char2: " + SecondCharacter.name);
			FirstCharacter.GetSpeechHandler().SetCharacter2(SecondCharacter);
			FirstCharacter.GetSpeechHandler().SetSecondaryTalker();
			FirstCharacter.GetSpeechHandler ().IsTalking = true;

			SetCharacter2(FirstCharacter);
			ToggleSpeech (true, false);
			SetMainTalker();
			Activate ();
		}
		// begins the chat, resets everything
		public void Activate() {
			IsTalking = true;
			DialogueIndex = 0;
			IsOpen = true;
			MyDialogueText.gameObject.GetComponent<Text> ().text = "";
			CheckForEmptyDialogue();
			UpdateSpeech ();
		}
		// ends the chat
		public void ExitChat() {
			IsTalking = false;
			IsOpen = false;
			ToggleSpeech (false);
			ResetAll ();
			ChattedCount++;
			MyCharacter.OnEndDialogue();
			MyCharacter2.OnEndDialogue();
			MyCharacter2.GetSpeechHandler ().IsTalking = false;
		}
		public bool HasOptions() {
			if (IsFirstTalker)
				return (!GetCurrentDialogue ().HasOptions ());
			else
				return !IsOptions;
		}
		void Update() {
			if (IsOpen) {
				if (HasOptions() && Input.GetKeyDown (KeyCode.Space)) {
					NextLine ();
				}
				if (Input.GetKeyDown (KeyCode.E)) {
					ExitChat ();
				}
			}
		}
		public void BeginSpeech(SpeechHandler MySpeech2, bool IsFirstSpeaker) {

		}


		public void ResetAll() {
			DialogueIndex = 0;
			MyDialogueText.text = "";
		}
		public void NextLine() {
			NextLine (0);
		}

		public void NextLine(int Value) {
			//Debug.LogError ("Next line beg : " + IsActive + " : " + gameObject.name);
			if (!IsFirstTalker) {
				MyCharacter2.GetSpeechHandler().CanUpdateDialogue = CanUpdateDialogue;
				MyCharacter2.GetSpeechHandler().NextLineDo(Value);
			} else {
				NextLineDo(Value);
			}
		}
		
		public void NextLineDo(int Value) 
		{
			if (CanUpdateDialogue)
			if (MyDialogues.Count > 0 && DialogueIndex < MyDialogues.Count) {
					DialogueLine MyDialogueLine = GetCurrentDialogue ();
					if (IsFirstTalker) 
					{
						// reciever gives the quest to the initiator - i.e. player recieves the quest off the npc
						if (MyDialogueLine.IsQuestGive) 
						{
							if (MyCharacter)
							{
								//int QuestIndex = MyDialogueLine.QuestIndex;
							string QuestName = GetMainQuestLog().GiveCharacterQuest(MyDialogueLine.QuestName, GetSecondaryQuestLog());
								if (QuestName != "") 
								{
									QuestsGiven.Add(QuestName);
								}
							}
						}
						// checking from recieving speech handler - handing the quest in
						else if (MyDialogueLine.IsQuestCheck)
						{
							if (MyCharacter2) 
							{
								//string QuestName = MyCharacter.MyQuests[MyDialogueLine.QuestIndex].Name;
								int MyQuestCompletedIndex = GetSecondaryQuestLog().RemoveQuest(MyDialogueLine.QuestName, MyCharacter);
								if (MyQuestCompletedIndex != -1) 
								{
									QuestsCompleted.Add(MyDialogueLine.QuestName);
								}
							}
						}
						
						if (MyDialogueLine.IsExitChat) 
						{
							ExitChat ();
							return;
						}
						QuestsGiven.Clear();
						for (int i = 0; i < GetSecondaryQuestLog().MyQuests.Count; i++) {
							QuestsGiven.Add (GetSecondaryQuestLog().MyQuests[i].Name);
						}
					}
					DialogueIndex = MyDialogueLine.GetNextLine ((ChattedCount == 0), Value, QuestsGiven, QuestsCompleted);
					CheckForEmptyDialogue(Value);
					UpdateSpeech ();
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
					MyCharacter2.GetSpeechHandler().MyDialogueText.GetComponent<SpeechAnimator> ().NewLine (NewDialogue.GetReverseSpeechDialogue(ChattedCount == 0));
					MyCharacter2.GetSpeechHandler().DeactivateChildren ();
					MyCharacter2.GetSpeechHandler().AddAnimationListener();
					MyCharacter2.GetSpeechHandler().CanUpdateDialogue = false;
				} else {
					ToggleSpeech(true, false);
					MyDialogueText.gameObject.GetComponent<SpeechAnimator> ().NewLine (NewDialogue.GetSpeechDialogue(ChattedCount == 0));
					DeactivateChildren ();
					// this way the handler knowns when the text has finished animation - stops skipping to next line
					AddAnimationListener();
					//MyDialogueText.gameObject.GetComponent<SpeechAnimator> ().OnFinishedAnimationFunction.AddListener(UpdateRespondType);
					CanUpdateDialogue = false;
				}
				if (NewDialogue.ReverseDialogueLines.Count > 1)
					MyCharacter2.GetSpeechHandler().IsOptions = true;
				else
					MyCharacter2.GetSpeechHandler().IsOptions = false;
			}
		}

		public void AddAnimationListener() {
			MyDialogueText.gameObject.GetComponent<SpeechAnimator> ().OnFinishedAnimationFunction.AddListener(UpdateRespondType);
		}

		public void ToggleSpeech(bool IsSpeech) {
			ToggleSpeech (IsSpeech, IsSpeech);
		}

		public void ToggleSpeech(bool IsSpeech, bool IsSpeech2) {
			MyDialogueText.gameObject.transform.parent.gameObject.SetActive (IsSpeech);
			Transform MyCharacterLabel = MyDialogueText.gameObject.transform.parent.parent.FindChild ("Label");	// a son of my parent is my brother, label is the brother of the speech text
			if (MyCharacterLabel != null) {
				MyCharacterLabel.gameObject.SetActive(!IsSpeech);
			}
			IsOpen = IsSpeech;
			ToggleSpeechBubble2 (IsSpeech2);
		}

		public void ToggleSpeechBubble2(bool IsSpeech2) {
			if (MyCharacter2) {
				MyCharacter2.GetSpeechHandler().IsOpen = IsSpeech2;
				MyCharacter2.GetSpeechHandler().MyCharacter2 = MyCharacter;
				MyCharacter2.GetSpeechHandler().MyDialogueText.gameObject.transform.parent.gameObject.SetActive (IsSpeech2);
				Transform MyCharacterLabel2 = MyCharacter2.GetSpeechHandler().MyDialogueText.transform.parent.parent.FindChild ("Label");
				if (MyCharacterLabel2 != null) {
					MyCharacterLabel2.gameObject.SetActive (!IsSpeech2);
				}
			}
		}
		// called at the end of animating text
		private void UpdateRespondType() {
			//Debug.LogError ("Finished animating text.");
			if (IsFirstTalker)
				UpdateRespondType (GetCurrentDialogue ());
			else {
				MyDialogueText.gameObject.transform.FindChild ("NextButton").gameObject.SetActive (true);
				CanUpdateDialogue = true;
			}
		}

		// handles the various setups for responses
		private void UpdateRespondType(DialogueLine NewDialogue) {
			float BubbleWidth = 100f*1.4f;
			float BubbleHeight = 100f*1.4f;
			RectTransform MyRect = MyDialogueText.GetComponent <RectTransform> ();

			DeactivateChildren ();

			if (NewDialogue.ReverseDialogueLines.Count <= 1) 
			{
				MyDialogueText.gameObject.transform.FindChild("NextButton").gameObject.SetActive(true);
			}
			else
			{
				ToggleSpeechBubble2(true);
				MyCharacter2.GetSpeechHandler().MyDialogueText.GetComponent<SpeechAnimator> ().NewLine (NewDialogue.GetTotalReverseText());
				for (int i = 0; i < NewDialogue.ReverseDialogueLines.Count; i++) 
				{
					GameObject MyBlockThing = MyCharacter2.GetSpeechHandler().MyDialogueText.transform.FindChild("BlockThing" + i).gameObject;
					MyBlockThing.SetActive(true);
					//.MyBlockThing.GetComponent<RawImage>().color = Color.grey;
					MyBlockThing.GetComponent<RawImage>().color = new Color32((byte)(Color.grey.r), 
					                                                          (byte)(Color.grey.g), 
					                                                          (byte)(Color.grey.b), 
					                                                          80);

					GameObject NewChild = new GameObject();
					NewChild.name = "Option"+(i);

					RawImage MyImage = NewChild.AddComponent<RawImage>();

					Button MyButton = NewChild.AddComponent<Button>();
					MyButton.targetGraphic = MyImage;
					ColorBlock MyColors = MyButton.colors;
					MyColors.normalColor = Color.grey;
					MyColors.highlightedColor = Color.cyan;
					MyColors.pressedColor = Color.green;
					MyButton.colors = MyColors;
					int Blarg = i;
					MyButton.onClick.AddListener( () =>{ 
						//Debug.LogError("......Gah.");
						NextLine(Blarg);
					});
					NewChild.transform.position = MyDialogueText.transform.position; 
					NewChild.transform.rotation = Quaternion.identity;	//MyDialogueText.transform.rotation;
					NewChild.transform.localScale = MyDialogueText.transform.localScale;
					Vector3 OffsetPosition = new Vector3(-MyRect.GetSize().x/4f + i*BubbleWidth*2f, -MyRect.GetSize().y/2f-BubbleHeight/2f,0.3f);
					NewChild.transform.position += OffsetPosition;
					NewChild.transform.SetParent(MyDialogueText.transform, false);
				}
			}
			CanUpdateDialogue = true;
		}

		private void DeactivateChildren() {
			for (int i = 0; i < MyDialogueText.gameObject.transform.childCount; i++) {
				GameObject ChildGameObject = MyDialogueText.gameObject.transform.GetChild(i).gameObject;
				if (ChildGameObject.name.Contains("Option"))
					Destroy (ChildGameObject);
				else
					MyDialogueText.gameObject.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}