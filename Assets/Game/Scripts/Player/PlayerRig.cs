using Game.Scripts.Characters.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Gameplay.Player
{
	public class PlayerRig : MonoBehaviour
	{
		private const string PLAYER_RIG_PREFAB_PATH = "Characters/Humans/Male/Player_Rig";

		[SerializeField]
		private PlayerMovementBehaviour playerMovementBehaviour;

		public PlayerMovementBehaviour PlayerMovementBehaviour => playerMovementBehaviour;

		[SerializeField]
		private UnityEngine.Animator playerAnimator;

		public UnityEngine.Animator PlayerAnimator => playerAnimator;

		[SerializeField]
		private PlayerInventory playerInventory;

		public PlayerInventory PlayerInventory => playerInventory;

		[SerializeField]
		private PlayerInteractionBehaviour playerInteractionBehaviour;

		public PlayerInteractionBehaviour PlayerInteractionBehaviour => playerInteractionBehaviour;
	}
}