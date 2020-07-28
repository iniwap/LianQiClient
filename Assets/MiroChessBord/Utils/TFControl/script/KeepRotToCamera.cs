using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class KeepRotToCamera : MonoBehaviour {

		public Camera _RefCam;
		public Quaternion _Rot = Quaternion.identity;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {
			if (_RefCam == null) {
				_RefCam = Camera.main;
			}
			Quaternion CamRot = _RefCam.transform.rotation;
			transform.rotation = CamRot * _Rot;
		}

	}
}
