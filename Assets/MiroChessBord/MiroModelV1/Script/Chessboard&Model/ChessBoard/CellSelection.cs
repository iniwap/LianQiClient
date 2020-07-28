using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class CellSelection : MonoBehaviour {

		public CellObjCtrl _cctrl;

		[System.Serializable]
		public class EventWithHexCoord: UnityEvent<HexCoord>{};

		public EventWithHexCoord _Click;
		public EventWithHexCoord _Enter;
		public EventWithHexCoord _Exit;

		public EventWithHexCoord _Select,_Deselect;

		public SpriteRenderer _SPHighlight, _SPSelected;

		public static bool _AllEnable = true;

		private bool _bSelected = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public bool IsSelected()
		{
			return _bSelected;
		}

		void OnMouseDown()
		{
			if (!enabled)
				return;
			if (!_AllEnable) {
				return;
			}
			HexCoord hc = _cctrl.GetComponent<HexCoord> ();
			if (hc != null) {
				//Debug.Log ("Click on: " + gameObject.name);
				_Click.Invoke (hc);
			}
		}

		void OnMouseEnter()
		{
			if (!enabled)
				return;
			if (!_AllEnable) {
				return;
			}
			HexCoord hc = _cctrl.GetComponent<HexCoord> ();
			if (hc != null) {
				//Debug.Log ("Mouse Enter: " + gameObject.name);
				_Enter.Invoke (hc);
			}
		}

		void OnMouseExit()
		{
			if (!enabled)
				return;
			if (!_AllEnable) {
				return;
			}
			HexCoord hc = _cctrl.GetComponent<HexCoord> ();
			if (hc != null) {
				//Debug.Log ("Mouse Exit: " + gameObject.name);
				_Exit.Invoke (hc);
			}
		}


		public void HightlightON()
		{
			_SPHighlight.enabled = true;


		}

		public void HightlightOFF()
		{
			_SPHighlight.enabled = false;

		}
		public void TurnSelection()
		{
			if (_SPSelected.enabled) {
				SelectionOFF ();
			} else {
				SelectionON ();
			}
		}

		public void SelectionON()
		{
			_SPSelected.enabled = true;
			HightlightOFF ();

			_Select.Invoke (_cctrl.GetComponent<HexCoord> ());
			_bSelected = true;
		}

		public void SelectionOFF()
		{
			_SPSelected.enabled = false;
			_Deselect.Invoke (_cctrl.GetComponent<HexCoord> ());
			_bSelected = false;
		}

	}

}