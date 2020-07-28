using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1PipeENCtrlBase : MonoBehaviour {

		public int _sp = 1;
		private int _spPrev = 1;

		// Use this for initialization
		void Start () {
			TurnENPumpBySP ();
		}
		
		// Update is called once per frame
		void Update () {
			if (_spPrev != _sp) {
				TurnENPumpBySP ();
				_spPrev = _sp;
			}
			
		}

		virtual public void TurnENPumpBySP ()
		{
			
		}
	}
}
