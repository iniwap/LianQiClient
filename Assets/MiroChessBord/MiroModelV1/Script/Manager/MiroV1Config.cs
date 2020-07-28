using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1Config : MonoBehaviour {

		public List<GameObject> _MiroObjs = new List<GameObject>();

		public GameObject _EN2FarmPrefab;
		public GameObject _EN2AbsorberPrefab;
		public GameObject _RingPrefab;

		public Transform _EN2FarmParent;
		public Transform _EN2AbsorberParent;
		public Transform _RingParent;


		public void SetMiroObjs(List<GameObject> mobjs)
		{
			_MiroObjs = mobjs;
		}

		[ContextMenu("MoveForward")]
		public void MoveForward()
		{
			foreach (GameObject gb in _MiroObjs) {

			}
		}

		[ContextMenu("CreateENPumpForEach")]
		public void CreateENPumpForEach()
		{
			
		}

		[ContextMenu("CreateENFarmForTwo")]
		public void CreateENFarmForTwo()
		{
			if (_MiroObjs.Count != 2) {
				Debug.Log ("Not exactly 2 MiroObjects!");
			}

			GameObject A = _MiroObjs [0];
			GameObject B = _MiroObjs [1];
			MiroV1MainBodyAnchors akrs = 
				A.GetComponent<MiroV1MainBodyAnchors> ();




		}

		[ContextMenu("CreateENAbsorberForTwo")]
		public void CreateENAbsorberForTwo()
		{
			
		}

		[ContextMenu("CreateRing")]
		public void CreateRing()
		{
			GameObject newRingObj = Instantiate (
				_RingPrefab,_RingParent) as GameObject;

			MiroRing ring = newRingObj.GetComponent<MiroRing> ();
			ring.ClearRingObjTfs ();
			foreach (GameObject gb in _MiroObjs) {
				ring.AddObjAsRingObj (gb);
			}

			ring.InitRing ();

		}

	}
}
