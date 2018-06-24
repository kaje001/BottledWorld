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
	//[SerializeField] float jumpForce = 5f;
	[SerializeField] int rotSpeed = 100;
	[SerializeField] int lifes = 1;
	[SerializeField] int coinsInLevel = 5;
	[SerializeField] int level = 1;
	bool swipeCon = false;
	Vector3 gravity;
	Vector3 gravityGyro;

	bool pause = false;
	bool freeze = false;
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

	List<int> coinIndexes = new List<int> ();
	int coinsLeftInLevel;

	//UI Elemente
	[SerializeField] GameObject panelCoins;
	[SerializeField] GameObject panelLifes;
	[SerializeField] Text txtCoins;
	[SerializeField] Text txtLifes;
	[SerializeField] Fade fadeImage;
		
	//Elemente für Dinge, die am Anfang des Levels gespawned werden muessen
	[SerializeField] GameObject coinPrefab;
	[SerializeField] GameObject coinPrefabInactive;
	GameObject[] coinSpawnpoints;
	[SerializeField] Transform parentCoins;

	Rigidbody rigPlayer;
	float startPlayerSpeed;
	
	//Checkpoint Elemente
	[SerializeField] Material matCheckpointChecked;
	[SerializeField] Material matCheckpointUnchecked;
	List<GameObject> touchedCheckpoints = new List<GameObject> ();
	Vector3 checkpointPosition;
	//Vector3 checkpointGravity;
	//Vector3 checkpointJumpVector;
	//Quaternion checkpointQuaternion;

	//For the Jump
	float maxJumpHeight = 0.14f;
	float jumpSpeed = 2.0f;
	float fallSpeed = 0.23f;
	bool inputJump = false;
	float curJumpHight = 0;

	IEnumerator coroutineJump;
	IEnumerator coroutineCheckCountdown;
	//private Vector3 velocity = Vector3.zero;

	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		txtPauseButton.gameObject.SetActive (false);
		rigPlayer = player.GetComponent<Rigidbody> ();
		checkpointPosition = player.transform.position;

		swipeCon = CoinController.Instance.state.settingsControls;
		if (swipeCon) {
			//GetComponent<GyroCon> ().enabled = false;
			//FindObjectOfType<TouchCon> ().enabled = true;
			gravityGyro = Physics.gravity;
		}

		//int curCoins = CoinController.Instance.GetCoinForLevel (level);
		//txtCoins.text = curCoins.ToString ();
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

		pause = true;
		//freeze = true;
		//StartCoroutine ("CountdownAfterPause");

		VFXandSoundTrigger.Instance.StartLevelMusic ();
	}

	void Update ()
	{
		GetRotateWorld ();
		if (!swipeCon) {
			RotateUI ();
		}
		gravity = Vector3.Lerp (gravity, gravityGyro, 7f * Time.deltaTime);

		if (!freeze) {
			Vector3 curPos = player.transform.position;

			Vector3 pos = gravity.normalized * 1.86f;
			pos = pos + (-gravity) * curJumpHight;
			pos.z = curPos.z;
			//player.transform.position = pos;
			float forwardMovement = 0;
			if (!pause) {
				SetPhysicsGravity ();
				forwardMovement = playerSpeed * Time.deltaTime;
			} 

			//rigPlayer.MovePosition (new Vector3 (pos.x, pos.y, curPos.z + forwardMovement)); //Der Spieler wird nach vorne bewegt
			//player.transform.position = Vector3.Lerp(curPos, new Vector3 (pos.x, pos.y, curPos.z + forwardMovement), 10f * Time.deltaTime);
			player.transform.position = new Vector3 (pos.x, pos.y, curPos.z + forwardMovement);


			//player.transform.Rotate(0f,0f,(rot * rotSpeed));
			player.transform.up = -gravity;
		}

		//Camera Movement

		//camDummy.position = Vector3.Lerp(camDummy.position, player.transform.position, 2f * Time.deltaTime);
		if (finished) {
			camDummy.position = new Vector3 (player.transform.position.x, player.transform.position.y, camDummy.position.z);
		} else {
			camDummy.position = player.transform.position;

		}
		//camDummy.rotation = Quaternion.Lerp(camDummy.rotation, player.transform.rotation, 220f * Time.deltaTime);
		//camDummy.rotation = Quaternion.Lerp(camDummy.rotation, player.transform.rotation, 4f * Time.deltaTime);
		camDummy.rotation = player.transform.rotation;
		if (!swipeCon) {
			Camera.main.transform.LookAt (camCenter);
		}

	}
	
	//RotateWorld wird immer bei einer TouchBewegung aufgerufen
	public void RotateWorld(float rot){
		//Die Funktion rotiert nicht die Flasche, sondern die Gravitation und den Spieler
			//Dadurch muss nicht die Ganze Geometrie gedreht und neu Berechnet werden (Performance :))
		gravityGyro = Quaternion.Euler(0, 0, rot * rotSpeed) * gravityGyro;
		//Debug.Log (gravityGyro);

		
		//jumpVec = Quaternion.Euler(0, 0, rot * rotSpeed) * jumpVec;
		
		/*player.transform.Rotate(0f,0f,(rot * rotSpeed));
		if (lockplayer) {
			Vector3 pos = gravity.normalized * 1.71f;
			pos.z = player.transform.position.z;
			player.transform.position = pos;
		}
		camDummy.rotation = player.transform.rotation;*/

	}


	public void RotateWorldGyro (Vector3 tempGravityGyro)
	{
		if (Vector3.Angle (gravity, tempGravityGyro) < 0.48f) {
			return;
		}

		gravityGyro = tempGravityGyro * 9.81f;


	}

	void GetRotateWorld(){
		if (swipeCon) {
			float rot = TouchCon.Instance.GetDragLength ();
			//Debug.Log (rot);
			gravityGyro = Quaternion.Euler(0, 0, rot * rotSpeed) * gravityGyro;
			//Debug.Log (gravityGyro);
		} else {
			Vector3 tempGravityGyro = GyroCon.Instance.GetGyroGravity ();
			if (Vector3.Angle (gravity, tempGravityGyro) < 1f) {
				return;
			}

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
	}

	public void LoadMenu ()
	{
		VFXandSoundTrigger.Instance.EndLevelMusic ();

		Screen.sleepTimeout = SleepTimeout.SystemSetting;
		SceneManager.LoadScene ("Menu");
	}

	public void LoadSameLevel (int newlevel)
	{
		if (newlevel == 1) {
			SceneManager.LoadScene ("Level1");
		}else if (newlevel == 2) {
			SceneManager.LoadScene ("Level2");
		}
	}

	
	//Der Spieler wird zum letzten aktivierten Checkpoint gesetzt
	public void SetToLastCheckpoint ()
	{
		txtPauseButton.gameObject.SetActive (false);
		rigPlayer.velocity = Vector3.zero;
		//Physics.gravity = checkpointGravity;
		Vector3 pos = player.transform.position;
		pos.z = checkpointPosition.z;
		player.transform.position = pos;
		//player.transform.rotation = checkpointQuaternion;
		playerSpeed = startPlayerSpeed;
		
	}

	public void Win(){
		if (!freeze) {
			finished = true;
			//freeze = true;
			//pause = true;
			//pausePanel.SetActive (true);
			txtPauseButton.gameObject.SetActive (false);
			txtScore.text = coinIndexes.Count.ToString () + "/" + coinsLeftInLevel;

			LastGameData.Instance.coins = coinIndexes.Count;
			LastGameData.Instance.hearts = lifes;
			LastGameData.Instance.deaths = 0;
			LastGameData.Instance.won = true;

			CoinController.Instance.AddCoinForLevel (level, coinIndexes.Count);

			foreach (int i in coinIndexes) {
				CoinController.Instance.CollectCoin (level, i);
			}

			CoinController.Instance.Save ();
			StartCoroutine (WaitForFinish ());
		}

	}

	IEnumerator WaitForFinish(){
		yield return new WaitForSeconds (1f);
		fadeImage.FadeIn (3f);
		yield return new WaitForSeconds (0.4f);
		SceneManager.LoadScene ("EndLevel");
	}

	// Es wird gecheckt, ob noch Leben verfuegbar sind
	public void CheckDeath ()
	{

		if (pause) {
			return;
		}

		if (lifes == 0) { //wenn nein dann das Spiel zuruegsetzten
			//LoadMenu ();
			pausePanel.SetActive (true);
			txtPauseButton.gameObject.SetActive (false);
			txtScore.text = coinIndexes.Count.ToString () + "/" + coinsLeftInLevel;
			pause = true;
			freeze = true;
		} else { //Wenn ja, dann zum letzten Checkpoint zuruegsetzten
			lifes--;
			txtLifes.text = lifes.ToString ();
			SetToLastCheckpoint ();
			pause = true;
			//StartCoroutine ("CountdownAfterPause");
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
		//CoinController.Instance.CollectCoin (level, coin.index);
		coinIndexes.Add (coin.index);
		//txtCoins.text = coinIndexes.Count.ToString ();
		CollectEffectUI (panelCoins, txtCoins, coinIndexes.Count);
		Destroy (ob);

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
		//Debug.Log ("test");

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
		StartCoroutine ("Boost");
	}

	IEnumerator Boost ()
	{
		VFXandSoundTrigger.Instance.TriggerBoost (player.transform, true);
		playerSpeed = playerSpeed*2f;
		yield return new WaitForSeconds (boostLenth);

		VFXandSoundTrigger.Instance.TriggerBoost (player.transform, false);
		ResetPlayerSpeed ();
	}

	public void SetCheckpoint (GameObject checkpointOb)
	{
		if (pause) {

			//UnpauseGame ();
			coroutineCheckCountdown = CheckpointCountdown();
			StartCoroutine(coroutineCheckCountdown);

			VFXandSoundTrigger.Instance.TriggerStartCountdown ();

			return;
		}

		touchedCheckpoints.Add (checkpointOb);
		//checkpointGravity = Physics.gravity;
		checkpointPosition = player.transform.position;
		//checkpointQuaternion = player.transform.rotation;

		VFXandSoundTrigger.Instance.TriggerCheckpoint (checkpointOb);
		//checkpointOb.GetComponent<Renderer> ().material = matCheckpointChecked;
	}

	public void LeaveCheckpoint(){
		if (pause == true && freeze == false) {
			StopCoroutine (coroutineCheckCountdown);
			txtCountdown.gameObject.SetActive (false);
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
		//txtLifes.text = lifes.ToString ();

		//Trigger VFX and Sound
		VFXandSoundTrigger.Instance.TriggerHeart();
		
	}


	public void HitObject ()
	{
		if (pause) {
			return;
		}

		//Trigger VFX and Sound
		VFXandSoundTrigger.Instance.TriggerDead(player.transform);

		StartCoroutine (WaitForCheckDeath ());

	}

	public void PauseGame ()
	{
		if (!pause) {

			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			pause = true;
			//freeze = true;
			objPause = Instantiate (pauseCheck, player.transform.position, player.transform.rotation);
			pausePanel.SetActive (true);
			txtScore.text = coinIndexes.Count.ToString () + "/" + coinsLeftInLevel;
			txtPauseButton.sprite = spriteResumeUI;
		} else if (pause) {
			
			Screen.sleepTimeout = SleepTimeout.SystemSetting;

			pausePanel.SetActive (false);
			txtPauseButton.sprite = spritePauseUI;
			//StartCoroutine ("CountdownAfterPause");
			//freeze = false;
			waitForUnpause = true;
		}

	}

	public void UnpauseTouched(){
		if (waitForUnpause) {
			pause = false;
			waitForUnpause = false;
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
		txtCountdown.gameObject.SetActive (true);
		for (int i = 2; i > 0; i--) {

			txtCountdown.text = i.ToString ();
			yield return new WaitForSeconds (1f);
		}

		txtCountdown.gameObject.SetActive (false);
		//freeze = false;
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
