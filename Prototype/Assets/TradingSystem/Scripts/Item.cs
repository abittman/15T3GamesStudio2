using UnityEngine;
using System.Collections;
// Rarirty of memory
public enum Quality{COMMON,UNCOMMON,RARE,EPIC,LEGENDARY,ARTIFACT};

public class Item : MonoBehaviour {
	public Quality quality;

	public float Sad, Happy, Fear , Confused;

	public string itemName;
	public string description;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetToolTip()
	{
		// Instantiate variables
		string stats = string.Empty;
		string color = string.Empty;
		string newLine = string.Empty;

		// If description contrains string add a new line
		if(description != string.Empty){
			newLine = "\n";
		}
		// Set rarity colors
		switch (quality) {
		case Quality.COMMON:
			color = "white";
			break;
		case Quality.UNCOMMON:
			color = "lime";
			break;
		case Quality.RARE:
			color = "navy";
			break;
		case Quality.EPIC:
			color = "magenta";
			break;
		case Quality.LEGENDARY:
			color = "orange";
			break;
		case Quality.ARTIFACT:
			color = "red";
			break;
		}
		// Format time
		if (Sad > 0) {
			stats += "\n+" + Sad.ToString () + " Sad";
		}
		if (Happy > 0) {
			stats += "\n+" + Happy.ToString () + " Happy";
		}
		if (Fear > 0) {
			stats += "\n+" + Fear.ToString () + " Fear";
		}
		if (Confused > 0) {
			stats += "\n+" + Confused.ToString () + " Confused";
		}

		return string.Format("<color=" + color + "><size=25>{0}</size></color><size=20><i><color=red>"+ newLine +"{1}</color></i>{2}</size>", itemName, description, stats);

	}
}
