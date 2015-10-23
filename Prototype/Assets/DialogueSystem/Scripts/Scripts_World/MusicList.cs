using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem {
	public class MusicList : GuiListHandler {
		public List<AudioClip> Music;
		AudioSource MySource;

		void Start() {
			MySource = gameObject.GetComponent<AudioSource> ();
			UpdateMusicGuiList ();
		}
		public void PlaySong(int i) {
			StopMusic ();
			MySource.PlayOneShot (Music [i]);
		}
		public void StopMusic() {
			MySource.Stop ();
		}
		public void UpdateMusicGuiList() {
			Debug.Log ("Refreshing Inventory Gui: " + Time.time);
			Clear ();
			for (int i = 0; i < Music.Count; i++) {
				AddGui(Music[i].name);
				MusicItemHandler MyMus = MyGuis[MyGuis.Count-1].AddComponent<MusicItemHandler>();
				MyMus.ListIndex = i;
				MyMus.MyMusicList = this;
			}
		}
	}
}
