using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SpriteColorCtrl : MonoBehaviour {
		public List<Color> _Colors = new List<Color>();
		public int _id = -1;
		public float _LerpSpd = 1.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			int cnt = _Colors.Count;
			if (_id >= 0 && _id < cnt) {
				float dt = Time.deltaTime;
				float lt = dt * _LerpSpd;
				SpriteRenderer sr = GetComponent<SpriteRenderer> ();
				Color cr = sr.color;
				Color crTgt = _Colors [_id];
				Color cr2 = Color.Lerp (cr, crTgt, lt);
				sr.color = cr2;
			}
		}
	}
}
