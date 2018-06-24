using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtAnt : MonoBehaviour {

	[SerializeField] GameObject ant;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (ant.transform);
	}
}
