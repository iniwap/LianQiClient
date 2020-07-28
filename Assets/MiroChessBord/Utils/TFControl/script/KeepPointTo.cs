using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPointTo : MonoBehaviour {

	public Transform _Tgt;

	// Update is called once per frame
	void Update () {
		transform.LookAt (_Tgt);
	}
}
