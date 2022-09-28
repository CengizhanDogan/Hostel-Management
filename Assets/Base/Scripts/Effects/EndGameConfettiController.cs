using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameConfettiController : MonoBehaviour
{
    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
        GameManager.Instance.OnStageSuccess.AddListener(() => GetComponent<ParticleSystem>().Play());
    }
    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
        GameManager.Instance.OnStageSuccess.RemoveListener(() => GetComponent<ParticleSystem>().Play());
    }
}
