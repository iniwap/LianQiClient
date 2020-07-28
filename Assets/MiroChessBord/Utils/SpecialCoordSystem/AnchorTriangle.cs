using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class AnchorTriangle : MonoBehaviour {
		[System.Serializable]
		public class Triangle
		{
			public Transform _A,_B,_C;
		}
		public List<Triangle> _Trs;
	}
}
