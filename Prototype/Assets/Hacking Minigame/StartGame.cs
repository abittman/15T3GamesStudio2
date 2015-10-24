using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public Canvas hackingMinigame;
	public Door door;
	Canvas clone;
	private bool hacking = false;
	
	void Start () {
		hackingMinigame.GetComponent<Canvas> ();
	}

	public void StartMinigame(){
		if (!hacking) {
			hacking = true;
			clone = Instantiate (hackingMinigame);
			clone.GetComponent<HackingGameScript>().sg = gameObject.GetComponent<StartGame>();
			clone.transform.position = new Vector3 (0, 0, 0);
		}
	}
	public void MinigameWon(){
			Destroy (clone.gameObject);
			Debug.Log ("You Win!");
			door.Unlock ();
			hacking = false;
	}
	public void MinigameLost(){
			Destroy (clone.gameObject);
			Debug.Log ("You have Lost.");
			door.Lock ();
			hacking = false;
	}
}
