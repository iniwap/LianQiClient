using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1GenObjFromInterpCurve : MonoBehaviour {

		public MiroInterpCurve _InterpCurve = new MiroInterpCurve();
		public AnimationCurve _ProbDistrib = new AnimationCurve();
		public GameObject _Prefab;
		public float _MaxAmount = 5.0f;

		public void SetPositions(List<Vector3> vts)
		{
			_InterpCurve.SetPositions (vts);
		}

		public void GenerateObj()
		{
			int count = Mathf.CeilToInt(Random.value * _MaxAmount);
			for (int i = 0; i < count; i++) {
				float t = Random.value;
				bool bGen = GenT (ref t);
				if (!bGen)
					break;
				GenerateObjAtT (t);
			}
		}

		bool GenT (ref float t)
		{
			float p = _ProbDistrib.Evaluate (t);
			float r = Random.value;
			int tryCount = 0;
			while (r > p) {
				t = Random.value;
				p = _ProbDistrib.Evaluate (t);
				r = Random.value;
				tryCount++;
				if (tryCount > 100) {
					return false;
				}
			}

			return true;
		}

		void GenerateObjAtT (float t)
		{
			Vector3 pos = _InterpCurve.GetPosAtT01 (t);
			GameObject newObj = Instantiate (_Prefab) as GameObject;
			newObj.transform.SetParent (transform, true);
			newObj.transform.localPosition = pos;
		}
	}
}
