using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PreviousGuessScript : MonoBehaviour {
	public Image light1, light2, light3, light4;
	public Text codeText;
	public Transform me;

	// Use this for initialization
	void Start () {
		light1.GetComponent <Image> ();
		light2.GetComponent <Image> ();
		light3.GetComponent <Image> ();
		light4.GetComponent <Image> ();
		codeText.GetComponent <Text> ();
	}
	public void setLight1(string colour){
		light1.color = getColour (colour);
	}
	public void setLight2(string colour){
		light2.color = getColour (colour);
	}
	public void setLight3(string colour){
		light3.color = getColour (colour);
	}
	public void setLight4(string colour){
		light4.color = getColour (colour);
	}
	public void setText(string text){
		codeText.text = text;
	}

	public void setPosition(Vector3 position){
		me.localPosition = position;
	}

	private Color getColour(string colour){
		if (colour == "green") {
			return Color.green;
		} else if (colour == "blue") {
			return Color.blue;
		} else {
			return Color.red;
		}
	}
}
