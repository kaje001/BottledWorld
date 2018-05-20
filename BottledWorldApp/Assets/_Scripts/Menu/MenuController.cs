using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public PhysicMaterial playermat;
	[SerializeField] Text txtTotalCoins;
	[SerializeField] GameObject panelSettings;
	[SerializeField] GameObject panelQuit;

	void Start(){
		playermat.dynamicFriction = 0.3f;
		playermat.staticFriction = 0.05f;

		panelQuit.SetActive (false);
		panelSettings.SetActive (false);

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

	public void ShowQuit(){
		panelQuit.SetActive (true);
	}

	public void HideQuit(){
		panelQuit.SetActive (false);
	}


	public void ShowSettings(){
		panelSettings.SetActive (true);
	}

	public void HideSettings(){
		panelSettings.SetActive (false);
	}

	public void SettingsButtonPressed(){
		if (panelSettings.activeSelf) {
			HideSettings ();
		} else {
			ShowSettings ();
		}
	}
}
