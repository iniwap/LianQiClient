using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1AbsorbPoint : MonoBehaviour {

		public bool _ON = false;
		public MiroV1BulletEmitterBase _emitter = null;
		public UnityEvent _EmitterDeLinked;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			/*
			if (_emitter != null) {
				_emitter.SetAbsorber (this);
			}*/
		}

		public void LinkEmitter(MiroV1BulletEmitterBase em)
		{
			_emitter = em;
			_emitter.SetAbsorber(this);
		}
			
		public void DelinkEmitters()
		{
			if (_emitter != null) {
				_emitter.RmAbsorber ();
				_emitter = null;
				_EmitterDeLinked.Invoke ();
			}
		}

		public bool IsEmitterLinked()
		{
			return (_emitter != null);
		}

		public bool IsON()
		{
			return _ON;
		}

		public void TurnON()
		{
			_ON = true;
		}

		public void TurnOFF()
		{
			_ON = false;
		}

	}
}
