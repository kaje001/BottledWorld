using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOverHight : MonoBehaviour {

	[SerializeField] float hight;
	[SerializeField] Color[] colors; //0  = transparent; 1 = black;
	bool visible = false;
	TextMesh textMesh;

	void Start(){
		textMesh = GetComponent<TextMesh> ();
		textMesh.color = colors [0];
	}

	// Update is called once per frame
	void Update () {
		if (!visible && transform.position.y > hight) {
			textMesh.color = colors [1];
			visible = true;
		} else if (visible && transform.position.y < hight) {
			textMesh.color = colors [0];
			visible = false;
		}
	}
}
