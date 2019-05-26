using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConEndLevel : MonoBehaviour {

	[SerializeField] Text textCoins;
	[SerializeField] Text textCoinsInLevel;

	[SerializeField] GameObject LockOpen;
    [SerializeField] GameObject LockClosed;
    [SerializeField] GameObject panelProgress;
    [SerializeField] Image imageBar;
	int coins = 0;
	[SerializeField] Text coinsBiomBegin;
	[SerializeField] Text coinsNextBiom;
	[SerializeField] Text coinsNextBiom2;

	[SerializeField] AudioClip soundUnlockLock;
	[SerializeField] AudioClip soundBarCountsUp;

	// Use this for initialization
	void Start () {

        if(LastGameData.Instance.biom == 2)
        {
            panelProgress.SetActive(false);
        }

		coins = CoinController.Instance.state.totalCoins - LastGameData.Instance.coins;

		if (CoinController.Instance.state.totalCoins - LastGameData.Instance.coins >= LastGameData.Instance.biom * 14 + 14) {
			//coins = LastGameData.Instance.biom * 14 + 14;

			imageBar.fillAmount = 1;
			LockOpen.SetActive (true);
			LockClosed.SetActive (false);
		} else {
			imageBar.fillAmount = (coins - LastGameData.Instance.biom * 14) * 0.0714f;
			LockClosed.SetActive (true);
			LockOpen.SetActive (false);
		}
			
		//coinsBiomBegin.text = (LastGameData.Instance.biom * 14).ToString();
		coinsBiomBegin.text = coins.ToString();
		coinsNextBiom.text = (LastGameData.Instance.biom * 14 + 14).ToString();
		coinsNextBiom2.text = (LastGameData.Instance.biom * 14 + 14).ToString();
		textCoins.text = "+ " + LastGameData.Instance.coins.ToString ();
		textCoinsInLevel.text = LastGameData.Instance.sugarCubesForLevel + "/" + LastGameData.Instance.totalSugarCubesLevel;
	}

	public void UpdateBar(){
        /*if (coins >= 14) {
			return;
		}*/
        if (LastGameData.Instance.biom == 2)
        {
            return;
        }
        StartCoroutine (FillBar());

	}

	IEnumerator FillBar(){
		
		yield return new WaitForSeconds (0.3f);
		if (coins >= LastGameData.Instance.biom * 14 + 14) {
			for(int i = 0; i < LastGameData.Instance.coins; i++){
				coins++;
				SoundManager.Instance.PlaySingle (soundBarCountsUp);
				coinsBiomBegin.text = coins.ToString();
				yield return new WaitForSeconds (0.3f);
			}
		} else {
			for(int i = 0; i < LastGameData.Instance.coins; i++){
				coins++;
				imageBar.fillAmount = 0.0714f * (coins - LastGameData.Instance.biom * 14);
				SoundManager.Instance.PlaySingle (soundBarCountsUp);
				coinsBiomBegin.text = coins.ToString();
				yield return new WaitForSeconds (0.3f);
				if (coins >= LastGameData.Instance.biom * 14 + 14) {
					SoundManager.Instance.PlaySingle (soundUnlockLock);
					LockOpen.SetActive (true);
					LockClosed.SetActive (false);
					break;
				}
			}
		}

	}
}
