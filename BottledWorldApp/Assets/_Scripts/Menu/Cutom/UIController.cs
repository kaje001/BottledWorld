using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	int activePanel = -1;

	[SerializeField] GameObject[] panelCategories; //0 = Hats, 1 = Socks, 2 = Backpacks, 3 = Wings

	void Start(){
		OnChangePanel (-1);
	}

	public void OnChangePanel(int panelIndex){
		int i = 0;
		foreach (GameObject ob in panelCategories) {
			if (i == panelIndex) {
				if (ob.activeSelf) {
					ob.SetActive (false);
					activePanel = -1;
				} else {
					ob.SetActive (true);
					activePanel = i;
				}
			} else {
				ob.SetActive (false);
				activePanel = -1;
			}
			i++;
		}
	}
}
