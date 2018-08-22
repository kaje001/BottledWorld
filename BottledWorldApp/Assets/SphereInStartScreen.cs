using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInStartScreen : MonoBehaviour {

	[SerializeField] Material[] mat;
	[SerializeField] AudioClip audioTick;

	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().material = mat [0];
	}
	
	void OnTriggerExit(Collider other){
		GetComponent<Renderer> ().material = mat [1];
		SoundManager.Instance.PlaySingle (audioTick);
	}
}
