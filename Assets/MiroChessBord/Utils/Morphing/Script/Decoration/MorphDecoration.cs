using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class MorphDecoration : MonoBehaviour {
		public MorphAnimCtrl _MAnimCtrl;
		[System.Serializable]
		public class EventWithInt: UnityEvent<int>{};
		public EventWithInt _CharIndexChanged;

		private int _CharIdPrev = 0;


		// Use this for initialization
		void Start () {
			_CharIdPrev = (int)_MAnimCtrl._Char;
		}
		
		// Update is called once per frame
		void Update () {
			int _CharId = (int)_MAnimCtrl._Char;

			if (_CharIdPrev != _CharId) {
				_CharIndexChanged.Invoke ((int)_CharId);
			}
			_CharIdPrev = _CharId;
		}
	}
}
