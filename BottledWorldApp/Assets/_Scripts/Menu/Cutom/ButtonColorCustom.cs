using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorCustom : MonoBehaviour {

	Image im;
	[SerializeField] Sprite[] outlines; // 0 = thin; 1 = thick
	[SerializeField] Sprite[] buttonStates; // 0 = silouett; 1 = colored
	public int index;
	public int cathegory; //0 = hat; 1 = socks; 2 = backs; 4 = colors;

	[SerializeField] Text textCosts;
	[SerializeField] GameObject spriteCube;
	[SerializeField] GameObject spriteEquipped;

	[SerializeField] Color[] colors; //0 = notselectedFrame; 1 = selectedFrame; 2 = fontColornotpurchasable; 3 = fontColorpurchasable; 4 = backgroundDark; 5 = backgroundLight

	void Awake(){
		im = GetComponent<Image> ();
		HighlightSelected ();
		//spriteEquipped.SetActive (false);
	}

	public void SetColor(int i){
		if (i == 0) {
			im.color = colors[0];
			im.sprite = outlines [0];
		} else if (i == 1) {
			im.color = colors[1];
			im.sprite = outlines [1];
		}
	}

	public void HighlightSelected(){
		SetColor (0);
		int i = 0;
		if (cathegory == 0) {
			if (CoinController.Instance.IsCustomEquipped(index)) {
				SetColor (1);
			}
		}else if (cathegory == 1) {
			if (CoinController.Instance.IsCustomEquipped (8 + index)) {
				SetColor (1);
			}
		}else if (cathegory == 2) {
			if (CoinController.Instance.IsCustomEquipped (16 + index)) {
				SetColor (1);
			}
		}else if (cathegory == 3) {
			if (CoinController.Instance.IsCustomEquipped (24 + index)) {
				SetColor (1);
			}
		}

	}

	public void UpdateView(int costs, bool purchasable, bool notOwned, bool equipped){
		textCosts.text = costs.ToString ();
		spriteCube.SetActive (notOwned);

		if (notOwned) {
			spriteEquipped.SetActive (false);
			transform.parent.GetComponent<Image> ().color = colors [4];
			if (purchasable) {
				transform.GetChild (0).GetComponent<Image> ().sprite = buttonStates [1];
				textCosts.color = colors [3];
			} else {
				transform.GetChild (0).GetComponent<Image> ().sprite = buttonStates [0];
				textCosts.color = colors [2];
			}
		} else {
			transform.parent.GetComponent<Image> ().color = colors [5];
			if (equipped) {
				spriteEquipped.SetActive (true);

			} else {
				spriteEquipped.SetActive (false);
			}
		}


	}

}
