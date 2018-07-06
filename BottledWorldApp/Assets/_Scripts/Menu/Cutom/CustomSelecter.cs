using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSelecter : MonoBehaviour {

	[SerializeField] GameObject[] hats;
	[SerializeField] GameObject[] socks;
	[SerializeField] GameObject[] backs;
	//[SerializeField] GameObject[] colors;

	int activeHat = -1;
	int activeSock = -1;
	int activeBack = -1;

	// Use this for initialization
	void Start () {
		ActivateSelectedObjects ();
	}


	void ActivateSelectedObjects(){
		int i = 0;

		for (i = 0; i < 8; i++) {
			if (i >= hats.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomSelected (i)) {
				hats [i].SetActive (true);
				activeHat = i;
			} else {
				hats [i].SetActive (false);
			}
		}

		for (i = 0; i < 8; i++) {
			if (i >= socks.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomSelected (8 + i)) {
				socks [i].SetActive (true);
				activeSock = i;
			} else {
				socks [i].SetActive (false);
			}
		}

		for (i = 0; i < 8; i++) {
			if (i >= backs.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomSelected (16 + i)) {
				backs [i].SetActive (true);
				activeBack = i;
			} else {
				backs [i].SetActive (false);
			}
		}
	}

	public void SelectHat(int index){
		if (activeHat > -1) {
			CoinController.Instance.DeselectCustom (activeHat);
		}
		CoinController.Instance.SelectCustom (index);
		activeHat = index;

		ActivateSelectedObjects ();
	}

	public void SelectSock(int index){
		if (activeHat > -1) {
			CoinController.Instance.DeselectCustom (8 + activeHat);
		}
		CoinController.Instance.SelectCustom (8 + index);
		activeSock = index;

		ActivateSelectedObjects ();
	}

	public void SelectBack(int index){
		if (activeHat > -1) {
			CoinController.Instance.DeselectCustom (16 + activeHat);
		}
		CoinController.Instance.SelectCustom (16 + index);
		activeBack = index;

		ActivateSelectedObjects ();
	}

	public void Reload(){
		ActivateSelectedObjects ();
	}

}
