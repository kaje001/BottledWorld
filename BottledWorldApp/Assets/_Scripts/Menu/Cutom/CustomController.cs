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

	[SerializeField] GameObject panelReallyBuy;

	int activeHat = 0;
	int activeSock = 0;
	int activeBack = 0;
	int activeColor = 0;

	int selectedHat = 0;
	int selectedSock = 0;
	int selectedBack = 0;
	int selectedColor = 0;

	/*int activeStartHat = 0;
	int activeStartSock = 0;
	int activeStartBack = 0;
	int activeStartColor = 0;*/

	int lastSelectedItem = -1;

	int selectedCustoms = 0;

	// Use this for initialization
	void Start () {
		ActivateSelectedObjects ();
		/*activeStartHat = activeHat;
		activeStartSock = activeSock;
		activeStartBack = activeBack;
		activeStartColor = activeColor;*/
		lastSelectedItem = -1;
		buttonBuy.SetActive (false);
		panelReallyBuy.SetActive (false);

		txtAvailableCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();

		//Debug.Log (bccs.Length);

		RefreshButtonCustomSelect ();
	}

	public void CustomOpen(){
		ActivateSelectedObjects ();
		lastSelectedItem = -1;
		buttonBuy.SetActive (false);
		panelReallyBuy.SetActive (false);
		txtAvailableCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();

		RefreshButtonCustomSelect ();
	}

	void ActivateSelectedObjects(){
		int i = 0;

		for (i = 0; i < 8; i++) {
			
			if (CoinController.Instance.IsCustomEquipped (i)) {
				activeHat = i;
				selectedHat = i;
				SelectCustom (i);
			}
		}

		for (; i < 16; i++) {
			if (CoinController.Instance.IsCustomEquipped (i)) {
				activeSock = i;
				selectedSock = i;
				SelectCustom (i);
			}
		}

		for (; i < 24; i++) {
			if (CoinController.Instance.IsCustomEquipped (i)) {
				activeBack = i;
				selectedBack = i;
				SelectCustom (i);
			}
		}

		for (; i < 32; i++) {
			if (CoinController.Instance.IsCustomEquipped (i)) {
				activeColor = i;
				selectedColor = i;
				SelectCustom (i);
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
			if (CheckOwned ()) {
				buttonBuy.SetActive (false);
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
			} else {
				PushNewCustom ();
				buttonBuy.SetActive (true);
				buttonSelect.SetActive (false);
			}
		} else {
			buttonBuy.SetActive (false);
			buttonSelect.SetActive (false);
			if (CheckOwned ()) {
				//PushNewCustom ();
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
			}
		}
	}

	public void SelectSock(int index){
		lastSelectedItem = 8 + index;
		if (CheckEnoughCoins ()) {
			if (CheckOwned ()) {
				buttonBuy.SetActive (false);
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
			} else {
				PushNewCustom ();
				buttonBuy.SetActive (true);
				buttonSelect.SetActive (false);
			}
		} else {
			buttonBuy.SetActive (false);
			buttonSelect.SetActive (false);
			if (CheckOwned ()) {
				//PushNewCustom ();
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
			}
		}
	}

	public void SelectBack(int index){
		lastSelectedItem = 16 + index;
		if (CheckEnoughCoins ()) {
			if (CheckOwned ()) {
				buttonBuy.SetActive (false);
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
			} else {
				PushNewCustom ();
				buttonBuy.SetActive (true);
				buttonSelect.SetActive (false);
			}
		} else {
			buttonBuy.SetActive (false);
			buttonSelect.SetActive (false);
			if (CheckOwned ()) {
				//PushNewCustom ();
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
			}
		}
	}


	public void SelectColor(int index){
		lastSelectedItem = 24 + index;
		if (CheckEnoughCoins ()) {
			if (CheckOwned ()) {
				buttonBuy.SetActive (false);
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
			} else {
				PushNewCustom ();
				buttonBuy.SetActive (true);
				buttonSelect.SetActive (false);
			}
		} else {
			buttonBuy.SetActive (false);
			buttonSelect.SetActive (false);
			if (CheckOwned ()) {
				//PushNewCustom ();
				//buttonSelect.SetActive (true);
				EquipSelectedItem();
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

		lastSelectedItem = selectedSock;
		PushNewCustom ();

		lastSelectedItem = selectedBack;
		PushNewCustom ();

		lastSelectedItem = selectedColor;
		PushNewCustom ();

		cusSelect.Reload ();

	}

	public void ShowReallyBuy(){
		panelReallyBuy.SetActive (true);
	}

	public void HideReallyBuy(){
		panelReallyBuy.SetActive (false);

	}

	public void BuySelectedItem(){
		if (CoinController.Instance.IsCustomOwned (lastSelectedItem)) {
			buttonBuy.SetActive (false);
			HideReallyBuy ();
			return;
		}
		CoinController.Instance.UnlockCustom (lastSelectedItem);
		CoinController.Instance.state.availableCoins -= prices [lastSelectedItem];

		txtAvailableCoins.text = "x " + CoinController.Instance.state.availableCoins.ToString ();
		buttonBuy.SetActive (false);
		RefreshButtonCustomSelect ();
		EquipSelectedItem ();
		HideReallyBuy ();
		//PushNewCustom ();
	}

	public void EquipSelectedItem(){
		if (lastSelectedItem > -1 && lastSelectedItem < 8) {
			CoinController.Instance.UnequipCustom (selectedHat);
			selectedHat = lastSelectedItem;

		}else if (lastSelectedItem >= 8 && lastSelectedItem < 16) {
			CoinController.Instance.UnequipCustom (selectedSock);
			selectedSock = lastSelectedItem;

		}else if (lastSelectedItem >= 16 && lastSelectedItem < 24) {
			CoinController.Instance.UnequipCustom (selectedBack);
			selectedBack = lastSelectedItem;

		}else if (lastSelectedItem >= 24) {
			CoinController.Instance.UnequipCustom (selectedColor);
			selectedColor = lastSelectedItem;

		}
		PushNewCustom ();
		CoinController.Instance.EquipCustom (lastSelectedItem);
		RefreshButtonCustomSelect ();
	}

	void PushNewCustom(){
		//Debug.Log ("lastSelectedItem: " + lastSelectedItem);
		if (lastSelectedItem > -1 && lastSelectedItem < 8) {
			DeselectCustom (activeHat);
			activeHat = lastSelectedItem;
		}else if (lastSelectedItem >= 8 && lastSelectedItem < 16) {
			DeselectCustom (activeSock);
			activeSock = lastSelectedItem ;
		}else if (lastSelectedItem >= 16 && lastSelectedItem < 24) {
			DeselectCustom (activeBack);
			activeBack = lastSelectedItem;
		}else if (lastSelectedItem >= 24) {
			DeselectCustom (activeColor);
			activeColor = lastSelectedItem;
		}
		SelectCustom (lastSelectedItem);

		cusSelect.ShowSelectedObjects (selectedCustoms);
	}

	void RefreshButtonCustomSelect(){
		foreach(ButtonColorCustom bcc in bccs){
			int index = bcc.cathegory * 8 + bcc.index;
			//Debug.Log (index);
			//Debug.Log(index + ": " + CoinController.Instance.IsCustomEquipped(index));
			bcc.UpdateView(prices[index], (CoinController.Instance.state.availableCoins >= prices[index]),!CoinController.Instance.IsCustomOwned(index), CoinController.Instance.IsCustomEquipped(index));
		}
	}



	//________________________________________________________________test__________________________________
	bool IsCustomSelected(int index){
		return (selectedCustoms & (1 << index)) != 0;
	}


	void SelectCustom(int index){
		selectedCustoms |= 1 << index;
	}


	void DeselectCustom(int index){
		selectedCustoms ^= 1 << index;
	}
}
