using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour {

	public PlayerController playCon;
	
	// Use this for initialization
	void Start () {
		
	}
	

	void Update () {
		if(transform.position.z>40){
			playCon.LoadMenu(); //Wenn der Spieler das Lvel schafft wird er zurückgesetzt
		}
	}

	
	//Es werden alle Interactables abgefragt und dann in der PlayerController Class getriggered
	void OnTriggerEnter(Collider other){
		if(other.transform.tag == "Coin"){ //eine Muenze einsammeln
			playCon.AddCoin(other.gameObject);
		}
		if(other.transform.tag == "ExtraLife"){ //extra Leben einsammeln
			playCon.GetExtraLife(other.gameObject);
		}
		if(other.transform.tag == "Jump"){ //JumpTrigger anlaufen
			playCon.PlayerJump();
		}
		if(other.transform.tag == "Slow"){ //wird noch nict verwendet
			playCon.SetPlayerSpeed(0.8f); 
		}
		if(other.transform.tag == "NormalSpeed"){ //wird noch nicht verwendet
			playCon.ResetPlayerSpeed();
		}
		if(other.transform.tag == "Speed"){ //SpeedBoost anlaufen
			playCon.PlayerSpeedBoost();
		}
		if(other.transform.tag == "Checkpoint"){ //Checkpoint aktivieren
			playCon.SetCheckpoint(other.gameObject);
		}



		if(other.transform.tag == "Obstacle"){
			playCon.CheckDeath(); //Bei einem Zusammenstoss mit einem Hinderniss wird im PlayerController gecheckt, ob noch leben verfügbar sind und ob schon ein Checkpoint activiert wurde
		}
	}
}
