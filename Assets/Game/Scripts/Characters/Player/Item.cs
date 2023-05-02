using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.Player
{
	public class Item : MonoBehaviour
	{
		[SerializeField]
		private ItemType itemType;

		[SerializeField]
		public bool canBePickedUp;

		[SerializeField]
		public bool canBeInteractedWith;

		[SerializeField]
		private TextMeshPro itemDescription;

		[SerializeField]
		private TextMeshPro pickupButtonUi;

		[SerializeField]
		private TextMeshPro interactButtonUi;

		[SerializeField]
		private ParticleSystem pickUpEffect;

		private bool isButtonsEnabled;

		private void Start()
		{
			itemDescription.gameObject.SetActive(false);
		}

		public void SetButtons(bool isVisible)
		{
			isButtonsEnabled = isVisible;

			if (canBeInteractedWith)
			{
				interactButtonUi.gameObject.SetActive(isVisible);
			}

			if (canBePickedUp)
			{
				pickupButtonUi.gameObject.SetActive(isVisible);
			}
		}

		public Item PickUpItem()
		{
			if (canBePickedUp)
			{
				if (pickUpEffect != null)
				{
					pickUpEffect.Play();
				}

				gameObject.SetActive(false);
				return this;
			}

			return null;
		}

		public string GetDescription()
		{
			return itemDescription.text;
		}

		public void ToggleButtons()
		{
			isButtonsEnabled = !isButtonsEnabled;
			SetButtons(isButtonsEnabled);
		}

		// void OnDrawGizmos()
		// {
		// 	Gizmos.color = Color.blue;
		// 	Gizmos.DrawWireSphere(transform.position, raycastRadius);
		// }
	}
}