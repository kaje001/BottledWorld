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
	public float value;
	public float steps;

	[SerializeField] Image image;
	[SerializeField] Text txtName;
	[SerializeField] Text txtDescription;

	[SerializeField] Image imageFill;
	[SerializeField] Text txtProgress;

	public void SetValues(){
		Debug.Log ("UpdateAchivlment");
		if (CoinController.Instance.IsAchievmentUnlocked (index)) {
			image.sprite = spriteActive;
			Debug.Log ("sprite");
		} else {
			image.sprite = spriteInactive;
		}

		txtName.text = stringName;
		txtDescription.text = stringDescription;

		if (steps > 1) {
			imageFill.fillAmount = value/steps;
			txtProgress.text = (int)value + "/" + steps;
		}
	}

}
