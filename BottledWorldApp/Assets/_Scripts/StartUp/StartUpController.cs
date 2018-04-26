using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpController : MonoBehaviour {
	
	public Fade fadeLogo;
	float timeStamp;
	bool faded = false;
	bool logo = false;
	
	public GameObject panelStart;

	// Use this for initialization
	void Start () {
		fadeLogo.FadeIn();
		timeStamp = Time.time + 4f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > timeStamp && ! faded){
			fadeLogo.FadeOut();
			faded = true;
			timeStamp = Time.time + 2f;
		}
		if(Time.time > timeStamp && ! logo){
			panelStart.SetActive(true);
		}
	}
	
	public void StartGame(){
		
		SceneManager.LoadScene("Menu");
	}
	
	
}
