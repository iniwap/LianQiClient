using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Die : MonoBehaviour {

		[ContextMenu("Die")]
		public void Die()
		{
			Destroy (gameObject);
		}
	}
}
