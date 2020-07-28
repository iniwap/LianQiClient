using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class CellDoubleClickSensor : MonoBehaviour {

		public float _TimeIntervalMax = 0.25f;

		//public UnityEvent _DoubleClicked;

		public CellObjCtrl _cellCtrl;
		public HexGridCtrl _gridCtrl;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		bool _bPressed = false;
		float _PressTime = float.NegativeInfinity;
		void OnMouseDown()
		{
			if (!MiroPlayingStatus.bPlaying) {
				return;
			}

			if (enabled) {
				if (!_bPressed) {
					_bPressed = true;
					_PressTime = Time.realtimeSinceStartup;

				} else {
					_bPressed = false;
					float t = Time.realtimeSinceStartup;
					float dt = t - _PressTime;
					if (dt <= _TimeIntervalMax) {
						//_DoubleClicked.Invoke ();
						_gridCtrl.PlacementChanged (_cellCtrl);
					}
				}
			}
		}
	}
}
