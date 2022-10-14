using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

public class Timer : MonoBehaviour
{
    public GameObject timerObject;
    private ITimer timer;

    private float startValue;
    private Vector3 scale;

    [SerializeField] private Transform uIObject;
    [SerializeField] private Image counter;

    public bool follow;
    private bool started;
    [ShowIf("follow")]
    public Transform followTransform;

    [SerializeField] private Gradient grad;

    void Start()
    {
        if (timerObject.TryGetComponent(out ITimer timer))
        {
            this.timer = timer;
        }

        SetGradient();
        
        scale = uIObject.localScale;
        uIObject.localScale = Vector3.zero;
    }

    private void SetGradient()
    {
        var colorKey = new GradientColorKey[3];
        colorKey[2].color = this.timer.TargetColor();
        colorKey[2].time = 0.75f;
        colorKey[1].color = Color.yellow;
        colorKey[1].time = 0.5f;
        if (colorKey[2].color == Color.red) { colorKey[0].color = Color.green; }
        else { colorKey[0].color = Color.yellow; }

        colorKey[0].time = 0.25f;
        grad.colorKeys = colorKey;
    }

    void Update()
    {
        if (!started) return;

        counter.fillAmount = timer.Time() / startValue;
        counter.color = grad.Evaluate(1 - counter.fillAmount);

        if (follow)
        {
            transform.position = followTransform.position;
        }
    }

    public void StartTimer()
    {
        uIObject.DOScale(scale, 0.5f).SetEase(Ease.OutBounce);
        startValue = timer.Time();
        started = true;
    }

    public void StopTimer()
    {
        uIObject.DOScale(0, 0.5f).SetEase(Ease.InBounce)
            .OnComplete(() =>
            {
                if (follow) Destroy(gameObject);
            });
        started = false;
    }

}
