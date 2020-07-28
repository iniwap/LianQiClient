using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1BlackDotMgr : MonoBehaviour {
		[System.Serializable]
		public class EventWithBlackDot: UnityEvent<MiroV1BlackDotBase>{};

		public EventWithBlackDot _NewBlackDot;

		public void NewBlackDot(MiroV1BlackDotBase bkDot)
		{
			_NewBlackDot.Invoke (bkDot);
		}
	}
}
