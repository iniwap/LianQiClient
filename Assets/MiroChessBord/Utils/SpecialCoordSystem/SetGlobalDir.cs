using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lyu
{
	public class SetGlobalDir : MonoBehaviour {

		public Vector3 _RotBias = Vector3.zero;

		public void SetDir(Vector3 dir)
		{
			Quaternion rotBias = Quaternion.Euler (_RotBias);

			float angle = Vector3.Angle (dir, Vector3.right);
			Quaternion rotDir = Quaternion.AngleAxis (angle,Vector3.forward);

			transform.rotation = rotDir * rotBias;
		}
	}
}