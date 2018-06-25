using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class UpAndDownFloating : MonoBehaviour {

	[SerializeField] float yGoal;

	// Use this for initialization
	void Start () {
		LeanTween.delayedCall(gameObject, Random.Range(0,120)/100f, 
			FloatUpDown
		);
	}

	void FloatUpDown(){
		LeanTween.moveLocalY (gameObject, gameObject.transform.localPosition.y + yGoal, 1.2f).setLoopPingPong (-1).setEase (LeanTweenType.easeInOutSine);
	}
	

}
