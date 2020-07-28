using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CircleCollider2DCtrl : MonoBehaviour {

		public CircleCollider2D[] _circClds;

		[ContextMenu("GetCircColliders")]
		public void GetCircColliders()
		{
			_circClds = GetComponentsInChildren<CircleCollider2D> ();
		}



	}
}
