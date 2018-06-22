using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEvents : MonoBehaviour {

	SphereController sphereCon;

	void Start(){
		sphereCon = GetComponent<SphereController> ();
	}

	void OnTriggerEnter(Collider other){

		//Debug.Log ("Trigger");

		if(other.transform.tag == "Jump"){ //JumpTrigger anlaufen
			sphereCon.SphereJump();
			Debug.Log ("Trigger");
		}
		if(other.transform.tag == "Slow"){ //wird noch nict verwendet
			sphereCon.SetSphereSpeed(0.8f); 
		}
		if(other.transform.tag == "NormalSpeed"){ //wird noch nicht verwendet
			sphereCon.ResetSphereSpeed();
		}
		if(other.transform.tag == "Speed"){ //SpeedBoost anlaufen
			sphereCon.SphereBoost();
		}

	}
}
