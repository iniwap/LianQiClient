using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ModelCompAccess : MonoBehaviour {

		public List<Transform> _BlackPointTFs = new List<Transform>();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
			
		public Transform GetNearestActiveBlackPointFor(Transform other)
		{
			return _BlackPointTFs [0];
		}
	}
}
