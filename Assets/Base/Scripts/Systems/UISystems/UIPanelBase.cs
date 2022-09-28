using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(LayoutElement))]
[TypeInfoBox("This Component Uses Canvas Group and Layout Element, These two components are added to the game object but hidden to keep inspector tidy")]
[BoxGroup("Panel", Order = 5)]

public abstract class UIPanelBase : Singleton<UIPanelBase>
{

	#region Properties
	private CanvasGroup canvasGroup;
	protected CanvasGroup CanvasGroup { get { return (canvasGroup == null) ? canvasGroup = GetComponent<CanvasGroup>() : canvasGroup; } }

	LayoutElement layoutElement;
	private LayoutElement LayoutElement { get { return (layoutElement == null) ? layoutElement = GetComponent<LayoutElement>() : layoutElement; } }
	#endregion

	[ButtonGroup("PanelControl"), GUIColor(0.1f, 1f, 0.1f)]
	public virtual void ShowPanel()
	{
		SetPanel(false, 1, true, true);
	}

	[ButtonGroup("PanelControl"), GUIColor(1f, 0.1f, 0.1f)]
	public virtual void HidePanel()
	{
		SetPanel(true, 0, false, false);
	}

	protected void SetPanel(bool ignoreLayout, float alpha, bool blockRaycast, bool interactable)
	{
		if (LayoutElement == null)
			Debug.LogError("Layout Element is null");

		if (CanvasGroup == null)
			Debug.LogError("CanvasGroup is null");


		CanvasGroup.alpha = alpha;
		CanvasGroup.blocksRaycasts = blockRaycast;
		CanvasGroup.interactable = interactable;

		RectTransform parent = transform.parent.GetComponent<RectTransform>();
		LayoutRebuilder.ForceRebuildLayoutImmediate(parent);

	}
}
