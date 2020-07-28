using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	static public class MiroPlayingStatus
	{

		static public bool bPlaying = true;

		static public void TurnPlaying(bool bPlay)
		{
			bPlaying = bPlay;
		}



	}
}
