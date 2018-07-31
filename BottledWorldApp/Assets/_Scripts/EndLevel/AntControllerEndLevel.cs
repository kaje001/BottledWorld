using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AntControllerEndLevel : MonoBehaviour {

	[SerializeField] Animator animator;

	[SerializeField] SplineInterpolator splineConAnt;
	[SerializeField] SplineInterpolator splineConCam;

	[SerializeField] GameObject stopTrigger;
	[SerializeField] GameObject jumpTrigger;
	[SerializeField] GameObject groundTrigger;
	[SerializeField] GameObject fadeTrigger;
	bool pause = false;
	bool jump = false;
	bool ground = false;
	bool fadeb = false;

	[SerializeField] ParticleSystem ps;

	[SerializeField] Fade fade;

	[SerializeField] GameObject panelScore;
	[SerializeField] GameObject panelSugar;
	[SerializeField] GameObject panelSugarInBottle;
	[SerializeField] GameObject panelUnlock;

	[SerializeField] AudioClip audioScore;
	//[SerializeField] AudioClip audioUnlockLevel;
	[SerializeField] AudioClip audioJump;

	// Use this for initialization
	void Start () {

		panelSugarInBottle.SetActive (false);
		panelSugar.SetActive (false);
		panelUnlock.SetActive (false);
		panelScore.SetActive (false);
		fade.FadeOut (2f);
	}

	void Update(){
		if (Vector3.Distance (transform.position, stopTrigger.transform.position) < 0.05f && !pause) {
			pause = true;
			PauseMovement ();
		}
		if (Vector3.Distance (transform.position, jumpTrigger.transform.position) < 0.05f && !jump) {
			jump = true;
			animator.SetTrigger ("jumpUp");
			SoundManager.Instance.PlaySingle (audioJump);

		}
		if (Vector3.Distance (transform.position, groundTrigger.transform.position) < 0.05f && !ground) {
			ground = true;
			animator.SetTrigger ("floor");
			SoundManager.Instance.PlaySingle (audioJump);
			ps.Play ();
		}
		if (Vector3.Distance (transform.position, fadeTrigger.transform.position) < 0.05f && !fadeb) {
			fadeb = true;
			fade.FadeIn (3f);
			StartCoroutine(WaitForLoadMenu ());
		}
	}

	void PauseMovement(){
		splineConAnt.pause = true;
		splineConCam.pause = true;
		panelScore.SetActive (true);
		StartCoroutine(ShowScore());
	}

	public void UnpauseMovement(){
		splineConAnt.pause = false;
		splineConCam.pause = false;
		animator.SetTrigger ("jumpDown");
		panelScore.SetActive (false);
	}

	public void RestartLevel(){
		SceneManager.LoadScene ("Level" + LastGameData.Instance.level);
	}

	IEnumerator ShowScore(){
		yield return new WaitForSeconds (0.8f);
		panelSugar.SetActive (true);
		SoundManager.Instance.PlaySingle (audioScore);
		yield return new WaitForSeconds (0.5f);
		panelSugarInBottle.SetActive (true);
		SoundManager.Instance.PlaySingle (audioScore);
		//playSound&Effect
		yield return new WaitForSeconds (0.5f);
		if(LastGameData.Instance.unlockLevel != 0 && !CoinController.Instance.IsLevelUnlocked(LastGameData.Instance.unlockLevel)){
			panelUnlock.SetActive (true); //Enable if unlocked a new Level
			CoinController.Instance.UnlockLevel (LastGameData.Instance.unlockLevel);
		}
		//playSound&Effect

	}
	IEnumerator WaitForLoadMenu(){
		yield return new WaitForSeconds (0.5f);
		SoundManager.Instance.StopMusic ();
		SceneManager.LoadScene ("Menu");
	}
}
