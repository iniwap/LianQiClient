using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class CCManager : MonoBehaviour {
		public CCAccess _CCAccessPrefab;
		private Dictionary<MiroModelV1, CellObjCtrl> _Model2Cell = 
			new Dictionary<MiroModelV1, CellObjCtrl>();

		public float _CaculatePeriod = 1.0f;

		// Use this for initialization
		void Start () {
			InvokeRepeating ("Caculate", 1.0f,_CaculatePeriod);
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void Caculate()
		{
			for(int k=0;k<1;k++)
			{
			int grpCnt = _CCAccessPrefab.GetGroupCount ();
			for (int i = 0; i < grpCnt; i++) {
				foreach (var item in _Model2Cell) {
						if (item.Value == null)
							continue;
						
						CCAccess cca = item.Value.GetComponent<CCAccess> ();
						if(cca != null)
							cca.CalculateGroup (i);
				}
			}
			}
		}

	

		public void ChangeCell(CellObjCtrl cctrl)
		{
			ClearNullItems ();
			MiroModelV1 model = null;
			if (cctrl._TgtObj != null) {
				model = cctrl._TgtObj.GetComponent<MiroModelV1> ();
				if (model != null) {
					ChangeItem (model, cctrl);
					Caculate ();
				}
			}
		}

		public void ChangeItem(MiroModelV1 model, CellObjCtrl cctrl)
		{
			_Model2Cell [model] = cctrl;
			Caculate ();
		}

		public CellObjCtrl GetCellObjCtrlOfModel(MiroModelV1 model)
		{
			if (_Model2Cell.ContainsKey (model)) {
				return _Model2Cell [model];
			} else {
				return null;
			}
		}

		public Hex GetHexOfModel(MiroModelV1 model)
		{
			if (_Model2Cell.ContainsKey (model)) {
				CellObjCtrl ctrl = _Model2Cell [model];
				HexCoord hc = ctrl.GetComponent<HexCoord> ();
				Hex h = hc._hex;
				return h;
			} else {
				return new Hex(int.MaxValue,0,0);
			}

		}

		public int GetDirOfModel(MiroModelV1 model)
		{
			if (_Model2Cell.ContainsKey (model)) {
				CellObjCtrl ctrl = _Model2Cell [model];
				int dir = ctrl.GetDir();
				return dir;
			} else {
				return -1;
			}
		}

		public void RemoveItem(MiroModelV1 model)
		{
			_Model2Cell.Remove (model);
			Caculate ();
		}

		public void RemoveNullItem()
		{
			
		}

		public void ClearAllItems()
		{
			_Model2Cell.Clear ();
			Caculate ();
		}
		public void ClearNullItems()
		{
			foreach (KeyValuePair<MiroModelV1, CellObjCtrl> item in _Model2Cell) {
				if (item.Value == null) {
					_Model2Cell.Remove (item.Key);
				}
			}
		}
	}
}
