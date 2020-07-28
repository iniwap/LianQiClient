using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1HPDisp : MonoBehaviour {
		public MorphAnimCtrl _animCtrl;
		public MiroModelV1 _model;
		public LineRenderer _LineRendererBG;
		public bool _bUpdating = true;

		private int _HPPrev = -1;
		public UnityEvent _HPChanged;
		public List<GameObject> _HighlightLRObjs = new List<GameObject> ();
		public GameObject _LRHighlightPrefab;
		public Transform _LRHParent;
		public bool _KeepHighlighting = false;
		private bool _KeepHighlightingPrev = false;
		public UnityEvent _KeepHighlightingON, _KeepHighlightingOFF;
		public float _KeepHighlightingTime = 15.0f;



		// Use this for initialization
		void Start () {
			SetLineRenderersColor ();

		}
		
		// Update is called once per frame
		void Update () {
			if (_bUpdating) {
				UpdateDisp ();
			}
		}

		public void UpdateDisp()
		{
			int hp = 
				_model.GetDispHP();
			
			if (hp != _HPPrev) {
				UpdateCharAnim (hp);

				MiroV1TimeToInvoke timer = GetComponent<MiroV1TimeToInvoke> ();
				timer._LeftTime = 0.7f;
				_HPChanged.Invoke ();

				_HPPrev = hp;
			}

			if (_KeepHighlightingPrev != _KeepHighlighting) {
				SetKeepHighlighting (_KeepHighlighting);
			}
			_KeepHighlightingPrev = _KeepHighlighting;
		}

		public void StartHighLightHP()
		{
			List<MiroV1BlackDotBase> bdots = _model.GetAliveBlackDots ();
			HighLightHP (bdots);

			TurnONKeepHighlighting ();
		}

		[ContextMenu("SetLineRenderersColor")]
		void SetLineRenderersColor ()
		{
			if (_LineRendererBG != null) {
				MiroV1ModelSetting modelSet = _model.GetComponent<MiroV1ModelSetting> ();
				Color cr = modelSet._colorSetting._ENBG;
				LineRendererSetting.SetColor (_LineRendererBG, cr);
			}
		}

		//bool _ToHighlightHP = false;
		void HighLightHP(List<MiroV1BlackDotBase> bdots)
		{
			if (_HighlightLRObjs.Count < bdots.Count) {
				int cnt = bdots.Count - _HighlightLRObjs.Count;
				for (int i = 0; i < cnt; i++) {
					GameObject newLRH = Instantiate (_LRHighlightPrefab) as GameObject;
					newLRH.transform.SetParent (_LRHParent);
					_HighlightLRObjs.Add (newLRH);
				}
			} else if (_HighlightLRObjs.Count > bdots.Count) {
				for (int i = _HighlightLRObjs.Count - 1; i > bdots.Count - 1; i--) {
					_HighlightLRObjs [i].SetActive (false);
				}
			}

			for (int i = 0; i < bdots.Count; i++) {
				if (i >= _HighlightLRObjs.Count) {
					break;
				}
				GameObject LRObj = _HighlightLRObjs [i];
				LRObj.SetActive (true);
				SetLRPath (bdots[i], LRObj);
				PlayHighlightAnim (LRObj);
			}

		}

		void SetLRPath (MiroV1BlackDotBase bdot, GameObject LRObj)
		{
			LRPathCtrlFromVts lrpthCtrl = LRObj.GetComponent<LRPathCtrlFromVts> ();
			List<Transform> tfs = new List<Transform> ();
			tfs.Add (bdot.transform);
			tfs.Add (transform);
			lrpthCtrl.SetVTTFs (tfs);
		}

		static void PlayHighlightAnim (GameObject LRObj)
		{
			Animator anim = LRObj.GetComponent<Animator> ();
			anim.SetTrigger ("Highlight");
			anim.SetBool ("Keep", true);
		}

		void UpdateCharAnim (int hp)
		{
			char hpchar;
			if (hp < 10) {
				hpchar = hp.ToString ().ToCharArray () [0];
			}
			else {
				char charA = 'A';
				hpchar = (char)(hp - 10 + (int)charA);
			}
			_animCtrl._Char = hpchar;
		}

		void SetKeepHighlighting (bool bKeep)
		{
			foreach (GameObject LRObj in _HighlightLRObjs) {
				if (LRObj.activeInHierarchy) {
					Animator anim = LRObj.GetComponent<Animator> ();
					anim.SetBool ("Keep", bKeep);
				}
			}
		}

		public void TurnONKeepHighlighting()
		{
			_KeepHighlighting = true;

			MiroTimeToInvoke timer = GetComponent<MiroTimeToInvoke> ();
			if (timer == null) {
				timer = gameObject.AddComponent<MiroTimeToInvoke> ();
			}
			timer._LeftTime = _KeepHighlightingTime;
			timer._Happen.AddListener (this.TurnOFFKeepHighlighting);
		}

		public void TurnOFFKeepHighlighting()
		{
			_KeepHighlighting = false;
		}




	}
}
