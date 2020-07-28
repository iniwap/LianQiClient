using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class Joint2DCtrl : MonoBehaviour {
		public Joint2D[] _joints;

		[ContextMenu("GetAllJoints2D")]
		void GetAllJoints2D()
		{
			_joints = GetComponentsInChildren<Joint2D> ();
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			foreach (Joint2D jt in _joints) {
				if (jt == null)
					continue;
				jt.enabled = true;
			}
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			foreach (Joint2D jt in _joints) {
				if (jt == null)
					continue;
				jt.enabled = false;
			}
		}


	}
}
