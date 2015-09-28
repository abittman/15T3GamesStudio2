using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	public Transform startParent;
	public bool isDragging = false;
	Vector3 startScale;

	#region IBeginDragHandler implementation

	void Start(){
		// Keep scale consistent
		startScale = transform.localScale;

	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log ("Drag");
		// Set the object being dragged to its parent
		itemBeingDragged =gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		// make the object follow the mouse
		isDragging = true;
		var v3 = Input.mousePosition;
		v3.z = 1f;
		v3 = Camera.main.ScreenToWorldPoint (v3);
		transform.position = v3;

		transform.LookAt (Camera.main.transform.position);
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		// if dragged into an empty space return to parent
		isDragging = false;
		DragHandler.itemBeingDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (transform.parent == startParent) {

			transform.position = startPosition;
		
		}

		//transform.Rotate (0, 180, 0);

		Debug.Log (transform.position);

	}

	#endregion



	void Update(){
		if (!isDragging) {
			// Keep the scale and position consistent through the update method
			transform.LookAt(Camera.main.transform.position);

			transform.position = new Vector3 (transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
			transform.localScale = startScale;
		}

	}





}
