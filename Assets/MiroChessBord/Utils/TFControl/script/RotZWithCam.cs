using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class RotZWithCam : MonoBehaviour {

		public Camera _Cam;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_Cam == null) {
				return;
			}
			Vector3 rotEulerCam = 
				_Cam.transform.rotation.eulerAngles;
			Vector3 rotEulerMe 
				= transform.rotation.eulerAngles;
			rotEulerMe.z = -rotEulerCam.z;
			Quaternion rot = Quaternion.Euler (rotEulerMe);
			transform.rotation = rot;
		}
	}
}
