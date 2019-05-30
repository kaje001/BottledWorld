using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSelf : MonoBehaviour {

    [SerializeField] int index = 0;

    private void Start()
    {
        if(index != 0)
        {
            gameObject.SetActive(false);
            if(index == 1 && !CoinController.Instance.state.info4thlevel)
            {
                CoinController.Instance.state.info4thlevel = true;
                gameObject.SetActive(true);
            }
            if (index == 2 && !CoinController.Instance.state.info9thlevel)
            {
                CoinController.Instance.state.info9thlevel = true;
                gameObject.SetActive(true);
            }
        }
    }

    public void HideMySelf(){
		gameObject.SetActive(false);
	}
}
