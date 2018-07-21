using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

public class IconWiggel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LeanTween.rotateZ(gameObject, 20,1.5f).setLoopPingPong (-1).setEase (LeanTweenType.easeInOutBack);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
