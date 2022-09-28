using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

namespace CMF
{
	public class PlayerInputController : CharacterInput
	{
		private InputManager inputManager;
		public InputManager InputManager => inputManager == null ? inputManager = InputManager.Instance : inputManager;

		private Transform cameraTransform;
		public Transform CameraTransform => cameraTransform == null ? cameraTransform = Camera.main.transform : cameraTransform;

		//If this is enabled, Unity's internal input smoothing is bypassed;
		private bool useRawInput = true;

		private KeyCode jumpKey = KeyCode.Space;

		private bool itCanMove;
		public bool ItCanMove { get { return itCanMove; } set { itCanMove = value; } }
		private void OnEnable()
		{
			if (Managers.Instance == null)
				return;
			LevelManager.Instance.OnLevelStart.AddListener(()=>ItCanMove = true);
		}
		private void OnDisable()
		{
			if (Managers.Instance == null)
				return;
			LevelManager.Instance.OnLevelStart.RemoveListener(() => ItCanMove = true);
		}
		public override float GetHorizontalMovementInput()
		{
			if (!ItCanMove)
				return 0f;
			return InputManager.JoystickDirection.x;
		}

		public override float GetVerticalMovementInput()
		{
			if (!ItCanMove)
				return 0f;
			return InputManager.JoystickDirection.y;
		}

		public override bool IsJumpKeyPressed()
		{
			return false;
		}
	}
}

