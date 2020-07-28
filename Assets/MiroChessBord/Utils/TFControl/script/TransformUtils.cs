using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class TransformUtils : MonoBehaviour {

		public static void SetGlobalScale(Transform tf, Vector3 globalScale)
		{
			tf.localScale = Vector3.one;
			tf.localScale = new Vector3 (
				globalScale.x / tf.lossyScale.x, 
				globalScale.y / tf.lossyScale.y, 
				globalScale.z / tf.lossyScale.z);

		}
	}
}
