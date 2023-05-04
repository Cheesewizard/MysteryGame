using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Characters.Player;
using Game.Scripts.Controls;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.UI.Dialogue
{
	public class DialogueBehaviour : MonoBehaviour
	{
		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		[SerializeField]
		private PlayerInteractionBehaviour playerInteractionBehaviour;

		[SerializeField]
		private TextMeshProUGUI textMeshPro;

		[SerializeField]
		private Canvas canvas;

		private CancellationTokenSource tokenSource;
		private CancellationTokenSource controllerTokenSource;

		private Action<bool> callback;

		private void Start()
		{
			canvas.gameObject.SetActive(false);
			playerInteractionBehaviour.OnExitItem += HandleExitItem;
		}

		private void HandleExitItem()
		{
			CancelTypeWriter();
			CancelDialogue();
			
			canvas.gameObject.SetActive(false);
			textMeshPro.text = string.Empty;
			DisableUIControls();
		}

		public async void AwaitCallBack(Action<bool> callback, List<string> textQueue)
		{
			EnableUIControls();
			canvas.gameObject.SetActive(true);

			this.callback = callback;
			await AnimateText(textQueue);

			canvas.gameObject.SetActive(false);
			DisableUIControls();
		}

		private async UniTask<UniTask> AnimateText(List<string> textQueue)
		{
			CancelDialogue();
			controllerTokenSource = new CancellationTokenSource();

			var isSuccess = true;
			if (textQueue == null)
			{
				Debug.LogError("Item text queue is null");
				controllerTokenSource?.Cancel();
			}

			foreach (var nextText in textQueue)
			{
				// Cancel previous the typewriter
				CancelTypeWriter();
				tokenSource = new CancellationTokenSource();

				var wasCompleted = await TypeWriterEffectUI.instance.TypeWriterTMP(nextText, tokenSource);
				if (!wasCompleted)
				{
					isSuccess = false;
					break;
				}

				await UniTask
					.WaitUntil(playerInput.Dialogue.Next.WasPerformedThisFrame,
						cancellationToken: controllerTokenSource.Token).SuppressCancellationThrow();
			}

			Debug.Log($"Dialogue succeeded? {isSuccess && !controllerTokenSource.Token.IsCancellationRequested}");
			callback?.Invoke(isSuccess && !controllerTokenSource.Token.IsCancellationRequested);
			return UniTask.CompletedTask;
		}

		private void CancelTypeWriter()
		{
			tokenSource?.Cancel();
		}

		private void CancelDialogue()
		{
			controllerTokenSource?.Cancel();
		}

		private void EnableUIControls()
		{
			playerInput.Player.Read.Disable();
			playerInput.Dialogue.Next.Enable();
		}

		private void DisableUIControls()
		{
			playerInput.Dialogue.Next.Disable();
			playerInput.Player.Read.Enable();
		}

		private void OnDestroy()
		{
			playerInteractionBehaviour.OnExitItem -= HandleExitItem;
			tokenSource?.Dispose();
		}
	}
}