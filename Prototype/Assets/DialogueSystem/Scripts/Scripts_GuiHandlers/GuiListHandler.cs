using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DialogueSystem {
	[System.Serializable]
	public class TooltipData {
		public string LabelText;
		public string DescriptionText;
		public TooltipData() {
			LabelText = "???";
			DescriptionText = "???";
		}
	}
	public class GuiListHandler : MonoBehaviour {
		public List<GameObject> MyGuis = new List<GameObject> ();
		public List<TooltipData> MyGuiTooltipDatas = new List<TooltipData> ();
		public GameObject GuiPrefab;
		public bool IsLabels = true;
		public int ScrollPositionY = 0;
		public int ListSizeX = 1;
		public int ListSizeY = 5;
		public float MarginX = 50f;
		public float MarginY = 50f;
		public float SizeX = 60f;
		public float SizeY = 60f;
		public GameObject TooltipGui;

		void Awake() {
			RectTransform MyGuiPrefabRect = GuiPrefab.GetComponent<RectTransform> ();
			if (MyGuiPrefabRect) {
				SizeX = MyGuiPrefabRect.GetSize ().x;
				SizeY = MyGuiPrefabRect.GetSize ().y;
			}
		}
		// Update is called once per frame
		void Update () {
			UpdateGuis ();
		}
		public void UpdateGuis() {
			for (int i = 0; i < MyGuis.Count; i++) {
				MyGuis [i].GetComponent<RectTransform> ().anchoredPosition = 
					Vector2.Lerp (MyGuis [i].GetComponent<RectTransform> ().anchoredPosition, 
					              GetCellPosition(i),
					              //new Vector2 (0, -(i - ScrollPosition) * (SizeY+MarginY)-SizeY/2f),
				    				Time.deltaTime);
			}
		}
	
		public void Scroll(bool Direction) {
			if (Direction) {
				int Max = ScrollPositionY*ListSizeX + ListSizeY*ListSizeX;
				//MyGuis.Count/ListSizeX-ListSizeY*ListSizeX
				if (Max < MyGuis.Count)	// 5 is the size that it can hold
					ScrollPositionY++;
			} else {
				if (ScrollPositionY > 0)
					ScrollPositionY--;
			}
		}

		public void Clear() {
			for (int i = 0; i < MyGuis.Count; i++) {
				Destroy(MyGuis[i]);
			}
			MyGuis.Clear ();
			MyGuiTooltipDatas.Clear ();
		}

		public void RemoveAt(int Index) {
			if (Index >= 0 && Index < MyGuis.Count) {
				Destroy (MyGuis [Index].gameObject);
				MyGuis.RemoveAt (Index);
				MyGuiTooltipDatas.RemoveAt (Index);
			}
		}
		public Vector3 GetCellPosition(int Index) {
			Vector3 CellPosition = new Vector3 ();
			int PositionX = Index % ListSizeX;
			int PositionY = Index / ListSizeX;
			CellPosition = new Vector3 ((PositionX) * (SizeX + MarginX)-SizeX*ListSizeX/2f, 
			                            -(PositionY - ScrollPositionY) * (SizeY + MarginY) - SizeY / 2f, 
			                            0);
			return CellPosition;
		}
		
		public void AddGui(string GuiLabel) {
			TooltipData NewTooltip = new TooltipData ();
			NewTooltip.LabelText = GuiLabel;
			AddGui (GuiLabel, NewTooltip);
		}
		public void AddGui(string GuiLabel, TooltipData MyTooltipData) {
			if (GuiPrefab == null) {
				GuiPrefab = new GameObject();
			}
			string NewName = GuiLabel;

			GameObject NewGuiCell = (GameObject)Instantiate (GuiPrefab, 
			                                                  GetCellPosition(MyGuis.Count), 
			                                                  Quaternion.identity);
			NewGuiCell.name = MyTooltipData.LabelText;
			if (IsLabels)
				NewGuiCell.transform.GetChild(0).gameObject.GetComponent<Text>().text = NewName;
			if (TooltipGui) {
				GuiTooltip MyGuiToolTip = NewGuiCell.AddComponent<GuiTooltip> ();
				MyGuiToolTip.ToolTipGui = TooltipGui;
				MyGuiToolTip.MyTooltipData = MyTooltipData;
			}

			NewGuiCell.transform.SetParent (gameObject.transform, false);
			MyGuis.Add (NewGuiCell);
			MyGuiTooltipDatas.Add (MyTooltipData);
		}
	}
}