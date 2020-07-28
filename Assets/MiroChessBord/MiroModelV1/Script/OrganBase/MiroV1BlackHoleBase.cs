using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class BlackHoleBase : MonoBehaviour {

		bool bGrown = false;

		[ContextMenu("GrowUp")]
		virtual public void GrowUp()
		{
			bGrown = true;
		}

		[ContextMenu("Shrink")]
		virtual public void Shrink()
		{
			bGrown = false;
		}

		[ContextMenu("AbsorbingON")]
		virtual public void AbsorbingON()
		{
			
		}

		[ContextMenu("AbsorbingOFF")]
		virtual public void AbsorbingOFF()
		{
			
		}

		public virtual void TurnDynamics(bool bON)
		{
			
		}

		public virtual bool IsGrown()
		{
			return bGrown;
		}

		virtual public List<MiroV1AbsorbPoint> GetAbsorbingPos()
		{
			List<MiroV1AbsorbPoint> poss = new List<MiroV1AbsorbPoint> ();
			return poss;
		}

		virtual public int GetAbsorbingAmt()
		{
			int absorbAmt = 0;
			List<MiroV1AbsorbPoint> absPts = GetAbsorbingPos ();
			for (int i = 0; i < absPts.Count; i++) {
				MiroV1AbsorbPoint apt = absPts [i];
				if (apt.IsEmitterLinked()) {
					absorbAmt++;
				}
			}
			return absorbAmt;
		}


	}
}
