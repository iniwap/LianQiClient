using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1AidBlackDots : MonoBehaviour {

		public List<MiroV1BlackDot> _AidDots = new List<MiroV1BlackDot>();

		public MiroV1BlackDot GetBlackDot(int dir)
		{
			return _AidDots [dir];
		}

		public int GetBlackDotDir(MiroV1BlackDot bdot)
		{
			int dir = -1;
			for (int i=0; i < 6; i++) {
				if (_AidDots [dir] == bdot) {
					dir = i;
				}
			}
			return dir;
		}
	}
}
