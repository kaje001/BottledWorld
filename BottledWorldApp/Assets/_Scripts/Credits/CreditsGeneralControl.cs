using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DentedPixel;

public class CreditsGeneralControl : MonoBehaviour {

	[SerializeField] GameObject imageMain;
	int rotation = 0;

	public void LoadStartUp(){
		SceneManager.LoadScene ("StartUp");
	}

	public void RotateRight(){
		if (rotation == 360 || rotation == -360) {
			rotation = 0;
		}
		rotation -= 45;
		LeanTween.rotateZ (imageMain, rotation, 1f).setEase (LeanTweenType.easeInOutCubic);
	}

	public void RotateLeft(){
		if (rotation == 360 || rotation == -360) {
			rotation = 0;
		}
		rotation += 45;
		LeanTween.rotateZ (imageMain, rotation, 1f).setEase (LeanTweenType.easeInOutCubic);
	}
}
