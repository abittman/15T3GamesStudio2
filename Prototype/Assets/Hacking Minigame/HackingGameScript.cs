using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HackingGameScript : MonoBehaviour
{
	int?[] code = new int?[4], hiddenCode = new int?[4];
	Vector3[] position = new Vector3[20];
	public Text text;
	public PreviousGuessScript guessRecord;
	public Transform parent;
	string codeDisplayed;
	int guessCount = 0;
	int rightNumberRightPosition = 0, rightNumberWrongPosition = 0 ;
	private Door thisDoor;
	
	void Start ()
	{
		parent.GetComponent<Transform> ();
		guessRecord.GetComponent <GameObject> ();
		int i = 0;
		while (i<4) {
			code [i] = null;
			hiddenCode [i] = null;
			i++;
		}
		fillPosition ();
		generateHiddenCode ();

	}

	private void generateHiddenCode ()
	{
		for (int i=0; i<4; i++) {
			while (true) {
				int randNum = Random.Range (0, 9);
				if (!checkForDuplicates (hiddenCode, randNum)) {
					hiddenCode [i] = randNum;
					break;
				}
			}
		}
	}

	private void fillPosition ()
	{
		Vector3 v3 = new Vector3 (-100, 125, 0);
		for (int i = 0; i<20; i++) {
			position [i] = v3;
			if (i != 9) {
				v3.y = v3.y + 35;
			} else {
				v3.y = 125;
				v3.x = 100;
			}
		}
	}

	public void numButtonPressed (int numPressed)
	{
		if (!checkForDuplicates (code, numPressed)) {
			for (int i=0; i<4; i++) {
				if (code [i] == null) {
					code [i] = numPressed;
					break;
				} else if (i == 3) {
					Debug.Log ("\nCode cannot add another letter");
				}
			}
		} else {
			Debug.Log ("\nCode cannot contain duplicates");
		}
		refreshInputText ();
	}

	private bool checkForDuplicates (int?[] array, int possibleDuplicate)
	{
		for (int i=0; i<4; i++) {
			if (array [i] == possibleDuplicate) {
				return true;
			}
		}
		return false;
	}

	public void backspaceButtonPressed ()
	{
		for (int i=0; i<4; i++) {
			if (code [i] == null) {
				if (i > 0) {
					i--;
					code [i] = null;
				}
				break;
			}
		}
		if (code [3] != null) {
			code [3] = null;
		}
		refreshInputText ();
	}

	public void enterButtonPressed ()
	{
		rightNumberRightPosition = 0;
		rightNumberWrongPosition = 0;
		if (code [3] != null) {
			if (guessCount < 20) {
				for (int i=0; i<4; i++) { //i hiddenCode, j code
					for (int j=0; j<4; j++) { 
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
				if (rightNumberRightPosition == 4) {
					//Win. Send result, delete Canvas
					thisDoor.unlockDoor ();
					Debug.Log ("You win!");
				} else {
					PreviousGuessScript clone = Instantiate (guessRecord);
					clone.transform.parent = parent;
					clone.setPosition (position [guessCount]);
					clone.setText (codeDisplayed);
					clone.setLight1 (getColour ());
					clone.setLight2 (getColour ());
					clone.setLight3 (getColour ());
					clone.setLight4 (getColour ());
					guessCount++;
					for (int i = 0; i<4; i++) {
						code [i] = null;
					}
					refreshInputText ();
					if (guessCount == 20) {
						//Loss Send result, delete Canvas
						thisDoor.lockDoor ();
						Debug.Log ("You have lost.");
					}
				}
			} else {
				Debug.Log ("Exceeded Max number of Guesses");
			}
		} else {
			Debug.Log ("Must Input 4 Digit Code");
		}
	}

	private string getColour ()
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

	private void writeCodeString ()
	{
		if (code [0] != null) {
			codeDisplayed = "" + code [0];
			for (int i=1; i<4; i++) {
				if (code [i] != null) {
					codeDisplayed = codeDisplayed + code [i];					
				}
			}
		} else {
			codeDisplayed = "";
		}
	}

	private void refreshInputText ()
	{
		writeCodeString ();
		text.text = codeDisplayed;
	}

	public void setDoor(Door door){
		thisDoor = door;
	}
}
