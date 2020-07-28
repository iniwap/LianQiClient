using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class PressUpPlacerDisp : MonoBehaviour {
		public CellObjCtrl _CCtrl;
		public ChessPressUpPlacer _Placer;
		public Animator _Anim;
		public SpriteRenderer _SRRing;
		public float _AlphaReady = 0.1f;
		public float _AlphaPlacing = 0.7f;
		public float _AlphaPlaced = 0.33f;

		private bool _bPressed = false;
		private bool _bEnable = false;
		private bool _bControl = false;

		// Use this for initialization
		void Start () {
			
		}

		private float alpha = 0.0f;
		// Update is called once per frame
		void Update () {
			bool bEnable = (_Placer.enabled == true);
			bool bPress = (_Placer.IsPressed ());
			bool bControl = (_CCtrl._TgtObj != null);

			if (!_bEnable && bEnable) {
				_Anim.SetTrigger ("Enable");
				alpha = 0.0f;
			} else if (!_bPressed && bPress) {
				_Anim.SetTrigger ("StartPlacing");
				alpha = _AlphaPlacing;
			} else if (_bPressed && !bPress) {
				_Anim.SetTrigger ("StopPlacing");
				alpha = _AlphaPlaced;
			} else if (bControl && !_bControl) {
				_Anim.SetTrigger ("Ready");
				alpha = _AlphaReady;
			} 

			Color crNow = _SRRing.color;
			if (bControl) {
				MiroV1ModelSetting setting = 
					_CCtrl._TgtObj.GetComponent<MiroV1ModelSetting> ();
				crNow = setting._colorSetting._ENBG;
			}
			crNow.a = alpha;
			_SRRing.color = crNow;
			//Debug.Log ("Alpha:" + alpha);
				
			_bPressed = bPress;
			_bEnable = bEnable;
			_bControl = bControl;

		}


	}
}
