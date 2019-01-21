using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentController : MonoBehaviour {

	[SerializeField] string[] nameAchievments;
	[SerializeField] string[] descriptionAchievments;
	[SerializeField] Sprite[] imageAchievmentsActive;
	[SerializeField] Sprite[] imageAchievmentsInactive;

	[SerializeField] GameObject prefabAchievment; //Child - 1: image, 2: name, 3: description
	[SerializeField] RectTransform parentAchievments;

	List<Achievment> achieves = new List<Achievment>();

	// Use this for initialization
	void Start () {
		parentAchievments.sizeDelta = new Vector2 (0f, nameAchievments.Length * 160);
		parentAchievments.position = Vector2.zero;
		for (int i = 0; i < nameAchievments.Length; i++) {
			GameObject ob = Instantiate (prefabAchievment, parentAchievments);
			ob.transform.localPosition = new Vector2 (0f, -160 * i - 80);
			Achievment achive = ob.GetComponent<Achievment> ();
			achive.index = i;
			achive.spriteActive = imageAchievmentsActive [i];
			achive.spriteInactive = imageAchievmentsInactive [i];
			achive.stringName = nameAchievments [i];
			achive.stringDescription = descriptionAchievments [i];
			achive.SetValues ();
			achieves.Add (achive);
			//ob.transform.GetChild (0).GetComponent<Image> ().sprite = imageAchievments [i];
			//ob.transform.GetChild (1).GetComponent<Text> ().text = nameAchievments [i];
			//ob.transform.GetChild (2).GetComponent<Text> ().text = descriptionAchievments [i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateAchievments(){
		foreach(Achievment achiev in achieves){
			achiev.SetValues ();
		}
	}
}
