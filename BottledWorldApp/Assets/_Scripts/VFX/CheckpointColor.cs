using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointColor : MonoBehaviour {

	[SerializeField] Color colorSelected;
	[SerializeField] Color colorUnselected;

	void Start(){
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
			sr.color = colorUnselected;
		}
	}

	public void ChangeColorChecked(){
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
			sr.color = colorSelected;
		}
	}
		
}
