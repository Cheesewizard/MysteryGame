using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class DialogueBehaviour : MonoBehaviour
	{
		[SerializeField]
		private PlayerInteractionBehaviour playerInteractionBehaviour;

		[SerializeField]
		private TextMeshPro textMeshPro;

		private bool isEnabled;

		private void Start()
		{
			textMeshPro.gameObject.SetActive(false);
			playerInteractionBehaviour.OnInteractWithItem += HandleInteractWithItem;
			playerInteractionBehaviour.OnExitItem += HandleExitItem;
		}

		private void HandleExitItem()
		{
			isEnabled = false;
			textMeshPro.gameObject.SetActive(isEnabled);
		}

		private void HandleInteractWithItem(string text)
		{
			isEnabled = !isEnabled;
			textMeshPro.gameObject.SetActive(isEnabled);
			textMeshPro.text = text;
		}

		private void OnDestroy()
		{
			playerInteractionBehaviour.OnInteractWithItem -= HandleInteractWithItem;
			playerInteractionBehaviour.OnExitItem -= HandleExitItem;
		}
	}
}