using Game.Scripts.Controls;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class ShowInventoryBehaviour : MonoBehaviour
	{
		[SerializeField]
		private GameObject pictureClue;

		private PlayerInput playerInput => PlayerInputLocator.GetPlayerInput();

		private void Update()
		{
			// Toggle image clue
			if (playerInput.Player.Inventory.WasPressedThisFrame())
			{
				var isActive = pictureClue.gameObject.activeSelf;
				pictureClue.SetActive(!isActive);
			}
		}
	}
}