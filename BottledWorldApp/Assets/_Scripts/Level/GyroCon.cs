using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroCon : MonoBehaviour {

	public static GyroCon Instance { get; set; }

	Gyroscope gyro;
	public PlayerController playCon;
	// Use this for initialization

	void Awake(){
		Instance = this;
	}

	void Start () {
		gyro = Input.gyro;
		if (!CoinController.Instance.state.settingsControls) {
			gyro.enabled = true;
		} else {
			Input.gyro.enabled = false;
		}
	}
	
	// Update is called once per frame
	/*void Update () {
		playCon.RotateWorldGyro (new Vector3(Input.gyro.gravity.x, Input.gyro.gravity.y, 0f).normalized);
		//txt.text = Input.gyro.attitude.ToString ();

	}*/

	public Vector3 GetGyroGravity(){

		return new Vector3(gyro.gravity.x, gyro.gravity.y, 0f).normalized;
	}
}
