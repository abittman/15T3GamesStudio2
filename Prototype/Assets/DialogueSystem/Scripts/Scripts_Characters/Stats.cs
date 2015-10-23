using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem {
	[System.Serializable]
	public class Stats 
	{
		public List<Stat> Data = new List<Stat>();
		public Stats() { }

		public Stats(List<string> StringData) {
			Data.Clear ();
			if (StringData.Count == 0)
				return;
			bool IsReadingStats = false;{
				for (int i = 0; i < StringData.Count; i++) {
					if (IsReadingStats) {
						string[] MyStrings = StringData[i].Split(' ');
						if (MyStrings.Length == 2) {
							Data.Add (new Stat(MyStrings));
						} else {
							//Debug.LogError("Problem with strings in stats: " + MyStrings.Length);
						}
					}
					if (StringData [i].Contains ("/stats")) 
						IsReadingStats = true;
				}
			}
		}

		// returns true if list expands, false if already in list
		public bool Add(Stat MyStat) {
			for (int i = 0; i < Data.Count; i++) {	
				if (Data[i].Name == MyStat.Name) {
					Data[i].Add (MyStat.Value);
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
	public class Stat 
	{
		public string Name;			// unique identifier for stats
		public string Description;	// used for tooltip
		public float Value;			// the state of the value

		public Stat() {
		}
		public Stat(string[] Data) {
			if (Data.Length == 2) {
				Name = Data [0];
				try {	Value = float.Parse(Data [1]);	} catch(System.FormatException e) {	}
			}

		}
		// expands the max, adds to state value too
		public void AddM(float AddValue) 
		{
			Value += AddValue;
		}

		public void Add(float AddValue) 
		{
			Value += AddValue;
		}

		public string GuiString() 
		{
			return (Name + ": " + Value);
		}
	}
	
	[System.Serializable]
	public class StatExtended : Stat 
	{
		public float Maximum;		// the max a value can be
		public float Recovery;		// The recovery rate / second
	}
}