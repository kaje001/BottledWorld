using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStartCon : MonoBehaviour {

	[SerializeField] GameObject mainCam;
	[SerializeField] PlayerController mainCon;
	[SerializeField] GameObject preCam;
	[SerializeField] GameObject preSpline;

	[SerializeField] float waitTime = 13;
	float timestamp;
	bool b = true;

	// Use this for initialization
	void Start () {
		mainCam.SetActive (false);
		mainCon.enabled = false;
		timestamp = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timestamp + waitTime && b) {
			b = false;

			mainCam.SetActive (true);
			mainCon.enabled = true;
			preCam.SetActive (false);
			preSpline.SetActive (false);
		}
	}
}
