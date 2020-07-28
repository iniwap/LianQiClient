using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class AddSpringToChildren : MonoBehaviour {

		public float _Frequence = 1.0f,_Damp = 1.0f;

		[ContextMenu("AddSprings")]
		public void AddSprings()
		{
			Rigidbody2D rbMe = GetComponent<Rigidbody2D> ();
			Rigidbody2D [] rbs = GetComponentsInChildren<Rigidbody2D> ();
			foreach (Rigidbody2D rb in rbs) {
				SpringJoint2D sp = 
					rb.gameObject.AddComponent<SpringJoint2D> ();
				sp.connectedBody = rbMe;
				sp.frequency = _Frequence;
				sp.dampingRatio = _Damp;
			}
		}
	}
}

