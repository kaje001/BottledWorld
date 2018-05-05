using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	//Grundeinstellungen
	public GameObject player;
	public GameObject tube;
	public Transform camDummy;
	public Transform camCenter;
	public float playerSpeed = 1.2f;
	public float boostLenth = 0.8f;
	public float jumpForce = 5f;
	public int rotSpeed = 100;
	public int lifes = 1;
	public int level = 1;
	public Vector3 gravity;
	public Vector3 gravityGyro;

	bool pause = false;
	bool freeze = false;
	bool waitForUnpause = false;
	[SerializeField] GameObject pauseCheck;
	GameObject objPause;
	public GameObject pausePanel;
	public Text txtPauseButton;
	public Text txtScore;
	public Text txtCountdown;

	List<int> coinIndexes = new List<int> ();
	int coinsLeftInLevel;

	//UI Elemente
	public Text txtCoins;
	public Text txtLifes;
		
	//Elemente für Dinge, die am Anfang des Levels gespawned werden muessen
	public GameObject coinPrefab;
	public GameObject coinPrefabInactive;
	GameObject[] coinSpawnpoints;
	public Transform parentCoins;

	Rigidbody rigPlayer;
	float startPlayerSpeed;
	
	//Checkpoint Elemente
	public Material matCheckpointChecked;
	public Material matCheckpointUnchecked;
	List<GameObject> touchedCheckpoints = new List<GameObject> ();
	Vector3 checkpointPosition;
	Vector3 checkpointGravity;
	//Vector3 checkpointJumpVector;
	//Quaternion checkpointQuaternion;


	private Vector3 velocity = Vector3.zero;

	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		txtPauseButton.transform.parent.gameObject.SetActive (false);
		rigPlayer = player.GetComponent<Rigidbody> ();

		int curCoins = CoinController.Instance.GetCoinForLevel (level);
		txtCoins.text = curCoins.ToString ();
		txtLifes.text = lifes.ToString ();

		startPlayerSpeed = playerSpeed;

		//Die Empties zum Spawner von Extra Leben und Coins werden eingelesen
		coinSpawnpoints = GameObject.FindGameObjectsWithTag("CoinSpawner");
		SpawnCoins ();

		coinsLeftInLevel = 20 - CoinController.Instance.GetCoinForLevel (level);

		pause = true;
		//freeze = true;
		//StartCoroutine ("CountdownAfterPause");
	}

	void Update ()
	{
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
		Camera.main.transform.LookAt (camCenter);
		//camDummy.position = Vector3.Lerp(camDummy.position, player.transform.position, 2f * Time.deltaTime);
		camDummy.position = player.transform.position;
		//camDummy.rotation = Quaternion.Lerp(camDummy.rotation, player.transform.rotation, 220f * Time.deltaTime);
		//camDummy.rotation = Quaternion.Lerp(camDummy.rotation, player.transform.rotation, 4f * Time.deltaTime);
		camDummy.rotation = player.transform.rotation;

	}
	
	//RotateWorld wird immer bei einer TouchBewegung aufgerufen
	/*public void RotateWorld(float rot){
		//Die Funktion rotiert nicht die Flasche, sondern die Gravitation und den Spieler
			//Dadurch muss nicht die Ganze Geometrie gedreht und neu Berechnet werden (Performance :))
		Vector3 gravity = Physics.gravity;
		gravity = Quaternion.Euler(0, 0, rot * rotSpeed) * gravity;
		Physics.gravity = gravity;
		
		//jumpVec = Quaternion.Euler(0, 0, rot * rotSpeed) * jumpVec;
		
		player.transform.Rotate(0f,0f,(rot * rotSpeed));
		if (lockplayer) {
			Vector3 pos = gravity.normalized * 1.71f;
			pos.z = player.transform.position.z;
			player.transform.position = pos;
		}
		camDummy.rotation = player.transform.rotation;

	}*/


	public void RotateWorldGyro (Vector3 tempGravityGyro)
	{
		if (Vector3.Angle (gravity, tempGravityGyro) < 1f) {
			return;
		}

		gravityGyro = tempGravityGyro * 9.81f;
		Physics.gravity = gravity;

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
		txtPauseButton.transform.parent.gameObject.SetActive (false);
		rigPlayer.velocity = Vector3.zero;
		Physics.gravity = checkpointGravity;
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
			txtPauseButton.transform.parent.gameObject.SetActive (false);
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
			txtPauseButton.transform.parent.gameObject.SetActive (false);
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

		if (pause) {
			return;
		}
			
		Coin coin = ob.GetComponent<Coin> ();
		//CoinController.Instance.CollectCoin (level, coin.index);
		coinIndexes.Add (coin.index);
		txtCoins.text = coinIndexes.Count.ToString ();
		Destroy (ob);

	}

	public void PlayerJump ()
	{
		if (pause) {
			return;
		}
		
		inputJump = true;
		//Debug.Log ("test");
		StartCoroutine ("Jump");
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

			txtPauseButton.transform.parent.gameObject.SetActive (true);
			pause = false;
		}

		touchedCheckpoints.Add (checkpointOb);
		checkpointGravity = Physics.gravity;
		checkpointPosition = player.transform.position;
		//checkpointQuaternion = player.transform.rotation;
		
		checkpointOb.GetComponent<Renderer> ().material = matCheckpointChecked;
	}

	
	public void GetExtraLife (GameObject ob)
	{
		Destroy (ob);
		lifes++;
		txtLifes.text = lifes.ToString ();
		
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
			txtPauseButton.text = "Resume";
		} else if (pause) {
			
			Screen.sleepTimeout = SleepTimeout.SystemSetting;

			pausePanel.SetActive (false);
			txtPauseButton.text = "Pause";
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

	float maxJumpHeight = 0.14f;
	float jumpSpeed = 2.0f;
	float fallSpeed = 0.23f;
	public bool inputJump = false;

	public float curJumpHight = 0;

	IEnumerator Jump ()
	{
		while (true) {
			if (curJumpHight >= maxJumpHeight - 0.05f)
				inputJump = false;
			if (inputJump)
				curJumpHight = Mathf.Lerp (curJumpHight, maxJumpHeight, jumpSpeed * Time.deltaTime);
			else if (!inputJump) {
				curJumpHight = curJumpHight - fallSpeed * Time.deltaTime;
				//Debug.Log (curJumpHight);
				if (curJumpHight <= 0) {
					curJumpHight = 0;
					StopCoroutine("Jump");
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
}
