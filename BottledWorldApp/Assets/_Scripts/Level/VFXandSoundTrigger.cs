using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXandSoundTrigger : MonoBehaviour {

	public static VFXandSoundTrigger Instance { get; set; }

	[SerializeField] GameObject vfxHeart;
	[SerializeField] GameObject vfxDead;
	[SerializeField] GameObject vfxJump; 
	[SerializeField] GameObject vfxCollect; 
	[SerializeField] GameObject vfxBoost;

	[SerializeField] VFX_UI_slide vfxUISugar;
	[SerializeField] ParticleSystem vfxUISugarBurst;
	[SerializeField] VFX_UI_slide vfxUIHeart;
	[SerializeField] ParticleSystem vfxUIHeartBurst;

	[SerializeField] AudioClip soundclipCollide;
	[SerializeField] AudioClip soundclipCollectSugar; 
	[SerializeField] AudioClip soundclipCollectHeart;
	[SerializeField] AudioClip soundclipJump;
	[SerializeField] AudioClip soundclipLanding;
	[SerializeField] AudioClip soundclipBoost;
	[SerializeField] AudioClip soundclipStart;
	[SerializeField] AudioClip soundclipAntFall;
	[SerializeField] AudioClip soundclipCountdown;
	[SerializeField] AudioClip soundclipCheckpoint;
	[SerializeField] AudioClip soundclipCountUIHeart;
	[SerializeField] AudioClip soundclipCountUISugar;
	[SerializeField] AudioClip soundclipGameOver;
	[SerializeField] AudioClip soundclipLevelEnd;
	[SerializeField] AudioClip soundclipButtonClick;

	[SerializeField] AudioClip music1;

	[SerializeField] Animator animator;
	[SerializeField] Animator animatorStars;

	[SerializeField] Material[] matNormal;
	[SerializeField] Material[] matGhosty;
	[SerializeField] SkinnedMeshRenderer playerMeshRenderer;

	CameraShake camShake;

	//0 = heart; 1 = dead; 2 = jump; 3 = collect; 4 = Boost
	GameObject[] vfx = new GameObject[5];

	void Awake(){
		Instance = this;
	}

	void Start(){
		playerMeshRenderer.materials = matGhosty;

		camShake = Camera.main.GetComponent<CameraShake> ();

		//vfx[0] = Instantiate (vfxHeart, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[0] = vfxHeart;
		vfx[1] = Instantiate (vfxDead, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[2] = Instantiate (vfxJump, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[3] = Instantiate (vfxCollect, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[4] = Instantiate (vfxBoost, new Vector3 (0f, 0f, 0f), Quaternion.identity);
	}


	void PlayPS(int index){
		
		foreach (ParticleSystem ps in vfx [index].transform.GetComponentsInChildren<ParticleSystem> ()) {
			ps.Play ();
		}
	}

	void StopPS(int index){
		foreach (ParticleSystem ps in vfx [index].transform.GetComponentsInChildren<ParticleSystem> ()) {
			ps.Stop ();
		}
	}


	public void TriggerCollect(Transform trans){
		StopPS (3);
		vfx [3].transform.position = trans.position;
		vfx [3].transform.rotation = trans.rotation;
		PlayPS (3);

		vfxUISugar.StartEffect ();
		StartCoroutine (BurstSugar());
		SoundManager.Instance.PlaySingle (soundclipCollectSugar);
		animator.SetTrigger ("collectedSugar");
	}
	IEnumerator BurstSugar(){
		yield return new WaitForSeconds (0.6f);
		vfxUISugarBurst.Play ();
		SoundManager.Instance.PlaySingle (soundclipCountUISugar);
	}

	public void TriggerHeart(){
		StopPS (0);
		//vfx [0].transform.position = trans.position;
		//vfx [0].transform.rotation = trans.rotation;
		PlayPS (0);
		vfx[0].GetComponent<Spiral>().PlayParticles();
		vfxUIHeart.StartEffect ();
		StartCoroutine (BurstHeart());
		SoundManager.Instance.PlaySingle (soundclipCollectHeart);
	}

	IEnumerator BurstHeart(){
		yield return new WaitForSeconds (0.6f);
		vfxUIHeartBurst.Play ();
		SoundManager.Instance.PlaySingle (soundclipCountUIHeart);
	}


	public void TriggerDead(Transform trans){
		StopPS (1);
		vfx [1].transform.position = trans.position;
		vfx [1].transform.rotation = trans.rotation;
		PlayPS (1);

		SoundManager.Instance.PlaySingle (soundclipCollide);

		camShake.shakeDuration = 0.5f;

		animator.SetTrigger ("knockdown");
		animatorStars.SetTrigger ("starsStart");

		playerMeshRenderer.materials = matGhosty;
	}

	public void TriggerAntFalling(){
		SoundManager.Instance.PlaySingleCancelable(soundclipAntFall);
	}


	public void TriggerGameOver(){
		SoundManager.Instance.PlaySingle (soundclipGameOver);
	}

	public void TriggerLevelEnd(){
		SoundManager.Instance.PlaySingle (soundclipLevelEnd);
	}

	public void TriggerJump(Transform trans){
		StopPS (2);
		vfx [2].transform.position = trans.position;
		vfx [2].transform.rotation = trans.rotation;
		PlayPS (2);

		SoundManager.Instance.PlaySingle (soundclipJump);

		animator.SetTrigger ("jumpTrigger");
	}

	public void TriggerJumpEnd(Transform trans){
		StopPS (2);
		vfx [2].transform.position = trans.position;
		vfx [2].transform.rotation = trans.rotation;
		PlayPS (2);

		SoundManager.Instance.PlaySingle (soundclipLanding);

		animator.SetTrigger ("jumpEnding");
	}

	public void TriggerBoost(Transform trans, bool b){
		
		StopPS (4);
		if (!b) {
			animator.SetTrigger ("boostEnd");
			return;
		}
		vfx [4].transform.position = trans.position;
		vfx [4].transform.rotation = trans.rotation;
		vfx [4].transform.parent = trans;
		PlayPS (4);

		SoundManager.Instance.PlaySingle (soundclipBoost);

		animator.SetTrigger ("boostStart");
	}


	public void TriggerStart(){
		
		//SoundManager.Instance.PlaySingle (soundclipStart);
		playerMeshRenderer.materials = matNormal;
	}

	public void TriggerCheckpoint(GameObject checkpointOb){

		checkpointOb.transform.GetChild (0).GetComponent<CheckpointColor> ().ChangeColorChecked ();

		SoundManager.Instance.PlaySingle (soundclipCheckpoint);
	}

	public void StartLevelMusic(){
		SoundManager.Instance.PlayMusic (music1);
	}

	public void EndLevelMusic(){
		SoundManager.Instance.StopMusic ();
	}

	public void TriggerStartCountdown(){

		SoundManager.Instance.PlaySingleCancelable (soundclipCountdown);
		animator.SetTrigger ("standUp");
		animatorStars.SetTrigger ("starsEnd");
	}

	public void TriggerCancelCountdown(){
		SoundManager.Instance.StopSingleCancelable ();

		animator.SetTrigger ("cancelStandUp");
		animatorStars.SetTrigger ("starsStart");

		playerMeshRenderer.materials = matGhosty;
	}

	public void TriggerStartRunning(){

		animator.SetTrigger ("startRunning");
	}


	public void TriggerPause(){

		playerMeshRenderer.materials = matGhosty;
	}

	public void TriggerButtonClick(){
		SoundManager.Instance.PlaySingle (soundclipButtonClick);
	}
}
