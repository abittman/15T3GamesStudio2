using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;


namespace DialogueSystem {
	public class GuiTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
		public GameObject ToolTipGui;
		public TooltipData MyTooltipData;

		public void OnPointerEnter(PointerEventData eventData) {
			ToolTipGui.SetActive (true);
			RectTransform MyRect = ToolTipGui.GetComponent<RectTransform> ();
			ToolTipGui.transform.GetChild(0).GetComponent<Text> ().text = MyTooltipData.LabelText;
			ToolTipGui.transform.GetChild(1).GetComponent<Text> ().text = MyTooltipData.DescriptionText;
		}

		public void OnPointerExit(PointerEventData eventData) {
			ToolTipGui.SetActive (false);
		}
	}
}
