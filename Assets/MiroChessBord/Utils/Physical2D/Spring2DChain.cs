using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class Spring2DChain : MonoBehaviour {
		public List<Rigidbody2D> _Anchors = new List<Rigidbody2D> ();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		[ContextMenu("InitSprings")]
		public void InitSprings()
		{
			for (int i = 0; i < _Anchors.Count - 1; i++) {
				Rigidbody2D rbA = _Anchors [i];
				Rigidbody2D rbB = _Anchors [i + 1];

				var sp = rbA.GetComponent<SpringJoint2D> ();
				sp.connectedBody = rbB;
			}
		}

		[ContextMenu("GetAnchorsFromChildRBs")]
		public void GetAnchorsFromChildRBs()
		{
			Rigidbody2D[] rbs = GetComponentsInChildren<Rigidbody2D> ();
			_Anchors = new List<Rigidbody2D> (rbs);
		}

		public void TurnSprings(bool bEnable)
		{
			foreach (var rb in _Anchors) {
				SpringJoint2D sp = rb.GetComponent<SpringJoint2D> ();
				sp.enabled = bEnable;
			}
		}

		public void TurnTargetJoints(bool bEnable)
		{
			for (int i = 0; i < _Anchors.Count-1; i++) {
				var rb = _Anchors [i];
				TargetJoint2D tj = rb.GetComponent<TargetJoint2D> ();
				tj.enabled = bEnable;
			}
		}
		
	}
}