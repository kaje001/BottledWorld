using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class VFX_UI_slide : MonoBehaviour {

	Vector3 homePos;
	[SerializeField] GameObject target;
	[SerializeField] float time = 0.5f;
	ParticleSystem[] particels;

	// Use this for initialization
	void Start () {
		particels = GetComponentsInChildren<ParticleSystem> ();
		homePos = transform.localPosition;
	}
	
	public void StartEffect(){
		foreach (ParticleSystem pars in particels) {
			pars.Play ();
		}
		LeanTween.moveLocal (gameObject, target.transform.localPosition, time).setEase (LeanTweenType.easeInOutCubic).setOnComplete(TweenFinished);

	}

	void TweenFinished(){
		foreach (ParticleSystem pars in particels) {
			pars.Stop ();
		}
		transform.localPosition = homePos;
	}
}
