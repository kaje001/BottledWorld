using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastGameData : MonoBehaviour {

	public static LastGameData Instance { get; set; } 

	public int coins = 0;
	public int hearts = 0;
	public int deaths = 0;
	public bool won = false;
	public bool wonFirstTime = false;
	public int unlockLevel = 0;
	public int level = 0;
	public int biom = 0;
	public int sugarCubesForLevel = 0;
	public int totalSugarCubesLevel = 0;

	void Awake(){
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}


}
