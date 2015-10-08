using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public Canvas hackingMinigame;
	Canvas clone;

	// Use this for initialization
	void Start () {
		hackingMinigame.GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.J)){
			startGame();
		}
	}

	private void startGame(){
			clone = Instantiate (hackingMinigame);
		clone.transform.position = new Vector3 (0, 0, 0);
	}
	public void gameWon(){
			clone = null;
			Debug.Log ("You Win!");
	}
	public void gameLost(){
			clone = null;
			Debug.Log ("You have Lost.");
	}
}
