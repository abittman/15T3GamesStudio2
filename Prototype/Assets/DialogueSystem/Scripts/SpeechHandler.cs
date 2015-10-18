using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// dialogue system

public enum ResponseType {
	Next,
	YesNo,
	Options4,
	Options2,
	End
}
[System.Serializable]
public class DialogueLine {
	public int NextDialogueLine = 0;
	public string SpeechDialogue;
	public ResponseType RespondType;	// 0 is next, 1 is yes or no, 2 is multiple options
	public List<string> MyResponseLines = new List<string>();
	public List<int> MyNextDialogues = new List<int>();		// index for each response

	public int GetNextLine(int Value) {
		switch (RespondType) {
			case(ResponseType.Next):
				return NextDialogueLine;
			break;
		case(ResponseType.YesNo):
			if (Value >= 0 && Value < MyNextDialogues.Count)
				return MyNextDialogues[Value];
			break;
		case(ResponseType.Options4):
			if (Value >= 0 && Value < MyNextDialogues.Count)
				return MyNextDialogues[Value];
			break;
		}
		return 0;
	}
	// need to add a variable for each response
	// or a unity function reference here, so one can be like, 'i would like to trade'->open trade window
}


public class SpeechHandler : MonoBehaviour {
	public List<DialogueLine> MyDialogues = new List<DialogueLine> ();
	private SpeechAnimator MySpeechAnimator;
	public int DialogueIndex = 0;
	private bool CanUpdateDialogue = true;	// used to make sure  the dialogue doesn't update when its still animating

	// Use this for initialization
	void Awake () {
		UpdateSpeech ();
	}
	public void ResetAll() {
		DialogueIndex = 0;
		UpdateSpeech ();
	}
	
	public void NextLine() {
		if (MyDialogues.Count > 0)
		if (DialogueIndex < MyDialogues.Count) {
			//if (CharacterIndex == MySpeech.Length) // has animation finished
			{
				DialogueIndex = GetCurrentDialogue().GetNextLine(0);
				UpdateSpeech ();
			}
		}
	}

	// 0 to 3, for the various options
	public void NextLine(int Value) {
		if (MyDialogues.Count > 0)
		if (DialogueIndex < MyDialogues.Count) {
			//if (CharacterIndex == MySpeech.Length) // has animation finished
			{
				DialogueIndex = GetCurrentDialogue().GetNextLine(Value);
				UpdateSpeech ();
			}
		}
	}

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
			CanUpdateDialogue = false;
			if (gameObject.GetComponent<SpeechAnimator> () == null) {
				gameObject.GetComponent<Text> ().text = NewDialogue.SpeechDialogue;
				UpdateRespondType (NewDialogue);
			} else {
				gameObject.GetComponent<SpeechAnimator> ().NewLine (NewDialogue.SpeechDialogue);
				DeactivateChildren ();
				gameObject.GetComponent<SpeechAnimator> ().OnFinishedAnimationFunction.AddListener(UpdateRespondType);
			}
		}
	}

	private void UpdateRespondType() {
		UpdateRespondType(GetCurrentDialogue());
	}

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
			transform.FindChild("Option1").gameObject.SetActive(true);
			transform.FindChild("Option2").gameObject.SetActive(true);
			transform.FindChild("Option3").gameObject.SetActive(true);
			transform.FindChild("Option4").gameObject.SetActive(true);
			transform.FindChild("Option1").gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = NewDialogue.MyResponseLines[0];
			transform.FindChild("Option2").gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = NewDialogue.MyResponseLines[1];
			transform.FindChild("Option3").gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = NewDialogue.MyResponseLines[2];
			transform.FindChild("Option4").gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = NewDialogue.MyResponseLines[3];
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
