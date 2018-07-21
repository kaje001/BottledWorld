using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {

	int activePanel = -1;


	[SerializeField] GameObject[] panelCategories; //0 = Hats, 1 = Socks, 2 = Backs, 3 = Colors
	[SerializeField] Image[] buttonsCategories; //0 = Hats, 1 = Socks, 2 = Backs, 3 = Colors
	[SerializeField] Sprite[] spriteUnselectedCategories; //0 = Hats, 1 = Socks, 2 = Backs, 3 = Colors
	[SerializeField] Sprite[] spriteSelectedCategories; //0 = Hats, 1 = Socks, 2 = Backs, 3 = Colors

	void Start(){
		OnChangePanel (-1);
		int i = 0;
		foreach (Image im in buttonsCategories) {
			im.sprite = spriteUnselectedCategories [i];
			i++;
		}
	}

	public void OnChangePanel(int panelIndex){
		EventSystem.current.SetSelectedGameObject (null);
		int i = 0;
		foreach (GameObject ob in panelCategories) {
			if (i == panelIndex) {
				if (ob.activeSelf) {
					ob.SetActive (false);
					buttonsCategories [i].sprite = spriteUnselectedCategories [i];
					activePanel = -1;
				} else {
					ob.SetActive (true);
					buttonsCategories [i].sprite = spriteSelectedCategories [i];
					activePanel = i;
				}
			} else {
				ob.SetActive (false);
				buttonsCategories [i].sprite = spriteUnselectedCategories [i];
				activePanel = -1;
			}
			i++;
		}

	}
}
