using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class MiroV1AntellerFromVts : MonoBehaviour {
		public MiroV1AntellerCtrl _AtrCtrl;
		public List<Transform> _VtTFs = new List<Transform>();
		public bool _bUpdate = true;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_bUpdate) {
				UpdateAtrCtrlTrace ();
			}

		}

		[ContextMenu("UpdateAtrCtrlTrace")]
		public void UpdateAtrCtrlTrace()
		{
			List<Vector3> vts = new List<Vector3> ();
			foreach (Transform tf in _VtTFs) {
				vts.Add (tf.position);
			}
			_AtrCtrl.SetTraceVts (vts, true);
		}

	}
}
