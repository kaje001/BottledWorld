using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSelecter : MonoBehaviour {

	[SerializeField] GameObject[] hats;
	[SerializeField] GameObject[] socksFL;
	[SerializeField] GameObject[] socksFR;
	[SerializeField] GameObject[] socksBL;
	[SerializeField] GameObject[] socksBR;
	[SerializeField] GameObject[] backs;
	List<GameObject[]> socks = new List<GameObject[]>();
	//[SerializeField] GameObject[] colors;

	// Use this for initialization
	void Start () {
		socks.Add (socksFL);
		socks.Add (socksFR);
		socks.Add (socksBL);
		socks.Add (socksBR);

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
			if (i >= socksFL.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomSelected (8 + i)) {
				foreach (GameObject[] gos in socks) {
					gos [i].SetActive (true);
				}
			} else {
				foreach (GameObject[] gos in socks) {
					gos [i].SetActive (false);
				}
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
