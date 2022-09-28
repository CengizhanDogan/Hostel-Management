using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIPanel
{
	public string panelID;
	public UIPanelBase panel;
}
public class UISceneCanvasManager : Singleton<UISceneCanvasManager>
{
	public List<UIPanel> panels = new List<UIPanel>();

	public UIPanelBase? GetPanelByID(string ID)
	{
		foreach (UIPanel item in panels)
		{
			if (item.panelID == ID)
				return item.panel;
		}
		return null;
	}
}
