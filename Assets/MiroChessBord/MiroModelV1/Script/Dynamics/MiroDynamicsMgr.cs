using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyu;

namespace MiroV1
{
	public class MiroDynamicsMgr : MonoBehaviour {

		public Transform _GridParent;
		public Transform _MirosParent;
		public Transform _ConnectionsParent;

		public MiroV1PlacementMgr _MiroMgr;
		HashSet<MiroModelDynamicsCtrl> _modelDynCtrls = new HashSet<MiroModelDynamicsCtrl>();

		public void TurnChessboardDynamics(bool bON)
		{
			Rigidbody2DsMgr rbMgr = 
				_GridParent.gameObject.GetComponent<Rigidbody2DsMgr> ();

			if (bON) {
				rbMgr.TurnToDynamic ();
			} else {
				rbMgr.TurnToStatic ();
			}

		}

		void GetMiroModelDynamcsCtrl ()
		{
			foreach (var dynCtrl in _modelDynCtrls) {
				if (dynCtrl == null) {
					_modelDynCtrls.Remove (dynCtrl);
				}
			}

			List<MiroModelV1> miros = 
				_MiroMgr.GetMiroModels ();
			foreach (var miro in miros) {
				MiroModelDynamicsCtrl miroDynCtrl = 
					miro.GetComponent<MiroModelDynamicsCtrl> ();
				if (miroDynCtrl != null) {
					_modelDynCtrls.Add (miroDynCtrl);
				}
			}
		}

		public void TurnMiroFrameDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnPhysicalFramwork (bON);
			}
		}

		public void TurnDispAssistsDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnDispAssistDynamics (bON);
			}

		}

		public void TurnEnergyGeneratorDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnENGeneratorDynamics (bON);
			}
		}

		public void TurnMainBodyDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnMainBodyDynamics (bON);
			}
		}

		public void TurnAntellersDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnAntellersDynamics (bON);
			}
		}

		public void TurnWeaponsDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnWeaponsDynamics (bON);
			}
		}

		public void TurnAbsorbDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnBlackHoleDynamics (bON);
			}
		}

		public void TurnPumpDynamics(bool bON)
		{
			GetMiroModelDynamcsCtrl ();
			foreach (var dc in _modelDynCtrls) {
				dc.TurnPumpDynamics (bON);
			}
		}

		public Transform _ENFarmParent;
		public void TurnENFarmDynamics(bool bON)
		{
			MiroV1ENFarmBase[] enfarms = 
				_ENFarmParent.GetComponentsInChildren<MiroV1ENFarmBase> ();

			foreach (var ef in enfarms) {
				ef.TurnDynamics (bON);
			}

		}

		public HexGridCtrl _gridCtrl;
		public void TurnAttackAreaDynamics(bool bON)
		{
			List<GameObject> linkObjs = _gridCtrl._NeighborLinks;
			foreach (GameObject gb in linkObjs) {
				CLinkAttackLR linkAtLr = gb.GetComponentInChildren<CLinkAttackLR> ();
				linkAtLr.enabled = bON;
			}
		}

		public Transform _RingParent;
		public void TurnRingDynamics(bool bON)
		{
			MiroRing[] rings = _RingParent.GetComponentsInChildren<MiroRing> ();
			foreach (var r in rings) {
				r.TurnDynamics (bON);
			}

		}

		public void TurnAllDynamics(bool bON)
		{
			TurnChessboardDynamics (bON);
			TurnMiroFrameDynamics (bON);
			TurnDispAssistsDynamics (bON);
			TurnEnergyGeneratorDynamics (bON);
			TurnMainBodyDynamics (bON);
			TurnAntellersDynamics (bON);
			TurnWeaponsDynamics (bON);
			TurnAbsorbDynamics (bON);
			TurnPumpDynamics (bON);
			TurnENFarmDynamics (bON);
			TurnAttackAreaDynamics (bON);
			TurnRingDynamics (bON);

		}

	}
}
