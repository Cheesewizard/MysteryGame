using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Characters.UI.Controller_Icons
{
	public class ControllerIcons : MonoBehaviour
	{
		[SerializeField]
		private InputActionReference inputAction;

		[SerializeField]
		private TextMeshPro textMeshPro;

		[SerializeField]
		private TMP_SpriteAsset keyboard;

		[SerializeField]
		private TMP_SpriteAsset gamepad;

		private string originalText;

		private bool IsGamepad => GameControllerManager.instance.IsGamepad;

		private void Awake()
		{
			originalText = textMeshPro.text;
			textMeshPro.text = ReadAndReplaceBinding(originalText, IsGamepad);
			GameControllerManager.instance.OnControllerChange += HandleControllerChanged;
		}

		private void HandleControllerChanged(bool isGamepad)
		{
			textMeshPro.text = ReadAndReplaceBinding(originalText, isGamepad);
		}

		private string ReadAndReplaceBinding(string textToDisplay, bool isGamepad)
		{
			var stringButtonName = GetButtonName(inputAction.action.ToString(), isGamepad);

			var newValue = $"<sprite=\"{(isGamepad ? gamepad.name : keyboard.name)}\" name={stringButtonName}>";
			return textToDisplay.Replace("BUTTONPROMPT", newValue );
		}

		private string GetButtonName(string stringButtonName, bool isGamepad)
		{
			try
			{
				var buttonName = stringButtonName.Split("/").Last();
				buttonName = buttonName.Substring(0, buttonName.Length - 1);

				var iconPath = "";
				if (!isGamepad)
				{
					iconPath = "keyboard_" + buttonName;
				}
				else
				{
					iconPath = buttonName;
				}

				return iconPath;
			}
			catch
			{
				return null;
			}
		}

		private void OnDestroy()
		{
			GameControllerManager.instance.OnControllerChange -= HandleControllerChanged;
		}
	}
}