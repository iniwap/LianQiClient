using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyu
{
	public class RandMove2D : MonoBehaviour {


		public float _MaxDist = 0.1f;


		public void Move()
		{
			Vector2 Dir = Random.insideUnitCircle;

			Vector2 movement = Dir * Random.Range(0.0f,_MaxDist);

			transform.position  = transform.position + (Vector3)movement;

		}


	}
}
