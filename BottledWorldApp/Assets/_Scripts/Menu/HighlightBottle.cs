﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBottle : MonoBehaviour {

	[SerializeField] GameObject hightlightPic;
	[SerializeField] Material GlasClear;
	[SerializeField] Material GlasMatt;
	[SerializeField] public bool active;

	void Start(){
		hightlightPic.SetActive (false);
	}

	public void Hightlight(bool b){
		hightlightPic.SetActive (b);
	}

	public void SetMaterial(){
		if (active) {
			transform.GetChild(0).GetComponentInChildren<Renderer> ().material = GlasClear;
		} else {
			transform.GetChild(0).GetComponentInChildren<Renderer> ().material = GlasMatt;
		}
	}
}
