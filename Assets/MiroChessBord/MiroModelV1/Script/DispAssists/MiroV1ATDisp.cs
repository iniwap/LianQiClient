using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class MiroV1ATDisp : MonoBehaviour {
		public MorphAnimCtrl _animCtrl;
		public MiroModelV1 _model;
		public bool _bUpdating = true;

		private int _ATPrev = -1;
		public List<GameObject> _HighlightLRObjs = new List<GameObject> ();
		public GameObject _LRHighlightPrefab;
		public Transform _LRHParent;
		public bool _KeepHighlighting = false;
		public AnimationCurve _EdgeHaloScaleOnAT;
		public GameObject _EdgeObj;
		public LineRenderer[] _StrokeLineRenderers = new LineRenderer[]{};
		public AnimationCurve _StrokeWidthOnAT;
		public Transform _CharTF;
		public AnimationCurve _CharScaleOnAT;
		//public Transform _EdgeHalo;


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_bUpdating) {
				UpdateDisp ();
			}
		}

		public void UpdateDisp()
		{
			int at = _model.GetDispAT ();

			if (at != _ATPrev) {
				UpdateCharAnim (at);

				List<MiroV1WeaponBase> wps = _model.GetAttackingWeapons ();
				HighlightAT (wps);

				float lrWd = _StrokeWidthOnAT.Evaluate (at);
				foreach (LineRenderer lr in _StrokeLineRenderers) {
					lr.widthMultiplier = lrWd;
				}

				float scl = _CharScaleOnAT.Evaluate (at);
				_CharTF.localScale = new Vector3 (scl, scl, 1.0f);

				float escl = _EdgeHaloScaleOnAT.Evaluate (at);
				_EdgeObj.transform.localScale = new Vector3 (escl, escl, 1.0f);

				_ATPrev = at;
			}

			SpriteColorCtrl scolorCtrl = 
				_EdgeObj.GetComponent<SpriteColorCtrl> ();
			if (at == 0) {
				scolorCtrl._id = 0;
				_animCtrl.gameObject.SetActive (false);
			} else {
				scolorCtrl._id = 1;
				_animCtrl.gameObject.SetActive (true);
			}

			SetLineRenderersColor ();

			SetKeepHighlighting (_KeepHighlighting);
		}

		[ContextMenu("SetLineRenderersColor")]
		void SetLineRenderersColor ()
		{
			MiroV1ModelSetting modelSet = _model.GetComponent<MiroV1ModelSetting> ();
			Color cr = modelSet._colorSetting._ENMax;

			foreach (LineRenderer lr in _StrokeLineRenderers) {
				LineRendererSetting.SetColor (lr, cr);
			}
		}

		void HighlightAT(List<MiroV1WeaponBase> wps)
		{
			if (_HighlightLRObjs.Count < wps.Count) {
				GameObject newLRH = Instantiate (_LRHighlightPrefab) as GameObject;
				newLRH.transform.SetParent (_LRHParent);
				_HighlightLRObjs.Add (newLRH);
			} else if (_HighlightLRObjs.Count > wps.Count) {
				int overflowCount = _HighlightLRObjs.Count - wps.Count;
				_HighlightLRObjs.RemoveRange (
					_HighlightLRObjs.Count - overflowCount, overflowCount);
			}

			int minCount = Mathf.Min (wps.Count, _HighlightLRObjs.Count);
			for (int i = 0; i < minCount; i++) {
				GameObject LRObj = _HighlightLRObjs [i];
				SetLRPath (wps[i].transform, LRObj);
				PlayHighlightAnim (LRObj, wps[i]._AT);
			}

		}

		void SetLRPath (Transform tfDot, GameObject LRObj)
		{
			LRPathCtrlFromVts lrpthCtrl = LRObj.GetComponent<LRPathCtrlFromVts> ();
			List<Transform> tfs = new List<Transform> ();
			tfs.Add (tfDot);
			tfs.Add (transform);
			lrpthCtrl.SetVTTFs (tfs);
		}

		static void PlayHighlightAnim (GameObject LRObj,int at)
		{
			if (!LRObj.activeInHierarchy)
				return;
			Animator anim = LRObj.GetComponent<Animator> ();
			anim.SetTrigger ("Highlight");
			anim.SetInteger ("AT", at);
		}

		void UpdateCharAnim (int at)
		{
			char atchar;
			if (at < 10) {
				atchar = at.ToString ().ToCharArray () [0];
			}
			else {
				char charA = 'A';
				atchar = (char)(at - 10 + (int)charA);
			}
			_animCtrl._Char = atchar;
		}

		void SetKeepHighlighting ( bool bKeep)
		{
			foreach (GameObject LRObj in _HighlightLRObjs) {
				if (!LRObj.activeInHierarchy) {
					continue;
				}
				Animator anim = LRObj.GetComponent<Animator> ();
				anim.SetBool ("Keep", bKeep);
			}
		}
	}
}
