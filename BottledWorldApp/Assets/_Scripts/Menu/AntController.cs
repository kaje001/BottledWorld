using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AntController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler{

	[SerializeField] float lerpSpeedDrag = 15f*20;
	[SerializeField] float lerpSpeedMove = 0.2f*20;
	public MenuController menuControll;
	public LayerMask layMask;

	Vector3 resetPos;
	Vector3 targetPos;
	float lerpSpeed;
	public bool isDraged = false;
	public bool isDrop = false;
	GameObject overObject = null;

	float timeNextPosition = 0;

	[SerializeField] Animator animator;

	// Use this for initialization
	void Start () {
		targetPos = transform.position;
		lerpSpeed = lerpSpeedMove;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDraged && Time.time > timeNextPosition) {
			targetPos = GetRandomPos ();
			timeNextPosition = Time.time + Random.Range (5, 50) / 10f;
			lerpSpeed = lerpSpeedMove;

			isDrop = false;
		}


		if (isDrop) {
			isDraged = false;
		}


		if (isDraged || isDrop) {
			transform.position = Vector3.Lerp (transform.position, targetPos, lerpSpeed * Time.deltaTime);
		} else {
			transform.position = Vector3.MoveTowards (transform.position, targetPos, lerpSpeed * Time.deltaTime);
			if (transform.position - targetPos != Vector3.zero) {
				transform.GetChild (0).rotation = Quaternion.Lerp (transform.GetChild (0).rotation, Quaternion.LookRotation (transform.position - targetPos), 3f * Time.deltaTime);
				animator.SetBool ("walking", true);
				animator.SetBool ("winken", false);
			} else {
				
				float angleToWorld = transform.GetChild (0).localEulerAngles.y;
				if (angleToWorld < 300f && angleToWorld > 240f) {
					//Debug.Log ("Winken");
					animator.SetBool ("winken", true);
				}
				animator.SetBool ("walking", false);
			}
		}

	}

	public void OnPointerDown(PointerEventData data){
		if (menuControll.custom) {
			return;
		}
		menuControll.draged = true;
		//Debug.Log ("Start Drag");
		resetPos = transform.position;
		targetPos = resetPos;
		isDraged = true;
		isDrop = false;
		lerpSpeed = lerpSpeedDrag;

		animator.SetBool ("draged", true);
	}



	public void OnDrag(PointerEventData data){
		if (menuControll.custom) {
			return;
		}
		Vector3 touchPointinWorld = Camera.main.ScreenToWorldPoint (new Vector3 (data.position.x, data.position.y+50f, 24f));

		//transform.position = Vector3.Lerp (transform.position, touchPointinWorld, Time.deltaTime);
		targetPos = touchPointinWorld;

		Ray ray = Camera.main.ScreenPointToRay(new Vector3 (data.position.x, data.position.y+40f, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo, 100f, layMask)) {
			if (hitInfo.collider.gameObject.tag == "Obstacle") {
				if (overObject != null) {
					menuControll.OverObjectLeave (overObject);
					overObject = null;
				}
			}else if (overObject == null) {
				overObject = hitInfo.collider.gameObject;
				menuControll.OverObject (overObject);
			}
		}else{
			if (overObject != null) {
				menuControll.OverObjectLeave (overObject);
				overObject = null;
			}
		}
	}


	public void OnPointerUp(PointerEventData data){
		if (menuControll.custom) {
			return;
		}

		menuControll.draged = false;
		targetPos = resetPos;
		isDrop = true;
		timeNextPosition = Time.time + 2f;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3 (data.position.x, data.position.y+40f, 0f));
		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo, 100f, layMask)) {

			menuControll.CheckActivatedObject (hitInfo.collider.gameObject);
		}

		animator.SetBool ("draged", false);
		animator.SetBool ("walking", false);
	}

	Vector3 GetRandomPos(){
		Vector3 newPos = new Vector3(Random.Range(100,240)/10f-2f,-6.16f,Random.Range(0,250)/10f-5f);

		return newPos;
	}

}
