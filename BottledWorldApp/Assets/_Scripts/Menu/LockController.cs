using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LockController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	[SerializeField] int biom;
	[SerializeField] int unlockLevel;
	[SerializeField] Animator lockAnim;
	[SerializeField] ParticleSystem particleSys;
	[SerializeField] GameObject particleSysExplode;
	[SerializeField] AudioClip soundUnlockLock;
	[SerializeField] AudioClip soundUnlockBiom;
	public bool unlockable = false;

	[SerializeField] TextMesh textNeededSugar;
	[SerializeField] HighlightBottle highlightBottleFirstBottle;

	// Use this for initialization
	void Start () {
		particleSys.Stop ();
		if (CoinController.Instance.IsBiomUnlocked (biom)) {
			transform.parent.gameObject.SetActive (false);
		}

		if ((biom-1) * 8 + 8 - CoinController.Instance.state.totalCoins <= 0) {
			textNeededSugar.text = "0";
			unlockable = true;
			ShowParticle ();
		} else {
			textNeededSugar.text = ((biom-1) * 8 + 8 - CoinController.Instance.state.totalCoins).ToString ();
		}
	}

	public void ShowParticle(){
		particleSys.Play ();
	}

	public void UnlockBiom(){
		lockAnim.SetTrigger ("LockOpen");
		SoundManager.Instance.PlaySingle (soundUnlockLock);
		CoinController.Instance.UnlockBiom (biom);
		StartCoroutine (HidePanel ());
		highlightBottleFirstBottle.active = true;
		highlightBottleFirstBottle.SetMaterial ();
		CoinController.Instance.UnlockLevel (unlockLevel);
		CoinController.Instance.Save();
	}

	IEnumerator HidePanel(){
		yield return new WaitForSeconds (0.2f);
		foreach (ParticleSystem ps in particleSysExplode.GetComponentsInChildren<ParticleSystem>()) {
			ps.Play ();
		}
		yield return new WaitForSeconds (1f);
		gameObject.SetActive (false);
		SoundManager.Instance.PlaySingle (soundUnlockBiom);
		yield return new WaitForSeconds (2f);
		transform.parent.gameObject.SetActive (false);
	}

	public void OnPointerDown(PointerEventData data){
		if (unlockable) {
			UnlockBiom ();
		}
	}

	public void OnPointerUp(PointerEventData data){
		
	}
}
