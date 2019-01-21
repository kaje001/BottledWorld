using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConEndLevel : MonoBehaviour {

	[SerializeField] Text textCoins;
	[SerializeField] Text textCoinsInLevel;

	[SerializeField] GameObject LockOpen;
	[SerializeField] GameObject LockClosed;
	[SerializeField] Image imageBar;
	int coins = 0;
	[SerializeField] Text coinsBiomBegin;
	[SerializeField] Text coinsNextBiom;
	[SerializeField] Text coinsNextBiom2;

	[SerializeField] AudioClip soundUnlockLock;
	[SerializeField] AudioClip soundBarCountsUp;

	// Use this for initialization
	void Start () {

		coins = CoinController.Instance.state.totalCoins - LastGameData.Instance.coins;

		if (CoinController.Instance.state.totalCoins - LastGameData.Instance.coins >= LastGameData.Instance.biom * 8 + 8) {
			//coins = LastGameData.Instance.biom * 8 + 8;

			imageBar.fillAmount = 1;
			LockOpen.SetActive (true);
			LockClosed.SetActive (false);
		} else {
			imageBar.fillAmount = (coins - LastGameData.Instance.biom * 8) * 0.125f;
			LockClosed.SetActive (true);
			LockOpen.SetActive (false);
		}
			
		//coinsBiomBegin.text = (LastGameData.Instance.biom * 8).ToString();
		coinsBiomBegin.text = coins.ToString();
		coinsNextBiom.text = (LastGameData.Instance.biom * 8 + 8).ToString();
		coinsNextBiom2.text = (LastGameData.Instance.biom * 8 + 8).ToString();
		textCoins.text = "+ " + LastGameData.Instance.coins.ToString ();
		textCoinsInLevel.text = LastGameData.Instance.sugarCubesForLevel + "/" + LastGameData.Instance.totalSugarCubesLevel;
	}

	public void UpdateBar(){
		/*if (coins >= 8) {
			return;
		}*/
		StartCoroutine (FillBar());

	}

	IEnumerator FillBar(){
		
		yield return new WaitForSeconds (0.3f);
		if (coins >= LastGameData.Instance.biom * 8 + 8) {
			for(int i = 0; i < LastGameData.Instance.coins; i++){
				coins++;
				SoundManager.Instance.PlaySingle (soundBarCountsUp);
				coinsBiomBegin.text = coins.ToString();
				yield return new WaitForSeconds (0.3f);
			}
		} else {
			for(int i = 0; i < LastGameData.Instance.coins; i++){
				coins++;
				imageBar.fillAmount = 0.125f * (coins - LastGameData.Instance.biom * 8);
				SoundManager.Instance.PlaySingle (soundBarCountsUp);
				coinsBiomBegin.text = coins.ToString();
				yield return new WaitForSeconds (0.3f);
				if (coins >= LastGameData.Instance.biom * 8 + 8) {
					SoundManager.Instance.PlaySingle (soundUnlockLock);
					LockOpen.SetActive (true);
					LockClosed.SetActive (false);
					break;
				}
			}
		}

	}
}
