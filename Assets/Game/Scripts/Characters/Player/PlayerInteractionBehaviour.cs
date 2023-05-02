using System;
using Game.Scripts.Controls;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class PlayerInteractionBehaviour : MonoBehaviour
	{
		[SerializeField]
		private PlayerInventory playerInventory;

		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		private Item currentItem;

		public event Action<string> OnInteractWithItem;
		public event Action OnExitItem;
		public event Action OnPickupItem;

		private void Update()
		{
			if (currentItem == null) return;

			// Interact
			if (currentItem.canBeInteractedWith)
			{
				if (playerInput.Player.Read.WasPressedThisFrame())
				{
					OnInteractWithItem?.Invoke(currentItem.GetDescription());
					currentItem.ToggleButtons();
				}
			}

			// Pickup
			if (currentItem.canBePickedUp)
			{
				if (playerInput.Player.Pickup.WasPressedThisFrame())
				{
					var pickedUpItem = currentItem.PickUpItem();
					if (pickedUpItem != null)
					{
						OnPickupItem?.Invoke();
						playerInventory.AddItem(pickedUpItem);
					}
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent<Item>(out var item))
			{
				currentItem = item;
				Debug.Log($"Current item {item.name}");
				item.SetButtons(true);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (currentItem == null) return;
			
			// Disable the interaction with the previous item
			currentItem.SetButtons(false);

			Debug.Log($"Leaving previous item {currentItem}");
			
			OnExitItem?.Invoke();
			currentItem = null;
		}
	}
}