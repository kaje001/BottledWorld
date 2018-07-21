using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsGyroCon : MonoBehaviour {

	Gyroscope gyro;
	Vector3 gravity;
	float rotSpeed = 100;

	// Use this for initialization
	void Start () {
		gravity = new Vector3 (0, 1f, 0);
		gyro = Input.gyro;
		gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		GetRotateWorld ();

		transform.up = new Vector3 (gyro.gravity.x, gyro.gravity.y);
		transform.GetChild (0).up = gravity;
	}

	void GetRotateWorld(){
		float rot = TouchCon.Instance.GetDragLength ();
		//Debug.Log (rot);
		gravity = Quaternion.Euler(0, 0, rot * rotSpeed) * gravity;
		//Debug.Log (gravityGyro);

	}
}
