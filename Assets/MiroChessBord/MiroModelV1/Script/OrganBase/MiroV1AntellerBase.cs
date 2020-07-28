using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1AntellerBase : MonoBehaviour {

		virtual public void SetSrcTF(Transform TFSrc)
		{
		}

		[ContextMenu("GrowUp")]
		virtual public void GrowUp()
		{
			
		}

		[ContextMenu("Shrink")]
		virtual public void Shrink()
		{
		}

		[ContextMenu("Scatter")]
		virtual public void Scatter()
		{
			
		}

		virtual public void TurnDynamics(bool bON)
		{
		}

		virtual public bool IsGrown()
		{
			return false;
		}


	}
}
