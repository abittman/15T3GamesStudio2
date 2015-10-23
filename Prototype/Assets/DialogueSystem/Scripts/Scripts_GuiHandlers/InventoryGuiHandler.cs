using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DialogueSystem {
	public class InventoryGuiHandler : GuiListHandler {
		public Character MyCharacter;
		public Inventory MyInventory;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			UpdateGuis ();
		}
		public void UpdateInventoryGui() {
			Debug.Log ("Refreshing Inventory Gui: " + Time.time);
			Clear ();
			for (int i = 0; i < MyInventory.MyItems.Count; i++) {
				TooltipData MyData = new TooltipData();
				MyData.LabelText = MyInventory.MyItems[i].Name;
				MyData.DescriptionText = MyInventory.MyItems[i].Description;
				for (int j = 0; j < MyInventory.MyItems[i].MyStats.Data.Count; j++) {
					Stat MyStat = MyInventory.MyItems[i].MyStats.Data[j];
					MyData.DescriptionText += "\n   " + MyStat.Name;
					if (MyStat.Value > 0) {
						MyData.DescriptionText += ": +" + MyStat.Value;
					}
					else if (MyStat.Value < 0) {
						MyData.DescriptionText += ": -" + Mathf.Abs(MyStat.Value).ToString();
					}
				}
				AddGui("x" + MyInventory.MyItems[i].Quantity, MyData);
				MyGuis[MyGuis.Count-1].transform.GetChild(0).GetComponent<RawImage>().texture = MyInventory.MyItems[i].MyTexture;
			}
		}
	}
}