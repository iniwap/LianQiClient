using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1BulletsOnOff : MonoBehaviour {

		[System.Serializable]
		public class ATCtrlGameObj
		{
			public int _AT;
			public GameObject _Obj;
		}
			
		public List<GameObject> _AllObjs = new List<GameObject>();
		public List<ATCtrlGameObj> _CtrlObj = new List<ATCtrlGameObj>();
		public int _AT = 0;
		private int _ATPrev = 0;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_ATPrev != _AT) {
				TurnPS ();
			}

			_ATPrev = _AT;
		}

		[ContextMenu("TurnPS")]
		public void TurnPS()
		{

			foreach (GameObject gb in _AllObjs) {
				gb.SetActive (false);
			}

			bool bTurned = false;
			foreach (ATCtrlGameObj ctrl in _CtrlObj) {
				if (ctrl._AT == _AT) {
					ctrl._Obj.SetActive (true);
					bTurned = true;
				}
			}

		}

		[ContextMenu("ATUp")]
		public void ATUp()
		{
			_AT++;
			_AT = Mathf.Clamp (_AT, 0, 10);
		}

		[ContextMenu("ATDown")]
		public void ATDown()
		{
			_AT--;
			_AT = Mathf.Clamp (_AT, 0, 10);
		}

	}
}