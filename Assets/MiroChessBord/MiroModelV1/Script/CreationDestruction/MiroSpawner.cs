using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroSpawner : MonoBehaviour {
		public int count = 0;
		public Bounds _bound;
		public GameObject _prefab;
		public Transform _parent;

		[ContextMenu("Spawn")]
		public void Spawn()
		{
			for (int i = 0; i < count; i++) {
				Vector3 pos = new Vector3 (
												Random.Range (_bound.min.x, _bound.max.x),
					              Random.Range (_bound.min.y, _bound.max.y),
					              Random.Range (_bound.min.z, _bound.max.z));
				Quaternion rot = Quaternion.AngleAxis (
					Random.Range (0, 360.0f), Vector3.forward);
				GameObject newObj = Instantiate (_prefab, pos, rot);
				newObj.transform.SetParent (_parent,true);
			}

		}

	}
}
