using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField] GameObject panelUnlock;

	// Use this for initialization
	void Start () {
		panelSugar.SetActive (false);
		panelUnlock.SetActive (false);
		panelScore.SetActive (false);
		fade.FadeOut (2f);
	}

	void Update(){
		if (Vector3.Distance (transform.position, stopTrigger.transform.position) < 0.005f && !pause) {
			pause = true;
			PauseMovement ();
		}
		if (Vector3.Distance (transform.position, jumpTrigger.transform.position) < 0.005f && !jump) {
			jump = true;
			animator.SetTrigger ("jumpUp");

		}
		if (Vector3.Distance (transform.position, groundTrigger.transform.position) < 0.005f && !ground) {
			ground = true;
			animator.SetTrigger ("floor");
			ps.Play ();
		}
		if (Vector3.Distance (transform.position, fadeTrigger.transform.position) < 0.005f && !fadeb) {
			fadeb = true;
			fade.FadeIn (3f);
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

	IEnumerator ShowScore(){
		yield return new WaitForSeconds (0.8f);
		panelSugar.SetActive (true);
		//playSound&Effect
		yield return new WaitForSeconds (0.7f);
		panelUnlock.SetActive (true);
		//playSound&Effect

	}
}
