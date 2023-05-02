using Cysharp.Threading.Tasks;
using Game.Scripts.Characters.Player;
using UnityEngine;

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

		public static async UniTask<PlayerRig> LoadAsync()
		{
			var loadedObject = await Resources.LoadAsync<PlayerRig>(PLAYER_RIG_PREFAB_PATH);
			var rig = (PlayerRig) Instantiate(loadedObject);
			return rig;
		}
	}
}