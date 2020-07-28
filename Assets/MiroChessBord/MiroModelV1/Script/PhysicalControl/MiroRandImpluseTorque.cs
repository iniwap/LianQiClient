using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroRandImpluseTorque : MonoBehaviour {

		public float _Power = 1.0f;

		public void RandTorque()
		{
			Rigidbody2D rb = GetComponent<Rigidbody2D> ();
			rb.AddTorque (Random.value * _Power,ForceMode2D.Impulse);
		}
	}
}
