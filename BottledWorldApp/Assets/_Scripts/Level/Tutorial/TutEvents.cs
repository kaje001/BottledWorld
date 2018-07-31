using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutEvents : MonoBehaviour {

	[SerializeField] PlayerController playcon;
	[SerializeField] GameObject pausePanel;

	[SerializeField] string[] texts;
	[SerializeField] Sprite[] spritesInfo;
	[SerializeField] Image imageInfo;
	[SerializeField] Text textInfo;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "TutTrigger") {
			int index = other.gameObject.GetComponent<TriggerNumber> ().index;
			ShowNextScreen (index);
		}
	}

	void ShowNextScreen(int index){
		playcon.PauseGame (pausePanel);
		imageInfo.sprite = spritesInfo [index];
		textInfo.text = texts [index];
	}

	public void UnpauseTut(){
		playcon.PauseGame (pausePanel);
	}
}
