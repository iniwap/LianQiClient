using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lyu
{
	public class GetPosOnLRPathCtrol : MonoBehaviour {

		public LineRendererPathCtrl _lrPthCtrl;
		[System.Serializable]
		public class EventWithVector3: UnityEvent<Vector3>{};
		[Range(0,1.0f)]
		public float _T = 0.0f;
		public bool _bSendDir = false;
		public EventWithVector3 _SendPos,_SendDir;
		public float _DirDeltaLen = 0.03f;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_lrPthCtrl == null)
				return;
			
			Vector3 gpos = 
				_lrPthCtrl.GetPosAtT01 (_T,true);
			_SendPos.Invoke (gpos);

			if (_bSendDir) {
				Vector3 pos0 = 
					_lrPthCtrl.GetPosAtT01 (_T - _DirDeltaLen);
				Vector3 pos1 = 
					_lrPthCtrl.GetPosAtT01 (_T + _DirDeltaLen);
				Vector3 dir = (pos1 - pos0).normalized;
				_SendDir.Invoke (dir);
			}
		}
	}
}
