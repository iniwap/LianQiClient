using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroPlayingStatusCtrl : MonoBehaviour {

		public void TurnONPlaying()
		{
			MiroPlayingStatus.TurnPlaying (true);
		}

		public void TurnOFFPlaying()
		{
			MiroPlayingStatus.TurnPlaying (false);
		}

	}
}
