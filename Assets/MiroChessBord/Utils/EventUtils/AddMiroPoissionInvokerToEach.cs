using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class AddMiroPoissionInvokerToEach : MonoBehaviour {


		public List<GameObject> _Objs;
		public float _Lamda = 1.0f;
		public UnityEvent _SetObjs,_AddInvokers;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void SetObjs(List<GameObject> objs)
		{
			_Objs = objs;
			_SetObjs.Invoke ();
		}

		[ContextMenu("AddInvokers")]
		public void AddInvokers()
		{
			foreach (GameObject gb in _Objs) {
				MiroPoissonInvoker ikr = 
					gb.AddComponent<MiroPoissonInvoker> ();
				ikr._lamda = _Lamda;
			}
			_AddInvokers.Invoke ();
		}
	}
}
