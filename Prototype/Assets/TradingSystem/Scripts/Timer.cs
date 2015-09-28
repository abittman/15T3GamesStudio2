using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour {
	public float Seconds = 59;
	public float Minutes = 0;
	Text timerTxt;
	bool enable = false;
	public GameObject Inv;
	public GameObject InvObject;
	public GameObject Watch;
	// Use this for initialization
	void Start () {
		timerTxt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Seconds <= 0) {
			Seconds = 59;
			if (Minutes >= 1) {
				Minutes --;
			} else {
				Minutes = 0;
				Seconds = 0;
				GameObject.Find ("TimerText").GetComponent<Text>().text = Minutes.ToString ("f0") + ":0" + Seconds.ToString("f0");
			}
		} else {
			Seconds -= Time.deltaTime;
		}

		if (Mathf.Round (Seconds) <= 9) {
			GameObject.Find ("TimerText").GetComponent<Text> ().text = Minutes.ToString ("f0") + ":0" + Seconds.ToString ("f0");
		} else {
			GameObject.Find ("TimerText").GetComponent<Text> ().text = Minutes.ToString ("f0") + ":" + Seconds.ToString ("f0");
		}

	}

	public void OpenInventory(){
		Toggle ();
	}




	public void Toggle() {
		enable = !enable;
		if (enable) {
			InvObject.transform.position = Watch.transform.position;
			Inv.gameObject.SetActive (enable);
		} else if(enable == false) {
			Debug.Log (transform.name);
			GameObject.Find ("InvButton").GetComponent<GUIEffects>().DismissObjects();
			//Inv.gameObject.SetActive (enable);
		}
		
	}
	
	public void Toggle(bool IsEnabled) {
		GameObject.Find ("Inv").gameObject.SetActive(IsEnabled);

	}
}
