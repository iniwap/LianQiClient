using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class BaryCoord : MonoBehaviour {

		public Transform _Anchor0;
		public Transform _Anchor1;
		public Transform _Anchor2;

		public bool _UpdatePosByBary = true;

		public float _ZBias = 0.5f;

		public Transform Anchor0 {
			get {
				return _Anchor0;
			}
			set {
				_Anchor0 = value;
			}
		}

		public Transform Anchor1 {
			get {
				return _Anchor1;
			}
			set {
				_Anchor1 = value;
			}
		}

		public Transform Anchor2 {
			get {
				return _Anchor2;
			}
			set {
				_Anchor2 = value;
			}
		}

		private List<Vector3> _AnchorCarts = new List<Vector3>();

		public Vector3 _BaryCoords;

		public Vector3 BaryCoords {
			get {
				return _BaryCoords;
			}
			set {
				_BaryCoords = value;
			}
		}

		private Vector3 _BaryN;

		public bool _GetBaryCoordAtStart = false;

		// Use this for initialization
		void Start () {
			

			ChechAndInitAnchorCartsList ();

			if (_GetBaryCoordAtStart) {
				GetBaryCoords ();
			}
		}
		
		// Update is called once per frame
		void Update () {
			UpdateAnchorCarts ();
			UpdateBaryN ();
			if (_UpdatePosByBary) {
				UpdatePos ();
			} else {
				GetBaryCoords ();
			}
		}

		void OnDrawGizmos() {
			if (_AnchorCarts.Count < 3) {
				return;
			}
			Gizmos.color = Color.red;
			Gizmos.DrawLine (_AnchorCarts [0], _AnchorCarts [1]);
			Gizmos.color = Color.green;
			Gizmos.DrawLine (_AnchorCarts [1], _AnchorCarts [2]);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine (_AnchorCarts [2], _AnchorCarts [0]);
		}

		private void UpdatePos ()
		{
			Vector3 Pos = Vector3.zero;
			for (int i = 0; i < 3; i++) {
				Pos += _BaryN [i] * _AnchorCarts [i];
			}
			Pos.z += _ZBias;
			transform.position = Pos;
		}

		private void UpdateAnchorCarts()
		{
			ChechAndInitAnchorCartsList ();
			_AnchorCarts [0] = _Anchor0.position;
			_AnchorCarts [1] = _Anchor1.position;
			_AnchorCarts [2] = _Anchor2.position;
		}

		private void UpdateBaryN()
		{
			float Sum = _BaryCoords [0] + _BaryCoords [1] + _BaryCoords [2];
			if (Sum != 0.0f) {
				_BaryN = _BaryCoords / Sum;
			} else {
				_BaryN = Vector3.one / 3.0f;
			}
		}

		[ContextMenu("GetBaryCoords")]
		public void GetBaryCoords()
		{
			UpdateAnchorCarts ();

			Vector3 P = transform.position;
			Vector3 A = _AnchorCarts [0];
			Vector3 B = _AnchorCarts [1];
			Vector3 C = _AnchorCarts [2];

			_BaryN = ComputeNBaryCoords (P, A, B, C);
			_BaryCoords = _BaryN;
		}

	

		void ChechAndInitAnchorCartsList ()
		{
			if (_AnchorCarts.Count != 3) {
				_AnchorCarts.Clear ();
				for (int i = 0; i < 3; i++) {
					_AnchorCarts.Add (Vector3.zero);
				}
			}
		}

		public static Vector3 ComputeNBaryCoords(Vector3 Pos,
			Vector3 A, Vector3 B, Vector3 C)
		{
			float DetT = (B.y - C.y) * (A.x - C.x) + (C.x - B.x) * (A.y - C.y);

			float Lamda1 = ((B.y - C.y) * (Pos.x - C.x) + (C.x - B.x) * (Pos.y - C.y)) / DetT;
			float Lamda2 = ((C.y - A.y) * (Pos.x - C.x) + (A.x - C.x) * (Pos.y - C.y)) / DetT;
			float Lamda3 = 1.0f - Lamda1 - Lamda2;
			return new Vector3 (Lamda1, Lamda2, Lamda3);
		}

		public static Vector3 ComputeNBaryCoords(Vector3 Pos,
			Vector3 A, Vector3 B, Vector3 C, ref bool bInsideABC)
		{
			bInsideABC = true;
			Vector3 BN = ComputeNBaryCoords (Pos, A, B, C);
			for (int i = 0; i < 3; i++) {
				if (BN [i] < 0.0f) {
					bInsideABC = false;
					break;
				}
			}
			return BN;
		}

		public Vector3 GetBaryCoordN()
		{
			return _BaryN;
		}
	}
}
