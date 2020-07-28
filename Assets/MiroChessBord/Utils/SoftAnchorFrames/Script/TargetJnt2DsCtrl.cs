using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class TargetJnt2DsCtrl : MonoBehaviour {

		public TargetJoint2D[] _tgtJnts;

		[ContextMenu("GetAllTargetJoint2Ds")]
		public void GetAllTargetJoint2Ds()
		{
			_tgtJnts = GetComponentsInChildren<TargetJoint2D> ();
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			foreach (TargetJoint2D jnt in _tgtJnts) {
				jnt.enabled = true;
			}
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			foreach (TargetJoint2D jnt in _tgtJnts) {
				jnt.enabled = false;
			}
		}

	}
}
