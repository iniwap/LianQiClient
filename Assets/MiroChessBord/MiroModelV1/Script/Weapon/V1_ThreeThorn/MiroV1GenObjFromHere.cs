using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1GenObjFromHere : MonoBehaviour {

		public GameObject _Prefab;

		public void Generate()
		{
			GameObject newObj = Instantiate (_Prefab) as GameObject;
			newObj.transform.position = transform.position;
			newObj.transform.SetParent (transform.parent,true);
		}
	}
}
