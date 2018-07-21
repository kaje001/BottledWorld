using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSelecter : MonoBehaviour {

	[SerializeField] GameObject[] hats;
	[SerializeField] GameObject[] socks;
	[SerializeField] GameObject[] backs;
	//[SerializeField] GameObject[] colors;

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
			} else {
				backs [i].SetActive (false);
			}
		}
	}

	public void Reload(){
		ActivateSelectedObjects ();
	}

}
