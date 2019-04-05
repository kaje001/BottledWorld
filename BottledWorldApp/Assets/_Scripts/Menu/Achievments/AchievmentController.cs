using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class AchievmentController : MonoBehaviour {

	string[] nameAchievments = new string[10];
	string[] descriptionAchievments = new string[10];
	[SerializeField] Sprite[] imageAchievmentsActive;
	[SerializeField] Sprite[] imageAchievmentsInactive;
	[SerializeField] int[] stepsAchievment;

	[SerializeField] GameObject prefabAchievment; //Child  1: image, 2: name, 3: description
	[SerializeField] GameObject prefabAchievmentProgress; //Child  1: image, 2: name, 3: description 4: ProgressBar
	[SerializeField] RectTransform parentAchievments;

	List<Achievment> achieves = new List<Achievment>();

	// Use this for initialization
	void Awake () {

        nameAchievments[0] = LocalizationManager.instance.GetLocalizedValue(28);
        nameAchievments[1] = LocalizationManager.instance.GetLocalizedValue(29);
        nameAchievments[2] = LocalizationManager.instance.GetLocalizedValue(30);
        nameAchievments[3] = LocalizationManager.instance.GetLocalizedValue(31);
        nameAchievments[4] = LocalizationManager.instance.GetLocalizedValue(32);
        nameAchievments[5] = LocalizationManager.instance.GetLocalizedValue(33);
        nameAchievments[6] = LocalizationManager.instance.GetLocalizedValue(34);
        nameAchievments[7] = LocalizationManager.instance.GetLocalizedValue(35);
        nameAchievments[8] = LocalizationManager.instance.GetLocalizedValue(36);
        nameAchievments[9] = LocalizationManager.instance.GetLocalizedValue(37);

        descriptionAchievments[0] = LocalizationManager.instance.GetLocalizedValue(18);
        descriptionAchievments[1] = LocalizationManager.instance.GetLocalizedValue(19);
        descriptionAchievments[2] = LocalizationManager.instance.GetLocalizedValue(20);
        descriptionAchievments[3] = LocalizationManager.instance.GetLocalizedValue(21);
        descriptionAchievments[4] = LocalizationManager.instance.GetLocalizedValue(22);
        descriptionAchievments[5] = LocalizationManager.instance.GetLocalizedValue(23);
        descriptionAchievments[6] = LocalizationManager.instance.GetLocalizedValue(24);
        descriptionAchievments[7] = LocalizationManager.instance.GetLocalizedValue(25);
        descriptionAchievments[8] = LocalizationManager.instance.GetLocalizedValue(26);
        descriptionAchievments[9] = LocalizationManager.instance.GetLocalizedValue(27);

        int contentSizeY = 0;

		//parentAchievments.position = Vector2.zero;
		for (int i = 0; i < nameAchievments.Length; i++) {
			GameObject ob = null;
			if (stepsAchievment [i] > 1) {
				ob = Instantiate (prefabAchievmentProgress, parentAchievments);
                ob.transform.localPosition = new Vector2(0f, -contentSizeY - 100);
                contentSizeY += 220;
            } else {
				ob = Instantiate (prefabAchievment, parentAchievments);
                ob.transform.localPosition = new Vector2(0f, -contentSizeY - 80);
                contentSizeY += 170;
            }
			Achievment achive = ob.GetComponent<Achievment> ();
			achive.index = i;
			achive.spriteActive = imageAchievmentsActive [i];
			achive.spriteInactive = imageAchievmentsInactive [i];
			achive.stringName = nameAchievments [i];
			achive.stringDescription = descriptionAchievments [i];
			achive.steps = stepsAchievment [i];
			achive.SetValues ();
			achieves.Add (achive);
			//ob.transform.GetChild (0).GetComponent<Image> ().sprite = imageAchievments [i];
			//ob.transform.GetChild (1).GetComponent<Text> ().text = nameAchievments [i];
			//ob.transform.GetChild (2).GetComponent<Text> ().text = descriptionAchievments [i];
		}

        parentAchievments.sizeDelta = new Vector2(0f, contentSizeY);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateAchievments(){
		//TODO Hier die values anpassen
		foreach(Achievment achiev in achieves){
			if (achiev.index == 1) {
				achiev.value = CoinController.Instance.CompletetLevelCount (0, 12);
			} else if (achiev.index == 2) {
				achiev.value = CoinController.Instance.state.totalDeaths;

			}else if (achiev.index == 3) {
				achiev.value = CoinController.Instance.CompletetLevelCount (1, 3);

			}else if (achiev.index == 4) {
				achiev.value = CoinController.Instance.CompletetLevelCount (5, 7);

			}else if (achiev.index == 5) {
				achiev.value = CoinController.Instance.CompletetLevelCount (9, 11);

			}
			achiev.SetValues ();
		}
	}
}
