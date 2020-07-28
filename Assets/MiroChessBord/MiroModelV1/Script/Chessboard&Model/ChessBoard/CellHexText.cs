using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CellHexText : MonoBehaviour {

		public CellObjCtrl _CCtrl;
		public TextMesh _TxtMsh;
		public bool _Updating = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_Updating) {
				UpdateTextMesh ();
			}
		}

		public void UpdateTextMesh()
		{
			HexCoord hc = _CCtrl.GetComponent<HexCoord> ();
			string txt = hc._hex.CoordString ();
			_TxtMsh.text = txt;
		}

	}
}
