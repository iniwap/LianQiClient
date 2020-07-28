using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class MiroRandImpluse : MonoBehaviour {
		public float _Power = 1.0f;

		[ContextMenu("RandImpluse")]
		public void RandImpluse()
		{
			Rigidbody2D rb = GetComponent<Rigidbody2D> ();
			rb.AddForce (Random.insideUnitCircle * _Power, ForceMode2D.Impulse);
		}
	}
}
