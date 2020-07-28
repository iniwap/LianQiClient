using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class MiroV1DispBase : MonoBehaviour {
		public MorphAnimCtrl _animCtrl;
		public MiroModelV1 _model;
		public bool _bUpdating = true;

		public List<GameObject> _HighlightLRObjs = new List<GameObject> ();
		public GameObject _LRHighlightPrefab;
		public Transform _LRHParent;
		public bool _KeepHighlighting = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_bUpdating) {
				//UpdateDisp ();
			}
		}

		public virtual int GetValue()
		{
			return 0;
		}

		/*
		public void UpdateDisp()
		{
			int value = GetValue ();;

			if (value != _ValuePrev) {
				UpdateCharAnim (hp);

				List<MiroV1BlackDotBase> bdots = _model.GetAliveBlackDots ();
				HighLightHP (bdots);

				_HPPrev = hp;
			}

			SetKeepHighlighting (_KeepHighlighting);

		}

		void UpdateCharAnim (int value)
		{
			char valueChar;
			if (value < 10) {
				valueChar = value.ToString ().ToCharArray () [0];
			}
			else {
				char charA = 'A';
				valueChar = (char)(value - 10 + (int)charA);
			}
			_animCtrl._Char = valueChar;
		}

		void HighlightValue(List<MiroV1WeaponBase> wps)
		{
			if (_HighlightLRObjs.Count < wps.Count) {
				GameObject newLRH = Instantiate (_LRHighlightPrefab) as GameObject;
				newLRH.transform.SetParent (_LRHParent);
				_HighlightLRObjs.Add(newLRH);
			}

			for (int i = 0; i < wps.Count; i++) {
				GameObject LRObj = _HighlightLRObjs [i];
				SetLRPath (wps[i].transform, LRObj);
				PlayHighlightAnim (LRObj, wps[i]._AT);
			}

		}
	*/

	}
}
