using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class PlayerInventory : MonoBehaviour
	{
		[SerializeField]
		private List<Item> inventory = new();

		public void AddItem(Item item)
		{
			inventory.Add(item);
		}

		public void RemoveItem(Item item)
		{
			if (inventory.Contains(item))
			{
				inventory.Remove(item);
			}
		}
	}
}