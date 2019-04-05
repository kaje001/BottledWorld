using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutEvents : MonoBehaviour {

	[SerializeField] PlayerController playcon;
	[SerializeField] GameObject pausePanel;

    string[] texts = new string[5];
	[SerializeField] Sprite[] spritesInfo;
	[SerializeField] Image imageInfo;
	[SerializeField] Text textInfo;

	// Use this for initialization
	void Start ()
    {
        texts[0] = LocalizationManager.instance.GetLocalizedValue(6);
        texts[1] = LocalizationManager.instance.GetLocalizedValue(5);
        texts[2] = LocalizationManager.instance.GetLocalizedValue(7);
        texts[3] = LocalizationManager.instance.GetLocalizedValue(8);
        texts[4] = LocalizationManager.instance.GetLocalizedValue(4);

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
