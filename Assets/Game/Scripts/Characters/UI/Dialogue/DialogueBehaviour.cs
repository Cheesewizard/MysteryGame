using System;
using System.Threading;
using Game.Scripts.Characters.Player;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.UI.Dialogue
{
	public class DialogueBehaviour : MonoBehaviour
	{
		[SerializeField]
		private PlayerInteractionBehaviour playerInteractionBehaviour;

		[SerializeField]
		private TextMeshProUGUI textMeshPro;

		[SerializeField]
		private Canvas canvas;

		private CancellationTokenSource tokenSource;

		private Action<bool> callback;

		private bool isEnabled;

		private void Start()
		{
			canvas.gameObject.SetActive(false);
			playerInteractionBehaviour.OnExitItem += HandleExitItem;
		}

		private void HandleExitItem()
		{
			CancelTypeWriter();
			isEnabled = false;
			canvas.gameObject.SetActive(isEnabled);
			textMeshPro.text = string.Empty;
		}

		public async void AwaitCallBack(Action<bool> callback, string text)
		{
			isEnabled = !isEnabled;
			canvas.gameObject.SetActive(isEnabled);
			
			CancelTypeWriter();
			tokenSource = new CancellationTokenSource();
			var wasCompleted = await TypeWriterEffectUI.instance.TypeWriterTMP(text, tokenSource);
			callback?.Invoke(wasCompleted);
		}

		private void CancelTypeWriter()
		{
			tokenSource?.Cancel();
		}

		private void OnDestroy()
		{
			playerInteractionBehaviour.OnExitItem -= HandleExitItem;
			tokenSource?.Dispose();
		}
	}
}