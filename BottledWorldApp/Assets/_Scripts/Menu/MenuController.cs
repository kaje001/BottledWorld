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
	[SerializeField] GameObject panelGotIt;
	[SerializeField] Toggle toggleSound;
	[SerializeField] Toggle toggleMusic;
	[SerializeField] Toggle toggleControls;

	[SerializeField] AudioClip musicMenu;
	[SerializeField] AudioClip soundOverBottle;
	[SerializeField] AudioClip soundAntWalking;
	[SerializeField] AudioClip soundUnlockSelf;
	[SerializeField] AudioClip soundButtonClick;
	[SerializeField] AudioClip soundCountUpSugar;
	[SerializeField] AudioClip audioUnlockLevel;

	[SerializeField] HighlightBottle[] bottles;

	bool slide = false;
	public bool custom = false;
	public bool draged = false;

	[SerializeField] CustomSelecter customSel;

	void Start(){
		playermat.dynamicFriction = 0.3f;
		playermat.staticFriction = 0.05f;

		panelQuit.SetActive (false);
		panelSettings.SetActive (false);

		txtTotalCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();

		SoundManager.Instance.PlayMusic (musicMenu);

		/*int i = 0;
		foreach (HighlightBottle hb in bottles) {
			if (CoinController.Instance.IsLevelUnlocked (i)) {
				hb.active = true;
			} else {
				hb.active = false;
			}
			hb.SetMaterial ();
			i++;
		}*/

		if (CoinController.Instance.state.gotIt) {
			panelGotIt.SetActive (false);
		}

		StartCoroutine (UnlockLevel (false));

		canvasCustoms.SetActive (false);

		CoinController.Instance.Save ();

	}

	void Update(){
		if (custom) {
			Camera.main.transform.position = camPositions [1].transform.position;
		}
	}

	public void CheckActivatedObject(GameObject ob){
		bool musicoff = false;
		if (ob.transform.tag == "Level0" && CoinController.Instance.IsLevelUnlocked(0)) {
			Debug.Log ("Start Level 0");
			SceneManager.LoadScene("Level0");
			musicoff = true;
		} else if (ob.transform.tag == "Level1" && CoinController.Instance.IsLevelUnlocked(1)) {
			Debug.Log ("Start Level 1");
			SceneManager.LoadScene("Level1");
			musicoff = true;
		} else if (ob.transform.tag == "Level2" && CoinController.Instance.IsLevelUnlocked(2)) {
			Debug.Log ("Start Level 2");
			SceneManager.LoadScene("Level2");
		}else if (ob.transform.tag == "Level3" && CoinController.Instance.IsLevelUnlocked(3)) {
			Debug.Log ("Start Level 3");
			SceneManager.LoadScene("Level3");
		}else if (ob.transform.tag == "Level4" && CoinController.Instance.IsLevelUnlocked(4)) {
			Debug.Log ("Start Level 4");
			SceneManager.LoadScene("Level4");
		}

		if (musicoff) {
			SoundManager.Instance.StopMusic ();
		}
	}

	public void OverObject(GameObject ob){

		if(ob.GetComponent<HighlightBottle> ().active){
			ob.GetComponent<HighlightBottle> ().Hightlight (true);
			SoundManager.Instance.PlaySingle (soundOverBottle);
		}

		/*if (ob.transform.tag == "Level1") {
			//Highlight Bottle1
		} else if (ob.transform.tag == "Level2") {
			Debug.Log ("Start Level 2");
			//SceneManager.LoadScene("Level2");
		}else if (ob.transform.tag == "Level3") {
			Debug.Log ("Start Level 3");
			//SceneManager.LoadScene("Level3");
		}else if (ob.transform.tag == "Level4") {
			Debug.Log ("Start Level 4");
			//SceneManager.LoadScene("Level4");
		}*/
	}

	public void OverObjectLeave(GameObject ob){

		if (ob.GetComponent<HighlightBottle> ().active) {
			ob.GetComponent<HighlightBottle> ().Hightlight (false);
		}

	}
	
	public void QuitGame(){
		SoundManager.Instance.PlaySingle (soundButtonClick);
		Debug.Log ("Exit Game");
		Application.Quit ();
		
	}

	public void PressedGotIt(){
		SoundManager.Instance.PlaySingle (soundButtonClick);
		panelGotIt.SetActive (false);
		CoinController.Instance.state.gotIt = true;
		CoinController.Instance.Save ();
	}

	public void ResetSaveGame(){
		SoundManager.Instance.PlaySingle (soundButtonClick);
		CoinController.Instance.ResetSaveState();
		//HideSettings ();
		//ShowSettings ();
		//txtTotalCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();
		//panelGotIt.SetActive (true);
		//customSel.Reload ();
		//StartCoroutine (UnlockLevel (false));
		SceneManager.LoadScene("Menu");
		// + "/" + CoinController.Instance.state.totalCoins.ToString ()
	}

	public void ShowQuit(){
		
		if (draged) {
			return;
		}
		SoundManager.Instance.PlaySingle (soundButtonClick);
		panelQuit.SetActive (true);
	}

	public void HideQuit(){
		SoundManager.Instance.PlaySingle (soundButtonClick);
		panelQuit.SetActive (false);
	}


	public void ShowSettings(){
		if (draged) {
			return;
		}
		SoundManager.Instance.PlaySingle (soundButtonClick);
		toggleSound.isOn = CoinController.Instance.state.settingsSound;
		toggleMusic.isOn = CoinController.Instance.state.settingsMusic;
		toggleControls.isOn = CoinController.Instance.state.settingsControls;

		panelSettings.SetActive (true);
	}

	public void HideSettings(){
		SoundManager.Instance.PlaySingle (soundButtonClick);
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
		SoundManager.Instance.PlaySingle (soundButtonClick);
		CoinController.Instance.state.settingsSound = b;
	}

	public void SetSettingMusic(bool b){
		SoundManager.Instance.PlaySingle (soundButtonClick);
		CoinController.Instance.state.settingsMusic = b;
		if (!b) {
			SoundManager.Instance.StopMusic ();
		} else {
			SoundManager.Instance.PlayMusic (musicMenu);
		}
	}

	public void SetSettingControl(bool b){
		SoundManager.Instance.PlaySingle (soundButtonClick);
		CoinController.Instance.state.settingsControls = b;
		if (b) {
			Input.gyro.enabled = false;

		}
	}

	public void ShowCustoms(){
		//trying to eleminate touch while drag bug
		if (slide || draged) {
			return;
		}
		SoundManager.Instance.PlaySingle (soundButtonClick);
		canvasDefault.SetActive (false);
		GetComponent<CustomController> ().CustomOpen ();
		StartCoroutine(SlideCam(camPositions[1].transform, true));
	}

	public void HideCustoms(){
		if (slide) {
			return;
		}
		SoundManager.Instance.PlaySingle (soundButtonClick);
		canvasCustoms.SetActive (false);
		custom = false;
		txtTotalCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();
		CoinController.Instance.Save ();
		StartCoroutine(SlideCam(camPositions[0].transform, false));
	}

	IEnumerator SlideCam(Transform targetPos, bool b){
		slide = true;
		while(Vector3.Distance(Camera.main.transform.position, targetPos.position) > 1.2f){
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

	IEnumerator UnlockLevel(bool b){
		yield return new WaitForSeconds (0.5f);
		int i = 0;
		foreach (HighlightBottle hb in bottles) {
			if (CoinController.Instance.IsLevelUnlocked (i)) {
				hb.active = true;
			} else {
				hb.active = false;
			}
			hb.SetMaterial ();
			i++;
		}
		if (b) {
			SoundManager.Instance.PlaySingle (audioUnlockLevel);
		}
	}

	public void LoadCredits(){
		SoundManager.Instance.StopMusic ();
		LastGameData.Instance.level = -1;
		SoundManager.Instance.PlaySingle (soundButtonClick);
		SceneManager.LoadScene("Credits");
	}

}
