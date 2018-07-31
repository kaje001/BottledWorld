using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{

	public static SoundManager Instance {get;set;}     //Allows other scripts to call functions from SoundManager.     

	public AudioSource efxSource1;  
	public AudioSource efxSource2;  
	public AudioSource efxSource3;  
	public AudioSource efxSource4;                   //Drag a reference to the audio source which will play the sound effects.
	public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.        


	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}


	public void PlaySingle(AudioClip clip)
	{
		if (!CoinController.Instance.state.settingsSound) {
			return;
		}

		if (!efxSource1.isPlaying) {
			efxSource1.clip = clip;
			efxSource1.Play ();
		} else if (!efxSource2.isPlaying) {
			efxSource2.clip = clip;
			efxSource2.Play ();
		} else if (!efxSource3.isPlaying) {
			efxSource3.clip = clip;
			efxSource3.Play ();
		} else {
			efxSource4.clip = clip;
			efxSource4.Play ();
		}


	}

	public void PlayMusic(AudioClip clip)
	{
		if (!CoinController.Instance.state.settingsMusic) {
			return;
		}
		musicSource.clip = clip;

		musicSource.Play ();
	}

	public void StopMusic(){
		musicSource.Stop ();
	}

}