using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CLinkAttackLR : MonoBehaviour {

		public TwoObjectLink _TwoLink;

		public LineRenderer _LRAB, _LRBA;

		public AnimationCurve _WdOnAT;
		public float _WdMult = 10.0f;

		public float _LerpSpd = 3.0f;

		public float _BiasAmt01 = 0.5f;
		public float _ZBias = 10.0f;
		public float _ZBiasOnWD = 0.05f;

		public float _NoiseSpd = 1.0f;
		public float _NoiseAmt = 0.5f;


		private float _ATAF = 0.0f;
		private float _WdA = 0.0f;


		private float _ATBF = 0.0f;
		private float _WdB = 0.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			bool bA2B, bB2A;

			// get comps
			CellObjCtrl ctrlA = 
				_TwoLink._A.GetComponent<CellObjCtrl> ();
			MiroModelV1 modelA = 
				GetModelFromCellObj (_TwoLink._A);
			MiroV1ModelSetting modelSettingA = null;
			if (modelA != null) {
				modelSettingA = 
					modelA.GetComponent<MiroV1ModelSetting> ();
			}
			
			MiroModelV1 modelB = 
				GetModelFromCellObj (_TwoLink._B);
			CellObjCtrl ctrlB = 
				_TwoLink._B.GetComponent<CellObjCtrl> ();
			MiroV1ModelSetting modelSettingB = null;
			if (modelB != null) {
				modelSettingB = 
					modelB.GetComponent<MiroV1ModelSetting> ();
			}

			// check direction
			bA2B = IsXPointToY (ctrlA, ctrlB);
			bB2A = IsXPointToY (ctrlB, ctrlA);
			bool bBias = bA2B && bB2A;

			// compute at
			float dt = Time.deltaTime;
			int ata = 0;
			if (modelA != null) {
				ata = bA2B ? modelA.GetAT () : 0;
			}
			_ATAF = Mathf.Lerp (_ATAF, ata, dt * _LerpSpd);
			int atb = 0;
			if(modelB!=null)
			{
				atb = bB2A? modelB.GetAT ():0;
			}
			float lerpT = dt * _LerpSpd; 
			_ATBF = Mathf.Lerp (_ATBF, atb, lerpT);

			// get Width 
			float wdA = _WdOnAT.Evaluate (_ATAF) * _WdMult;
			float wdB = _WdOnAT.Evaluate (_ATBF) * _WdMult;
			if (!bA2B) {
				wdA = 0.0f;
			}
			if (!bB2A) {
				wdB = 0.0f;
			}
			_WdA = Mathf.Lerp (_WdA, wdA, lerpT);
			_WdB = Mathf.Lerp (_WdB, wdB, lerpT);

			// config LR
			Transform tfa, tfb;
			tfa = _TwoLink._A.transform;
			tfb = _TwoLink._B.transform;

			ConfigLR (_LRAB,tfa,tfb, modelSettingA, _ATAF, _WdA, bBias);
			ConfigLR (_LRBA,tfa,tfb, modelSettingB, _ATBF, -_WdB, bBias);

		}

		private void ConfigLR (
			LineRenderer lr, 
			Transform akrA, 
			Transform akrB, 
			MiroV1ModelSetting modelASetting,
			float at,
			float wd,
			bool bBias)
		{
			float wwd = wd;
			// set line width
			if (modelASetting == null) {
				wd = 0.0f;
			}
			wd = Mathf.Abs (wd);
			lr.widthMultiplier = wd;
			if (wd == 0.0f) {
				return;
			}

			// set line path
			int ptCnt = lr.positionCount;
			float step = 1.0f / (float)(ptCnt-1);
			Vector3 Bias = Vector2.zero;
			if (bBias) {
				Vector3 posA = akrA.position;
				Vector3 posB = akrB.position;
				Vector3 AB = posB - posA;
				Vector3 AB01 = AB.normalized;
				float deg = 90.0f;
				if (wwd < 0) {
					deg = -90.0f;
				}
				Bias = Quaternion.AngleAxis (deg, Vector3.forward) *
					AB01 * (wd * _BiasAmt01);
			}

			float TNow = _NoiseSpd * Time.realtimeSinceStartup;
			for (int i = 0; i < ptCnt; i++) {
				
				float t = i * step;
				Vector3 posA = akrA.position;
				Vector3 posB = akrB.position;
				Vector3 p = Vector3.Lerp (posA, posB, t) + Bias + _ZBias * Vector3.forward;

				if (i > 0) {
					Vector3 posPrev = lr.GetPosition (i - 1);
					Vector3 prevToThis01 = (p - posPrev).normalized;
					Vector3 NDir = Quaternion.AngleAxis (90.0f, Vector3.forward) * prevToThis01;
					Vector3 NoiseBias =_NoiseAmt * NDir * 
						(Mathf.PerlinNoise (p.x+TNow, p.y-TNow)-0.5f);
					p += NoiseBias;
					p += _ZBiasOnWD * wd * Vector3.forward;
				}
					
				lr.SetPosition (i, p);
			}

			// set line color 
			lr.startColor = modelASetting._colorSetting._ENBG;
			lr.endColor = modelASetting._colorSetting._ENBG;

		}

		private MiroModelV1 GetModelFromCellObj(GameObject Obj)
		{
			MiroModelV1 model = null;
			if (Obj == null) {
				return model;
			}

			CellObjCtrl cctrl = 
				Obj.GetComponent<CellObjCtrl> ();

			if (cctrl._TgtObj == null) {
				return model;
			}

			model = cctrl._TgtObj.GetComponent<MiroModelV1> ();
			return model;
		}

		private bool IsXPointToY(CellObjCtrl X,CellObjCtrl Y)
		{
			if (X == null || Y == null) {
				return false;
			}

			HexCoord HA = X.GetComponent<HexCoord> ();
			HexCoord HB = Y.GetComponent<HexCoord> ();

			if (HA == null || HB == null) {
				return false;
			}

			if (X._TgtObj == null) {
				return false;
			}

			Transform ANbrTf = HA._Neighbors [X.GetDir()];
			if (ANbrTf == null) {
				return false;
			}
			CellObjCtrl NbrCtrl = ANbrTf.GetComponent<CellObjCtrl> ();
			if (NbrCtrl == Y) {
				return true;
			} else {
				return false;
			}

		}
	}


}
