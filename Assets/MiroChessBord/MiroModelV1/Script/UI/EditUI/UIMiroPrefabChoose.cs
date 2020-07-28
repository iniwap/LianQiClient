using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class UIMiroPrefabChoose : MonoBehaviour {

		public MiroV1PlacementMgr _Mgr;
		public Text _TxtPrefabName;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void ChoosePrefab(float id)
		{
			_Mgr.SetMiroPrefabID( (int)id );
			_TxtPrefabName.text = _Mgr.GetSelectedMiroPrefabCampName();
		}

	}
}