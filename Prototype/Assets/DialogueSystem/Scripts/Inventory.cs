using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem {
	public class Inventory : MonoBehaviour {
		public List<Item> MyItems = new List<Item>();

		public void AddItem(Item NewItem) {
			// first check to stack item
			for (int i = 0; i < MyItems.Count; i++) {
				if (MyItems[i].Name == NewItem.Name) {
					MyItems[i].Quantity++;
					return;
				}
			}
			// if no item of type, add to list
			MyItems.Add (NewItem);
		}
	}
}