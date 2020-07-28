using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lyu
{
	public class TargetJoint2DCtrl : MonoBehaviour {

		public TargetJoint2D[] jnts;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}


		[ContextMenu("GetChildrenTargetJoint2Ds")]
		public void GetChildrenTargetJoint2Ds()
		{
			jnts = GetComponentsInChildren<TargetJoint2D> ();
		}

		[ContextMenu("TurnON")]
		public void TurnON()
		{
			TurnJoints (true);
		}

		[ContextMenu("TurnOFF")]
		public void TurnOFF()
		{
			TurnJoints (false);
		}

		public void TurnJoints(bool bEnable)
		{
			foreach (var jnt in jnts) {
				jnt.enabled = bEnable;
			}
		}


	}
}
