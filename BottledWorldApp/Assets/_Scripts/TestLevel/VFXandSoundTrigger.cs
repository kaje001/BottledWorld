using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXandSoundTrigger : MonoBehaviour {

	public static VFXandSoundTrigger Instance { get; set; }

	[SerializeField] GameObject vfxHeart;
	[SerializeField] GameObject vfxDead;
	[SerializeField] GameObject vfxJump;
	[SerializeField] GameObject vfxCollect;

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

	public void TriggerHeart(){
		//vfx [0].transform.position = trans.position;
		//vfx [0].transform.rotation = trans.rotation;
		//PlayPS (0);
		vfx[0].GetComponent<Spiral>().PlayParticles();
	}


	public void TriggerDead(Transform trans){
		vfx [1].transform.position = trans.position;
		vfx [1].transform.rotation = trans.rotation;
		PlayPS (1);
	}


	public void TriggerJump(Transform trans){
		vfx [2].transform.position = trans.position;
		vfx [2].transform.rotation = trans.rotation;
		PlayPS (2);
	}


	public void TriggerCollect(Transform trans){
		vfx [3].transform.position = trans.position;
		vfx [3].transform.rotation = trans.rotation;
		PlayPS (3);
	}
}
