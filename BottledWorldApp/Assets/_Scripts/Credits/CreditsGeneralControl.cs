using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DentedPixel;

public class CreditsGeneralControl : MonoBehaviour {

	[SerializeField] GameObject imageMain;
	int rotation = 0;

	[SerializeField] AudioClip audioButtonClick;

	public void LoadStartUp(){
		SoundManager.Instance.PlaySingle (audioButtonClick);
		if (LastGameData.Instance.level == -1) {
			SceneManager.LoadScene ("Menu");

		} else if (LastGameData.Instance.level == -2) {
			SceneManager.LoadScene ("StartUp");

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
