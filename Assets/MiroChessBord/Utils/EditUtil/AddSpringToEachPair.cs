using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class AddSpringToEachPair : MonoBehaviour {

		public Rigidbody2D [] _rigids;
		public float _dampingRatio = 0.0f;
		public float _frequence = 1.0f;
		public UnityEvent _GetRigidbody2Ds;

		[ContextMenu("GetAllRigid2D")]
		public void GetAllRigid2D()
		{
			_rigids = GetComponentsInChildren<Rigidbody2D> ();
			_GetRigidbody2Ds.Invoke ();
		}

		[ContextMenu("UpdateSprings")]
		public void UpdateSprings()
		{
			foreach (Rigidbody2D rba in _rigids) {
				SpringJoint2D [] sjts = 
					rba.gameObject.GetComponents<SpringJoint2D> ();
				foreach (SpringJoint2D sjt in sjts) {
					DestroyImmediate (sjt);
				}
				foreach (Rigidbody2D rbb in _rigids) {
					if (rba != rbb) {
						SpringJoint2D sjt = 
							rba.gameObject.AddComponent<SpringJoint2D> ();
						sjt.connectedBody = rbb;
						sjt.frequency = _frequence;
						sjt.dampingRatio = _dampingRatio;
					}
				}
			}
		}
		
	}
}