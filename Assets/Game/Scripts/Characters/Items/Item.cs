using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Characters.Items
{
	public class Item : MonoBehaviour
	{
		[Header("Item Interactivity Properties")]
		[SerializeField]
		private ItemType itemType;
		public ItemType ItemType => itemType;

		[SerializeField]
		private bool canBePickedUp;

		public bool CanBePickedUp => canBePickedUp;

		[SerializeField]
		private bool canBeInteractedWith;

		public bool CanBeInteractedWith => canBeInteractedWith;

		[SerializeField]
		private bool canReadForever;

		public bool CanReadForever => canReadForever;

		[Header("Dependencies")]
		[SerializeField]
		private ItemText itemText;
		
		[SerializeField]
		private TextMeshPro pickupButtonUi;

		[SerializeField]
		private TextMeshPro interactButtonUi;

		[Header("Item Effects")]
		[SerializeField]
		private ParticleSystem pickUpEffect;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip pickUpSound;

		private bool isButtonsEnabled;

		private bool isRead;
		public bool IsRead => isRead;

		private bool isPickedUp;
		public bool IsPickedUp => isPickedUp;

		private bool isReading;
		public bool IsReading => isReading;

		public Item PickUpItem()
		{
			if (canBePickedUp && isRead)
			{
				if (pickUpEffect != null)
				{
					pickUpEffect.Play();
				}

				if (pickUpSound != null)
				{
					audioSource.PlayOneShot(pickUpSound);
				}

				isPickedUp = true;
				gameObject.SetActive(false);
				return this;
			}

			return null;
		}

		public List<string> GetDescription()
		{
			return itemText.GetItemText();
		}

		public void CheckIfTextHasBeenRead(bool hasRead)
		{
			isRead = hasRead;
			MarkAsNotReading();

			if (isRead)
			{
				ShowButtons();
			}
			else
			{
				HideButtons();
			}
		}

		public void ShowButtons()
		{
			if (isRead && !canReadForever)
			{
				interactButtonUi.gameObject.SetActive(false);
				if (canBePickedUp)
				{
					pickupButtonUi.gameObject.SetActive(true);
				}
			}

			if (!isRead || canReadForever)
			{
				pickupButtonUi.gameObject.SetActive(false);
				interactButtonUi.gameObject.SetActive(true);
			}
		}

		public void HideButtons()
		{
			interactButtonUi.gameObject.SetActive(false);
			pickupButtonUi.gameObject.SetActive(false);
		}

		public void MarkAsIsReading()
		{
			isReading = true;
		}

		private void MarkAsNotReading()
		{
			isReading = false;
		}
	}
}