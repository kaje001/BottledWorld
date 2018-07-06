using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorCustom : MonoBehaviour {

	Image im;
	[SerializeField] Sprite[] outlines;
	[SerializeField] int index;
	[SerializeField] int cathegory; //0 = hat; 1 = socks; 2 = backs; 4 = colors;

	void Start(){
		im = GetComponent<Image> ();
		HighlightSelected ();
	}

	public void SetColor(int i){
		if (i == 0) {
			im.color = Color.black;
			im.sprite = outlines [0];
		} else if (i == 1) {
			im.color = Color.green;
			im.sprite = outlines [1];
		}
	}

	void HighlightSelected(){
		int i = 0;
		if (cathegory == 0) {
			if (CoinController.Instance.IsCustomSelected (index)) {
				SetColor (1);
			}
		}else if (cathegory == 1) {
			if (CoinController.Instance.IsCustomSelected (8 + index)) {
				SetColor (1);
			}
		}else if (cathegory == 2) {
			if (CoinController.Instance.IsCustomSelected (16 + index)) {
				SetColor (1);
			}
		}else if (cathegory == 3) {
			if (CoinController.Instance.IsCustomSelected (24 + index)) {
				SetColor (1);
			}
		}

	}
}
