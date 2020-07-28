using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace  MiroV1
{
	public class MiroV1PeriodicInvoke : MonoBehaviour {

		public float _TimeBias = 0.0f;
		public float _Period = 1.0f;
		private float _LeftTime;
		public UnityEvent _Evnt;

		// Use this for initialization
		void Start () {
			Init ();
		}

		void OnEnable()
		{
			Init ();
		}

		// Update is called once per frame
		void Update () {
			_LeftTime -= Time.deltaTime;
			if (_LeftTime <= 0.0f) {
				_Evnt.Invoke ();
				Reset ();
			}
		}

		[ContextMenu("Init")]
		public void Init()
		{
			Reset ();
			_LeftTime += _TimeBias;
		}
		[ContextMenu("Reset")]
		public void Reset()
		{
			_LeftTime = _Period;
		}
	}

}