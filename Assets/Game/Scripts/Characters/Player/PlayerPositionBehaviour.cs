using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class PlayerPositionBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Transform playerRoot;

		private void Update()
		{
			gameObject.transform.position = playerRoot.position;
		}
	}
}