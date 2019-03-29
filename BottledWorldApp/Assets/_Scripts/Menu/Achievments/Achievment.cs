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

    [SerializeField] GameObject imageLock;
    [SerializeField] GameObject imageSugarCube;
    [SerializeField] GameObject imageStar;

    public void SetValues(){
		if (CoinController.Instance.IsAchievmentUnlocked (index)) {
            if (CoinController.Instance.IsAchievmentClaimed(index))
            {
                image.sprite = spriteActive;
                imageLock.SetActive(false);
                imageStar.SetActive(false);
                imageSugarCube.SetActive(false);
            }
            else
            {
                image.sprite = spriteActive;
                imageLock.SetActive(true);
                imageSugarCube.SetActive(true);
                imageStar.SetActive(true);
            }
		} else {
            image.sprite = spriteInactive;
            imageLock.SetActive(true);
            imageSugarCube.SetActive(false);
            imageStar.SetActive(false);
        }
        

		txtName.text = stringName;
		txtDescription.text = stringDescription;

		if (steps > 1) {
			imageFill.fillAmount = value/steps;
			txtProgress.text = (int)value + "/" + steps;
		}
	}

    public void OnClickLock()
    {
        if (CoinController.Instance.IsAchievmentUnlocked(index))
        {
            if (!CoinController.Instance.IsAchievmentClaimed(index))
            {
                imageLock.SetActive(false);
                imageStar.SetActive(false);
                imageSugarCube.SetActive(false);
                CoinController.Instance.ClaimAchievment(index);
                CoinController.Instance.state.availableCoins++;
                CoinController.Instance.state.totalCoins++;
            }
        }
    }

}
