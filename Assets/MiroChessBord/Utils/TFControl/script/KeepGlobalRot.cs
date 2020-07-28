using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class KeepGlobalRot : MonoBehaviour {

		public Quaternion _Rot = Quaternion.identity;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			transform.rotation = _Rot;
			
		}
	}
}