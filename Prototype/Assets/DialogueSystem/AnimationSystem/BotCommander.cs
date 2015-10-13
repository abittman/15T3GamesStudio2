using UnityEngine;
using System.Collections;

public class BotCommander : MonoBehaviour {
	public GameObject MyBot;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit MyHit;
			if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out MyHit, 100)) {
				if (MyHit.collider.gameObject.tag == "Ground" && MyBot) {
					Movement MyBotAI = MyBot.GetComponent<Movement>();
					if (MyBotAI) {
						MyBotAI.MoveToPosition(MyHit.point);
					}
				}
				if (MyHit.collider.gameObject.tag == "Bot") {
					MyBot = MyHit.collider.gameObject;
				}
			}
		}
	}
}
