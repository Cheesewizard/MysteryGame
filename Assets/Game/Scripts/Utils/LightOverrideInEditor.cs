using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Game.Scripts.Utils
{
	public class LightToggle : MonoBehaviour
	{
		[SerializeField]
		private float lightIntensity;

		[SerializeField]
		private float editorLightIntensity;

		[SerializeField]
		private bool isOverrideLight;

		[SerializeField]
		private Light2D light;

		private void Awake()
		{
			if (light != null) light.intensity = lightIntensity;
		}

#if UNITY_EDITOR
		private void ToggleLight()
		{
			if (light != null) light.intensity = isOverrideLight ? editorLightIntensity : lightIntensity;
		}

		private void OnValidate()
		{
			ToggleLight();
		}
#endif
	}
}