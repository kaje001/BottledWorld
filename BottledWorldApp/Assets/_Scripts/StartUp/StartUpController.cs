using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpController : MonoBehaviour {
	
	public Fade fadeLogo;
	float timeStamp;
	bool faded = false;
	bool logo = false;
	bool starting = false;

	public GameObject panelStart;
	public GameObject arrow;

	[SerializeField] Transform antMiddle;
	float lastAngle = 0;

	// Use this for initialization
	void Start () {
		fadeLogo.FadeIn();
		timeStamp = Time.time + 2f;
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
			starting = true;
			antMiddle.gameObject.SetActive (true);
			//arrow.SetActive (true);
		}
	}
	
	public void StartGame(){
		
		SceneManager.LoadScene("Menu");
	}

	public void RotateAntGyro(Vector2 gravityGyro){
		if (starting) {
			float angle = Vector2.SignedAngle (Vector2.down, gravityGyro);

			if (angle > lastAngle || angle < lastAngle - 15) {
				return;
			}
			
			if (angle > -20 && angle < 30) {
				angle = -20;
			}
			if (angle < -175) {
				angle = 179f;
			}
			//Debug.Log (angle);
			antMiddle.Rotate (0f, 0f, angle - lastAngle);

			if (angle > 30 && angle < 35) {
				StartGame ();
			}

			lastAngle = angle;
		}
	}
	
}
