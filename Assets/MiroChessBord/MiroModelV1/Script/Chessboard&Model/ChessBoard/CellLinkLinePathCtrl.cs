using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class CellLinkLinePathCtrl : MonoBehaviour {
		public LineRendererPathCtrl _LRPathCtrl;
		public TwoObjectLink _TwoObj;
		public float _ZBias = 10.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			Vector3 posa = _TwoObj.GetA().transform.position +
				_ZBias * Vector3.forward;
			Vector3 posb = _TwoObj.GetB().transform.position + 
				_ZBias * Vector3.forward;

			//Vector3 posal = transform.InverseTransformPoint (posa);
			//Vector3 posbl = transform.InverseTransformPoint (posb);

			List<Vector3> vts = new List<Vector3> ();
			vts.Add (posa);
			vts.Add (posb);

			_LRPathCtrl.SetTraceVts (vts);
			
		}
	}
}
