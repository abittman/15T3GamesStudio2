using UnityEngine;
using System.Collections;


namespace DialogueSystem {
	/*	Used for interaction with objects in the world
	 * 		a character ray traces the objects, and interacts with them
	*/
	public class ItemObject : MonoBehaviour {
		[Header("Pickup")]
		private bool HasActivated = false;
		[Tooltip("Is the item is picked up?")]
		public bool IsItemPickup = true;
		[Tooltip("Is the item destroyed when picked up?")]
		public bool IsDestroyedOnPickup = true;
		[Tooltip("Added to the characters inventory when picked up")]
		public Item MyItem;
		[Tooltip("Functions are called when item is interacted with (mouseclick)")]
		public UnityEngine.Events.UnityEvent OnItemInteract;
		
		[Header("Sounds")]
		[Tooltip("Played when item is picked up")]
		public AudioClip MyPickupSound;
		void Awake() {
		}

		public Item GetItem() {
			return MyItem;
		}

		public void Pickup() {
			if (!HasActivated) {
				HasActivated = true;
				GameObject ItemLeftOver = new GameObject();
				ItemLeftOver.transform.position = transform.position;
				ItemLeftOver.transform.rotation = transform.rotation;
				ItemLeftOver.transform.localScale = transform.localScale;
				if (MyPickupSound) {
					AudioSource MySource = ItemLeftOver.AddComponent<AudioSource>();
					if (MyPickupSound != null)
						MySource.PlayOneShot(MyPickupSound);
				}
				#if UNITY_EDITOR  
					// Editor code here.
					ParticleSystem ps1 = gameObject.GetComponent<ParticleSystem>();
					UnityEditorInternal.ComponentUtility.CopyComponent(ps1);
					UnityEditorInternal.ComponentUtility.PasteComponentAsNew(ItemLeftOver);
					ParticleSystem ps2 = ItemLeftOver.GetComponent<ParticleSystem>();
					ItemLeftOver.AddComponent<ParticlesEmmisionOverLifetime>();
				#endif

				OnItemInteract.Invoke ();

				if (IsDestroyedOnPickup)
					Destroy (gameObject);
			}
		}
		public void Reset() {
			HasActivated = false;
		}
	}
}
