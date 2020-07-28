using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LineRendererPathFromChildren : MonoBehaviour {
		public Transform _Parent;
		public bool _bClose = true;
		private List<Transform> _chidlTFs = new List<Transform>();

		// Use this for initialization
		void Start () {
			if (_Parent == null) {
				_Parent = transform;
			}
			//_chidlTFs = _Parent.GetComponentsInChildren<Transform> ();
		}
			
		// Update is called once per frame
		void Update () {
			if (_chidlTFs.Count == 0)
				return;
			
			LineRenderer lr = GetComponent<LineRenderer> ();
			lr.useWorldSpace = true;
			if (_bClose) {
				lr.positionCount = _chidlTFs.Count + 1;
			} else {
				lr.positionCount = _chidlTFs.Count;
			}

			for (int i = 0; i < _chidlTFs.Count; i++) {
				lr.SetPosition (i, _chidlTFs[i].position);
			}
			if (_bClose) {
				lr.SetPosition (_chidlTFs.Count, _chidlTFs [0].position);
			}
		}

		[ContextMenu("GetChildTFs")]
		public void GetChildTFs ()
		{
			_chidlTFs.Clear ();
			int childCnt = transform.childCount;
			for (int i = 0; i < childCnt; i++) {
				_chidlTFs.Add (transform.GetChild (i));
			}
		}

		public void ClearTFs()
		{
			_chidlTFs.Clear ();
		}
	}

}