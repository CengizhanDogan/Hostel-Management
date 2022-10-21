using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;

public class StarPanel : MonoBehaviour
{
    [SerializeField] private List<Image> stars = new List<Image>();

    private float lastStarValue;
    private void OnEnable()
    {
        EventManager.OnCustomerLeave.AddListener(SetStars);
    }
    private void OnDisable()
    {
        EventManager.OnCustomerLeave.RemoveListener(SetStars);
    }
    private void Start()
    {
        SetStars();
    }
    private void SetStars()
    {
        float targetValue = PlayerPrefs.GetInt(PlayerPrefKeys.HostelStarLevel);

        float starValue = Mathf.Floor(targetValue / 20f);
        float fillValue = (targetValue - (starValue * 20));

        foreach (var star in stars)
        {
            if (star.fillAmount < 1 && stars.IndexOf(star) < starValue)
                star.fillAmount = 1;
            else if (stars.IndexOf(star) > starValue) star.fillAmount = 0;

            if (stars.IndexOf(star) < starValue || stars.IndexOf(star) > starValue)
                continue;

            DOTween.To(() => star.fillAmount, x =>
            star.fillAmount = x, ((float)fillValue / 20f), 0.25f);
        }
        if (lastStarValue != starValue)
        {
            lastStarValue = starValue;
            GameAnalytics.NewDesignEvent("StarLevel", starValue);
        }
    }
}
