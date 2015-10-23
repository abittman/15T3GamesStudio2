using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem {
	/*	Attach to same object as the character or treasurechest
	 * 		-Used to hold items generally
	 * 		
	*/
	public class Inventory : MonoBehaviour {
		public UnityEngine.Events.UnityEvent OnAddItem = null;
		public List<Item> MyItems = new List<Item>();

		void Awake() {
			HandleAddItemEvent();
		}

		public void AddItem(Item NewItem) {
			// first check to stack item
			for (int i = 0; i < MyItems.Count; i++) {
				if (MyItems[i].Name == NewItem.Name) {
					MyItems[i].Quantity++;
					HandleAddItemEvent();
					return;
				}
			}
			// if no item of type, add to list
			MyItems.Add (NewItem);
			HandleAddItemEvent();
		}
		public void HandleAddItemEvent() {
			if (OnAddItem != null) {
				OnAddItem.Invoke();
			}
		}
	}
}