using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	[RequireComponent(typeof(LineRendererPathCtrl))]
	public class LRPathCtrlFromVts : MonoBehaviour {

		private LineRendererPathCtrl _AtrCtrl;
		public List<Transform> _VtTFs = new List<Transform>();
		public bool _bUpdate = true;

		public int _startId = 0,_endId = 0;
		public bool _Global = false;
		public float _ZBias = 0.0f;

		// Use this for initialization
		void Start () {
			_AtrCtrl = GetComponent<LineRendererPathCtrl> ();
		}

		// Update is called once per frame
		void Update () {
			if (_bUpdate) {
				UpdateLRPathCtrlTrace ();
			}

		}

		[ContextMenu("UpdateLRPathCtrlTrace")]
		public void UpdateLRPathCtrlTrace()
		{
			List<Vector3> vts = new List<Vector3> ();

			if (_startId >= 0) {
				for (int i = _startId; i <= _endId; i++) {
					if (_VtTFs [i] == null) {
						return;
					}
					if (_Global) {
						vts.Add (_VtTFs [i].position + new Vector3(0,0,_ZBias));
					} else {
						vts.Add (_VtTFs [i].localPosition + new Vector3(0,0,_ZBias));
					}
				}
			} else {
				foreach (Transform tf in _VtTFs) {
					if (_Global) {
						vts.Add (tf.position+ new Vector3(0,0,_ZBias));
					} else {
						vts.Add (tf.localPosition+ new Vector3(0,0,_ZBias));
					}
				}
			}

			_AtrCtrl.SetTraceVts (vts);
		}

		public void SetVTTFs(List<Transform> tfs)
		{
			_VtTFs = tfs;
		}

	}

}
