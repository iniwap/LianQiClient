using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class ConfigAbsorbingSrcTF : MonoBehaviour {

		public HexGridCtrl _gridCtrl;

		public void ConfigAbsorbingSrcTFs()
		{
			foreach (GameObject cellObj in _gridCtrl._Cells) {
				CellObjCtrl cctrl = 
					cellObj.GetComponent<CellObjCtrl> ();
				ConfigAbsorbingSrcFor (cctrl);
			}
		}

		static void ConfigAbsorbingSrcFor (CellObjCtrl cCtrl)
		{
			int dirFwd = cCtrl.GetDir();
			int dirBwd = (int)Mathf.Repeat ((float)(cCtrl.GetDir() + 3), (float)6);
			if (cCtrl._TgtObj == null) {
				return;
			}
			MiroModelV1 model = cCtrl._TgtObj.GetComponent<MiroModelV1> ();
			HexCoord hc = cCtrl.GetComponent<HexCoord> ();
			Transform tfFwd = hc._Neighbors [dirFwd];
			Transform tfBwd = hc._Neighbors [dirBwd];
			model.SetFwdAbsorbingSrcTF (tfFwd);
			model.SetBwdAbsorbingSrcTF (tfBwd);
		}
	}
}
