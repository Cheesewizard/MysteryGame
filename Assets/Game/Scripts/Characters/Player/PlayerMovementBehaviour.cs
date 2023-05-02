using System;
using Game.Scripts.Controls;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class PlayerMovementBehaviour : MonoBehaviour
	{
		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		[SerializeField]
		private Camera targetCamera;

		[SerializeField]
		private float movementSpeed = 20;

		public event Action<bool> OnMovement;
		public event Action OnKilled;

		private void Start()
		{
			Cursor.visible = false; // maybe remove. Could add laser site as an item further into the game
			playerInput?.Player.Enable();
		}

		private void Update()
		{
			LookAtMouse();
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

			var worldPoint = targetCamera.ScreenToWorldPoint(new Vector3(input.x, input.y, targetCamera.nearClipPlane));
			var difference = worldPoint - transform.position;

			var angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
			transform.parent.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
		}
	}
}