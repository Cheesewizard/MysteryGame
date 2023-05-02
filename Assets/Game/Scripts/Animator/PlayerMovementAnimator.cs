using Game.Scripts.Gameplay.Player;
using UnityEngine;

namespace Game.Scripts.Animator
{
	public class PlayerMovementAnimator : MonoBehaviour
	{
		[SerializeField]
		private PlayerRig playerRig;

		private void Start()
		{
			playerRig.PlayerMovementBehaviour.OnMovement += HandleMovementAnimation;
			playerRig.PlayerMovementBehaviour.OnKilled += HandleDeathAnimation;
		}

		private void HandleMovementAnimation(bool isWalking)
		{
			playerRig.PlayerAnimator.SetBool("IsWalking", isWalking);
		}

		private void HandleDeathAnimation()
		{
			// This uses a state behaviour within the animator that deletes the gameObject after the death animation.
			playerRig.PlayerAnimator.SetTrigger("Death");
		}

		private void OnDestroy()
		{
			playerRig.PlayerMovementBehaviour.OnMovement -= HandleMovementAnimation;
			playerRig.PlayerMovementBehaviour.OnKilled -= HandleDeathAnimation;
		}
	}
}