using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class SpringJoint2DCtrl : MonoBehaviour {

		public SpringJoint2D [] _springJnts;

		[ContextMenu("GetAllSpringJoint2D")]
		public void GetAllSpringJoint2D()
		{
			_springJnts = 
				GetComponentsInChildren<SpringJoint2D> ();
		}

		[ContextMenu("TurnONAutoConfidDist")]
		public void TurnONAutoConfidDist()
		{
			foreach (SpringJoint2D sjt in _springJnts) {
				if (sjt == null)
					continue;
				sjt.autoConfigureDistance = true;
			}
		}

		[ContextMenu("TurnOFFAutoConfidDist")]
		public void TurnOFFAutoConfidDist()
		{
			foreach (SpringJoint2D sjt in _springJnts) {
				if (sjt == null)
					continue;
				sjt.autoConfigureDistance = false;
			}
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			foreach (SpringJoint2D sjt in _springJnts) {
				if (sjt == null)
					continue;
				sjt.enabled = true;
			}
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			foreach (SpringJoint2D sjt in _springJnts) {
				if (sjt == null)
					continue;
				sjt.enabled = false;
			}
		}

	}
}

