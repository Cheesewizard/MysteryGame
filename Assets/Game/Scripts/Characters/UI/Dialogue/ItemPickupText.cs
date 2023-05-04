using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Scripts.Characters.Items;
using Game.Scripts.Characters.Player;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.UI.Dialogue
{
	public class ItemPickupText : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI textMeshPro;

		[SerializeField]
		private PlayerInteractionBehaviour playerInteractionBehaviour;
		
		[SerializeField]
		private GameObject textCanvas;

		[SerializeField]
		private float textDelay = 2f;

		private string defaultText;

		private Queue<Action> actions = new();

		private void Start()
		{
			playerInteractionBehaviour.OnPickupItem += HandleItemPickedUp;
			defaultText = textMeshPro.text;
			textCanvas.SetActive(false);
		}

		private async void HandleItemPickedUp(Item pickedUpItem)
		{
			actions.Enqueue(() => DisplayItemText(pickedUpItem));

			while (actions.Count > 0)
			{
				var job = actions.Dequeue();
				job?.Invoke();

				textCanvas.SetActive(true);
				await UniTask.Delay(TimeSpan.FromSeconds(textDelay));
				textCanvas.SetActive(false);
			}
		}

		private void DisplayItemText(Item pickedUpItem)
		{
			textMeshPro.text = $"{defaultText} {pickedUpItem.ItemType}";
		}


		private void OnDestroy()
		{
			playerInteractionBehaviour.OnPickupItem -= HandleItemPickedUp;
		}
	}
}