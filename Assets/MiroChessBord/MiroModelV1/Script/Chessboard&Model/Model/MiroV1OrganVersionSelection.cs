using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class MiroV1OrganVersionSelection : MonoBehaviour {

		public List<GameObject> _Organs = new List<GameObject> ();
		public int _id = 0;
		private int _idPrev;

		[System.Serializable]
		public class EventWithGameObject: UnityEvent<GameObject>{};

		public EventWithGameObject _SelectObj;

		// Use this for initialization
		void Start () {
			
			if (_Organs.Count==0) {
				GetChildAsOrganVersions ();
			}
			_idPrev = _id;
			SelectOrganById ();
			
		}
		
		// Update is called once per frame
		void Update () {
			if (_idPrev != _id) {
				SelectOrganById ();
			}
			_idPrev = _id;
			
		}

		[ContextMenu("GetChildAsOrganVersions")]
		public void GetChildAsOrganVersions ()
		{
			_Organs.Clear ();
			int chdCnt = transform.childCount;
			for (int i = 0; i < chdCnt; i++) {
				Transform tfchd = transform.GetChild (i);
				_Organs.Add (tfchd.gameObject);
			}
		}

		[ContextMenu("SelectOrganById")]
		public void SelectOrganById()
		{
			_SelectObj.Invoke (_Organs [_id]);
		}

		public GameObject GetSelectedOrgan()
		{
			return _Organs [_id];
		} 


	}
}
