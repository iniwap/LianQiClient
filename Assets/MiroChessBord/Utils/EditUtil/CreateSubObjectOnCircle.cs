using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CreateSubObjectOnCircle : MonoBehaviour {
		public float _StartAngle = 0.0f;
		public float _AngleStep = 60.0f;
		public float _StartRadius = 1.0f;
		public float _RadiusStep = 0.0f;
		public int _Count = 6;
		public List<GameObject> _Objs = new List<GameObject>();
		public GameObject _Prefab;

		[ContextMenu("GenerateObjs")]
		public void GenerateObjs()
		{
			for (int i = 0; i < _Count; i++) {
				float angle = _StartAngle + i * _AngleStep;
				float radius = _StartRadius + i * _RadiusStep;
				float x = Mathf.Cos (angle * Mathf.Deg2Rad) * radius;
				float y = Mathf.Sin (angle * Mathf.Deg2Rad) * radius;
				Vector2 localPos = new Vector2 (x, y);

				GameObject newObj = Instantiate(_Prefab) as GameObject;
				newObj.transform.SetParent (transform);
				newObj.transform.localPosition = localPos;
				_Objs.Add (newObj);
			}
		}

		[ContextMenu("ClearObjs")]
		public void ClearObjs()
		{
			foreach (GameObject gb in _Objs) {
				DestroyImmediate (gb);
			}
			_Objs.Clear ();
		}

	}
}
