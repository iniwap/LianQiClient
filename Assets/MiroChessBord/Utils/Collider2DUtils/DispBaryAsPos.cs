using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class DispBaryAsPos : MonoBehaviour {
		public BaryCoord _Bary;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			transform.localPosition = _Bary.GetBaryCoordN();
		}
	}
}
