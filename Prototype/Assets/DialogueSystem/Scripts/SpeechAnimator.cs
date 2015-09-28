using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeechAnimator : MonoBehaviour {
	private Text AnimationTextReference;	// just a reference to the npc's dialogue
	private string MySpeech = "";	// used to animate the text
	private float LastUpdatedTime = 0;
	private float UpdateCoolDown = 0.1f;
	public float UpdateCoolDownMin = 0.05f;
	public float UpdateCoolDownMax = 0.1f;
	public int CharacterIndex;
	public List<AudioClip> MySpeechSounds = new List<AudioClip>();
	public AudioSource MyAudioSource;
	public UnityEngine.Events.UnityEvent OnFinishedAnimationFunction;

	// Use this for initialization
	void Awake () {
		AnimationTextReference = gameObject.GetComponent<Text> ();
		if (MyAudioSource == null) 
			MyAudioSource = gameObject.GetComponent<AudioSource> ();
	}
	
	void Update () {
		UpdateAnimation ();
	}

	// animates from AnimationTextReference.text to MySpeech (both strings)
	private void UpdateAnimation() {
		if (CharacterIndex < MySpeech.Length && 
		    Time.time - LastUpdatedTime > UpdateCoolDown) {
			// make sound - different sound for each character added - its meant to sound like a type writer
			if (CharacterIndex % 2 == 0 && MySpeechSounds.Count > 0) 
			{
				MyAudioSource.PlayOneShot(MySpeechSounds[Random.Range(0,MySpeechSounds.Count)]);
				if (MySpeech[CharacterIndex] == ' ')
					MyAudioSource.volume = 0.1f;
				else
					MyAudioSource.volume = 1f;
			}
			// add another character to it
			AnimationTextReference.text += MySpeech[CharacterIndex];
			CharacterIndex++;
			
			LastUpdatedTime = Time.time;
			UpdateCoolDown = Random.Range(UpdateCoolDownMin, UpdateCoolDownMax);

			if (CharacterIndex == MySpeech.Length) {	// end of animation
				//Debug.LogError("Calling function: " + Time.time);	// debugging
				OnFinishedAnimationFunction.Invoke();
			}
		}
	}

	// begins animation a new, normally use new line instead to change what it animates too
	public void ResetAnimation() {
		AnimationTextReference.text = "";	// set to nothing
		LastUpdatedTime = Time.time;
		CharacterIndex = 0;
		UpdateCoolDown = Random.Range(UpdateCoolDownMin, UpdateCoolDownMax);
	}

	public void NewLine(string NewLine) {
		MySpeech = NewLine;
		ResetAnimation ();
	}
}
