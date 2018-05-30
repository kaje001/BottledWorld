using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	
	//Grundeinstellungen
	[SerializeField] GameObject player;
	[SerializeField] GameObject tube;
	[SerializeField] Transform camDummy;
	[SerializeField] Transform camCenter;
	[SerializeField] float playerSpeed = 1.2f;
	[SerializeField] float boostLenth = 0.8f;
	//[SerializeField] float jumpForce = 5f;
	[SerializeField] int rotSpeed = 100;
	[SerializeField] int lifes = 1;
	[SerializeField] int level = 1;
	bool swipeCon = false;
	Vector3 gravity;
	Vector3 gravityGyro;

	bool pause = false;
	bool freeze = false;
	bool waitForUnpause = false;
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
	[SerializeField] Text txtCoins;
	[SerializeField] Text txtLifes;
		
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

		int curCoins = CoinController.Instance.GetCoinForLevel (level);
		txtCoins.text = curCoins.ToString ();
		txtLifes.text = lifes.ToString ();

		startPlayerSpeed = playerSpeed;

		//Die Empties zum Spawner von Extra Leben und Coins werden eingelesen
		coinSpawnpoints = GameObject.FindGameObjectsWithTag("CoinSpawner");
		SpawnCoins ();

		coinsLeftInLevel = 5 - CoinController.Instance.GetCoinForLevel (level);

		pause = true;
		//freeze = true;
		//StartCoroutine ("CountdownAfterPause");
	}

	void Update ()
	{
		GetRotateWorld ();
		gravity = Vector3.Lerp (gravity, gravityGyro, 7f * Time.deltaTime);

		if (!freeze) {
			Vector3 curPos = player.transform.position;

			Vector3 pos = gravity.normalized * 1.86f;
			pos = pos + (-gravity) * curJumpHight;
			pos.z = curPos.z;
			//player.transform.position = pos;
			float forwardMovement = 0;
			if (!pause) {
				forwardMovement = playerSpeed * Time.deltaTime;
			} 

			//rigPlayer.MovePosition (new Vector3 (pos.x, pos.y, curPos.z + forwardMovement)); //Der Spieler wird nach vorne bewegt
			//player.transform.position = Vector3.Lerp(curPos, new Vector3 (pos.x, pos.y, curPos.z + forwardMovement), 10f * Time.deltaTime);
			player.transform.position = new Vector3 (pos.x, pos.y, curPos.z + forwardMovement);


			//player.transform.Rotate(0f,0f,(rot * rotSpeed));
			player.transform.up = -gravity;
		}

		//Camera Movement
		if (!swipeCon) {
			Camera.main.transform.LookAt (camCenter);
		}
		//camDummy.position = Vector3.Lerp(camDummy.position, player.transform.position, 2f * Time.deltaTime);
		camDummy.position = player.transform.position;
		//camDummy.rotation = Quaternion.Lerp(camDummy.rotation, player.transform.rotation, 220f * Time.deltaTime);
		//camDummy.rotation = Quaternion.Lerp(camDummy.rotation, player.transform.rotation, 4f * Time.deltaTime);
		camDummy.rotation = player.transform.rotation;

	}
	
	//RotateWorld wird immer bei einer TouchBewegung aufgerufen
	public void RotateWorld(float rot){
		//Die Funktion rotiert nicht die Flasche, sondern die Gravitation und den Spieler
			//Dadurch muss nicht die Ganze Geometrie gedreht und neu Berechnet werden (Performance :))
		gravityGyro = Quaternion.Euler(0, 0, rot * rotSpeed) * gravityGyro;
		//Debug.Log (gravityGyro);
		Physics.gravity = gravity;
		
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
		if (Vector3.Angle (gravity, tempGravityGyro) < 1f) {
			return;
		}

		gravityGyro = tempGravityGyro * 9.81f;
		Physics.gravity = gravity;

	}

	void GetRotateWorld(){
		if (swipeCon) {
			float rot = TouchCon.Instance.GetDragLength ();
			Debug.Log (rot);
			gravityGyro = Quaternion.Euler(0, 0, rot * rotSpeed) * gravityGyro;
			//Debug.Log (gravityGyro);
			Physics.gravity = gravity;
		} else {
			Vector3 tempGravityGyro = GyroCon.Instance.GetGyroGravity ();
			if (Vector3.Angle (gravity, tempGravityGyro) < 1f) {
				return;
			}

			gravityGyro = tempGravityGyro * 9.81f;
			Physics.gravity = gravity;
		}
	}

	public void LoadMenu ()
	{

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
			freeze = true;
			pause = true;
			pausePanel.SetActive (true);
			txtPauseButton.gameObject.SetActive (false);
			txtScore.text = coinIndexes.Count.ToString () + "/" + coinsLeftInLevel;

			CoinController.Instance.AddCoinForLevel (level, coinIndexes.Count);

			foreach (int i in coinIndexes) {
				CoinController.Instance.CollectCoin (level, i);
			}

			CoinController.Instance.Save ();
		}

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
		txtCoins.text = coinIndexes.Count.ToString ();
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
		playerSpeed = playerSpeed*2f;
		yield return new WaitForSeconds (boostLenth);
		ResetPlayerSpeed ();
	}

	public void SetCheckpoint (GameObject checkpointOb)
	{
		if (pause) {

			//UnpauseGame ();
			coroutineCheckCountdown = CheckpointCountdown();
			StartCoroutine(coroutineCheckCountdown);
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
		}
	}

	void UnpauseGame(){
		txtPauseButton.gameObject.SetActive (true);
		VFXandSoundTrigger.Instance.TriggerStart ();
		pause = false;
	}

	public void GetExtraLife (GameObject ob)
	{
		Destroy (ob);
		lifes++;
		txtLifes.text = lifes.ToString ();

		//Trigger VFX and Sound
		//VFXandSoundTrigger.Instance.TriggerHeart();
		
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
}
