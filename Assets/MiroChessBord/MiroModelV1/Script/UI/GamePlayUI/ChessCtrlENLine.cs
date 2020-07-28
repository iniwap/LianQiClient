using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ChessCtrlENLine : MonoBehaviour {

		public MiroV1PlacementMgr _Mgr;
		public Transform _CtrTF;
		private Transform _TgtTF = null;
		private int _Dir = -1;
		public RectTransform[] _6DirRTFs;

		public float _NoiseSpd = 1.0f;
		public float _NoisePwr = 1.0f;
		public float _Sat = 1.0f;

		public LineRenderer _L0,_L1;

		// Use this for initialization
		void Start () {
			
		}



		// Update is called once per frame
		void Update () {
			if (_TgtTF == null || _Dir<0 || _CtrTF==null) {
				_L0.enabled = false;
				_L1.enabled = false;
				return;
			}
			_L0.enabled = true;
			_L1.enabled = true;

			// Get start and end pos
			Vector3 SrcPos = RectTFPosToGlobalPos (_6DirRTFs [_Dir].position);
			Vector3 CtrPos = RectTFPosToGlobalPos (_CtrTF.position);
			Vector3 TgtPos = _TgtTF.position;

			// set path
			SetLRPath(_L0,SrcPos,CtrPos);
			SetLRPath (_L1, CtrPos, TgtPos);

			// set color
			GameObject Miro = _Mgr.GetSelectedMiroPrefab ();
			var setting = 
				Miro.GetComponent<MiroV1ModelSetting> ();
			Color cr = setting._colorSetting._ENMax;
			SetLRColor (_L0, cr);
			SetLRColor (_L1, cr);


		}

		private Vector3 RectTFPosToGlobalPos(Vector3 rtfPos)
		{
			Ray ray = Camera.main.ScreenPointToRay (rtfPos);
			Vector3 SrcPos = ray.GetPoint (0.1f);
			return SrcPos;
		}

		private void SetLRPath(LineRenderer LR,Vector3 SrcPos, Vector3 TgtPos)
		{
			float t = Time.realtimeSinceStartup;
			int pcnt = LR.positionCount;
			float step = 1.0f / (float)(pcnt-1);
			for (int i = 0; i < pcnt; i++) {
				Vector3 pos = Vector3.Lerp (SrcPos, TgtPos, i * step);
				if (i > 0 || i < pcnt-1) {
					float dx = _NoisePwr *( Mathf.PerlinNoise (
						pos.x + _NoiseSpd * t, 100.0f * pos.y)-0.5f);
					float dy = _NoisePwr *( Mathf.PerlinNoise (
						pos.x + 1.23434f + _NoiseSpd * t, 108.12345f * pos.y)-0.5f);
					pos += new Vector3 (dx, dy, 0);
				}
				LR.SetPosition (i, pos);
			}
		}

		private void SetLRColor(LineRenderer LR, Color Cr)
		{
			LR.startColor = Cr;
			LR.endColor = Cr;
		}


		public void SetTgtMiro(MiroModelV1 miro)
		{
			_TgtTF = miro.transform;

			if (_Dir >= 0) {
				StartShakingDir (_Dir);
			}
		}

		public void SetTgtMiroTF(Transform tf)
		{
			_TgtTF = tf;
			if (_Dir >= 0) {
				StartShakingDir (_Dir);
			}
		}

		public void ClearTgtMiro()
		{
			_TgtTF = null;
			StopShakingDir (_Dir);
		}

		public void SetDir(int dir)
		{
			if(dir!=_Dir )
			{
				StartShakingDir (dir);
			}
			if (_Dir >= 0) {
				StopShakingDir (_Dir);
			}
			_Dir = dir;
		}

		private void StartShakingDir(int dir)
		{
			var ns = 
				_6DirRTFs [dir].GetComponent<Lyu.NoiseScaleFromBase> ();
			ns.enabled = true;
		}

		private void StopShakingDir(int dir)
		{
			var ns = 
				_6DirRTFs [dir].GetComponent<Lyu.NoiseScaleFromBase> ();
			ns.enabled = false;
		}

		public void ResetTgtTF()
		{
			_TgtTF = null;
			for (int i = 0; i < 6; i++) {
				StopShakingDir (i);
			}
		}

		public void SetNoisePwr(float npwr)
		{
			_NoisePwr = npwr;
		}

		public void SetSat(float sat)
		{
			_Sat = sat;
		}

	}
}
