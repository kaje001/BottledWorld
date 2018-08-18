using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressLevel : MonoBehaviour {

	[SerializeField] Image imageFill;
	[SerializeField] GameObject imageAnt;

	public void UpdateBar(float value){
		imageFill.fillAmount = value;
		imageAnt.transform.localPosition = new Vector3 (600 * value - 300, imageAnt.transform.localPosition.y, imageAnt.transform.localPosition.z);
	}
}
