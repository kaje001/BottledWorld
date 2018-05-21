using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotItScript : MonoBehaviour {

	void Start(){
		if (CoinController.Instance.state.gotIt) {
			gameObject.SetActive (false);
		}
	}

	public void PressedGotIt(){
		gameObject.SetActive (false);
		CoinController.Instance.state.gotIt = true;
		CoinController.Instance.Save ();
	}
}
