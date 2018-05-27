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

	public bool IsCoinCollected(int level, int index){
		if (level == 1) {
			return (state.coinsCollectedLevel1 & (1 << (index))) != 0;
		} else if (level == 2) {
			return (state.coinsCollectedLevel2 & (1 << (index))) != 0;
		} else if (level == 3) {
			return (state.coinsCollectedLevel3 & (1 << (index))) != 0;
		} else if (level == 4) {
			return (state.coinsCollectedLevel4 & (1 << (index))) != 0;
		} else {
			return false;
		}

	}
		
	public void CollectCoin(int level, int index){
		if (level == 1) {
			state.coinsCollectedLevel1 |= 1 << index;
		} else if (level == 2) {
			state.coinsCollectedLevel2 |= 1 << index;
		} else if (level == 3) {
			state.coinsCollectedLevel3 |= 1 << index;
		} else if (level == 4) {
			state.coinsCollectedLevel4 |= 1 << index;
		}
	}

	public bool IsCustomOwned(int index){
		return (state.customs & (1 << index)) != 0;
	}


	public void UnlockCustom(int index){
		state.customs |= 1 << index;
	}


	public void LockCustom(int index){
		state.customs ^= 1 << index;
	}

	public void ResetSaveState(){
		PlayerPrefs.DeleteKey ("save");
		Load ();
	}

	public int AddCoinForLevel(int level, int value = 1){
		state.totalCoins += value;
		state.availableCoins += value;
		if (level == 1) {
			state.coinsLevel1 += value;
			return state.coinsLevel1;
		} else if (level == 2) {
			state.coinsLevel2 += value;
			return state.coinsLevel2;
		} else if (level == 3) {
			state.coinsLevel3 += value;
			return state.coinsLevel3;
		} else if (level == 4) {
			state.coinsLevel4 += value;
			return state.coinsLevel4;
		} else{
			return 0;
		}
	}


	public int GetCoinForLevel(int level){
		if (level == 1) {
			return state.coinsLevel1;
		} else if (level == 2) {
			return state.coinsLevel2;
		} else if (level == 3) {
			return state.coinsLevel3;
		} else if (level == 4) {
			return state.coinsLevel4;
		} else{
			return 0;
		}
	}

}
