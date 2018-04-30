using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	//Grundeinstellungen
	public GameObject player;
	public GameObject tube;
	public Transform camDummy;
	public float playerSpeed = 1.5f;
	public float boostLenth = 0.8f;
	public float jumpForce = 5f;
	public int rotSpeed = 100;
	public int lifes = 1;
	public int level = 1;
	public Vector3 gravity;

	public bool lockplayer = false; 
	bool pause = false;
	public GameObject pausePanel;
	public Text txtPauseButton;
	public Text txtScore;
	public Text txtCountdown;
	
	//UI Elemente
	public Text txtCoins;
	public Text txtLifes;
		
	//Elemente für Dinge, die am Anfang des Levels gespawned werden muessen
	public GameObject[] collectablePrefabs;
	public List<GameObject[]> collectableSpawnpoints = new List<GameObject[]>();
	public Transform parentCoins;
	
	public GameObject[] movingObstaclePrefabs;
	List<GameObject> movingObstacles = new List<GameObject>();
	public Transform[] movingObstacleSpawnpoints;
	public Transform parentMovingObstacles;
	
	//Reset Elemente
	int coinCount = 0;
	//Vector3 jumpVec;
	Rigidbody rigPlayer;
	Vector3 startPosition;
	Vector3 startGravity;
	//Vector3 startJumpVector;
	Quaternion startQuaternion;
	float startSpeed;
	int startLifes;
	
	//Checkpoint Elemente
	public Material matCheckpointChecked;
	public Material matCheckpointUnchecked;	
	bool checkpointActive = false;
	List<GameObject> touchedCheckpoints = new List<GameObject>();
	Vector3 checkpointPosition;
	Vector3 checkpointGravity;
	//Vector3 checkpointJumpVector;
	Quaternion checkpointQuaternion;



	void Start () {
		
		//Variablen zum Reseten des Spiels werden eingelesen
		startPosition = player.transform.position;
		startQuaternion = player.transform.rotation;
		startGravity = gravity;
		startSpeed = playerSpeed;
		rigPlayer = player.GetComponent<Rigidbody>();
		startLifes = lifes;
		
		//jumpVec = -gravity * jumpForce*200;
		//startJumpVector = jumpVec;

		int curCoins = 0;
		if (level == 1) {
			curCoins = CoinController.Instance.state.coinsLevel1;
		}else if(level == 2) {
			curCoins = CoinController.Instance.state.coinsLevel2;
		}else if(level == 3) {
			curCoins = CoinController.Instance.state.coinsLevel3;
		}
		txtCoins.text = curCoins.ToString();
		
		//Die Empties zum Spawner von Extra Leben und Coins werden eingelesen
		collectableSpawnpoints.Add(GameObject.FindGameObjectsWithTag("CoinSpawner"));
		collectableSpawnpoints.Add(GameObject.FindGameObjectsWithTag("ExtraLifeSpawner"));
		
		ResetGame(); //Das Spiel wird zurückgesetzt, um die Startbedingungen zu schaffen
	}
	
	void Update () {
		if (!pause) {
			Vector3 curPos = rigPlayer.position;

			if (lockplayer) {
				Vector3 pos = gravity.normalized * 1.71f;
				pos.z = player.transform.position.z;
				pos = pos + (-gravity) * curJumpHight;
				player.transform.position = pos;
				rigPlayer.MovePosition (new Vector3 (pos.x, pos.y, curPos.z + playerSpeed / 30f)); //Der Spieler wird nach vorne bewegt

			} else {
				rigPlayer.MovePosition (new Vector3 (curPos.x, curPos.y, curPos.z + playerSpeed / 30f)); //Der Spieler wird nach vorne bewegt

			}

			//Camera Movement
	
			//jumpVec = -gravity * jumpForce*200;

			//player.transform.Rotate(0f,0f,(rot * rotSpeed));
			player.transform.up = -gravity;
		}

		camDummy.position = player.transform.position;
		camDummy.rotation = player.transform.rotation;
		Camera.main.transform.rotation = Quaternion.identity;
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


	public void RotateWorldGyro(Vector2 gravityGyro){
		gravity.x = gravityGyro.x * 9.81f;
		gravity.y = gravityGyro.y * 9.81f;
		gravity.z = 0f;
		Physics.gravity = gravity;



	}

	public void LoadMenu(){

		SceneManager.LoadScene("Menu");
	}
	
	//Wenn das Level auf Anfangszustand zurückgesetzt werden soll
	public void ResetGame(){
		rigPlayer.velocity = Vector3.zero;
		player.transform.position = startPosition;
		player.transform.rotation = startQuaternion;
		Physics.gravity = startGravity;
		playerSpeed = startSpeed;
		//jumpVec = startJumpVector;
		
		lifes = startLifes;
		coinCount = 0;
		txtCoins.text = coinCount.ToString();
		txtLifes.text = lifes.ToString();
		
		DestroyGeneratedObjects(); //Alle noch vorhandenen Coins, Extra Leben und Moving Objects (wie Steine etc.) werden entfernt
			//damit es nicht zu Stacks kommt
		ResetCheckpoints(); //Die aktivierten Checkpoints werden unchecked
		SpawnCollectables(); //Coins und Extra Leben werden gespwaned
		SpawnMovingObstacles(); //Moving Objects (wie Steine, Pendel etc.) werden gespawned

		PauseGame ();
	}
	
	//Der Spieler wird zum letzten aktivierten Checkpoint gesetzt
	public void SetToLastCheckpoint(){
		if(checkpointActive){
			rigPlayer.velocity = Vector3.zero;
			Physics.gravity = checkpointGravity;
			player.transform.position = checkpointPosition;
			player.transform.rotation = checkpointQuaternion;
			playerSpeed = startSpeed;
			//jumpVec = checkpointJumpVector;
		}else{
			ResetPlayer();
		}
		
	}
	
	//Nur der Spieler wird zurückgesetzt and den Anfang des Levels, die Objekte bleiben aber so wie sie sind
	public void ResetPlayer(){
		
		rigPlayer.velocity = Vector3.zero;
		player.transform.position = startPosition;
		player.transform.rotation = startQuaternion;
		Physics.gravity = startGravity;
		playerSpeed = startSpeed;
		//jumpVec = startJumpVector;
	}
	
	// Es wird gecheckt, ob noch Leben verfuegbar sind
	public void CheckDeath(){
		if(lifes==0){ //wenn nein dann das Spiel zuruegsetzten
			//ResetGame();
			LoadMenu();
			pause = true;
			pausePanel.SetActive (true);
			txtPauseButton.gameObject.SetActive(false);
		}else{ //Wenn ja, dann zum letzten Checkpoint zuruegsetzten
			lifes--;
			txtLifes.text = lifes.ToString();
			SetToLastCheckpoint();
			pause = true;
			StartCoroutine("CountdownAfterPause");
		}
	}
	
	void DestroyGeneratedObjects(){
		
		foreach (Transform child in parentMovingObstacles.transform)
		{
			Destroy(child.gameObject);
		}
		foreach (Transform child in parentCoins.transform)
		{
			Destroy(child.gameObject);
		}
	}
	
	void SpawnCollectables(){
		int i = 0;
		foreach(GameObject[] tempList in collectableSpawnpoints){
			foreach(GameObject trans in tempList){
				GameObject tempOb = Instantiate(collectablePrefabs[i], trans.transform.position, Quaternion.identity);
				tempOb.transform.parent = parentCoins;
			}
			i++;
		}
		
	}
	
	void SpawnMovingObstacles(){
		foreach(Transform trans in movingObstacleSpawnpoints){
			GameObject tempOb = Instantiate(movingObstaclePrefabs[0], trans.position, Quaternion.identity);
			tempOb.transform.parent = parentMovingObstacles;
			movingObstacles.Add(tempOb);
		}
	}
	
	void ResetCheckpoints(){
		checkpointActive = false;
		foreach(GameObject ob in touchedCheckpoints){
			ob.GetComponent<Renderer>().material = matCheckpointUnchecked;
	
		}
		touchedCheckpoints.Clear();
	}
	
	//Ab hier sind die die Funktionen für die Interactables
	public void AddCoin(GameObject ob){
		Destroy(ob);
		int curCoins = 0;
		if (level == 1) {
			CoinController.Instance.state.coinsLevel1++;
			curCoins = CoinController.Instance.state.coinsLevel1;
		}else if(level == 2) {
			curCoins = CoinController.Instance.state.coinsLevel2++;
		}else if(level == 3) {
			curCoins = CoinController.Instance.state.coinsLevel3++;
		}
		txtCoins.text = curCoins.ToString();
	}
	
	public void PlayerJump(){
		Vector3 vec = Physics.gravity.normalized;
		//rigPlayer.AddForce(-vec * jumpForce*200);
		inputJump = true;
		StartCoroutine("Jump");
	}
	
	public void SetPlayerSpeed(float f){
		playerSpeed = f;
	}
	public void ResetPlayerSpeed(){
		playerSpeed = startSpeed;
	}
	
	public void PlayerSpeedBoost(){
		StartCoroutine("Boost");
	}
	
	IEnumerator Boost() {
		playerSpeed = 3f;
		yield return new WaitForSeconds(boostLenth);
		ResetPlayerSpeed();
	}
	
	public void SetCheckpoint(GameObject checkpointOb){
		checkpointActive = true;
		touchedCheckpoints.Add(checkpointOb);
		checkpointGravity = Physics.gravity;
		checkpointPosition = player.transform.position;
		checkpointQuaternion = player.transform.rotation;
		//checkpointJumpVector = jumpVec;
		
		checkpointOb.GetComponent<Renderer>().material = matCheckpointChecked;
	}
	
	
	public void GetExtraLife(GameObject ob){
		Destroy(ob);
		lifes++;
		txtLifes.text = lifes.ToString();
		
	}

	public void PauseGame(){
		if (!pause) {
			pause = true;
			pausePanel.SetActive (true);
			int curCoins = 0;
			if (level == 1) {
				curCoins = CoinController.Instance.state.coinsLevel1;
			}else if(level == 2) {
				curCoins = CoinController.Instance.state.coinsLevel2;
			}else if(level == 3) {
				curCoins = CoinController.Instance.state.coinsLevel3;
			}
			txtScore.text = curCoins.ToString () + "/8";
			txtPauseButton.text = "Resume";
		} else if (pause) {
			pausePanel.SetActive (false);
			txtPauseButton.text = "Pause";
			StartCoroutine("CountdownAfterPause");
		}

	}


	float maxJumpHeight = 0.17f;
	float jumpSpeed = 2.0f;
	float fallSpeed = 0.2f;
	public bool inputJump = false;

	public float curJumpHight = 0;

	IEnumerator Jump(){
		while(true)
		{
			if(curJumpHight >= maxJumpHeight-0.05f)
				inputJump = false;
			if(inputJump)
				curJumpHight = Mathf.Lerp (curJumpHight, maxJumpHeight, jumpSpeed * Time.smoothDeltaTime);
			else if(!inputJump)
			{
				curJumpHight = curJumpHight - fallSpeed * Time.smoothDeltaTime;
				//Debug.Log (curJumpHight);
				if (curJumpHight <= 0) {
					curJumpHight = 0;
					StopAllCoroutines ();
				}
			}

			yield return null;
		}
	}

	IEnumerator CountdownAfterPause(){
		txtCountdown.gameObject.SetActive (true);
		for (int i = 3; i > 0; i--) {
			txtCountdown.text = i.ToString ();
			yield return new WaitForSeconds(1f);
		}

		txtCountdown.gameObject.SetActive (false);
		pause = false;
	}
}
