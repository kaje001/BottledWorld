using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

	[SerializeField] float time;

	// Use this for initialization
	void Start () {
		StartCoroutine (Destroy (time));
	}

	IEnumerator Destroy(float timeTillDestroy){
		yield return new WaitForSeconds (timeTillDestroy);
		Destroy (gameObject);
	}

}
