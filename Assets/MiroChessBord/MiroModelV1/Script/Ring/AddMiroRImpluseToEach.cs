using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
public class AddMiroRImpluseToEach : MonoBehaviour {

		public bool _FixRotation = true;

	public List<GameObject> _Objs;
		public UnityEvent _SetObjs,_AddImpluse;

	// Use this for initialization
	void Start () {

	}

	public void SetObjs(List<GameObject> objs)
	{
		_Objs = objs;
		_SetObjs.Invoke ();
	}

	[ContextMenu("AddMiroImpluse")]
	public void AddMiroImpluse()
	{
		foreach(GameObject gb in _Objs)
		{
			MiroRandImpluse impluse = 
				gb.GetComponent<MiroRandImpluse> ();
			if (impluse == null) {
				impluse = gb.AddComponent<MiroRandImpluse> ();
			}
			Rigidbody2D rb = gb.GetComponent<Rigidbody2D> ();
			if (rb == null) {
				rb = gb.AddComponent<Rigidbody2D> ();
			}
			rb.gravityScale = 0.0f;
				_AddImpluse.Invoke ();
				if (_FixRotation) {
					rb.constraints = RigidbodyConstraints2D.FreezeRotation;
				}
		}
	}
}
}
