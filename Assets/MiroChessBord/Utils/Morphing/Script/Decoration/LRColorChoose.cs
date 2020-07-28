using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class LRColorChoose : MonoBehaviour {

		public List<Color> _Colors = new List<Color>();
		public int _id = -1;
		public float _LerpSpd = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			int cnt = _Colors.Count;
			if (_id > 0 && _id < cnt) {
				float dt = Time.deltaTime;
				float lt = dt * _LerpSpd;
				LineRenderer lr = GetComponent<LineRenderer> ();
				Color endCr = lr.endColor;
				Color startCr = lr.startColor;
				Color crTgt = _Colors [_id];
				Color endCr2 = Color.Lerp (endCr, crTgt, lt);
				Color startCr2 = Color.Lerp (startCr, crTgt, lt);
				lr.endColor = endCr2;
				lr.startColor = startCr2;
			}
			
		}

		public void RandomChooseColor()
		{
			_id = Mathf.FloorToInt (Random.Range (0.0f, (float)_Colors.Count));
		}
	}
}
