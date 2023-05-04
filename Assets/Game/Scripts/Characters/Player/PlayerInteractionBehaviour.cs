using System;
using Game.Scripts.Characters.Items;
using Game.Scripts.Characters.UI.Dialogue;
using Game.Scripts.Controls;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class PlayerInteractionBehaviour : MonoBehaviour
	{
		[SerializeField]
		private PlayerInventory playerInventory;

		[SerializeField]
		private DialogueBehaviour dialogueBehaviour;

		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		private Item currentItem;
		private Item previousItem;

		public event Action<string> OnInteractWithItem;
		public event Action OnExitItem;
		public event Action OnPickupItem;

		private void Update()
		{
			if (currentItem == null) return;

			// Interact
			if (currentItem.CanBeInteractedWith && !currentItem.IsRead ||
			    currentItem.CanReadForever && !currentItem.IsReading)
			{
				if (playerInput.Player.Read.WasPressedThisFrame())
				{
					currentItem.MarkAsIsReading();
					dialogueBehaviour.AwaitCallBack(currentItem.CheckIfTextHasBeenRead,
						currentItem.GetDescription());

					OnInteractWithItem?.Invoke(currentItem.GetDescription());
				}
			}

			// Pickup
			if (currentItem.CanBePickedUp && currentItem.IsRead)
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
				previousItem = currentItem;
				currentItem = item;
				Debug.Log($"Current item {item.name}");
				item.ShowButtons();
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (currentItem == null) return;

			currentItem.HideButtons();

			// Disable the interaction with the previous item
			if (previousItem != null)
			{
				previousItem.HideButtons();
				previousItem = null;
			}

			Debug.Log($"Leaving previous item {currentItem}");

			OnExitItem?.Invoke();
			currentItem = null;
		}
	}
}