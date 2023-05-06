using System;
using System.Collections.Generic;
using Game.Scripts.Characters.Items;
using Game.Scripts.Characters.UI.Dialogue;
using Game.Scripts.Controls;
using Game.Scripts.Gameplay.Trigger;
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

		private bool isInTrigger;

		public event Action<List<string>> OnInteractWithItem;
		public event Action OnExitItem;
		public event Action<Item> OnPickupItem;

		private void Update()
		{
			if (currentItem == null || !isInTrigger) return;

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
						OnPickupItem?.Invoke(pickedUpItem);
						playerInventory.AddItem(pickedUpItem);
					}
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			isInTrigger = true;
			HandleObjectiveTrigger(other);

			var item = other.GetComponentInParent<Item>();
			if (item != null)
			{
				previousItem = currentItem;
				currentItem = item;
				Debug.Log($"Current item {item.name}");
				item.ShowButtons();
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			isInTrigger = false;

			dialogueBehaviour.CancelTypeWriter();
			dialogueBehaviour.CancelDialogue();

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

		private void HandleObjectiveTrigger(Collider2D other)
		{
			var objectiveTrigger = other.GetComponent<ObjectiveTrigger>();
			if (objectiveTrigger != null)
			{
				if (!objectiveTrigger.IsCompleted)
				{
					if (!objectiveTrigger.ObjectiveStartRead)
					{
						dialogueBehaviour.AwaitCallBack(objectiveTrigger.OnObjectiveStartTextRead,
							objectiveTrigger.ObjectiveText);
					}

					if (objectiveTrigger.ObjectiveStartRead)
					{
						var objectiveComplete = objectiveTrigger.CheckIfItemsInventory(playerInventory);
						if (objectiveComplete)
						{
							dialogueBehaviour.AwaitCallBack(objectiveTrigger.OnObjectiveCompletedTextRead,
								objectiveTrigger.ObjectiveCompletedText);
						}
						else
						{
							dialogueBehaviour.AwaitCallBack(objectiveTrigger.OnObjectiveStartTextRead,
								objectiveTrigger.ObjectiveText);
						}
					}
				}
			}
		}
	}
}