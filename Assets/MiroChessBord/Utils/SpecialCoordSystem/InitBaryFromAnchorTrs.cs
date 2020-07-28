using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class InitBaryFromAnchorTrs : MonoBehaviour {
		public AnchorTriangle _AnchorTrs;
		public List<BaryCoord> _Barys;
		public bool _bInitAtStart = true;


			

		// Use this for initialization
		void Start () {
			if (_bInitAtStart) {
				GetAllBarysInChridren ();
				InitBarys ();
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}



		[ContextMenu("GetAllBarysInChridren")]
		public void GetAllBarysInChridren()
		{
			BaryCoord [] bcs = GetComponentsInChildren<BaryCoord> ();
			_Barys.Clear ();
			for (int i = 0; i < bcs.Length; i++) {
				_Barys.Add (bcs [i]);
			}
		}

		[ContextMenu("InitBarys")]
		public void InitBarys()
		{
			foreach (BaryCoord By in _Barys) {
				int idNearest = GetNearestTriangleId (By);

				Transform TA,TB,TC;
				TA = _AnchorTrs._Trs [idNearest]._A;
				TB = _AnchorTrs._Trs [idNearest]._B;
				TC = _AnchorTrs._Trs [idNearest]._C;

				Vector3 BN = BaryCoord.ComputeNBaryCoords (
					By.transform.position, 
					TA.position,
					TB.position,
					TC.position);

				By.BaryCoords = BN;
				By.Anchor0 = TA;
				By.Anchor1 = TB;
				By.Anchor2 = TC;
				By.GetBaryCoords ();
			}
		}

		[ContextMenu("GetAndInitChildBarys")]
		public void GetAndInitChildBarys()
		{
			GetAndInitChildBarys ();
			InitBarys ();
		}

		private int GetNearestTriangleId (BaryCoord By)
		{
			int idNearest = 0;
			float distRecord = 1000000.0f;
			for (int i = 0; i < _AnchorTrs._Trs.Count; i++) {
				Transform A, B, C;
				A = _AnchorTrs._Trs [i]._A;
				B = _AnchorTrs._Trs [i]._B;
				C = _AnchorTrs._Trs [i]._C;
				Vector3 Pos = By.transform.position;
				Vector3 Center = (A.position + B.position + C.position) / 3.0f;
				float dist = Vector3.Distance (Pos, Center);
				if (dist <= distRecord) {
					distRecord = dist;
					idNearest = i;
				}
			}
			return idNearest;
		}
	}
}
