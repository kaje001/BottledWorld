using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public bool antIsDraged = false;
	public PhysicMaterial playermat;
	[SerializeField] Text txtTotalCoins;

	void Start(){
		playermat.dynamicFriction = 0.3f;
		playermat.staticFriction = 0.05f;

		txtTotalCoins.text = CoinController.Instance.state.availableCoins.ToString () + "/" + CoinController.Instance.state.totalCoins.ToString ();
	}

	public void CheckActivatedObject(GameObject ob){
		
		if (ob.transform.tag == "Level1") {
			Debug.Log ("Start Level 1");
			SceneManager.LoadScene("Level1");
		} else if (ob.transform.tag == "Level2") {
			Debug.Log ("Start Level 2");
			//SceneManager.LoadScene("TestLevel2");
		}else if (ob.transform.tag == "Level3") {
			Debug.Log ("Start Level 2");
			//SceneManager.LoadScene("TestLevel3");
		}else if (ob.transform.tag == "Level4") {
			Debug.Log ("Start Level 2");
			//SceneManager.LoadScene("TestLevel4");
		}
	}
	
	public void QuitGame(){
		
			Debug.Log ("Exit Game");
			Application.Quit ();
		
	}

	public void ResetSaveGame(){
		CoinController.Instance.ResetSaveState();
	}

}
