using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class BanDirDisp : MonoBehaviour {
		public MiroV1PlacementMgr _MiroMgr;
		public ChessPlacerUI _PlacerUI;
		public int _Dir = 0;
		public Color _BanColor;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void UpdateDisp()
		{
			bool bBanned = _PlacerUI._BanDirs [_Dir];

			GameObject miroPref = 
				_MiroMgr.GetSelectedMiroPrefab ();
			var setting = miroPref.GetComponent<MiroV1ModelSetting> ();

			Image img= GetComponent<Image> ();
			if (bBanned) {
				img.color = _BanColor;
			} else {
				img.color = setting._colorSetting._ENMax;
			}
		}

		public void TurnState()
		{
			_PlacerUI._BanDirs [_Dir] = !_PlacerUI._BanDirs [_Dir];
		}
	}
}
