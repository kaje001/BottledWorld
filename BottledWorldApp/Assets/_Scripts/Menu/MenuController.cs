using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public PhysicMaterial playermat;
	[SerializeField] Text txtTotalCoins;
	[SerializeField] GameObject[] camPositions; // 0 = default, 1 = custom
	[SerializeField] GameObject panelSettings;
	[SerializeField] GameObject panelQuit;
	[SerializeField] GameObject canvasDefault;
	[SerializeField] GameObject canvasCustoms;
	[SerializeField] Toggle toggleSound;
	[SerializeField] Toggle toggleMusic;
	[SerializeField] Toggle toggleControls;

	bool slide = false;
	public bool custom = false;

	void Start(){
		playermat.dynamicFriction = 0.3f;
		playermat.staticFriction = 0.05f;

		panelQuit.SetActive (false);
		panelSettings.SetActive (false);

		txtTotalCoins.text = CoinController.Instance.state.availableCoins.ToString () + "/" + CoinController.Instance.state.totalCoins.ToString ();

	}

	void Update(){
		if (custom) {
			Camera.main.transform.position = camPositions [1].transform.position;
		}
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
		HideSettings ();
		ShowSettings ();
		txtTotalCoins.text = CoinController.Instance.state.availableCoins.ToString () + "/" + CoinController.Instance.state.totalCoins.ToString ();
	}

	public void ShowQuit(){
		panelQuit.SetActive (true);
	}

	public void HideQuit(){
		panelQuit.SetActive (false);
	}


	public void ShowSettings(){

		toggleSound.isOn = CoinController.Instance.state.settingsSound;
		toggleMusic.isOn = CoinController.Instance.state.settingsMusic;
		toggleControls.isOn = CoinController.Instance.state.settingsControls;

		panelSettings.SetActive (true);
	}

	public void HideSettings(){
		panelSettings.SetActive (false);
		CoinController.Instance.Save ();
	}

	public void SettingsButtonPressed(){
		if (panelSettings.activeSelf) {
			HideSettings ();
		} else {
			ShowSettings ();
		}
	}

	public void SetSettingSound(bool b){
		CoinController.Instance.state.settingsSound = b;
	}

	public void SetSettingMusic(bool b){
		CoinController.Instance.state.settingsMusic = b;
	}

	public void SetSettingControl(bool b){
		CoinController.Instance.state.settingsControls = b;
	}

	public void ShowCustoms(){
		if (slide) {
			return;
		}
		canvasDefault.SetActive (false);
		StartCoroutine(SlideCam(camPositions[1].transform, true));
	}

	public void HideCustoms(){
		if (slide) {
			return;
		}
		canvasCustoms.SetActive (false);
		custom = false;
		StartCoroutine(SlideCam(camPositions[0].transform, false));
	}

	IEnumerator SlideCam(Transform targetPos, bool b){
		slide = true;
		while(Vector3.Distance(Camera.main.transform.position, targetPos.position) > 0.1f){
			Transform pos = Camera.main.transform;
			pos.position = Vector3.Lerp(pos.position, targetPos.position, 3f * Time.deltaTime);
			pos.rotation = Quaternion.Lerp ( pos.rotation, targetPos.rotation, 3f * Time.deltaTime);
			Camera.main.transform.position = pos.position;
			Camera.main.transform.rotation = pos.rotation;
			yield return null;
		}
		if(b){
			custom = true;
			canvasCustoms.SetActive (true);
		}else{
			custom = false;
			canvasDefault.SetActive (true);
		}
		slide = false;
	}

}
