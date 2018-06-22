using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour {

	GameObject sphere;
	[SerializeField] float sphereSpeed = 1.2f;
	[SerializeField] float boostLenth = 0.8f;
	float startSphereSpeed;
	Vector3 startSpherePos;

	//For the Jump
	float maxJumpHeight = 0.14f;
	float jumpSpeed = 2.0f;
	float fallSpeed = 0.23f;
	bool inputJump = false;
	float curJumpHight = 0;

	IEnumerator coroutineJump;


	void Start(){
		sphere = gameObject;
		startSphereSpeed = sphereSpeed;
		startSpherePos = sphere.transform.position;
	}

	void Update(){
		Vector3 curPos = sphere.transform.position;

		Vector3 pos = startSpherePos;
		pos = pos + sphere.transform.up.normalized * curJumpHight * 9;
		pos.z = curPos.z;

		float forwardMovement = sphereSpeed * Time.deltaTime;
		sphere.transform.position = new Vector3 (pos.x, pos.y, curPos.z - forwardMovement);
	}

	public void SphereJump(){
		inputJump = true;
		//Debug.Log ("test");

		coroutineJump = Jump();
		StartCoroutine (coroutineJump);
	}

	public void SetSphereSpeed(float newSpeed){
		sphereSpeed = newSpeed;
	}

	public void ResetSphereSpeed(){
		sphereSpeed = startSphereSpeed;
	}

	public void SphereBoost(){
		StartCoroutine ("Boost");
	}

	IEnumerator Jump ()
	{
		while (true) {
			Debug.Log (curJumpHight);
			if (curJumpHight >= maxJumpHeight - 0.05f) {
				inputJump = false;
			}
			if (inputJump) {
				curJumpHight = Mathf.Lerp (curJumpHight, maxJumpHeight, jumpSpeed * Time.deltaTime);
			}
			else if (!inputJump) {
				curJumpHight = curJumpHight - fallSpeed * Time.deltaTime;
				//Debug.Log (curJumpHight);
				if (curJumpHight <= 0) {
					curJumpHight = 0;

					yield break;
				}
			}

			yield return null;
		}
	}

	IEnumerator Boost ()
	{
		sphereSpeed = sphereSpeed*2f;
		yield return new WaitForSeconds (boostLenth);

		ResetSphereSpeed ();
	}

}
