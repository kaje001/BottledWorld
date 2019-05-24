using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	
	//Grundeinstellungen
	[SerializeField] GameObject player;
	[SerializeField] Transform camDummy;
	[SerializeField] Transform camCenter;
	[SerializeField] float playerSpeed = 1.2f;
	[SerializeField] float boostLenth = 0.8f;
	[SerializeField] int rotSpeed = 100;
	[SerializeField] int lifes = 1;
	[SerializeField] int coinsInLevel = 5;
	[SerializeField] int level = 1;
	[SerializeField] int biom = 0;
	[SerializeField] bool unlockLevel = false;
	bool swipeCon = false;
	Vector3 gravity;
	Vector3 gravityGyro;

	bool pause = true;
	bool freeze = false;
	bool inCheckpoint = false;
	bool waitForUnpause = false;
	bool finished = false;
	[SerializeField] GameObject pauseCheck;
	GameObject objPause;
	[SerializeField] GameObject pausePanel;
	[SerializeField] Text txtScore;
	[SerializeField] Text txtCountdown;
	[SerializeField] Image txtPauseButton;
	[SerializeField] Sprite spritePauseUI;
	[SerializeField] Sprite spriteResumeUI;
	[SerializeField] GameObject textGameOver;

	List<int> coinIndexes = new List<int> ();
	List<int> coinTempIndexes = new List<int> ();
	List<GameObject> coinsTempObjects = new List<GameObject>();
	int coinsLeftInLevel;

	//UI Elemente
	[SerializeField] GameObject panelCoins;
	[SerializeField] GameObject panelLifes;
	[SerializeField] Text txtCoins;
	[SerializeField] Text txtLifes;
	[SerializeField] Fade fadeImage;

	[SerializeField] GameObject panelLoading;
		
	//Elemente für Dinge, die am Anfang des Levels gespawned werden muessen
	[SerializeField] GameObject coinPrefab;
	[SerializeField] GameObject coinPrefabInactive;
	GameObject[] coinSpawnpoints;
	[SerializeField] Transform parentCoins;

	float startPlayerSpeed;
	
	//Checkpoint Elemente
	[SerializeField] Material matCheckpointChecked;
	[SerializeField] Material matCheckpointUnchecked;
	List<GameObject> touchedCheckpoints = new List<GameObject> ();
	Vector3 checkpointPosition;
	[SerializeField] GameObject[] arrowsCheckpoint; //0 = right; 1 = left
	bool arrowsActive = true;
	Vector3 pausePoint;
	[SerializeField] GameObject firstCheckpoint;
	[SerializeField] GameObject finishinLine;
	float totalDistance;
	[SerializeField] ProgressLevel progLevel;

	//For the Jump
	float maxJumpHeight = 0.14f;
	float jumpSpeed = 2.0f;
	float fallSpeed = 0.23f;
	bool inputJump = false;
	float curJumpHight = 0;

	[SerializeField] float immuneLenth = 3f;
	[SerializeField] GameObject immuneSphere;
	bool immune = false;

	IEnumerator coroutineJump;
	IEnumerator coroutineBoost;
	IEnumerator coroutineCheckCountdown;

	void Start ()
	{
		panelLoading.SetActive (false);
		txtPauseButton.gameObject.SetActive (false);
		if (immuneSphere != null) {
			immuneSphere.SetActive(false);
		}
		if (immuneSphere != null) {
			immuneSphere.SetActive (false);
		}
		checkpointPosition = firstCheckpoint.transform.GetChild(0).position;

		foreach (GameObject ob in arrowsCheckpoint) {
			ob.SetActive (false);
		}
		pausePoint = firstCheckpoint.transform.GetChild(0).position;
		totalDistance = Vector3.Distance (firstCheckpoint.transform.position, finishinLine.transform.position);

		swipeCon = CoinController.Instance.state.settingsControls;
		if (swipeCon) {
			gravityGyro = Physics.gravity;
		} else {
			gravity = Physics.gravity;
		}

		txtCoins.text = "0";
		txtLifes.text = lifes.ToString ();

		startPlayerSpeed = playerSpeed;

		//Die Empties zum Spawner von Extra Leben und Coins werden eingelesen
		coinSpawnpoints = GameObject.FindGameObjectsWithTag("CoinSpawner");
		SpawnCoins ();

		coinsLeftInLevel = coinsInLevel - CoinController.Instance.GetCoinForLevel (level);

		LastGameData.Instance.coins = 0;
		LastGameData.Instance.hearts = 0;
		LastGameData.Instance.deaths = 0;
		LastGameData.Instance.won = false;
		LastGameData.Instance.wonFirstTime = false;
		LastGameData.Instance.unlockLevel = 0;

		pause = true;

		VFXandSoundTrigger.Instance.StartLevelMusic ();
	}

	void Update ()
	{
		GetRotateWorld ();
		if (!swipeCon) {
			RotateUI ();
		}
		gravity = Vector3.Lerp (gravity, gravityGyro, 7f * Time.deltaTime);

		if (gravity.y > 9.5f) {
			if (gravity.x >= 0 && gravity.x < 0.02f) {
				gravity.x = 0.02f;
			}else if (gravity.x <= 0 && gravity.x > -0.02f) {
				gravity.x = -0.02f;
			}
		}

		if (!freeze) {
			Vector3 curPos = player.transform.position;

			Vector3 pos = gravity.normalized * 1.86f;
			pos = pos + (-gravity) * curJumpHight;
			pos.z = curPos.z;

			float forwardMovement = 0;
			if (!pause) {
				SetPhysicsGravity ();
				forwardMovement = playerSpeed * Time.deltaTime;

				if (arrowsActive) {
					arrowsActive = false;
					foreach (GameObject ob in arrowsCheckpoint) {
						ob.SetActive (false);
					}
				}
			} else {
				arrowsActive = true;
				if (!arrowsCheckpoint[0].activeSelf && Vector3.Distance (arrowsCheckpoint [1].transform.position, pausePoint) > Vector3.Distance (arrowsCheckpoint [0].transform.position, pausePoint)) {
					arrowsCheckpoint [0].SetActive (true);
					arrowsCheckpoint [1].SetActive (false);
				} else if (!arrowsCheckpoint[1].activeSelf  && Vector3.Distance (arrowsCheckpoint [1].transform.position, pausePoint) < Vector3.Distance (arrowsCheckpoint [0].transform.position, pausePoint)){
					arrowsCheckpoint [1].SetActive (true);
					arrowsCheckpoint [0].SetActive (false);
				}

				if (inCheckpoint && arrowsActive) {
					arrowsActive = false;
					foreach (GameObject ob in arrowsCheckpoint) {
						ob.SetActive (false);
					}
				}
			}


			player.transform.position = new Vector3 (pos.x, pos.y, curPos.z + forwardMovement);

			player.transform.up = -gravity;
		}

		//Camera Movement

		if (finished) {
			camDummy.position = new Vector3 (player.transform.position.x, player.transform.position.y, camDummy.position.z);
		} else {
			camDummy.position = player.transform.position;

		}
		camDummy.rotation = player.transform.rotation;
		if (!swipeCon) {
			Camera.main.transform.LookAt (camCenter);
		}

	}

	void GetRotateWorld(){
		if (swipeCon) {
			//eleminate multi touch bug
			if (Input.touchCount > 1) {
				return;
			}
			float rot = TouchCon.Instance.GetDragLength ();
			gravityGyro = Quaternion.Euler(0, 0, rot * rotSpeed) * gravityGyro;

		} else {
			Vector3 tempGravityGyro = GyroCon.Instance.GetGyroGravity ();
			if (Vector3.Angle (gravity, tempGravityGyro) < 1f) {
				return;
			}

//			Debug.Log (tempGravityGyro);
			gravityGyro = tempGravityGyro * 9.81f;
		}

	}

	void SetPhysicsGravity(){
		if (Vector3.Angle (gravity, Physics.gravity) > 1f) {

			Physics.gravity = gravity;
		}
	}

	void RotateUI(){
		panelCoins.transform.up = -gravity;
		panelLifes.transform.up = -gravity;
		txtPauseButton.transform.up = -gravity;
		txtCountdown.transform.up = -gravity;
	}

	public void LoadMenu ()
	{
		VFXandSoundTrigger.Instance.TriggerButtonClick ();
		VFXandSoundTrigger.Instance.EndLevelMusic ();

		SceneManager.LoadScene ("Menu");
		panelLoading.SetActive (true);
	}

	public void LoadSameLevel ()
	{
		VFXandSoundTrigger.Instance.TriggerButtonClick ();
		SceneManager.LoadScene ("Level" + level);
		panelLoading.SetActive (true);

	}

	
	//Der Spieler wird zum letzten aktivierten Checkpoint gesetzt
	public void SetToLastCheckpoint ()
	{
		txtPauseButton.gameObject.SetActive (false);
		Vector3 pos = player.transform.position;
		pos.z = checkpointPosition.z;
		player.transform.position = pos;
		pausePoint = checkpointPosition;
		playerSpeed = startPlayerSpeed;

		foreach (GameObject ob in coinsTempObjects) {
			ob.SetActive (true);
		}
		coinsTempObjects.Clear ();
		coinTempIndexes.Clear();
		CollectEffectUI (panelCoins, txtCoins, coinIndexes.Count);

		
	}

	public void Win(){
		if (!freeze) {
			finished = true;
			txtPauseButton.gameObject.SetActive (false);

			VFXandSoundTrigger.Instance.TriggerLevelEnd();

			StartCoroutine (WaitForFinish ());
		}

	}

	IEnumerator WaitForFinish(){
		yield return new WaitForSeconds (1f);
		fadeImage.FadeIn (3f);
		yield return new WaitForSeconds (0.8f);

		foreach (int i in coinTempIndexes) {
			coinIndexes.Add (i);
		}


		CoinController.Instance.AddCoinForLevel (level, coinIndexes.Count);

		LastGameData.Instance.coins = coinIndexes.Count;
		LastGameData.Instance.hearts = lifes;
		LastGameData.Instance.deaths = 0;
		LastGameData.Instance.won = true;
		LastGameData.Instance.level = level;
		LastGameData.Instance.biom = biom;
		LastGameData.Instance.totalSugarCubesLevel = coinsInLevel;
		LastGameData.Instance.sugarCubesForLevel = CoinController.Instance.GetCoinForLevel(level);

		if (unlockLevel) {
			LastGameData.Instance.unlockLevel = level + 1;
		}

		if (!CoinController.Instance.IsLevelCompleted (level)) {
			CoinController.Instance.CompleteLevel (level);
			LastGameData.Instance.wonFirstTime = true;
		}

		foreach (int i in coinIndexes) {
			CoinController.Instance.CollectCoin (level, i);
		}

		CoinController.Instance.Save ();

		SceneManager.LoadScene ("EndLevel");
		//panelLoading.SetActive (true);
	}

	// Es wird gecheckt, ob noch Leben verfuegbar sind
	public void CheckDeath ()
	{

		if (pause) {
			return;
		}

		if (lifes == 0) { //wenn nein dann das EndScreen GameOver
			//LoadMenu ();
			pausePanel.SetActive (true);
			progLevel.UpdateBar (Vector3.Distance (firstCheckpoint.transform.position, player.transform.position) / totalDistance);
			txtPauseButton.gameObject.SetActive (false);
			txtScore.text = coinIndexes.Count.ToString () + "/" + coinsLeftInLevel;
			pause = true;
			freeze = true;
			textGameOver.SetActive (true);

			VFXandSoundTrigger.Instance.TriggerGameOver ();
		} else { //Wenn ja, dann zum letzten Checkpoint zuruegsetzten
			lifes--;
			txtLifes.text = lifes.ToString ();
			SetToLastCheckpoint ();
			pause = true;
		}

		CoinController.Instance.state.totalDeaths += 1;
		if (CoinController.Instance.state.totalDeaths == 30 && !CoinController.Instance.IsAchievmentUnlocked (2)) {
			CoinController.Instance.UnlockAchievment (2);
		}
	}


	void SpawnCoins ()
	{
		int i = 0;
		foreach (GameObject trans in coinSpawnpoints) {
			if (!CoinController.Instance.IsCoinCollected (level, i)) {
				GameObject tempOb = Instantiate (coinPrefab, trans.transform.position, trans.transform.rotation);
				Coin coin = tempOb.transform.GetChild(0).gameObject.GetComponent<Coin> ();
				coin.index = i;
				coin.active = true;
				tempOb.transform.parent = parentCoins;
			} else {
				GameObject tempOb = Instantiate (coinPrefabInactive, trans.transform.position, trans.transform.rotation);
				Coin coin = tempOb.transform.GetChild(0).gameObject.GetComponent<Coin> ();
				coin.index = i;
				coin.active = false;
				tempOb.transform.parent = parentCoins;
			}
			i++;
		}
		
	}


	void ResetCheckpoints ()
	{
		foreach (GameObject ob in touchedCheckpoints) {
			ob.GetComponent<Renderer> ().material = matCheckpointUnchecked;
	
		}
		touchedCheckpoints.Clear ();
	}
	
	//Ab hier sind die die Funktionen für die Interactables

	//Ein Zuckerwürfel aufsammeln
	public void AddCoin (GameObject ob)
	{
		Coin coin = ob.GetComponent<Coin> ();
		coinTempIndexes.Add (coin.index);
		coinsTempObjects.Add (ob);

		CollectEffectUI (panelCoins, txtCoins, coinIndexes.Count + coinTempIndexes.Count);
		ob.SetActive (false);

		//Trigger VFX and Sound
		VFXandSoundTrigger.Instance.TriggerCollect(player.transform);
	}

	public void PlayerJump ()
	{
		if (pause) {
			return;
		}


		//Trigger VFX and Sound
		VFXandSoundTrigger.Instance.TriggerJump(player.transform);
		
		inputJump = true;

		coroutineJump = Jump();
		StartCoroutine (coroutineJump);

	}

	public void SetPlayerSpeed (float f)
	{
		playerSpeed = f;
	}

	public void ResetPlayerSpeed ()
	{
		playerSpeed = startPlayerSpeed;
	}

	public void PlayerSpeedBoost ()
	{
		if (coroutineBoost != null) {
			StopCoroutine (coroutineBoost);
		} 
		coroutineBoost = Boost ();
		StartCoroutine (coroutineBoost);
	}

	IEnumerator Boost ()
	{
		VFXandSoundTrigger.Instance.TriggerBoost (player.transform, true);
		playerSpeed = startPlayerSpeed*2f;
		yield return new WaitForSeconds (boostLenth);

		VFXandSoundTrigger.Instance.TriggerBoost (player.transform, false);
		ResetPlayerSpeed ();
	}

	public void PlayerImmunityBoost(){
		immune = true;
		immuneSphere.SetActive(true);

        //Trigger VFX and Sound
        VFXandSoundTrigger.Instance.TriggerImmuPickUp();

        StartCoroutine (ImmuneReset ());
	}

	IEnumerator ImmuneReset ()
	{
		yield return new WaitForSeconds (immuneLenth - 0.6f);
		immuneSphere.SetActive(false);
		yield return new WaitForSeconds (0.2f);
		immuneSphere.SetActive(true);
		yield return new WaitForSeconds (0.2f);
		immuneSphere.SetActive(false);
		yield return new WaitForSeconds (0.1f);
		immuneSphere.SetActive(true);
		yield return new WaitForSeconds (0.1f);
		immuneSphere.SetActive(false);
		immune = false;

        //Trigger VFX and Sound
        VFXandSoundTrigger.Instance.TriggerImmuEnd();

    }

	public void SetCheckpoint (GameObject checkpointOb)
	{
		if (pause) {
			coroutineCheckCountdown = CheckpointCountdown();
			StartCoroutine(coroutineCheckCountdown);

			VFXandSoundTrigger.Instance.TriggerStartCountdown ();

			return;
		}

		touchedCheckpoints.Add (checkpointOb);
		checkpointPosition = player.transform.position;
		pausePoint = player.transform.position;

		foreach (int i in coinTempIndexes) {
			coinIndexes.Add (i);
		}
		coinTempIndexes.Clear();
		coinsTempObjects.Clear ();

		VFXandSoundTrigger.Instance.TriggerCheckpoint (checkpointOb);
	}

	public void LeaveCheckpoint(){
		if (pause == true && freeze == false) {
			StopCoroutine (coroutineCheckCountdown);
			txtCountdown.gameObject.SetActive (false);
			inCheckpoint = false;
			VFXandSoundTrigger.Instance.TriggerCancelCountdown();
		}
	}

	void UnpauseGame(){
		txtPauseButton.gameObject.SetActive (true);
		VFXandSoundTrigger.Instance.TriggerStart ();
		VFXandSoundTrigger.Instance.TriggerStartRunning ();
		pause = false;
	}

	public void GetExtraLife (GameObject ob)
	{
		Destroy (ob);
		lifes++;
		CollectEffectUI (panelLifes, txtLifes, lifes);

		//Trigger VFX and Sound
		VFXandSoundTrigger.Instance.TriggerHeart();
		
	}


	public void HitObject ()
	{
		if (pause || finished || inputJump || immune) {
			return;
		}

		//Trigger VFX and Sound
		VFXandSoundTrigger.Instance.TriggerDead(player.transform);

		StartCoroutine (WaitForCheckDeath ());

	}

	public void PauseGame ()
	{

		VFXandSoundTrigger.Instance.TriggerButtonClick ();
		if (!pause) {
			pause = true;
			pausePoint = player.transform.position;
			objPause = Instantiate (pauseCheck, player.transform.position, player.transform.rotation);
			pausePanel.SetActive (true);
			progLevel.UpdateBar (Vector3.Distance (firstCheckpoint.transform.position, player.transform.position) / totalDistance);
			txtScore.text = (coinIndexes.Count + coinTempIndexes.Count).ToString () + "/" + coinsLeftInLevel;
			txtPauseButton.sprite = spriteResumeUI;
			VFXandSoundTrigger.Instance.TriggerPause ();
		} else if (pause) {
			pausePanel.SetActive (false);
			txtPauseButton.sprite = spritePauseUI;
			waitForUnpause = true;
		}

	}

	public void PauseGame (GameObject pauseTutPanel)
	{

		VFXandSoundTrigger.Instance.TriggerButtonClick ();
		if (!pause) {
			pause = true;
			pausePoint = player.transform.position;
			objPause = Instantiate (pauseCheck, player.transform.position, player.transform.rotation);
			pauseTutPanel.SetActive (true);
			txtPauseButton.gameObject.SetActive(false);
			VFXandSoundTrigger.Instance.TriggerPause ();

		} else if (pause) {
			pauseTutPanel.SetActive (false);
			txtPauseButton.gameObject.SetActive(true);

			waitForUnpause = true;
		}

	}


	public void UnpauseTouched(){
		if (waitForUnpause) {
			pause = false;
			waitForUnpause = false;
			VFXandSoundTrigger.Instance.TriggerStart ();
			Destroy (objPause);
		}
	}

	void CollectEffectUI(GameObject ui, Text text, int value){
		StartCoroutine(PulseUI(ui, text, value));
	}

	IEnumerator Jump ()
	{
		while (true) {
			if (curJumpHight >= maxJumpHeight - 0.05f) {
				inputJump = false;
			}
			if (inputJump) {
				curJumpHight = Mathf.Lerp (curJumpHight, maxJumpHeight, jumpSpeed * Time.deltaTime);
			}
			else if (!inputJump) {
				curJumpHight = curJumpHight - fallSpeed * Time.deltaTime;
				//Debug.Log (curJumpHight);
				if (curJumpHight <= 0) {
					curJumpHight = 0;

					VFXandSoundTrigger.Instance.TriggerJumpEnd (player.transform);

					yield break;
				}
			}

			yield return null;
		}
	}

	IEnumerator CountdownAfterPause ()
	{
		txtCountdown.gameObject.SetActive (true);
		for (int i = 3; i > 0; i--) {
			
			txtCountdown.text = i.ToString ();
			yield return new WaitForSeconds (1f);
		}

		txtCountdown.gameObject.SetActive (false);
		//freeze = false;
	}


	IEnumerator CheckpointCountdown ()
	{
		inCheckpoint = true;
		txtCountdown.gameObject.SetActive (true);
		for (int i = 3; i > 0; i--) {

			txtCountdown.text = i.ToString ();
			yield return new WaitForSeconds (0.6f);
		}

		txtCountdown.gameObject.SetActive (false);
		//freeze = false;

		inCheckpoint = false;
		UnpauseGame();
	}


	IEnumerator WaitForCheckDeath ()
	{
		pause = true;
		freeze = true;
		yield return new WaitForSeconds (0.5f);
		player.SetActive (false);
		yield return new WaitForSeconds (0.5f);

		pause = false;
		freeze = false;
		player.SetActive (true);
		CheckDeath ();
	}

	IEnumerator PulseUI(GameObject ui, Text text, int value){
		yield return new WaitForSeconds (0.4f);
		float scale = 1f;
		int i = 0;
		for (i = 0; i < 10; i++) {
			scale += 0.08f;
			ui.transform.localScale = new Vector3 (scale, scale, scale);
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (0.1f);
		text.text = value.ToString();
		yield return new WaitForSeconds (0.1f);
		for (i = 10; i > 0; i--) {
			scale -= 0.08f;
			ui.transform.localScale = new Vector3 (scale, scale, scale);
			yield return new WaitForSeconds (0.01f);
		}

		ui.transform.localScale = Vector3.one;
	}
}
