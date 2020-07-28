using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class KeepGlobalScale : MonoBehaviour {
		public Vector3 _GScale;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			//Lyu.TransformUtils.SetGlobalScale (transform, _GScale);
			Transform tfParent = transform.parent;
			transform.SetParent (null);
			transform.localScale = _GScale;
			transform.SetParent (tfParent);
		}
	}
}
