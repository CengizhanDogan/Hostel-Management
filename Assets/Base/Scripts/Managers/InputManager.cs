using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class InputManager : Singleton<InputManager>
{
	private bool isStarted = true;
	[ReadOnly] public Vector2 JoystickDirection;
	private void OnEnable()
	{
		if (Managers.Instance == null)
			return;
		SceneController.Instance.OnSceneLoaded.AddListener(() => isStarted = false);
	}
	private void OnDisable()
	{
		if (Managers.Instance == null)
			return;
		SceneController.Instance.OnSceneLoaded.RemoveListener(() => isStarted = false);
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && !isStarted)
		{
			isStarted = true;
			GameManager.Instance.StartGame();
			LevelManager.Instance.StartLevel();
		}
		OnTapHoldInput();

	}

	#region TapHoldInput

	public UnityEvent OnTapDown = new UnityEvent();
	public UnityEvent OnTapUp = new UnityEvent();
	[ReadOnly] public bool isDown;

	public void OnTapHoldInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			OnTapDown.Invoke();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			OnTapUp.Invoke();
		}
	}
	#endregion
}