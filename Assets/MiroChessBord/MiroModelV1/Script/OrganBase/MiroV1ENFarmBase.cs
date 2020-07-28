using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1ENFarmBase : MonoBehaviour {


		virtual public FixedJoint2D GetJointA()
		{
			return null;	
		}

		virtual public FixedJoint2D GetJointB()
		{
			return null;
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

		[ContextMenu("TurnOnRepeatAnimation")]
		virtual public void TurnOnRepeatAnimation()
		{
			
		}

		[ContextMenu("TurnOffRepeatAnimation")]
		virtual public void TurnOffRepeatAnimation()
		{
			
		}


		virtual public void TurnDynamics(bool bON)
		{
		}



	}
}
