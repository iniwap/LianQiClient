using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1PipeShellWidthStartCtrl : MonoBehaviour {

		public MiroV1PipeShellCtrl _pipeShellCtrl;
		public List<float> _Width = new List<float>();

		public float _SPF = 0;
		public float _LerpSpd = 5.0f;
		public float _UpdateThres = 0.01f;

		[Range(0,9)]
		public int _SP=0;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {

			float sp = (float)_SP;
			float deltaSP = Mathf.Abs ( _SPF - sp);

			bool bUpdate = false;
			if (deltaSP > _UpdateThres) {
				_SPF = Mathf.Lerp (_SPF, sp, Time.deltaTime * _LerpSpd);
				bUpdate = true;
			} else if (
				deltaSP > 0.0f && deltaSP <=_UpdateThres) {
				_SPF = sp;
				bUpdate = true;
			} 

			if (bUpdate) {
				UpdateWidthMultiplier ();
			}

		}

		public void UpdateWidthMultiplier()
		{
			int sp0 = Mathf.FloorToInt (_SPF);
			int sp1 = Mathf.CeilToInt (_SPF);
			float wdm0 = _Width [sp0];
			float wdm1 = _Width [sp1];
			float t = _SPF - sp0;
			float wd = Mathf.Lerp (wdm0, wdm1, t);

			_pipeShellCtrl._WdStart = wd;
		}
	}
}