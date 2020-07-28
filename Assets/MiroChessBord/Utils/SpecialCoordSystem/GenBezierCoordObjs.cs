using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class GenBezierCoordObjs : MonoBehaviour {
		public BezierCurve _BCurve;
		public float _AlongStart = 0.0f, _AlongStep = 0.05f;
		public float _ShiftStart = 0.5f, _ShiftStep = 0.0f;
		public float _DepthStart = 0.0f, _DepthStep = 0.0f;
		public int _count = 20;


		public List<BezierCurveCoord> _BCoords = new List<BezierCurveCoord> ();
		public GameObject _Prefab;

		public UnityEvent _Generated;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void SetCount(int cnt)
		{
			_count = cnt;
		}

		public void SetAlongStep(float astep)
		{
			_AlongStep = astep;
		}

		[ContextMenu("GenerateObjs")]
		public void GenerateObjs()
		{
			//_AlongStep = 1.0f / (float)_count;
			for (int i = 0; i < _count; i++) {
				GameObject newObj = Instantiate (_Prefab,Vector3.zero,Quaternion.identity) as GameObject;
				newObj.transform.SetParent (transform);
				BezierCurveCoord bcd = newObj.AddComponent<BezierCurveCoord> ();
				bcd._bCurve = _BCurve;
				bcd._Along = _AlongStart + _AlongStep * i;
				bcd._Shift = _ShiftStart + _ShiftStep * i;
				bcd._Depth = _DepthStart + _DepthStep * i;
				_BCoords.Add (bcd);
			}
				
			_Generated.Invoke ();
		}

		[ContextMenu("ClearObjs")]
		private void ClearObjsImmediate()
		{
			foreach (BezierCurveCoord bcd in _BCoords) {
				DestroyImmediate (bcd.gameObject);
			}
			_BCoords.Clear ();
		}

		public void DisableBezierCoordsUpdating()
		{
			foreach (BezierCurveCoord bcc in _BCoords) {
				bcc._UpdatingPos = false;
			}
		}

		public void ClearObjs()
		{
			foreach (BezierCurveCoord bcd in _BCoords) {
				Destroy (bcd.gameObject);
			}
			_BCoords.Clear ();
		}

	}
}
