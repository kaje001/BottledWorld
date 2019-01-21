using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievment : MonoBehaviour {

	public int index;

	public Sprite spriteActive;
	public Sprite spriteInactive;
	public string stringName;
	public string stringDescription;

	[SerializeField] Image image;
	[SerializeField] Text txtName;
	[SerializeField] Text txtDescription;

	public void SetValues(){
		if (CoinController.Instance.IsAchievmentÚnlocked (index)) {
			image.sprite = spriteActive;
		} else {
			image.sprite = spriteInactive;
		}

		txtName.text = stringName;
		txtDescription.text = stringDescription;
	}

}
