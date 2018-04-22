using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public bool antIsDraged = false;
	public PhysicMaterial playermat;

	void Start(){
		playermat.dynamicFriction = 0.3f;
		playermat.staticFriction = 0.05f;
	}

	public void CheckActivatedObject(GameObject ob){
		
		if (ob.transform.tag == "Level1") {
			Debug.Log ("Start Level 1");
			SceneManager.LoadScene("TestLevel1");
		} else if (ob.transform.tag == "Level2") {
			Debug.Log ("Start Level 2");
			SceneManager.LoadScene("TestLevel2");
		}else if (ob.transform.tag == "Level3") {
			Debug.Log ("Start Level 2");
			SceneManager.LoadScene("TestLevel3");
		}else if (ob.transform.tag == "Level4") {
			Debug.Log ("Start Level 2");
			SceneManager.LoadScene("TestLevel4");
		}else if (ob.transform.tag == "Exit") {
			Debug.Log ("Exit Game");
			Application.Quit ();
		}
	}

}
