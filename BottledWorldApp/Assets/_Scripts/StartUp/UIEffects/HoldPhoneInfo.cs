using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldPhoneInfo : MonoBehaviour {

	[SerializeField] Sprite sprite1;
	[SerializeField] Sprite sprite2;
	Image im;

	// Use this for initialization
	void Start () {
		im = GetComponent<Image> ();
		StartCoroutine (SpriteSwap());
	}
	
	IEnumerator SpriteSwap(){
		while (true) {
			im.sprite = sprite1;
			yield return new WaitForSeconds (2f);

			im.sprite = sprite2;
			yield return new WaitForSeconds (2f);
		}
	}
}
