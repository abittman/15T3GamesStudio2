using System.Collections.Generic;

namespace DialogueSystem {
	[System.Serializable]
	public class Stats {
		public List<Stat> Data = new List<Stat>();
		
		// returns true if list expands, false if already in list
		public bool Add(Stat MyStat) {
			for (int i = 0; i < Data.Count; i++) {	
				if (Data[i].Name == MyStat.Name) {
					Data[i].Add (MyStat.Max);
					return false;
				}
			}
			Data.Add (MyStat);
			return true;
		}
		
		public bool Contains(Stat MyStat) {
			for (int i = 0; i < Data.Count; i++) {
				if (Data[i].Name == MyStat.Name) {
					return true;
				}
			}
			return false;
		}
	}
	
	[System.Serializable]
	public class Stat {
		public string Name;		// unique identifier for stats
		public float Value;		// the state of the value
		public float Max;		// max the value can be
		
		// expands the max, adds to state value too
		public void AddM(float AddValue) {
			Max += AddValue;
			Value += AddValue;
		}
		public void Add(float AddValue) {
			Value += AddValue;
		}
		public string GuiString() {
			return (Name + ": " + Value + "/" + Max);
		}
	}
}