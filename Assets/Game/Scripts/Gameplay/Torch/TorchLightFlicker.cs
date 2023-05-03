using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Scripts.Characters.Player
{
	public class TorchLightFlicker : MonoBehaviour
	{
		[SerializeField]
		private float minTorchLightInterval = 1f;

		[SerializeField]
		private float maxTorchLightInterval = 1f;

		[SerializeField]
		private float lightOutDuration = 0.1f;

		[SerializeField]
		private float baseIntensity;

		[ReadOnly]
		[SerializeField]
		private float timeUntilNextLightOn;

		public event Action<float> OnTorchLightOut;
		
		public bool canTorchLightBeChanged = true;

		private void Start()
		{
			timeUntilNextLightOn = Random.Range(minTorchLightInterval, maxTorchLightInterval);
		}

		private void Update()
		{
			if (!canTorchLightBeChanged) return;

			timeUntilNextLightOn -= Time.deltaTime;

			if (timeUntilNextLightOn <= 0.0f)
			{
				var flickerIntensity = Random.Range(minTorchLightInterval, maxTorchLightInterval);
				OnTorchLightOut?.Invoke(flickerIntensity);

				// Reset timeUntilNextLightOn to a random value
				timeUntilNextLightOn = Random.Range(minTorchLightInterval, maxTorchLightInterval);
			}
		}
	}
}