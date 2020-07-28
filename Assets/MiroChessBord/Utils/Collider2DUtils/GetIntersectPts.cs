using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class GetIntersectPts : MonoBehaviour {

		public List<GameObject> _Objs = new List<GameObject> ();
		public int count = 20;
		// Use this for initialization
		void Start () {
			for (int i = 0; i < count; i++) {
				GameObject newObj = new GameObject ();
				_Objs.Add (newObj);
				newObj.name = i.ToString();
				newObj.transform.SetParent (transform,false);
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}


		void OnTriggerStay2D(Collider2D other)
		{
			

		}

		void OnCollisionStay2D(Collision2D col)
		{
			int cnt = col.contacts.Length;
			for (int i = 0; i < cnt; i++) {
				ContactPoint2D cpt = col.contacts [i];
				if (i < count) {
					Vector3 pt = cpt.point;
					_Objs [i].transform.position = pt;
				}
			}

		}

	}


}
