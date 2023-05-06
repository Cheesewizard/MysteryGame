using UnityEngine;

namespace Game.Scripts.Characters.UI.Dialogue
{
	public class SoundManager : MonoBehaviour
	{
		public static SoundManager instance;

		[SerializeField]
		private AudioSource audioSource;

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

		public void PlayOneShot(AudioClip clip)
		{
			audioSource.PlayOneShot(clip);
		}
	}
}
