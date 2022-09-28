using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class BlockInputPanel : UIPanelBase
{
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageSuccess.AddListener(ShowPanel);
        GameManager.Instance.OnStageFail.AddListener(ShowPanel);
        SceneController.Instance.OnSceneLoaded.AddListener(HidePanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        GameManager.Instance.OnStageSuccess.RemoveListener(ShowPanel);
        GameManager.Instance.OnStageFail.RemoveListener(ShowPanel);
        SceneController.Instance.OnSceneLoaded.RemoveListener(HidePanel);
    }
}
