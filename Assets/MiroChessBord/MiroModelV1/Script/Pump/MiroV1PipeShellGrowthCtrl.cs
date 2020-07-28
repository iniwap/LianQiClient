using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	[System.Serializable]
	public class MiroEventWithListVts: UnityEvent< List<Vector3> >{}

	public class MiroV1PipeShellGrowthCtrl : MonoBehaviour {

		public TraceCurve _Bone = new TraceCurve ();
		public AnimationCurve _WdGrow = new AnimationCurve();
		[Range(0,1.0f)]
		public float _GrowProgress = 0.0f;
		private float _GrowProgressPrev = 0.0f;

		public MiroEventWithListVts _ListVts;
		public int _VtsCount = 8;

		// Use this for initialization
		void Start () {
			UpdateLineRendererGrowthProgress ();
		}
		
		// Update is called once per frame
		void Update () {
			if (_GrowProgressPrev != _GrowProgress) {
				UpdateLineRendererGrowthProgress ();
				_GrowProgressPrev = _GrowProgress;
			}
			
		}

		[ContextMenu("GenListVts")]
		public void GenListVts()
		{
			List<Vector3> LstVts = new List<Vector3> ();
			for (int i = 0; i < _VtsCount; i++) {
				float t = (float)i / (float)(_VtsCount - 1);
				t *= _GrowProgress;
				Vector3 vt = (Vector3)_Bone.Evaluate (t);
				LstVts.Add (vt);
			}
			_ListVts.Invoke (LstVts);
		}

		[ContextMenu("UpdateLineRendererGrowthProgress")]
		void UpdateLineRendererGrowthProgress ()
		{
			UpdateLineRendererPositions ();
			UpdateLineRendererWidthMultiplier ();
		}


		[ContextMenu("UpdateLineRendererPositions")]
		public void UpdateLineRendererPositions()
		{
			LineRenderer lr = GetComponent<LineRenderer> ();
			int numPos = lr.positionCount;

			float length = _Bone._TotalLength;
			float step = length * _GrowProgress / (float)numPos;

			for (int i = 0; i < numPos; i++) {
				float t = i * step;
				Vector4 pos = _Bone.Evaluate (t);
				lr.SetPosition (i, (Vector3)pos);
			}

			GenListVts ();
		}
			
		[ContextMenu("UpdateLineRendererWidthMultiplier")]
		public void UpdateLineRendererWidthMultiplier()
		{
			float wd = _WdGrow.Evaluate (_GrowProgress);

			LineRenderer lr = GetComponent<LineRenderer> ();
			lr.widthMultiplier = wd;
		}
	}
}
