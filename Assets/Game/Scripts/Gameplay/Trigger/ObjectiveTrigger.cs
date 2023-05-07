using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Characters.Items;
using Game.Scripts.Characters.Player;
using Game.Scripts.Characters.UI.Dialogue;
using Unity.Collections;
using UnityEngine;

namespace Game.Scripts.Gameplay.Trigger
{
	public class ObjectiveTrigger : MonoBehaviour
	{
		[Header("Optional triggers")]
		[SerializeField]
		private List<GameObject> objectsToDisable;

		[SerializeField]
		private List<GameObject> objectsToEnable;

		[SerializeField]
		private List<GameObject> objectsToDisableAfterRead;

		[SerializeField]
		private List<ItemType> requiredItemsForTrigger;

		[SerializeField]
		private bool shouldRemoveItemsFromInventory;

		[SerializeField]
		private bool requiresItems;
		public bool RequiresItems => requiresItems;

		[SerializeField]
		private List<string> objectiveText;

		public List<string> ObjectiveText => objectiveText;


		[SerializeField]
		private List<string> objectiveCompletedText;

		public List<string> ObjectiveCompletedText => objectiveCompletedText;

		[ReadOnly]
		[SerializeField]
		private bool isCompleted;

		public bool IsCompleted => isCompleted;

		[ReadOnly]
		[SerializeField]
		private bool objectiveStartRead;

		public bool ObjectiveStartRead => objectiveStartRead;


		[ReadOnly]
		[SerializeField]
		private bool objectiveCompleteRead;

		public bool ObjectiveCompleteRead => objectiveCompleteRead;

		private DialogueBehaviour dialogueBehaviour;

		public bool CheckIfItemsInventory(PlayerInventory playerInventory)
		{
			var inventoryCopy = playerInventory.GetInventory();
			foreach (var itemType in requiredItemsForTrigger)
			{
				if (!inventoryCopy.Exists(x => x.ItemType == itemType))
				{
					Debug.Log($"Item does not exist in player inventory {itemType}, failure");
					return false;
				}

				var item = inventoryCopy.FirstOrDefault(x => x.ItemType == itemType);
				if (item != null)
				{
					inventoryCopy.Remove(item);
				}
			}

			if (shouldRemoveItemsFromInventory)
			{
				foreach (var itemType in requiredItemsForTrigger)
				{
					playerInventory.RemoveItem(itemType);
				}
			}

			Debug.Log($"Items for trigger succeeded");
			return true;
		}

		public void OnObjectiveStartTextRead(bool wasRead)
		{
			objectiveStartRead = wasRead;
			
			if (wasRead)
			{
				DisableObjectsAfterRead();
			}
		}

		public void OnObjectiveCompletedTextRead(bool wasRead)
		{
			objectiveCompleteRead = wasRead;
			isCompleted = objectiveCompleteRead;

			if (wasRead)
			{
				EnableObjects();
				DisableObjects();

				//Disable the trigger
				gameObject.SetActive(false);
			}
		}

		private void EnableObjects()
		{
			foreach (var toEnableObject in objectsToEnable)
			{
				toEnableObject.gameObject.SetActive(true);
			}
		}

		private void DisableObjects()
		{
			foreach (var toDisableObject in objectsToDisable)
			{
				toDisableObject.gameObject.SetActive(false);
			}
		}

		private void DisableObjectsAfterRead()
		{
			foreach (var toDisableObject in objectsToDisableAfterRead)
			{
				toDisableObject.gameObject.SetActive(false);
			}
		}
	}
}