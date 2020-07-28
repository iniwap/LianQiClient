using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class HexCoord : MonoBehaviour {

		public Hex _hex;

		public Dictionary<int,Transform> _Neighbors = new Dictionary<int,Transform>();

	}
}
