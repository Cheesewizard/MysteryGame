using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Scripts.Characters.Player
{
	public class TorchLightBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Light2D torchLight;

		[SerializeField]
		private TorchLightFlicker torchLightFlicker;

		[SerializeField]
		private float lightSpeed = 5f;

		private bool isTorchAtDefault = true;

		private float startingTorchIntensity;

		private void Start()
		{
			if (torchLightFlicker != null)
			{
				torchLightFlicker.OnTorchLightOut += HandleTorchLight;
			}

			startingTorchIntensity = torchLight.intensity;
		}

		private void HandleTorchLight(float flickerIntensity)
		{
			if (!isTorchAtDefault) return;

			isTorchAtDefault = false;
			torchLight.intensity = flickerIntensity;
			isTorchAtDefault = true;
		}

		private void Update()
		{
			if (!isTorchAtDefault) return;

			// Gradually return to the base intensity
			torchLight.intensity =
				Mathf.Lerp(torchLight.intensity, startingTorchIntensity, Time.deltaTime * lightSpeed);
		}

		private void OnDisable()
		{
			if (torchLightFlicker != null)
			{
				torchLightFlicker.OnTorchLightOut -= HandleTorchLight;
			}
		}
	}
}