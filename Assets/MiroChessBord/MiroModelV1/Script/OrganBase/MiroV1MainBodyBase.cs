using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1MainBodyBase : MonoBehaviour {

		[Range(0,100)]
		public int _HP = 0;

		[Range(0,100)]
		public int _HPMax = 0;

		public float _LerpSpd = 1.0f;
		private float _HPF = 0;
		private float _HPMF = 0;

		[System.Serializable]
		public class EventWithInt: UnityEvent<float>{};
		public EventWithInt _HPMaxFChanged;

		// Update is called once per frame
		void Update () {
			UpdatePropValues ();
			UpdateDisplay ();
			UpdateOther ();
		}

		void UpdatePropValues()
		{
			_HP = Mathf.Clamp (_HP, 0, _HPMax);

			float t = Time.deltaTime * _LerpSpd;
			_HPF = Mathf.Lerp (_HPF, (float)_HP, t);
			_HPMF = Mathf.Lerp (_HPMF, (float)_HPMax, t);

			_HPMaxFChanged.Invoke (_HPMF);
		}

		virtual public void AdjustBodyAnchors(
			MiroV1MainBodyAnchors bodyAnchors)
		{
			
		}

		virtual public void UpdateDisplay()
		{
		}

		virtual public void UpdateOther()
		{
		}

		[ContextMenu("GrowUp")]
		virtual public void GrowUp()
		{
			
		}

		[ContextMenu("Scatter")]
		virtual public void Scatter()
		{
			
		}


		virtual public void TurnDynamics(bool bON)
		{
			
		}

		protected float GetHPFloat()
		{
			return _HPF;
		}

		protected float GetHPMaxFloat()
		{
			return _HPMF;
		}

	}
}
