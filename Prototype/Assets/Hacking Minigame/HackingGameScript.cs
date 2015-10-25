using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HackingGameScript : MonoBehaviour
{
	int MAX_GUESSES = 20, CODE_LENGTH = 4;
	int?[] code = new int?[4], hiddenCode = new int?[4];
	Vector3[] position = new Vector3[20];
	public Text text;
	public PreviousGuessScript guessRecord;
	public Transform parent;
	string codeDisplayed;
	int guessCount = 0, rightNumberRightPosition = 0, rightNumberWrongPosition = 0 ;
	public StartGame sg;
	
	void Start ()
	{
		parent.GetComponent<Transform> ();
		guessRecord.GetComponent <GameObject> ();
		int i = 0;
		while (i<CODE_LENGTH) {
			code [i] = null;
			hiddenCode [i] = null;
			i++;
		}
		FillPosition ();
		GenerateHiddenCode ();
		sg.MinigameWon ();
	}

	private void GenerateHiddenCode ()
	{
		for (int i=0; i<CODE_LENGTH; i++) {
			while (true) {
				int randNum = Random.Range (0, 9);
				if (!CheckForDuplicates (hiddenCode, randNum)) {
					hiddenCode [i] = randNum;
					break;
				}
			}
		}
	}

	private void FillPosition ()
	{
		Vector3 v3 = new Vector3 (-100, 125, 0);
		for (int i = 0; i<MAX_GUESSES; i++) {
			position [i] = v3;
			if (i != 9) {
				v3.y = v3.y + 35;
			} else {
				v3.y = 125;
				v3.x = 100;
			}
		}
	}

	public void NumButtonPressed (int numPressed)
	{
		if (!CheckForDuplicates (code, numPressed)) {
			for (int i=0; i<CODE_LENGTH; i++) {
				if (code [i] == null) {
					code [i] = numPressed;
					break;
				} else if (i == CODE_LENGTH-1) {
					Debug.Log ("\nCode cannot add another letter");
				}
			}
		} else {
			Debug.Log ("\nCode cannot contain duplicates");
		}
		RefreshInputText ();
	}

	private bool CheckForDuplicates (int?[] array, int possibleDuplicate)
	{
		for (int i=0; i<CODE_LENGTH; i++) {
			if (array [i] == possibleDuplicate) {
				return true;
			}
		}
		return false;
	}

	public void BackspaceButtonPressed ()
	{
		for (int i=0; i<CODE_LENGTH; i++) {
			if (code [i] == null) {
				if (i > 0) {
					i--;
					code [i] = null;
				}
				break;
			}
		}
		if (code [CODE_LENGTH-1] != null) {
			code [CODE_LENGTH-1] = null;
		}
		RefreshInputText ();
	}

	public void EnterButtonPressed ()
	{
		rightNumberRightPosition = 0;
		rightNumberWrongPosition = 0;
		if (code [CODE_LENGTH-1] != null) {
			if (guessCount < MAX_GUESSES) {
				for (int i=0; i<CODE_LENGTH; i++) { //i hiddenCode, j code
					for (int j=0; j<CODE_LENGTH; j++) { 
						if (i == j) {
							if (hiddenCode [i] == code [j]) {
								rightNumberRightPosition++;
							}
						} else {
							if (hiddenCode [i] == code [j]) {
								rightNumberWrongPosition++;
							}
						}
					}
				}
				if (rightNumberRightPosition == CODE_LENGTH) {
					GameWon();
				} else {
					PreviousGuessScript clone = Instantiate (guessRecord);
					clone.transform.parent = parent;
					clone.setPosition (position [guessCount]);
					clone.setText (codeDisplayed);
					clone.setLight1 (GetColour ());
					clone.setLight2 (GetColour ());
					clone.setLight3 (GetColour ());
					clone.setLight4 (GetColour ());
					guessCount++;
					for (int i = 0; i<CODE_LENGTH; i++) {
						code [i] = null;
					}
					RefreshInputText ();
					if (guessCount == MAX_GUESSES) {
						GameLost();
					}
				}
			} else {
				Debug.Log ("Exceeded Max number of Guesses");
			}
		} else {
			Debug.Log ("Must Input 4 Digit Code");
		}
	}

	private string GetColour ()
	{
		if (rightNumberRightPosition > 0) {
			rightNumberRightPosition--;
			return "green";
		} else if (rightNumberWrongPosition > 0) {
			rightNumberWrongPosition--;
			return "blue";
		}
		return "red";
	}

	private void WriteCodeString ()
	{
		if (code [0] != null) {
			codeDisplayed = "" + code [0];
			for (int i=1; i<CODE_LENGTH; i++) {
				if (code [i] != null) {
					codeDisplayed = codeDisplayed + code [i];					
				}
			}
		} else {
			codeDisplayed = "";
		}
	}

	private void RefreshInputText ()
	{
		WriteCodeString ();
		text.text = codeDisplayed;
	}

	private void GameWon(){
		sg.MinigameWon ();
	}

	private void GameLost(){
		sg.MinigameLost ();
	}
}
