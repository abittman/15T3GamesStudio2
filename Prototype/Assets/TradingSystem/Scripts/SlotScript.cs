using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler {

	bool swapped = false;
	// Find child of current slot which is the item
	public GameObject item{
		get{
			if(transform.childCount>0){
				return transform.GetChild (0).gameObject;
			}
			return null;
		}
	}
	#region IDropHandler implementation
	
	public void OnDrop (PointerEventData eventData)
	{
		// if no item set item being dragged to new parent
		if (!item) {
			DragHandler.itemBeingDragged.transform.SetParent(transform);


			Debug.Log ("Drop");
		}
		// if there is an item swap parents
		if (item) {


			item.transform.SetParent (DragHandler.itemBeingDragged.transform.parent);

			DragHandler.itemBeingDragged.transform.SetParent(transform);



			Debug.Log ("Drop");
		}
	}
	
	#endregion


	public void OnOver(){

		item.transform.GetComponent <DragHandler> ().isDragging = true;
		item.transform.eulerAngles = new Vector3 (0, 0, 0);
		Debug.Log ("CLicked");
	}
	public void OnExit(){
		item.transform.GetComponent <DragHandler> ().isDragging = false;

	}
	void Update(){

	}
	
}
