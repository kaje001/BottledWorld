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

	[SerializeField] AudioClip soundclipJump;
	[SerializeField] AudioClip soundclipCollide;
	[SerializeField] AudioClip soundclipCollect; 
	[SerializeField] AudioClip soundclipBoost;
	[SerializeField] AudioClip soundclipStart;
	[SerializeField] AudioClip soundclipHeart;

	[SerializeField] AudioClip music1;

	[SerializeField] Animator animator;
	[SerializeField] Animator animatorStars;

	CameraShake camShake;

	//0 = heart; 1 = dead; 2 = jump; 3 = collect; 4 = Boost
	GameObject[] vfx = new GameObject[5];

	void Awake(){
		Instance = this;
	}

	void Start(){
		camShake = Camera.main.GetComponent<CameraShake> ();

		//vfx[0] = Instantiate (vfxHeart, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[0] = vfxHeart;
		vfx[1] = Instantiate (vfxDead, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[2] = Instantiate (vfxJump, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[3] = Instantiate (vfxCollect, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[4] = Instantiate (vfxBoost, new Vector3 (0f, 0f, 0f), Quaternion.identity);
	}

	void Update(){

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

	public void TriggerHeart(){
		StopPS (0);
		//vfx [0].transform.position = trans.position;
		//vfx [0].transform.rotation = trans.rotation;
		PlayPS (0);
		vfx[0].GetComponent<Spiral>().PlayParticles();

		SoundManager.Instance.PlaySingle (soundclipHeart);
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

		SoundManager.Instance.PlaySingle (soundclipCollide);

		animator.SetTrigger ("jumpEnding");
	}


	public void TriggerCollect(Transform trans){
		StopPS (3);
		vfx [3].transform.position = trans.position;
		vfx [3].transform.rotation = trans.rotation;
		PlayPS (3);

		SoundManager.Instance.PlaySingle (soundclipCollect);
		animator.SetTrigger ("collectedSugar");
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
		
		SoundManager.Instance.PlaySingle (soundclipStart);
	}

	public void TriggerCheckpoint(GameObject checkpointOb){

		checkpointOb.transform.GetChild (0).GetComponent<CheckpointColor> ().ChangeColorChecked ();

		SoundManager.Instance.PlaySingle (soundclipStart);
	}

	public void StartLevelMusic(){
		SoundManager.Instance.PlayMusic (music1);
	}

	public void EndLevelMusic(){
		SoundManager.Instance.StopMusic ();
	}

	public void TriggerStartCountdown(){

		animator.SetTrigger ("standUp");
		animatorStars.SetTrigger ("starsEnd");
	}

	public void TriggerCancelCountdown(){

		animator.SetTrigger ("cancelStandUp");
		animatorStars.SetTrigger ("starsStart");
	}

	public void TriggerStartRunning(){

		animator.SetTrigger ("startRunning");
	}

}
