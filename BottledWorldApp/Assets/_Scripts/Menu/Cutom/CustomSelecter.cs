using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSelecter : MonoBehaviour {

	[SerializeField] GameObject[] hats;
	[SerializeField] GameObject[] socksFL;
	[SerializeField] GameObject[] socksFR;
	[SerializeField] GameObject[] socksBL;
    [SerializeField] GameObject[] socksBR;
    [SerializeField] GameObject[] socksArmR;
    [SerializeField] GameObject[] socksArmL;
    [SerializeField] GameObject[] backs;
	[SerializeField] Texture2D[] colors;
	[SerializeField] Material[] mats;
	List<GameObject[]> socks = new List<GameObject[]>();
    //[SerializeField] GameObject[] colors;

    // Use this for initialization
    void Start () {
		socks.Add (socksFL);
		socks.Add (socksFR);
		socks.Add (socksBL);
        socks.Add(socksBR);
        socks.Add(socksArmR);
        socks.Add(socksArmL);

        ActivateEquippedObjects ();
	}

	void ActivateEquippedObjects(){
		int i = 0;

		for (i = 0; i < 8; i++) {
			if (i >= hats.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomEquipped (i)) {
				hats [i].SetActive (true);
			} else {
				hats [i].SetActive (false);
			}
		}

		for (i = 0; i < 8; i++) {
			if (i >= socksFL.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomEquipped (8 + i)) {
				foreach (GameObject[] gos in socks) {
					gos [i].SetActive (true);
				}
            } else {
				foreach (GameObject[] gos in socks) {
					gos [i].SetActive (false);
				}
			}
            
        }

		for (i = 0; i < 8; i++) {
			if (i >= backs.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomEquipped (16 + i)) {
				backs [i].SetActive (true);
			} else {
				backs [i].SetActive (false);
			}
		}

		for (i = 0; i < 8; i++) {
			if (i >= colors.Length) {
				break;
			}
			if (CoinController.Instance.IsCustomEquipped (24 + i)) {
				foreach (Material mat in mats) {
					mat.mainTexture = colors [i];
				}
			} else {
				
			}
		}
	}

	public void Reload(){
		ActivateEquippedObjects ();
	}


	//___________________________________test__________________________

	public void ShowSelectedObjects(int selectedObjects){
		int i = 0;
        
        for (i = 0; i < 8; i++) {
			if (i >= hats.Length) {
				break;
			}
			if ((selectedObjects & (1 << i)) != 0 ) {
				hats [i].SetActive (true);
			} else {
				hats [i].SetActive (false);
			}
		}

		for (i = 0; i < 8; i++) {
			if (i >= socksFL.Length) {
				break;
			}
			if ((selectedObjects & (1 << (8 + i))) != 0 ){
				foreach (GameObject[] gos in socks) {
					gos [i].SetActive (true);
				}
			} else {
				foreach (GameObject[] gos in socks) {
					gos [i].SetActive (false);
				}
			}
		}

		for (i = 0; i < 8; i++) {
			if (i >= backs.Length) {
				break;
			}
			if ((selectedObjects & (1 << (16 + i))) != 0 ){
				backs [i].SetActive (true);
			} else {
				backs [i].SetActive (false);
			}
		}

		for (i = 0; i < 8; i++) {
			if (i >= colors.Length) {
				break;
			}
			if ((selectedObjects & (1 << (24 + i))) != 0 )
            {
                foreach (Material mat in mats) {
					mat.mainTexture = colors [i];
				}
			} else {

			}
		}
	}
}
