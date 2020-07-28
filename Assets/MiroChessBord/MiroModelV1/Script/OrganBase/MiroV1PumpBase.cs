using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroV1PumpBase : MonoBehaviour {

		private int _AidingRDir = -1;

		public void SetAidingRDir(int aidingRDir)
		{
			_AidingRDir = aidingRDir;
		}

		public int GetAidingRDir()
		{
			return _AidingRDir;
		}

		[ContextMenu("GrowUP")]
		virtual public void GrowUP()
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

		[ContextMenu("Recover")]
		virtual public void Recover()
		{
			
		}

		virtual public void TurnDynamics(bool bON)
		{
			
		}

		virtual public bool IsGrown()
		{
			return false;
		}

		virtual public Transform GetAnchorTFA()
		{
			return null;
		}

		virtual public Transform GetAnchorTFB()
		{
			return null;
		}

		virtual public MiroModelV1 GetModelAider()
		{
			return null;
		}

		virtual public MiroModelV1 GetModelAidee()
		{
			return null;
		}

	}
}
