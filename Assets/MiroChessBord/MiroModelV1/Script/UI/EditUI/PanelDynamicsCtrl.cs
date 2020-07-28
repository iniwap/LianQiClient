using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class PanelDynamicsCtrl : MonoBehaviour {

		public List<Toggle> _Toggles = new List<Toggle>();

		public void TurnToggles(bool bON)
		{
			foreach (Toggle tg in _Toggles) {
				tg.isOn = bON;
			}
		}

		[ContextMenu("GetChildToggles")]
		public void GetChildToggles ()
		{
			Toggle[] tgs = GetComponentsInChildren<Toggle> ();
			_Toggles = new List<Toggle> (tgs);
		}

	}
}
