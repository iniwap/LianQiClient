using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LRFollower : MonoBehaviour {

		public MorphCharComps _comps;

		[Range(0,2)]
		public int _idLRI;
		public float _LerpSpd = 1.0f;
		private float _idLR;

		public float _WdOnScl = 1.5f;
		public float _WdShift = 0.0f;


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			float lt = Time.deltaTime * _LerpSpd;
			_idLR = Mathf.Lerp (_idLR, (float)_idLRI, lt);

			int idL = Mathf.FloorToInt (_idLR);
			int idR = Mathf.CeilToInt (_idLR);
			float t = (float)idR - _idLR;

			LineRenderer lrl = _comps._LRs [idL];
			LineRenderer lrr = _comps._LRs [idR];
			LineRenderer lrMe = GetComponent<LineRenderer> ();
			if (lrMe.positionCount != lrl.positionCount) {
				lrMe.positionCount = lrl.positionCount;
			}

			Vector3 scl = 
				_comps.transform.lossyScale;
			float xysMean = 0.5f * (scl.x + scl.y);
			float wd = xysMean * _WdOnScl;
			lrMe.widthMultiplier = wd;

			Vector3[] poss = new Vector3[lrl.positionCount];
			for (int i = 0; i < poss.Length; i++) {
				Vector3 pl = lrl.GetPosition (i);
				Vector3 pr = lrr.GetPosition (i);
				Vector3 pInterp = Vector3.Lerp (pl, pr, t);
				poss [i] = pInterp;
			}

			for (int i = 0; i < poss.Length; i++) {
				int id0 = i;
				int id1 = i + 1;
				int id = id0;
				Vector3 pos = poss [id];
				if (id1 >= poss.Length) {
					id0 -= 1;
					id1 -= 1;
				}
				Vector3 v0 = poss [id0];
				Vector3 v1 = poss [id1];
				Vector3 vec = v1 - v0;
				Vector3 vert = Quaternion.AngleAxis (90.0f, Vector3.forward) * vec;
				Vector3 vert01 = vert.normalized;
				Vector3 pos2 = pos + vert01 * _WdShift * wd;
				poss [id] = pos2;
			}
			lrMe.SetPositions (poss);

		}

		public void RandomChangeLrId()
		{
			_idLRI = Mathf.RoundToInt(Random.Range (0.0f, 2.0f));

		}


	}
}
