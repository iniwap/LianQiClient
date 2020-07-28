using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class VehicleDescreteCtrl : MonoBehaviour {
		public Transform _TF;
		public float _RotStep=60.0f, _MoveStep=2.0f;
		public float _RotLerpSpd = 1.0f, _MoveLerpSpd = 1.0f;

		private Vector2 _TgtPos;
		private Quaternion _TgtRot;

		public void Start()
		{
			if (_TF == null) {
				_TF = transform;
			}
			_TgtPos = _TF.localPosition;
			_TgtRot = _TF.localRotation;
		}

		public void Update()
		{
			float dt = Time.deltaTime;
			float rlt = dt * _RotLerpSpd;
			float mlt = dt * _MoveLerpSpd;

			//Debug.Log ("MoveStep:" + _MoveStep);

			_TF.localRotation = Quaternion.Lerp (
				_TF.localRotation, _TgtRot, rlt);
			_TF.localPosition = Vector2.Lerp (
				_TF.localPosition, _TgtPos, mlt);
		}

		[ContextMenu("MoveFWD")]
		public void MoveFWD()
		{
			Vector3 Move =  _TgtRot * Vector3.right * _MoveStep;
			_TgtPos += (Vector2)Move;
		}

		[ContextMenu("MoveBWD")]
		public void MoveBWD()
		{
			Vector3 Move = _TgtRot * Vector3.right * (-_MoveStep);
			_TgtPos += (Vector2)Move;
		}

		[ContextMenu("TurnRight")]
		public void TurnRight()
		{
			_TgtRot *= Quaternion.AngleAxis (-_RotStep, Vector3.forward);
		}

		[ContextMenu("TurnLeft")]
		public void TurnLeft()
		{
			_TgtRot *= Quaternion.AngleAxis (_RotStep, Vector3.forward);
		}
	}
}
