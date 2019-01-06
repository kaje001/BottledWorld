using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHandler : MonoBehaviour {

	[SerializeField] Transform[] spawnerSphere;
	[SerializeField] GameObject spherePrefab;
	[SerializeField] GameObject movingObjectsParent;
	[SerializeField] float sphereSize = 0.35f;
	[SerializeField] float spawnTime = 5.5f;

	void Start(){

		StartCoroutine (SphereSpawnRoutine ());
	}

	void OnTriggerEnter(Collider other){
		Destroy (other.gameObject);
	}

	IEnumerator SphereSpawnRoutine(){
		while (true) {

			yield return new WaitForSeconds (spawnTime);
			foreach (Transform t in spawnerSphere) {
				GameObject ob = Instantiate (spherePrefab, movingObjectsParent.transform);
				ob.transform.position = t.position;
				ob.transform.rotation = t.rotation;
				ob.transform.localScale *= sphereSize;
			}

		}
	}
}
