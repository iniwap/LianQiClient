using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CellDirDisp : MonoBehaviour {

		public CellObjCtrl _cellObjCtrl;
		public Quaternion _BaseRot;
		public Vector3 _RotAxis;
		public float _LerpSpd = 5.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {

			//_cellObjCtrl._Dir

			float rotDegs = 
				_cellObjCtrl.GetDir() * 60.0f;
			Quaternion rot = _BaseRot * Quaternion.AngleAxis (rotDegs, _RotAxis);

			Quaternion rotNow = transform.localRotation;

			float dt = Time.deltaTime;
			Quaternion rot2 = Quaternion.Lerp (rotNow, rot, dt * _LerpSpd);
			transform.localRotation = rot2;
		}
	}
}
