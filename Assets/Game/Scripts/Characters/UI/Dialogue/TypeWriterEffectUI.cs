using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.UI.Dialogue
{
	public class TypeWriterEffectUI : MonoBehaviour
	{
		public static TypeWriterEffectUI instance;

		[SerializeField]
		private TMP_Text _tmpProText;

		[SerializeField]
		private float delayBeforeStart = 0f;

		[SerializeField]
		private float timeBtwChars = 0.1f;

		[SerializeField]
		private string leadingChar = "";

		[SerializeField]
		private bool leadingCharBeforeDelay = false;

		private string writer;

		private void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(this);
		}

		public async UniTask<bool> TypeWriterTMP(string descriptionText, CancellationTokenSource cancellationToken)
		{
			writer = descriptionText;
			_tmpProText.text = "";

			_tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

			try
			{
				await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeStart), cancellationToken: cancellationToken.Token);
				cancellationToken.Token.ThrowIfCancellationRequested();

				foreach (var c in writer)
				{
					cancellationToken.Token.ThrowIfCancellationRequested();

					if (_tmpProText.text.Length > 0)
					{
						_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
					}

					_tmpProText.text += c;
					_tmpProText.text += leadingChar;
					await UniTask.Delay(TimeSpan.FromSeconds(timeBtwChars), cancellationToken: cancellationToken.Token);
				}

				if (leadingChar != "")
				{
					_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
				}

				return await UniTask.FromResult(true);
			}
			catch
			{
				return await UniTask.FromResult(false);
			}
		}
	}
}