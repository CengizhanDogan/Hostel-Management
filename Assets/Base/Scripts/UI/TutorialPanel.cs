using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : UIPanelBase
{
	[SerializeField] private RawImage swerveHand;
	private Vector3 handStartPosition;
	private void Start()
	{
		handStartPosition = swerveHand.rectTransform.localPosition;
	}
	public void OnEnable()
	{
		SceneController.Instance.OnSceneLoaded.AddListener(ShowTutorial);
		LevelManager.Instance.OnLevelStart.AddListener(HidePanel);
	}
	public void OnDisable()
	{
		SceneController.Instance.OnSceneLoaded.RemoveListener(ShowTutorial);
		LevelManager.Instance.OnLevelStart.RemoveListener(HidePanel);
	}
	private void ShowTutorial()
	{
		base.ShowPanel();
		swerveHand.rectTransform.localPosition = handStartPosition;
		swerveHand.rectTransform.DOLocalMoveX(-195f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
	}
	private void HidePanel()
	{
		base.HidePanel();
		DOTween.Kill(swerveHand.rectTransform);
	}
}
