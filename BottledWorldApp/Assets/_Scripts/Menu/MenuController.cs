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
	[SerializeField] GameObject canvasAchievments;
	[SerializeField] GameObject panelGotIt;
	[SerializeField] GameObject panelReallyReset;
	[SerializeField] GameObject panelLoading;
	[SerializeField] GameObject imageNewAchivment;
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
	[SerializeField] GameObject panelSugarInfo;
	[SerializeField] Text textSugarInfo;

	bool slide = false;
	public bool custom = false;
	public bool achievments = false;
	public bool draged = false;

	[SerializeField] CustomSelecter customSel;
	[SerializeField] int[] coinsInLevels;

	void Start(){
		playermat.dynamicFriction = 0.3f;
		playermat.staticFriction = 0.05f;

		panelLoading.SetActive (false);
		panelQuit.SetActive (false);
		panelSettings.SetActive (false);
		panelSugarInfo.SetActive (false);
		panelReallyReset.SetActive (false);

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
		canvasAchievments.SetActive (false);

		if (LastGameData.Instance.wonFirstTime) {
			//TODO: Check for Level Achievments 

			if (LastGameData.Instance.level == 0 && !CoinController.Instance.IsAchievmentUnlocked (9)) {
				CoinController.Instance.UnlockAchievment (9);
			}
			if (LastGameData.Instance.level == 3 && !CoinController.Instance.IsAchievmentUnlocked (3)) {
				CoinController.Instance.UnlockAchievment (3);
			}
			if (LastGameData.Instance.level == 7 && !CoinController.Instance.IsAchievmentUnlocked (4)) {
				CoinController.Instance.UnlockAchievment (4);
			}
			if (LastGameData.Instance.level == 11 && !CoinController.Instance.IsAchievmentUnlocked (5)) {
				CoinController.Instance.UnlockAchievment (5);
			}
			if (LastGameData.Instance.level == 4 && !CoinController.Instance.IsAchievmentUnlocked (6)) {
				CoinController.Instance.UnlockAchievment (6);
			}
			if (LastGameData.Instance.level == 8 && !CoinController.Instance.IsAchievmentUnlocked (7)) {
				CoinController.Instance.UnlockAchievment (7);
			}
			if (LastGameData.Instance.level == 12 && !CoinController.Instance.IsAchievmentUnlocked (8)) {
				CoinController.Instance.UnlockAchievment (8);
			}

			if (CoinController.Instance.state.completedLevels == 8191 && !CoinController.Instance.IsAchievmentUnlocked (1)) {
				CoinController.Instance.UnlockAchievment (8);
			}
		}

		if (CoinController.Instance.state.newAchievmentUnlocked == true) {
			imageNewAchivment.SetActive (true);
		} else {
			imageNewAchivment.SetActive (false);
		}

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
			panelLoading.SetActive (true);
			musicoff = true;
		} else if (ob.transform.tag == "Level1" && CoinController.Instance.IsLevelUnlocked(1)) {
			Debug.Log ("Start Level 1");
			SceneManager.LoadScene("Level1");
			panelLoading.SetActive (true);
			musicoff = true;
		} else if (ob.transform.tag == "Level2" && CoinController.Instance.IsLevelUnlocked(2)) {
			Debug.Log ("Start Level 2");
			SceneManager.LoadScene("Level2");
			panelLoading.SetActive (true);
		}else if (ob.transform.tag == "Level3" && CoinController.Instance.IsLevelUnlocked(3)) {
			Debug.Log ("Start Level 3");
			SceneManager.LoadScene("Level3");
			panelLoading.SetActive (true);
		}else if (ob.transform.tag == "Level4" && CoinController.Instance.IsLevelUnlocked(4)) {
			Debug.Log ("Start Level 4");
			SceneManager.LoadScene("Level4");
			panelLoading.SetActive (true);
		} else if (ob.transform.tag == "Level5" && CoinController.Instance.IsLevelUnlocked(5)) {
			Debug.Log ("Start Level 5");
			SceneManager.LoadScene("Level5");
			panelLoading.SetActive (true);
			musicoff = true;
		} else if (ob.transform.tag == "Level6" && CoinController.Instance.IsLevelUnlocked(6)) {
			Debug.Log ("Start Level 6");
			SceneManager.LoadScene("Level6");
			panelLoading.SetActive (true);
		}else if (ob.transform.tag == "Level7" && CoinController.Instance.IsLevelUnlocked(7)) {
			Debug.Log ("Start Level 7");
			SceneManager.LoadScene("Level7");
			panelLoading.SetActive (true);
		}else if (ob.transform.tag == "Level8" && CoinController.Instance.IsLevelUnlocked(8)) {
			Debug.Log ("Start Level 8");
			SceneManager.LoadScene("Level8");
			panelLoading.SetActive (true);
		} else if (ob.transform.tag == "Level9" && CoinController.Instance.IsLevelUnlocked(9)) {
			Debug.Log ("Start Level 9");
			SceneManager.LoadScene("Level9");
			panelLoading.SetActive (true);
			musicoff = true;
		} else if (ob.transform.tag == "Level10" && CoinController.Instance.IsLevelUnlocked(10)) {
			Debug.Log ("Start Level 10");
			SceneManager.LoadScene("Level10");
			panelLoading.SetActive (true);
		}else if (ob.transform.tag == "Level11" && CoinController.Instance.IsLevelUnlocked(11)) {
			Debug.Log ("Start Level 11");
			SceneManager.LoadScene("Level11");
			panelLoading.SetActive (true);
		}else if (ob.transform.tag == "Level12" && CoinController.Instance.IsLevelUnlocked(12)) {
			Debug.Log ("Start Level 12");
			SceneManager.LoadScene("Level12");
			panelLoading.SetActive (true);
		}

		if (musicoff) {
			SoundManager.Instance.StopMusic ();
		}
	}

	public void OverObject(GameObject ob){
		HighlightBottle hb = ob.GetComponent<HighlightBottle> ();
		if(hb.active){
			hb.Hightlight (true);
			SoundManager.Instance.PlaySingle (soundOverBottle);

			panelSugarInfo.SetActive (true);
			textSugarInfo.text = CoinController.Instance.GetCoinForLevel(hb.level) + "/" + coinsInLevels[hb.level];
			panelSugarInfo.transform.localPosition = new Vector3 (hb.positionInShelf * 120 - 180, panelSugarInfo.transform.localPosition.y, panelSugarInfo.transform.localPosition.z);
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
			panelSugarInfo.SetActive (false);
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

	public void ShowReallyReset(){
		if (draged) {
			return;
		}
		SoundManager.Instance.PlaySingle (soundButtonClick);
		panelReallyReset.SetActive (true);
	}

	public void HideReallyReset(){
		SoundManager.Instance.PlaySingle (soundButtonClick);
		panelReallyReset.SetActive (false);
	}

	public void ShowAchievments(){
		//trying to eleminate touch while drag bug
		if (slide || draged) {
			return;
		}
		CoinController.Instance.state.newAchievmentUnlocked = false;
		SoundManager.Instance.PlaySingle (soundButtonClick);
		canvasDefault.SetActive (false);
		imageNewAchivment.SetActive (false);
		GetComponent<AchievmentController> ().UpdateAchievments ();
		StartCoroutine(SlideCam(camPositions[2].transform, 2));
	}

	public void HideAchievments(){
		if (slide) {
			return;
		}
		SoundManager.Instance.PlaySingle (soundButtonClick);
		canvasAchievments.SetActive (false);
		achievments = false;
		txtTotalCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();
		CoinController.Instance.Save ();
		StartCoroutine(SlideCam(camPositions[0].transform, 0));
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
		panelLoading.SetActive (true);
		// + "/" + CoinController.Instance.state.totalCoins.ToString ()
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
		StartCoroutine(SlideCam(camPositions[1].transform, 1));
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
		StartCoroutine(SlideCam(camPositions[0].transform, 0));
	}

	IEnumerator SlideCam(Transform targetPos, int b){
		slide = true;
		while(Vector3.Distance(Camera.main.transform.position, targetPos.position) > 1.2f){
			Transform pos = Camera.main.transform;
			pos.position = Vector3.Lerp(pos.position, targetPos.position, 3f * Time.deltaTime);
			pos.rotation = Quaternion.Lerp ( pos.rotation, targetPos.rotation, 3f * Time.deltaTime);
			Camera.main.transform.position = pos.position;
			Camera.main.transform.rotation = pos.rotation;
			yield return null;
		}
		if (b == 1) {
			custom = true;
			canvasCustoms.SetActive (true);
		} else if (b == 0) {
			custom = false;
			achievments = false;
			canvasDefault.SetActive (true);
			canvasAchievments.SetActive (false);
		} else if (b == 2) {
			achievments = true;
			canvasAchievments.SetActive (true);
		}
		slide = false;
	}

	IEnumerator UnlockLevel(bool b){
		yield return new WaitForSeconds (0.5f);
		int i = 0;
		foreach (HighlightBottle hb in bottles) {
			if (CoinController.Instance.IsLevelUnlocked (i)) {
				hb.active = true;
				//Debug.Log (i);
			} else {
				hb.active = false;
			}
			if (CoinController.Instance.GetCoinForLevel(i) == coinsInLevels[i]) {
				hb.stared = true;
			} else {
				hb.stared = false;
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
		panelLoading.SetActive (true);
	}

}
