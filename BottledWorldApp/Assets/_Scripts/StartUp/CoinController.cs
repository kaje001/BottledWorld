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

	//Saving and Loading
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

	public void ResetSaveState(){
		PlayerPrefs.DeleteKey ("save");
		Load ();
	}

	//Customization
	public bool IsCustomOwned(int index){
		return (state.unlockedCustoms & (1 << index)) != 0;
	}


	public void UnlockCustom(int index){
		state.unlockedCustoms |= 1 << index;
	}


	public void LockCustom(int index){
		state.unlockedCustoms ^= 1 << index;
	}

	public bool IsCustomEquipped(int index){
		return (state.equippedCustoms & (1 << index)) != 0;
	}


	public void EquipCustom(int index){
		state.equippedCustoms |= 1 << index;
	}


	public void UnequipCustom(int index){
		state.equippedCustoms ^= 1 << index;
	}

	//Coins
	public bool IsCoinCollected(int level, int index){
		if (level == 0) {
			return (state.coinsCollectedLevel0 & (1 << (index))) != 0;
		} else if (level == 1) {
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
		if (level == 0) {
			state.coinsCollectedLevel0 |= 1 << index;
		} else if (level == 1) {
			state.coinsCollectedLevel1 |= 1 << index;
		} else if (level == 2) {
			state.coinsCollectedLevel2 |= 1 << index;
		} else if (level == 3) {
			state.coinsCollectedLevel3 |= 1 << index;
		} else if (level == 4) {
			state.coinsCollectedLevel4 |= 1 << index;
		}
	}

	public int AddCoinForLevel(int level, int value = 1){
		state.totalCoins += value;
		state.availableCoins += value;
		if (level == 0) {
			state.coinsLevel0 += value;
			return state.coinsLevel0;
		} else if (level == 1) {
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
		if (level == 0) {
			return state.coinsLevel0;
		} else if (level == 1) {
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

	//Level
	public bool IsLevelUnlocked(int index){
		return (state.unlockedLevels & (1 << index)) != 0;
	}

	public void UnlockLevel(int index){
		state.unlockedLevels |= 1 << index;
	}

	public void LockLevel(int index){
		state.unlockedLevels ^= 1 << index;
	}

	//Bioms
	public bool IsBiomUnlocked(int index){
		return (state.unlockedBioms & (1 << index)) != 0;
	}

	public void UnlockBiom(int index){
		state.unlockedBioms |= 1 << index;
	}

	public void LockBiom(int index){
		state.unlockedBioms ^= 1 << index;
	}

}
