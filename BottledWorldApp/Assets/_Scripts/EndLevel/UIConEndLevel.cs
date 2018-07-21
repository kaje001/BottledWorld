using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConEndLevel : MonoBehaviour {

	[SerializeField] Text textCoins;

	// Use this for initialization
	void Start () {
		textCoins.text = "+ " + LastGameData.Instance.coins.ToString ();
	}

}
