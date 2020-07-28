using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MiroV1
{
	public class TwoObjectLink : MonoBehaviour {
		public GameObject _A,_B;

		[System.Serializable]
		public class EventWithGameObject: UnityEvent<GameObject>{};

		public EventWithGameObject _LinkA, _LinkB;

		public void SetA(GameObject A)
		{
			_A = A;
			_LinkA.Invoke (_A);
		}

		public void SetB(GameObject B)
		{
			_B = B;
			_LinkB.Invoke (_B);
		}

		public GameObject GetA()
		{
			return _A;
		}
		public GameObject GetB()
		{
			return _B;
		}
	}
}
