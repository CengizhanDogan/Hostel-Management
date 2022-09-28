using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class LevelPanel : UIPanelBase
{
	public GameObject winPanel, losePanel, inGamePanel;
	public RectTransform gameEndingImage;

	[SerializeField] private TextMeshProUGUI[] levelTexts;
	public TextMeshProUGUI[] inGameCoinTexts, endGameCoinTexts;

	private const float END_GAME_PANEL_OPENING_DELAY = 0.5f;

	private int collectedCoin;
	private void OnEnable()
	{
		if (Managers.Instance == null)
			return;
		SceneController.Instance.OnSceneLoaded.AddListener(SetPanelAttributes);
		GameManager.Instance.OnStageSuccess.AddListener(() =>
		{
			ShowEndPanel(winPanel);
			SetFakeLevel();
		});
		EventManager.OnGemCollected.AddListener((x, y) => collectedCoin++);
		GameManager.Instance.OnStageFail.AddListener(() => ShowEndPanel(losePanel));
		LevelManager.Instance.OnLevelStart.AddListener(ShowInGamePanel);
	}
	private void OnDisable()
	{
		if (Managers.Instance == null)
			return;
		SceneController.Instance.OnSceneLoaded.AddListener(SetPanelAttributes);
		GameManager.Instance.OnStageSuccess.RemoveListener(() =>
		{
			ShowEndPanel(winPanel);
			SetFakeLevel();
		});
		EventManager.OnGemCollected.RemoveListener((x, y) => collectedCoin++);
		GameManager.Instance.OnStageFail.RemoveListener(() => ShowEndPanel(losePanel));
		LevelManager.Instance.OnLevelStart.RemoveListener(ShowInGamePanel);

	}
	private void SetPanelAttributes()
	{
		gameEndingImage.DOScale(Vector3.zero, 1f);
		losePanel.SetActive(false);
		winPanel.SetActive(false);
		inGamePanel.SetActive(false);
		collectedCoin = 0;
		foreach (var item in levelTexts)
			item.text = "Level " + PlayerPrefs.GetInt(PlayerPrefKeys.FakeLevel, 1);
		foreach (var item in inGameCoinTexts)
			item.text = PlayerPrefs.GetInt(PlayerPrefKeys.Coin, 0).ToString();
	}
	private void ShowInGamePanel()
	{
		inGamePanel.SetActive(true);
	}
	private void ShowEndPanel(GameObject item)
	{
		inGamePanel.SetActive(false);
		foreach (var moneyText in endGameCoinTexts)
			moneyText.text = collectedCoin.ToString();
		Run.After(END_GAME_PANEL_OPENING_DELAY, () =>
		{
			item.SetActive(true);
			item.transform.localScale = Vector3.zero;
			item.transform.DOScale(Vector3.one, 0.7f);
		});
	}
	public void ReloadLevel()
	{
		losePanel.SetActive(false);
		gameEndingImage.DOScale(Vector3.one * 6f, 1)
			.OnComplete(() => LevelManager.Instance.ReloadLevel());

	}
	public void LoadNextLevel()
	{
		winPanel.SetActive(false);
		gameEndingImage.DOScale(Vector3.one * 6f, 1)
			.OnComplete(() => LevelManager.Instance.LoadNextLevel());
	}
	private void SetFakeLevel()
	{
		int fakeLevel = PlayerPrefs.GetInt(PlayerPrefKeys.FakeLevel, 1);
		fakeLevel++;
		PlayerPrefs.SetInt(PlayerPrefKeys.FakeLevel, fakeLevel);
	}
}
