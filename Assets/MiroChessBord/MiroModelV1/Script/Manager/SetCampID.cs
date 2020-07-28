using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class SetCampID : MonoBehaviour {

		public void SetID0()
		{
			SetID (0);
		}

		public void SetID1()
		{
			SetID (1);
		}

		public void SetID2()
		{
			SetID (2);
		}

		public void SetID3()
		{
			SetID (3);
		}

		private bool SetID(int id)
		{
			MiroV1PlacementMgr mgr = GetComponent<MiroV1PlacementMgr> ();
			int maxCnt = mgr._MiroPrefabs.Count;
			if (id >= maxCnt) {
				return false;
			}
			mgr.SetMiroPrefabID (id);

			return true;
		}

	}
}
