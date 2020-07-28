using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class ChessPressUpPlacer : MonoBehaviour {

		public MiroV1PlacementMgr _mgr;
		public CellObjCtrl _cctrl;

		[System.Serializable]
		public class EventTF: UnityEvent<Transform>{}

		public EventTF _Press,_StartDrag;
		public UnityEvent _Up;

		void OnEnable()
		{
			//print ("Enable!" + name);
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		private bool _bPressed = false;
		private bool _bReadyToDrag = false;

		void OnMouseDown()
		{
			if (!MiroPlayingStatus.bPlaying) {
				return;
			}

			if (CheckBlock ()) {
				return;
			}

			if (enabled) {
				_bReadyToDrag = true;
				_bPressed = true;
				_Press.Invoke (_cctrl.transform);
			}

		}

		void OnMouseDrag()
		{

			if (CheckBlock ()) {
				return;
			}

			if (enabled && _bReadyToDrag) {
				//Spawn ();
				_StartDrag.Invoke(_cctrl.transform);
				_bReadyToDrag = false;
			}
		}

		void OnMouseUp()
		{
			if (!MiroPlayingStatus.bPlaying) {
				return;
			}

			if (CheckBlock ()) {
				return;
			}
				
			if (enabled) {
				_bPressed = false;
				//Debug.Log (_cctrl.name + " OnMouseUp");
				_Up.Invoke();
			}
		}

		private bool CheckBlock()
		{
			CellObjCtrl ctrl = GetComponentInParent<CellObjCtrl> ();
			return ctrl.IsBlocked ();
		}

		public void ResetPressed()
		{
			_bPressed = false;
		}

		private void Spawn()
		{
			//Debug.Log (_cctrl.name + " OnMouseDown");
			_mgr.AddBirthTF (_cctrl.transform, true);
			_mgr.Spawn();
			_mgr.ClearBirthTFs ();
		}

		public bool IsPressed()
		{
			return _bPressed;
		}


	}
}