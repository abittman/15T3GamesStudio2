using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem {
	[ExecuteInEditMode]
	public class ItemManager : MonoBehaviour {
		public bool IsUpdateItems = false;
		public List<Item> MyItems;

		// Use this for initialization
		void Start () {
			IsUpdateItems = true;
		}
		
		// Update is called once per frame
		void Update () {
			if (IsUpdateItems) {
				IsUpdateItems = false;
				UpdateAllItems();
			}
		}

		public void UpdateAllItems() {
			//GameObject[] MyGuiBackgrounds = GameObject.FindGameObjectsWithTag("GuiBackground", true);
			GameObject[] AllObjects = (GameObject[])Resources.FindObjectsOfTypeAll (typeof(UnityEngine.GameObject));
			//Debug.LogError ("Found: " + AllObjects.Length + " Objects!");
			foreach (GameObject MyObject in AllObjects) {
				if (MyObject.GetComponent<ItemObject> ()) {
					Item MyItem = MyObject.GetComponent<ItemObject> ().MyItem;
					CheckItemForReplace (MyItem);	
				} else if (MyObject.GetComponent<Inventory> ()) {
					Inventory MyInventory = MyObject.GetComponent<Inventory> ();
					
					for (int j = 0; j < MyInventory.MyItems.Count; j++) {
						Item MyItem = MyInventory.MyItems [j];
						CheckItemForReplace (MyItem);
					}
				}
			}
		}
		private void CheckItemForReplace(Item NewItem) {
			for (int i = 0; i < MyItems.Count; i++) {
				if (ReplaceItem(NewItem, MyItems[i]))
					i = MyItems.Count;
			}
		}
		private bool ReplaceItem(Item Item1, Item Item2) {
			if (Item1.Name != Item2.Name)
					return false;
			Item1.Description = Item2.Description;
			Item1.MyTexture = Item2.MyTexture;
			Item1.MyStats = Item2.MyStats;
			return true;
		}
	}
}
