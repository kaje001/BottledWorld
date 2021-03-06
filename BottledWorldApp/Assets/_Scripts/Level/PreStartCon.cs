﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStartCon : MonoBehaviour
{

	[SerializeField] GameObject mainCamDummy;
	[SerializeField] PlayerController mainCon;
	[SerializeField] GameObject mainPanel;
	[SerializeField] GameObject preCamDummy;
	[SerializeField] Transform preCamCenter;
	[SerializeField] GameObject preSpline;
	[SerializeField] GameObject prePanel;

	[SerializeField] Fade fadeOverlay;

	[SerializeField] float waitTime = 15f;
	float timestamp = 0;
	bool a = true;
	bool b = true;
	Vector3 gravity;
	Vector3 gravityGyro;

	// Use this for initialization
	void Start ()
	{
		mainCamDummy.SetActive (false);
		mainPanel.SetActive (false);
		mainCon.enabled = false;
		timestamp = Time.time;
		fadeOverlay.FadeOut (1f);

		VFXandSoundTrigger.Instance.TriggerAntFalling ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > (timestamp + waitTime-1.5f) && a) {
			a = false;

			fadeOverlay.FadeIn (2f);
		}
		if (Time.time > (timestamp + waitTime+0.2f) && b) {
			b = false;

			fadeOverlay.FadeOut (3f);
			mainCamDummy.SetActive (true);
			mainPanel.SetActive (true);
			mainCon.enabled = true;
			preCamDummy.SetActive (false);
			preSpline.SetActive (false);
			prePanel.SetActive (false);
			enabled = false;
		}

		if (CoinController.Instance.state.settingsControls) {

		} else {
			gravityGyro = GyroCon.Instance.GetGyroGravity ();
			gravity = Vector3.Lerp (gravity, gravityGyro, 7f * Time.deltaTime);
			Vector3 v = gravity;
			//v.x = -v.x;
			preCamDummy.transform.up = -v;
			preCamDummy.transform.GetChild(0).LookAt (preCamCenter);
		}

		if ((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && a && b) {
			timestamp = Time.time;
			waitTime = 1.5f;
			SoundManager.Instance.StopSingleCancelable ();
		}
	}
		
}
