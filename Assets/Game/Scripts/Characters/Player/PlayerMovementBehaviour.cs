using System;
using Game.Scripts.Controls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Characters.Player
{
	public class PlayerMovementBehaviour : MonoBehaviour
	{
		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		[SerializeField]
		private Camera targetCamera;

		[SerializeField]
		private float movementSpeed = 20;

		[SerializeField]
		private bool isGamepad = false;

		[SerializeField]
		private float gamepadRotationSpeed = 0.05f;

		[SerializeField]
		private float mouseRotationSpeed = 0.08f;

		public event Action<bool> OnMovement;
		public event Action OnKilled;

		private void Start()
		{
			Cursor.visible = false; // maybe remove.
			playerInput?.Player.Enable();

			// Detect when input has been changed
			InputSystem.onDeviceChange += InputDeviceChanged;
		}

		private void InputDeviceChanged(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
		{
			// if (inputDevice.description.ToString() == "Keyboard" || inputDevice.description.ToString() == "Mouse")
			// {
			// 	isGamepad = false;
			// }
			// else
			// {
			// 	isGamepad = true;
			// }
		}

		private void Update()
		{
			// Mouse & Keyboard
			if (!isGamepad)
			{
				LookAtMouse();
			}
			else
			{
				LookAtRightStick();
			}

			Move();
		}

		private void Move()
		{
			var input = playerInput.Player.Move.ReadValue<Vector2>();

			var newPosition = new Vector2(input.x, input.y) * (movementSpeed * Time.deltaTime);

			OnMovement?.Invoke(newPosition != Vector2.zero);
			transform.root.Translate(newPosition.x, newPosition.y, 0f, Space.World);
		}

		private void LookAtMouse()
		{
			var input = playerInput.Player.Rotate.ReadValue<Vector2>();

			var worldPoint =
				targetCamera.ScreenToWorldPoint(new Vector3(input.x, input.y, targetCamera.nearClipPlane));
			var difference = worldPoint - transform.position;

			var angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

			transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation,
				Quaternion.Euler(new Vector3(0, 0, angle)), mouseRotationSpeed);
		}

		private void LookAtRightStick()
		{
			if (playerInput.Player.Rotate.IsPressed())
			{
				var input = playerInput.Player.Rotate.ReadValue<Vector2>();

				var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

				transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation,
					Quaternion.Euler(new Vector3(0, 0, angle)), gamepadRotationSpeed);
			}
		}

		private void OnDisable()
		{
			InputSystem.onDeviceChange -= InputDeviceChanged;
		}
	}
}