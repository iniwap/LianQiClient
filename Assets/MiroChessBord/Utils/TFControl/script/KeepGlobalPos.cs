using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepGlobalPos : MonoBehaviour {

	public Vector3 _GPos;
	public bool _GetPosOnEnable = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = _GPos;
		//print ("Keep Global Pos");

	}

	void OnEnable()
	{
		if (_GetPosOnEnable) {
			_GPos = transform.position;
		}
	}
		
}
