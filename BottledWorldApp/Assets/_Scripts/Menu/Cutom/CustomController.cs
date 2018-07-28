using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomController : MonoBehaviour {

	[SerializeField] GameObject buttonBuy;
	[SerializeField] GameObject buttonSelect;

	[SerializeField] CustomSelecter cusSelect;
	[SerializeField] ButtonColorCustom[] bccs;

	[SerializeField] int[] prices;

	[SerializeField] Text txtAvailableCoins;

	int activeHat = 0;
	int activeSock = 0;
	int activeBack = 0;

	int selectedHat = 0;
	int selectedSock = 0;
	int selectedBack = 0;

	int activeStartHat = 0;
	int activeStartSock = 0;
	int activeStartBack = 0;

	int lastSelectedItem = -1;

	// Use this for initialization
	void Start () {
		ActivateSelectedObjects ();
		activeStartHat = activeHat;
		activeStartSock = activeSock;
		activeStartBack = activeBack;
		lastSelectedItem = -1;
		buttonBuy.SetActive (false);

		txtAvailableCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();

		Debug.Log (bccs.Length);

		RefreshButtonCustomSelect ();
	}

	void ActivateSelectedObjects(){
		int i = 0;

		for (i = 0; i < 8; i++) {
			
			if (CoinController.Instance.IsCustomSelected (i)) {
				activeHat = i;
				selectedHat = i;
			}
		}

		for (i = 0; i < 8; i++) {
			if (CoinController.Instance.IsCustomSelected (8 + i)) {
				activeSock = i;
				selectedSock = i;
			}
		}

		for (i = 0; i < 8; i++) {
			if (CoinController.Instance.IsCustomSelected (16 + i)) {
				activeBack = i;
				selectedBack = i;
			}
		}
	}

	public void SelectHat(int index){
		/*if (activeHat > -1) {
			CoinController.Instance.DeselectCustom (activeHat);
		}
		CoinController.Instance.SelectCustom (index);
		activeHat = index;

		cusSelect.Reload();*/

		lastSelectedItem = index;
		if (CheckEnoughCoins ()) {
			PushNewCustom ();
			if (CheckOwned ()) {
				buttonBuy.SetActive (false);
				//buttonSelect.SetActive (true);
				SelectSelectedItem();
			} else {
				buttonBuy.SetActive (true);
				buttonSelect.SetActive (false);
			}
		} else {
			buttonBuy.SetActive (false);
			buttonSelect.SetActive (false);
			if (CheckOwned ()) {
				PushNewCustom ();
				//buttonSelect.SetActive (true);
				SelectSelectedItem();
			}
		}
	}

	public void SelectSock(int index){
		lastSelectedItem = 8 + index;
		if (CheckEnoughCoins ()) {
			PushNewCustom ();
			if (CheckOwned ()) {
				buttonBuy.SetActive (false);
				//buttonSelect.SetActive (true);
				SelectSelectedItem();
			} else {
				buttonBuy.SetActive (true);
				buttonSelect.SetActive (false);
			}
		} else {
			buttonBuy.SetActive (false);
			buttonSelect.SetActive (false);
			if (CheckOwned ()) {
				PushNewCustom ();
				//buttonSelect.SetActive (true);
				SelectSelectedItem();
			}
		}
	}

	public void SelectBack(int index){
		lastSelectedItem = 16 + index;
		if (CheckEnoughCoins ()) {
			PushNewCustom ();
			if (CheckOwned ()) {
				buttonBuy.SetActive (false);
				//buttonSelect.SetActive (true);
				SelectSelectedItem();
			} else {
				buttonBuy.SetActive (true);
				buttonSelect.SetActive (false);
			}
		} else {
			buttonBuy.SetActive (false);
			buttonSelect.SetActive (false);
			if (CheckOwned ()) {
				PushNewCustom ();
				//buttonSelect.SetActive (true);
				SelectSelectedItem();
			}
		}
	}

	bool CheckEnoughCoins(){
		bool b = false;
		if (CoinController.Instance.state.availableCoins >= prices [lastSelectedItem]) {
			b = true;
		}
		return b;
	}

	bool CheckOwned(){
		bool b = false;
		if (CoinController.Instance.IsCustomOwned (lastSelectedItem)) {
			b = true;
		}
		return b;
	}

	public void ExitCustom(){

		lastSelectedItem = selectedHat;
		PushNewCustom ();

		lastSelectedItem = 8 + selectedSock;
		PushNewCustom ();

		lastSelectedItem = 16 + selectedBack;
		PushNewCustom ();

		/*if (selectedHat == activeHat) {

		} else {
			lastSelectedItem = selectedHat;
			PushNewCustom ();
		}

		if (selectedSock == activeSock) {

		} else {
			lastSelectedItem = 8 + selectedSock;
			PushNewCustom ();
		}

		if (selectedBack == activeBack) {

		} else {
			lastSelectedItem = 16 + selectedBack;
			PushNewCustom ();
		}*/


		/*if (CoinController.Instance.IsCustomOwned (activeHat)) {

		} else {
			lastSelectedItem = activeStartHat;
			PushNewCustom ();
		}
		if (CoinController.Instance.IsCustomOwned (8 + activeSock)) {

		} else {
			lastSelectedItem = 8 + activeStartSock;
			PushNewCustom ();
		}
		if (CoinController.Instance.IsCustomOwned (16 + activeBack)) {

		} else {
			lastSelectedItem = 16 + activeStartBack;
			PushNewCustom ();
		}*/
	}

	public void BuySelectedItem(){
		CoinController.Instance.UnlockCustom (lastSelectedItem);
		CoinController.Instance.state.availableCoins -= prices [lastSelectedItem];

		txtAvailableCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();
		buttonBuy.SetActive (false);
		RefreshButtonCustomSelect ();
		SelectSelectedItem ();
		//PushNewCustom ();
	}

	public void SelectSelectedItem(){
		if (lastSelectedItem > -1 && lastSelectedItem < 8) {
			selectedHat = lastSelectedItem;
		}else if (lastSelectedItem >= 8 && lastSelectedItem < 16) {
			selectedSock = lastSelectedItem - 8;
		}else if (lastSelectedItem >= 16 && lastSelectedItem < 24) {
			selectedBack = lastSelectedItem - 16;
		}
		RefreshButtonCustomSelect ();
	}

	void PushNewCustom(){
		//Debug.Log ("lastSelectedItem: " + lastSelectedItem);
		if (lastSelectedItem > -1 && lastSelectedItem < 8) {
			CoinController.Instance.DeselectCustom (activeHat);
			activeHat = lastSelectedItem;
		}else if (lastSelectedItem >= 8 && lastSelectedItem < 16) {
			CoinController.Instance.DeselectCustom (8 + activeSock);
			activeSock = lastSelectedItem - 8;
		}else if (lastSelectedItem >= 16 && lastSelectedItem < 24) {
			CoinController.Instance.DeselectCustom (16 + activeBack);
			activeBack = lastSelectedItem - 16;
		}
		CoinController.Instance.SelectCustom (lastSelectedItem);

		cusSelect.Reload ();
	}

	void RefreshButtonCustomSelect(){
		foreach(ButtonColorCustom bcc in bccs){
			int index = bcc.cathegory * 8 + bcc.index;
			//Debug.Log (index);
			bcc.UpdateView(prices[index], (CoinController.Instance.state.availableCoins >= prices[index]),!CoinController.Instance.IsCustomOwned(index),CoinController.Instance.IsCustomSelected(index));
		}
	}
}
