using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem {
	[System.Serializable]
	public class Item {
		[Tooltip("A unique identifier for the Item")]
		public string Name;
		[Tooltip("Used in the tooltip to describe the item")]
		public string Description;
		//public bool IsPickup = false;
		[Tooltip("How many of that item there is.")]
		public int Quantity;
		[Tooltip("How much the item is worth")]
		public int Value;
		public Stats MyStats;
		public Texture MyTexture;

		public Item() {

		}

		public string GetCommand(string Data) {
			if (Data.Length == 0)
				return "";
			for (int i = 0; i < Data.Length; i++) {
				if (Data [i] == '/') {
					Data = Data.Substring(i);
					//Debug.LogError(Data);
					i = Data.Length;
				}
			}
			if (Data [0] == '/') {
				string[] New = Data.Split(' ');
				return New[0];
			} else {
				return "";
			}
		}

		public Item(List<string> Data) {
			Quantity = 1;	// default
			//Debug.LogError ("Loading Item");
			for (int i = 0; i < Data.Count; i++) {
				//Debug.LogError (" ItemData: " + Data[i]);
				string Command = GetCommand(Data[i]);
				string Other = DialogueLine.RemoveCommand(Data[i], Command + " ");
				//Debug.LogError (Command + " ----- " + Other);
				switch (Command) {
					case ("/item"):
						Name = Other;
						break;
					case ("/description"):
						Description = Other;
						break;
					case ("/quantity"):
						try { Quantity = int.Parse(Other); 	} catch(System.FormatException e) {	}
					break;
					case ("/value"):
						try { Value = int.Parse(Other); 	} catch(System.FormatException e) {	}
						break;
					case ("/stats"):
						MyStats = new Stats(Data);
						break;
				}
			}
		}
	}

}
// maybe make item action as well, ie (open a door)

// give worldItem, a function, so i can have other scripts activate when they are selected - ie flip a car, open a door