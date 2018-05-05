using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroConStartUp : MonoBehaviour {
	Gyroscope gyro;
	public StartUpController playCon;
	// Use this for initialization
	void Start () {
		gyro = Input.gyro;
		gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		playCon.RotateAntGyro (new Vector2(Input.gyro.gravity.x, Input.gyro.gravity.y).normalized);
		//txt.text = Input.gyro.attitude.ToString ();

	}
}
