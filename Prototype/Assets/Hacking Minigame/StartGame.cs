using UnityEngine;
using System.Collections;
using DialogueSystem;

public class StartGame : MonoBehaviour
{
	public Canvas hackingMinigame;
	public Door door;
	Canvas clone;
	private bool hacking = false;
	public string questName;
	public QuestLog questLog;
	//private QuestLog questLog;
	
	void Start ()
	{
		hackingMinigame.GetComponent<Canvas> ();
		questLog.GetComponent<QuestLog> ();
	}

	public void StartMinigame ()
	{
		if (!hacking) {
			if (questLog.DoesHaveQuest (questName)) {
				Debug.Log ("Player Has Quest");
				hacking = true;
				clone = Instantiate (hackingMinigame);
				clone.GetComponent<HackingGameScript> ().sg = gameObject.GetComponent<StartGame> ();
				clone.transform.position = new Vector3 (0, 0, 0);
			} else if (!questLog.DoesHaveQuest (questName)) {
				Debug.Log ("Player either does not have Quest, or the Quest has previously been completed.");
			} 
		}
	}

	public void MinigameWon ()
	{
		Destroy (clone.gameObject);
		Debug.Log ("You Win!");
		door.Unlock ();
		hacking = false;
		if (questName != "") {
			questLog.CompleteQuest (questName);
		}
	}

	public void MinigameLost ()
	{
		Destroy (clone.gameObject);
		Debug.Log ("You have Lost.");
		door.Lock ();
		hacking = false;
	}
}
