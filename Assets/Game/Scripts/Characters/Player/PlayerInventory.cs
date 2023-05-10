using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Characters.Items;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class PlayerInventory : MonoBehaviour
	{
		[SerializeField]
		private List<Item> inventory = new();

		public void AddItem(Item item)
		{
			Debug.Log($"Added item to player inventory {item.ItemType}");
			inventory.Add(item);
		}

		public void RemoveItem(ItemType itemType)
		{
			if (inventory.Exists(x => x.ItemType == itemType))
			{
				var item = inventory.FirstOrDefault(x => x.ItemType == itemType);
				if (item != null)
				{
					Debug.Log($"Deleting item from player inventory {itemType}");
					inventory.Remove(item);
				}
			}
		}

		public List<Item> GetInventory()
		{
			var clone = new List<Item>();
			foreach (var item in inventory)
			{
				clone.Add(item);
			}

			return clone;
		}
	}
}