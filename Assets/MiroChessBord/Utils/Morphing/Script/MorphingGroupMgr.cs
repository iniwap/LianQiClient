using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class MorphingGroupMgr : MonoBehaviour {
		public MorphingGroup _TgtMorphGroup;
		public List<MorphingGroup> _morphGroups = 
			new List<MorphingGroup>();
		public int _SelectId = 0;
		private int _SelectIdPrev = -1;

		// Update is called once per frame
		void Update () {

			if (_SelectId != _SelectIdPrev && 
				_SelectId>=0 && 
				_SelectId<_morphGroups.Count) {
				_TgtMorphGroup.LoadFromAnother (_morphGroups [_SelectId]);
				_SelectIdPrev = _SelectId;
			}
			
		}

		public void SetSelectedId(float id)
		{
			_SelectId = (int)id;
		}
	}
}
