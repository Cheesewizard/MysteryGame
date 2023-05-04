using System;
using System.Linq;
using Game.Scripts.Controls;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Characters.UI.Controller_Icons
{
	public class GameControllerManager : MonoBehaviour
	{
		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		public static GameControllerManager instance;

		public event Action<bool> OnControllerChange;

		private const string KEYBOARD_CONTROLLER_NAME = "Keyboard & Mouse";
		private const string GAMEPAD_CONTROLLER_NAME = "Gamepad";

		private void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);
		}

		[ReadOnly]
		[SerializeField]
		private bool isGamepad;

		public bool IsGamepad => isGamepad;

		private void Start()
		{
			InputSystem.onDeviceChange += HandleDeviceChanged;
			//InputSystem.onActionChange += HandleActionChanged;
			SetControllerBinding();
		}

		private void HandleActionChanged(object obj, InputActionChange inputActionChange)
		{
			if (inputActionChange == InputActionChange.ActionPerformed)
			{
				var receivedInputAction = (InputAction) obj;
				var lastDevice = receivedInputAction.activeControl.device;

				isGamepad = !lastDevice.name.Equals("Keyboard") || !lastDevice.name.Equals("Mouse");
				SetControllerBinding();
				OnControllerChange?.Invoke(isGamepad);
			}
		}

		private void HandleDeviceChanged(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
		{
			isGamepad = inputDevice.description.deviceClass != "Keyboard" &&
			            inputDevice.description.deviceClass != "Mouse";
			
			Debug.Log($"Gamepad = {isGamepad}");
			switch (inputDeviceChange)
			{
				case InputDeviceChange.Added:
					isGamepad = true;
					break;
				case InputDeviceChange.Removed:
					isGamepad = false;
					break;
				case InputDeviceChange.Disconnected:
					isGamepad = false;
					break;
				case InputDeviceChange.Reconnected:
					isGamepad = true;
					break;
			}

			SetControllerBinding();
			OnControllerChange?.Invoke(isGamepad);
		}

		private void SetControllerBinding()
		{
			if (isGamepad)
			{
				var bindingGroup =
					playerInput.controlSchemes.First(x => x.name == GAMEPAD_CONTROLLER_NAME).bindingGroup;

				playerInput.bindingMask = InputBinding.MaskByGroup(bindingGroup);
			}
			else
			{
				var bindingGroup =
					playerInput.controlSchemes.First(x => x.name == KEYBOARD_CONTROLLER_NAME).bindingGroup;

				playerInput.bindingMask = InputBinding.MaskByGroup(bindingGroup);
			}
		}

		private void OnDestroy()
		{
			InputSystem.onDeviceChange -= HandleDeviceChanged;
			//InputSystem.onActionChange -= HandleActionChanged;
		}
	}
}