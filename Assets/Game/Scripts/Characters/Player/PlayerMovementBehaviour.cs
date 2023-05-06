using System;
using Game.Scripts.Characters.UI.Controller_Icons;
using Game.Scripts.Controls;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class PlayerMovementBehaviour : MonoBehaviour
	{
		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		[SerializeField]
		private Rigidbody2D rigidbody;

		[SerializeField]
		private Camera targetCamera;

		[SerializeField]
		private float movementSpeed = 20;

		private static bool IsGamepad => GameControllerManager.instance.IsGamepad;

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
		}

		private void FixedUpdate()
		{
			// Mouse & Keyboard
			if (!IsGamepad)
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

			var newPosition = new Vector2(input.x, input.y);

			OnMovement?.Invoke(newPosition != Vector2.zero);

			rigidbody.MovePosition((Vector2) transform.position + newPosition * (movementSpeed * Time.deltaTime));
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
	}
}