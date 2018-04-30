using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

	public static CoinController Instance{ set; get; }

	public SaveState state;

	void Awake(){
		
		DontDestroyOnLoad (this);
		Instance = this;
		Load ();
	}

	public void Save(){
		PlayerPrefs.SetString ("save", Helper.Serialize<SaveState>(state));
	}

	public void Load(){
		if (PlayerPrefs.HasKey ("save")) {
			state = Helper.Deserialize<SaveState> (PlayerPrefs.GetString("save"));
		} else {
			state = new SaveState ();
			Save ();
			Debug.Log ("new savestate created");
		}
	}

	public bool IsCoinCollectedBiom1(int level, int index){
		return (state.coinsBiom1 & (1 << (index + level*8))) != 0;
	}


	public bool IsCustomOwned(int index){
		return (state.customs & (1 << index)) != 0;
	}

	public void CollectCoinBiom1(int level, int index){
		state.coinsBiom1 |= 1 << (index + level*8);
	}

	public void UnlockCustom(int index){
		state.customs |= 1 << index;
	}

	public void ResetSaveState(){
		PlayerPrefs.DeleteKey ("save");
	}
}
