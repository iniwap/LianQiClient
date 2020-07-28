using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
public class BezierCurveFromTFs : MonoBehaviour {
		public BezierCurve _BCurve;
		public List<Transform> _VertexTFs = new List<Transform>();
		public Transform _VertexParent;

		public bool _bClose = true;
		public float _Smooth = 0.1f;
		private float _SmoothPrev = 0.1f;

		[System.Serializable]
		public class EventWithListObj: UnityEvent<List<GameObject> >{};
		public EventWithListObj _SendBPObjs;

		// Use this for initialization
		void Start () {
			CheckAndGetBezierCurve ();

			_SmoothPrev = _Smooth;
		}

		// Update is called once per frame
		void Update () {

			if (_Smooth != _SmoothPrev) {
				UpdateBezierPointHandles ();
			}

		}

		public void SetListTFs(List<Transform> tfs)
		{
			_VertexTFs = tfs;
		}

		[ContextMenu("GetVerticesTFFromParent")]
		public void GetVerticesTFFromParent()
		{
			int cnt = 
				_VertexParent.childCount;

			for (int i = 0; i < cnt; i++) {
				_VertexTFs.Add (_VertexParent.GetChild (i));
			}
		}

		[ContextMenu("GenerateBezierCurve")]
		public void GenerateBezierCurve()
		{
			CheckAndGetBezierCurve ();
			ClearBCurve ();

			foreach (Transform tf in _VertexTFs) {
				_BCurve.AddPointAt (tf.position);
			}

			SendObjs ();

			UpdateBezierPointHandles ();

		}

		void SendObjs ()
		{
			List<GameObject> gbs = new List<GameObject> ();
			BezierPoint[] anchors = _BCurve.GetAnchorPoints ();
			foreach (BezierPoint bp in anchors) {
				gbs.Add (bp.gameObject);
			}
			_SendBPObjs.Invoke (gbs);
		}

		void UpdateBezierPointHandles ()
		{
			BezierPoint[] anchors = _BCurve.GetAnchorPoints ();
			int count = anchors.Length;
			for (int i = 0; i < count; i++) {
				int idPrev = i - 1;
				int idNext = i + 1;
				if (_bClose) {
					if (idPrev < 0) {
						idPrev = count - 1;
					}
					if (idNext > count - 1) {
						idNext = 0;
					}
				}
				else {
					idPrev = Mathf.Clamp (idPrev, 0, count - 1);
					idNext = Mathf.Clamp (idNext, 0, count - 1);
				}
				BezierPoint bpPrev = anchors [idPrev];
				BezierPoint bpThis = anchors [i];
				BezierPoint bpNext = anchors [idNext];
				Vector3 PrevToNext = bpNext.position - bpPrev.position;
				Vector3 handle1 = -PrevToNext * _Smooth * 0.5f;
				Vector3 handle2 = -handle1;
				bpThis.handleStyle = BezierPoint.HandleStyle.Connected;
				bpThis.handle1 = handle1;
				bpThis.handle2 = handle2;
			}
		}

		void CheckAndGetBezierCurve ()
		{
			if (_BCurve == null) {
				_BCurve = GetComponent<BezierCurve> ();
			}
		}

		void ClearBCurve ()
		{
			BezierPoint[] anchors = _BCurve.GetAnchorPoints ();
			foreach (BezierPoint bp in anchors) {
				if (bp != null) {
					DestroyImmediate (bp.gameObject);
				}
				_BCurve.RemovePoint (bp);
			}
		}
}
}