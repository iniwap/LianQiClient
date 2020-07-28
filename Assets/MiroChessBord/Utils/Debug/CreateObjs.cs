using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class CreateObjs : MonoBehaviour {

		public GameObject _Prefab;
		public Transform _Parent;
		public int _Count = 10;
		public Bounds _bounds = new Bounds ();

		public Bounds _rotBounds = new Bounds();

		public KeyCode _key = KeyCode.A;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKeyDown (_key)) {
				CreateObj ();
			}
			
		}

		public void CreateObj()
		{
			for (int i = 0; i < _Count; i++) {
				GameObject newObj = 
					Instantiate (_Prefab,_Parent) as GameObject;

				Vector3 lpos = Vector3.zero;
				for (int j = 0; j < 3; j++) {
					lpos [j] = Random.Range (
						_bounds.min [j], _bounds.max [j]);
				}
				newObj.transform.localPosition = lpos;

				Vector3 euler = Vector3.zero;
				for (int j = 0; j < 3; j++) {
					euler [j] = Random.Range (
						_rotBounds.min [j], _rotBounds.max [j]);
				}

				Quaternion rot = Quaternion.Euler (euler);
				newObj.transform.localRotation = rot;

			}
		}
	}
}
