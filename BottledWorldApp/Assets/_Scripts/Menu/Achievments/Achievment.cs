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
    public int valueSugar;

	[SerializeField] Image image;
	[SerializeField] Text txtName;
	[SerializeField] Text txtDescription;

	[SerializeField] Image imageFill;
	[SerializeField] Text txtProgress;

    [SerializeField] GameObject imageLock;
    [SerializeField] GameObject imageSugarCube;
    [SerializeField] Text txtRewardSugar;
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
                image.sprite = spriteInactive;
                imageLock.SetActive(false);
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
        txtRewardSugar.text = "+" + valueSugar.ToString();

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
                //imageLock.SetActive(false);
                //imageStar.SetActive(false);
                //imageSugarCube.SetActive(false);
                CoinController.Instance.ClaimAchievment(index);
                CoinController.Instance.state.availableCoins+= valueSugar;
                CoinController.Instance.state.totalCoins+= valueSugar;
                GameObject.FindObjectOfType<AchievmentController>().UpdateAchievments();
            }
        }
    }

}
