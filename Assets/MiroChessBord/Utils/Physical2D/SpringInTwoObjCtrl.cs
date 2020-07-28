using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class SpringInTwoObjCtrl : MonoBehaviour {

		public Rigidbody2D _A,_B;

		[System.Serializable]
		public class SpringSetting
		{
			public bool _bAutoConfigAnchors;
			public bool _bAutoConfigDist;
			public float _Dist;

			public float _Freq;
			public float _Damp;
			public SpringSetting()
			{
				_bAutoConfigAnchors = false;
				_bAutoConfigDist = false;
				_Dist = 1.0f;

				_Freq = 1.0f;
				_Damp = 0.618f;
			}
		}
		public SpringSetting _spA = new SpringSetting();
		public SpringSetting _spB = new SpringSetting();

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_A == null || _B == null) {
				return;
			}

			SpringJoint2D spa = GetSpringJoint2D (_A);
			SpringJoint2D spb = GetSpringJoint2D (_B);
			ConfigSpringJoint2D (spa, _spA);
			ConfigSpringJoint2D (spb, _spB);
		}

		private SpringJoint2D GetSpringJoint2D(Rigidbody2D rb)
		{
			
			Rigidbody2D rb2 = _B;
			if (rb == _B) {
				rb2 = _A;
			} 
			SpringJoint2D sp;
			sp = rb.GetComponent<SpringJoint2D> ();
			if (sp == null) {
				sp = rb.gameObject.AddComponent<SpringJoint2D> ();
			}
			sp.connectedBody = rb2;

			return sp;
		}

		void ConfigSpringJoint2D (SpringJoint2D sp, SpringSetting setting)
		{
			sp.autoConfigureConnectedAnchor = setting._bAutoConfigAnchors;
			sp.autoConfigureDistance = setting._bAutoConfigDist;
			if (!setting._bAutoConfigDist) {
				sp.distance = setting._Dist;
			}
			sp.frequency = setting._Freq;
			sp.dampingRatio = setting._Damp;
		}

		public void LinkObjA(GameObject Obj)
		{
			Rigidbody2D rb = Obj.GetComponent<Rigidbody2D> ();
				_A = rb;
		}

		public void LinkObjB(GameObject Obj)
		{
			Rigidbody2D rb = Obj.GetComponent<Rigidbody2D> ();
			_B = rb;
		}

	}
}
