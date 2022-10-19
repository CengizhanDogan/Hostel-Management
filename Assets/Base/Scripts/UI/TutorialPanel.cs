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
        LevelManager.Instance.OnLevelStart.AddListener(HideTutorialPanel);
    }
    public void OnDisable()
    {
        SceneController.Instance.OnSceneLoaded.RemoveListener(ShowTutorial);
        LevelManager.Instance.OnLevelStart.RemoveListener(HideTutorialPanel);
    }
    private void ShowTutorial()
    {
        if (PlayerPrefs.GetInt(PlayerPrefKeys.Tutorial) == 0) base.ShowPanel();
        else base.HidePanel();
        PlayerPrefs.SetInt(PlayerPrefKeys.Tutorial, 1);
    }
    private void HideTutorialPanel()
    {
        base.HidePanel();
    }
}
