using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXandSoundTrigger : MonoBehaviour {

	public static VFXandSoundTrigger Instance { get; set; }

	[SerializeField] GameObject vfxHeart;
	[SerializeField] GameObject vfxDead;
	[SerializeField] GameObject vfxJump;
	[SerializeField] GameObject vfxCollect;

	[SerializeField] AudioClip soundclipJump;
	[SerializeField] AudioClip soundclipCollide;
	[SerializeField] AudioClip soundclipCollect;
	[SerializeField] AudioClip soundclipStart;

	//0 = heart; 1 = dead; 2 = jump; 3 = collect 
	GameObject[] vfx = new GameObject[4];

	void Awake(){
		Instance = this;
	}

	void Start(){

		//vfx[0] = Instantiate (vfxHeart, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[0] = vfxHeart;
		vfx[1] = Instantiate (vfxDead, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[2] = Instantiate (vfxJump, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		vfx[3] = Instantiate (vfxCollect, new Vector3 (0f, 0f, 0f), Quaternion.identity);
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
		//vfx [0].transform.position = trans.position;
		//vfx [0].transform.rotation = trans.rotation;
		//PlayPS (0);
		vfx[0].GetComponent<Spiral>().PlayParticles();
	}


	public void TriggerDead(Transform trans){
		StopPS (1);
		vfx [1].transform.position = trans.position;
		vfx [1].transform.rotation = trans.rotation;
		PlayPS (1);

		SoundManager.Instance.PlaySingle (soundclipCollide);
	}


	public void TriggerJump(Transform trans){
		StopPS (2);
		vfx [2].transform.position = trans.position;
		vfx [2].transform.rotation = trans.rotation;
		PlayPS (2);

		SoundManager.Instance.PlaySingle (soundclipJump);
	}


	public void TriggerCollect(Transform trans){
		StopPS (3);
		vfx [3].transform.position = trans.position;
		vfx [3].transform.rotation = trans.rotation;
		PlayPS (3);

		SoundManager.Instance.PlaySingle (soundclipCollect);
	}

	public void TriggerStart(){
		
		SoundManager.Instance.PlaySingle (soundclipStart);
	}

	public void TriggerCheckpoint(GameObject checkpointOb){

		checkpointOb.transform.GetChild (0).GetComponent<CheckpointColor> ().ChangeColorChecked ();

		SoundManager.Instance.PlaySingle (soundclipStart);
	}
}
