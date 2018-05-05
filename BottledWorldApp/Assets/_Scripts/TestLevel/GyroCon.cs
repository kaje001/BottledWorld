using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroCon : MonoBehaviour {
	Gyroscope gyro;
	public PlayerController playCon;
	// Use this for initialization
	void Start () {
		gyro = Input.gyro;
		gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		playCon.RotateWorldGyro (new Vector3(Input.gyro.gravity.x, Input.gyro.gravity.y, 0f).normalized);
		//txt.text = Input.gyro.attitude.ToString ();

	}
}
