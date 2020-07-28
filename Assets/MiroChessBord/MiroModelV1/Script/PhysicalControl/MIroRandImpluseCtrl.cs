using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MIroRandImpluseCtrl : MonoBehaviour {

		public MiroRandImpluse[] _randImpluses;

		public float _Power = 1.0f;
		public bool _bUpdating = false;

		public void Update()
		{
			if (_bUpdating) {
				foreach (MiroRandImpluse imp in _randImpluses) {
					imp._Power = _Power;
				}
			}
		}

		[ContextMenu("GetAllRandImpluses")]
		public void GetAllRandImpluses()
		{
			_randImpluses = GetComponentsInChildren<MiroRandImpluse> ();
		}




	}
}
