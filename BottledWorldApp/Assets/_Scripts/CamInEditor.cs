using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] 
public class CamInEditor : MonoBehaviour {

	[SerializeField] Transform camCenter;
	[SerializeField, Range(0,1)] float f;

	void OnValidate(){
		transform.LookAt (camCenter);
		Vector3 pos = Vector3.zero;
		pos.y = f;
		camCenter.localPosition = pos;
	}
}
