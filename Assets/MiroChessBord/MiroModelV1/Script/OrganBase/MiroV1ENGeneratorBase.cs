using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ENGeneratorBase : MonoBehaviour {

		[Range(0,9)]
		public int _EN = 0;
		protected int _ENPrev = 0;

		[Range(0,9)]
		public int _ENMax = 0;
		protected int _ENMaxPrev = 0;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		public void Update () {
			_EN = Mathf.Clamp (_EN, 0, _ENMax);

			UpdateAnyway ();

			if (_EN != _ENPrev) {
				UpdateOnENChange (_EN,_ENPrev);
			}
			if (_ENMax != _ENMaxPrev) {
				UpdateOnENMaxChange (_ENMax, _ENMaxPrev);
			}

			_ENPrev = _EN;
			_ENMaxPrev = _ENMax;
		}

		virtual public void UpdateAnyway()
		{
			
		}

		virtual public void UpdateOnENChange(int EN, int ENPrev)
		{
			
		}

		virtual public void UpdateOnENMaxChange(int ENMax, int ENMaxPrev)
		{
			
		}
			
		virtual public List<Transform> GetENDotTFs()
		{
			List<Transform> tfList = new List<Transform> ();

			for (int i = 0; i < 9; i++) {
				tfList.Add (transform);
			}
			return tfList;
		}

		virtual public void TurnDynamics(bool bON)
		{
			
		}


	}
}
