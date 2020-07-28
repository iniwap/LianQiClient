using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SetPosToFixJoint2DTgt : MonoBehaviour {



		public void SetPos()
		{
			FixedJoint2D tj = GetComponent<FixedJoint2D> ();
			Transform tf = tj.connectedBody.transform;
			transform.position = tf.position;
		}
	}
}