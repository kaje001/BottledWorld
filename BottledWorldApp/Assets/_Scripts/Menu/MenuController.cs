using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public bool antIsDraged = false;

	public void CheckActivatedObject(GameObject ob){
		
		if (ob.transform.tag == "Level1") {
			Debug.Log ("Start Level 1");
			SceneManager.LoadScene("TestLevel");
		} else if (ob.transform.tag == "Level2") {
			Debug.Log ("Start Level 2");
		}else if (ob.transform.tag == "Exit") {
			Debug.Log ("Exit Game");
			Application.Quit ();
		}
	}

}
