using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ToolTipTrade : MonoBehaviour {
	public GameObject toolTipObject;
	private static GameObject toolTip;
	public Text sizeText;
	public Text visualText;
	
	public GameObject item{
		get{
			if(transform.childCount>0){
				return transform.GetChild (0).gameObject;
			}
			return null;
		}
	}
	// Use this for initialization
	void Start () {
		toolTip = toolTipObject;
	}
	
	// Update is called once per frame
	void Update () {
		toolTip.transform.LookAt (Camera.main.transform.position);
		toolTip.transform.Rotate (0, 180, 0);

	}
	
	
	public void ShowToolTip(GameObject slot){
		Debug.Log ("ShowTip");
		
		if (slot.GetComponent<SlotScript> ().item) {
			
			toolTip.SetActive (true);
			visualText.text = slot.transform.GetChild (0).gameObject.GetComponent<Item>().GetToolTip();
			print (slot.transform.GetChild (0).gameObject.name);
			sizeText.text = visualText.text;
			
			var v3 = Input.mousePosition;
			v3.z = 1f;
			v3.y -= slot.GetComponent<RectTransform> ().sizeDelta.y - slot.transform.localScale.y;
			v3 = Camera.main.ScreenToWorldPoint (v3);
			toolTip.transform.position = v3;
			
		}
		
		
	}
	public void HideToolTip(){
		Debug.Log ("hideTip");
		toolTip.SetActive (false);
		
	}
}
