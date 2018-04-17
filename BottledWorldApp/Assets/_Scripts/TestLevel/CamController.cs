using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour {

	public Slider camHeight;
	public Slider camDist;
	public Slider camAngle;

	public Camera mainCam;
	float lastHeight = 0f;
	float lastDist = 0f;
	float lastAngle = 0f;

	public Text a;
	public Text b;
	public Text c;

	// Use this for initialization
	void Start () {
		camHeight.onValueChanged.AddListener(delegate {
			ChangeHeight();
		});
		camDist.onValueChanged.AddListener(delegate {
			ChangeDis();
		});
		camAngle.onValueChanged.AddListener(delegate {
			ChangeAngle();
		});


		a.text = camHeight.value.ToString ();
		b.text = camDist.value.ToString ();
		c.text = camAngle.value.ToString ();

	}


	public void ChangeHeight(){
		Vector3 pos = mainCam.transform.localPosition;
		pos.y += camHeight.value - lastHeight;
		mainCam.transform.localPosition = pos;
		lastHeight = camHeight.value;
		a.text = camHeight.value.ToString ();
	}

	public void ChangeDis(){

		Vector3 pos = mainCam.transform.localPosition;
		pos.z += camDist.value - lastDist;
		mainCam.transform.localPosition = pos;
		lastDist = camDist.value;
		b.text = camDist.value.ToString ();
	}

	public void ChangeAngle(){

		mainCam.transform.Rotate(camAngle.value-lastAngle,0f,0f,Space.Self);
		lastAngle = camAngle.value;
		c.text = camAngle.value.ToString ();
	}
}
