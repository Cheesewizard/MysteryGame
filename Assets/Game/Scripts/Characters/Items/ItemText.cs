using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Characters.Items
{
	public class ItemText : MonoBehaviour
	{
		[SerializeField]
		private List<string> itemTextRows;

		public List<string> GetItemText()
		{
			return itemTextRows;
		}
	}
}