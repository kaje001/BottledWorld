﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpController : MonoBehaviour {

	[SerializeField] Fade fadeLogo;
	[SerializeField] Fade fadeCaution;
	float timeStamp;
	bool faded = false;
	bool logo = false;
	bool starting = false;

	[SerializeField] GameObject panelLoading;

	[SerializeField] GameObject panelStart;
	[SerializeField] GameObject arrow;

	[SerializeField] Transform antMiddle;
	float lastAngle = 0;

	Vector3 startGyroGravity;
	[SerializeField] GameObject buttonStartGame;
	[SerializeField] GameObject textStartRotate;
	[SerializeField] GameObject panelGyroNotWorking;

	[SerializeField] Texture2D tex;
	[SerializeField] Material mat;

	[SerializeField] AudioClip soundButtonClick;

	// Use this for initialization
	void Start () {
		//fadeLogo.FadeIn(2f);
		//StartCoroutine(FadeCaution());
		//timeStamp = Time.time + 2f;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		panelLoading.SetActive (false);

		panelGyroNotWorking.SetActive (false);

		if (CoinController.Instance.state.settingsControls) {
			textStartRotate.SetActive (false);
			buttonStartGame.SetActive (true);
		} else {
			textStartRotate.SetActive (true);
			buttonStartGame.SetActive (false);
		}

		mat.mainTexture = tex;

		startGyroGravity = Input.gyro.gravity;
		StartCoroutine (CheckGyro ());
	}
	
	// Update is called once per frame
	/*void Update () {
		if(Time.time > timeStamp && ! faded){
			fadeLogo.FadeOut(2f);
			faded = true;
			timeStamp = Time.time + 2f;
		}
		if(Time.time > timeStamp && ! logo){
			panelStart.SetActive(true);
			starting = true;
			antMiddle.gameObject.SetActive (true);
			//arrow.SetActive (true);
		}
	}*/
	
	public void StartGame(){

		SoundManager.Instance.PlaySingle (soundButtonClick);
		SceneManager.LoadScene("Menu");
		panelLoading.SetActive (true);
	}

	public void LoadCredits(){

		LastGameData.Instance.level = -2;
		SoundManager.Instance.PlaySingle (soundButtonClick);
		SceneManager.LoadScene("Credits");
		panelLoading.SetActive (true);
	}

	public void RotateAntGyro(Vector2 gravityGyro){
		//if (starting) {
			float angle = Vector2.SignedAngle (Vector2.down, gravityGyro);

			if (angle > lastAngle || angle < lastAngle - 15) {
				return;
			}
			
			if (angle > -20 && angle < 30) {
				angle = -20;
			}
			if (angle < -175) {
				angle = 179f;
			}
			//Debug.Log (angle);
			antMiddle.Rotate (0f, 0f, angle - lastAngle);

			if (angle > 30 && angle < 35) {
				StartGame ();
			}

			lastAngle = angle;
		//}
	}

	/*IEnumerator FadeCaution(){
		yield return new WaitForSeconds (2f);
		fadeLogo.FadeOut(2f);
		yield return new WaitForSeconds (2f);
		fadeCaution.FadeIn (4f);
		yield return new WaitForSeconds (2f);
		fadeCaution.FadeOut (4f);
		yield return new WaitForSeconds (1f);
		panelStart.SetActive(true);
		starting = true;
		antMiddle.gameObject.SetActive (true);
	}*/

	IEnumerator CheckGyro(){
		yield return new WaitForSeconds (3f);

		if (startGyroGravity == Input.gyro.gravity && !CoinController.Instance.state.settingsControls) {
			textStartRotate.SetActive (false);
			buttonStartGame.SetActive (true);
			panelGyroNotWorking.SetActive (true);
            CoinController.Instance.state.settingsControls = true;

        }

        yield return new WaitForSeconds(17f);

        if (!CoinController.Instance.state.settingsControls)
        {
            textStartRotate.SetActive(false);
            buttonStartGame.SetActive(true);
            panelGyroNotWorking.SetActive(true);
            CoinController.Instance.state.settingsControls = true;
        }
        
    }
	
}
