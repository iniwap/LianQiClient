using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class Rigidbody2DsMgr : MonoBehaviour {

		public Rigidbody2D[] _Rigidbodys;

		[ContextMenu("GetChildRigidbody2Ds")]
		public void GetChildRigidbody2Ds ()
		{
			_Rigidbodys = GetComponentsInChildren<Rigidbody2D> ();
		}

		[ContextMenu("TurnToDynamic")]
		public void TurnToDynamic()
		{
			ChangeType (RigidbodyType2D.Dynamic);
		}

		[ContextMenu("TurnToKinematic")]
		public void TurnToKinematic()
		{
			ChangeType (RigidbodyType2D.Kinematic);
		}

		[ContextMenu("TurnToStatic")]
		public void TurnToStatic()
		{
			ChangeType (RigidbodyType2D.Static);
		}

		public void ChangeType(RigidbodyType2D type)
		{
			foreach (Rigidbody2D rb in _Rigidbodys) {
				rb.bodyType = type;
			}
		}








	}
}
