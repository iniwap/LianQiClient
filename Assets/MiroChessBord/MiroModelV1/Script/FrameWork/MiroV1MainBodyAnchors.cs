using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1MainBodyAnchors : MonoBehaviour {

		public Transform Ctr;
		private List<List<Transform> > _TFS;
		public List<Transform> F, FR, BR, B, BL, FL;

		public Transform GetAnchor(Direction dir,Distance dist)
		{
			if (dist == Distance.CENTER) {
				return Ctr;
			}
			int x = (int)dir;
			int y = (int)dist;

			if (_TFS == null) {
				GetTFArray ();
			}
				
			return _TFS [x] [y];
		}

		public Transform GetAnchor(int dir, int dist)
		{
			return GetAnchor ((Direction)dir, (Distance)dist);
		}

		[ExecuteInEditMode]
		[ContextMenu("GetTFArray")]
		public void GetTFArray()
		{
			_TFS = new List<List<Transform> > ();
			_TFS.Add (F);
			_TFS.Add (FR);
			_TFS.Add (BR);
			_TFS.Add (B);
			_TFS.Add (BL);
			_TFS.Add (FL);
		}
	}
}
