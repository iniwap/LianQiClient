using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ENContainerBase : MonoBehaviour {

		[Range(0,54)]
		public int _AT = 0;
		public float _LerpSpd = 5.0f;
		private float _ATF = 0.0f;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		virtual public void Update () {
			UpdateATF ();
		}

		void UpdateATF ()
		{
			float dt = Time.deltaTime;
			float lerpT = dt * _LerpSpd;
			_ATF = Mathf.Lerp (_ATF, (float)_AT, lerpT);
		}

		protected float GetATFloat()
		{
			return _ATF;
		}
	}
}
