using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DentedPixel;

public class CreditsGeneralControl : MonoBehaviour {

	[SerializeField] GameObject imageMain;
	[SerializeField] GameObject panelLoading;
	int rotation = 0;

	[SerializeField] AudioClip audioButtonClick;
	[SerializeField] AudioClip musicCredits;

	void Start(){
		SoundManager.Instance.PlayMusic (musicCredits);
	}

	public void LoadStartUp(){
		SoundManager.Instance.StopMusic ();
		SoundManager.Instance.PlaySingle (audioButtonClick);
		if (LastGameData.Instance.level == -1) {
			SceneManager.LoadScene ("Menu");
			panelLoading.SetActive (true);

		} else if (LastGameData.Instance.level == -2) {
			SceneManager.LoadScene ("StartUp");
			panelLoading.SetActive (true);

		}
	}

	public void RotateRight(){
		SoundManager.Instance.PlaySingle (audioButtonClick);
		if (rotation == 360 || rotation == -360) {
			rotation = 0;
		}
		rotation -= 45;
		LeanTween.rotateZ (imageMain, rotation, 1f).setEase (LeanTweenType.easeInOutCubic);
	}

	public void RotateLeft(){
		SoundManager.Instance.PlaySingle (audioButtonClick);
		if (rotation == 360 || rotation == -360) {
			rotation = 0;
		}
		rotation += 45;
		LeanTween.rotateZ (imageMain, rotation, 1f).setEase (LeanTweenType.easeInOutCubic);
	}
}
