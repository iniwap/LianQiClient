using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MiroV1
{
	public class HexGridCtrl : MonoBehaviour {

		public ChessBoardSetting _BoardSetting;

		[Range(1,16)]
		public int _GridLevel = 4;
		public float _CellInterval = 2.0f;
		public Transform _CellParent;
		public GameObject _CellPrefab;

		public List<GameObject> _Cells = new List<GameObject>();
		public Dictionary<Hex,GameObject> _HexToCells = new Dictionary<Hex,GameObject>();
		private List<Hex> _Hexes = new List<Hex>();
		private CellObjCtrl _LastOperatedCCtrl = null;

		public GameObject _LinkPrefab;
		public Transform _LinkParent;
		public List<GameObject> _NeighborLinks = new List<GameObject>();
		//[System.Serializable]

		public Dictionary<Vector2,GameObject> _HexPairToNbLinks = 
			new Dictionary<Vector2, GameObject>();

		public List<CellSelection> _SelectedCells = 
			new List<CellSelection>();

		[System.Serializable]
		public class EventWithCell: UnityEvent<CellObjCtrl> {};
		public EventWithCell _PlacementChanged;
		public UnityEvent _GridGenerated;

		// Use this for initialization
		void Start () {
			if (_BoardSetting != null) {
				_GridLevel = _BoardSetting._EdgeSize + 1;
			}
			CheckCellParent ();
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void setGridLevel(int gridLevel){
			_GridLevel = gridLevel;
			_BoardSetting._EdgeSize = _GridLevel - 1;
		}

		public bool IsBlockedAtHex(Hex h)
		{
			if (!_HexToCells.ContainsKey (h)) {
				return true;
			}
			var cellObj = _HexToCells[h];
			if (cellObj == null) {
				return true;
			}

			CellObjCtrl cctrl = cellObj.GetComponent<CellObjCtrl> ();
			bool blocked = cctrl.IsBlocked ();
			return blocked;
		}

		public bool IsEmptyAtHex(Hex h)
		{
			if (!_HexToCells.ContainsKey (h)) {
				return false;
			}

			var cellObj = _HexToCells[h];
			if (cellObj == null) {
				return false;
			}

			CellObjCtrl cctrl = cellObj.GetComponent<CellObjCtrl> ();
			bool bEmpty = (cctrl._TgtObj==null);
			return bEmpty;
		}

		public void SetLastOperatedCellObjCtrl(CellObjCtrl cctrl)
		{
			_LastOperatedCCtrl = cctrl;
		}

		public Hex GetLastOperatedCCtrlHex()
		{
			Hex h = new Hex (int.MaxValue, 0, 0);
			if (_LastOperatedCCtrl != null) {
				HexCoord hc = _LastOperatedCCtrl.GetComponent<HexCoord> ();
				h = hc._hex;
			}
			return h;
		}

		public GameObject GetCellObjAt(Hex hex)
		{
			if (_HexToCells.ContainsKey (hex)) {
				GameObject cellObj = _HexToCells [hex];
				return cellObj;
			} else {
				return null;
			}
		}

		public CellObjCtrl GetCellCtrlAt(Hex hex)
		{
			GameObject cellObj = GetCellObjAt (hex);
			if (cellObj == null) {
				return null;
			}
			CellObjCtrl cctrl = cellObj.GetComponent<CellObjCtrl> ();
			return cctrl;
		}

		public MiroV1ModelSetting GetMiroModelSettingAt(Hex hex)
		{
			GameObject cellObj = GetCellObjAt (hex);
			if (cellObj == null) {
				return null;
			}
			CellObjCtrl cctrl = cellObj.GetComponent<CellObjCtrl> ();
			MiroV1ModelSetting setting = CellObjCtrlUtils.GetModelSettingFromCtrl (cctrl);
			return setting;
		}

		public MiroModelV1 GetMiroModelAt(Hex hex)
		{
			CellObjCtrl cctrl = GetCellCtrlAt (hex);
			return CellObjCtrlUtils.GetMiroModelFromCell (cctrl);
		}

		public void TurnONCellSelection()
		{
			CellSelection._AllEnable = true;
		}

		public void TurnOFFCellSelection()
		{
			CellSelection._AllEnable = false;
		}

		[ContextMenu("GenGrid")]
		public void GenGrid()
		{
			CheckCellParent ();

			Layout L = new Layout(
				Layout.pointy,
				new Point(_CellInterval,_CellInterval),
				new Point(0.0f,0.0f));

			Hex h0 = new Hex (0, 0, 0);
			
			int lv = _GridLevel-1;
			for (int q = -lv; q < lv+1; q++) {
				for (int r = -lv; r < lv+1; r++) {
					int s = -r - q;
					Hex h = new Hex(q,r,s);
					Point pos = Layout.HexToPixel (L, h);

					int dist = Hex.Distance(h, h0);
					if (dist >= _GridLevel) continue;

					GameObject newObj = 
						Instantiate (_CellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					newObj.transform.SetParent (_CellParent, true);
					newObj.transform.localPosition = 
						new Vector3((float)pos.x,(float)pos.y,0.0f);
					_Cells.Add (newObj);
					_HexToCells [h] = newObj;
					if (!_Hexes.Contains (h)) {
						_Hexes.Add (h);
					}

					newObj.name = 
						"(" + h.q.ToString () +
						"," + h.r.ToString () + ","
						+ h.s.ToString () + ")";

					AddHexInfoToNewCell (h, newObj);
				}
			}

			foreach (GameObject gb in _Cells) {
				AddHexNeighborInfoToNewCell (gb);
			}

			if (_LinkParent == null) {
				return;
			}
			//Debug.Log ("CellObjCnt:" + _Cells.Count);

			int count = 0;
			for (int i = 0; i < _Cells.Count; i++) {
				
				for (int j = i+1; j < _Cells.Count; j++) {

					count++;
					//Debug.Log ("count:" + count);

					GameObject A = _Cells [i];
					GameObject B = _Cells [j];
					HexCoord ac = A.GetComponent<HexCoord> ();
					HexCoord bc = B.GetComponent<HexCoord> ();
					Hex HexA = ac._hex;
					Hex HexB = bc._hex;

					if (Hex.Distance (HexA, HexB) != 1) {
						continue;
					}

					Vector2 linkPos = Vector2.zero;

					Point pta = Layout.HexToPixel (L, HexA);
					Point ptb = Layout.HexToPixel (L, HexB);

					Vector2 PA = new Vector2 ((float)pta.x, (float)pta.y);
					Vector2 PB = new Vector2 ((float)ptb.x, (float)ptb.y);

					linkPos = Vector2.Lerp (PA, PB, 0.5f);
					//Debug.Log ("linkPos:" + linkPos);
					if (_HexPairToNbLinks.ContainsKey (linkPos)) {
						continue;
					}
						
					Vector3 pos = Vector3.Lerp (
						A.transform.position, 
						B.transform.position, 0.5f);

					GameObject newLink = 
						Instantiate (_LinkPrefab, Vector3.zero, Quaternion.identity);
					InitNewLinkObjTF (pos, newLink);
					AddNewLinkObjToContainers (linkPos, newLink);
	
					TwoObjectLink lk = newLink.GetComponent<TwoObjectLink> ();
					lk.SetA(A);
					lk.SetB(B);
					newLink.name = A.name + "-" + B.name;


				}
			}

			_GridGenerated.Invoke ();


		}

		[ContextMenu("ClearGrid")]
		public void ClearGridImmediate()
		{
			foreach (GameObject gb in _Cells) {
				DestroyImmediate (gb);
			}
			foreach (GameObject gb in _NeighborLinks) {
				DestroyImmediate (gb);
			}

			ClearContainers ();
		}

		[ContextMenu("CancelSelection")]
		public void CancelSelection()
		{
			foreach (GameObject gb in _Cells) {
				CellSelection cellSel = gb.GetComponentInChildren<CellSelection> ();
				if (cellSel != null) {
					cellSel.SelectionOFF ();
				}
			}
		}

		public void TurnCellBlock(bool bBlock)
		{
			foreach (GameObject gb in _Cells) {
				CellObjCtrl ctrl = gb.GetComponent<CellObjCtrl> ();
				CellSelection cellSel = gb.GetComponentInChildren<CellSelection> ();
				bool bSelect = cellSel.IsSelected ();

				if (ctrl != null && bSelect) {
					ctrl.SetBlock (bBlock);
				}
			}
		}

		public void SelectCell(HexCoord hc)
		{
			CellSelection sel = 
				hc.GetComponentInChildren<CellSelection> ();

			_SelectedCells.Add (sel);
		}

		public void UnselectCell(HexCoord hc)
		{
			CellSelection sel = 
				hc.GetComponentInChildren<CellSelection> ();

			_SelectedCells.Remove (sel);
		}

		public void ClearGrid()
		{
			foreach (GameObject gb in _Cells) {
				Destroy (gb);
			}
			foreach (GameObject gb in _NeighborLinks) {
				Destroy (gb);
			}
			ClearContainers ();
		}
			
		public void PlacementChanged(CellObjCtrl ctrl)
		{
			_PlacementChanged.Invoke (ctrl);
			//Debug.Log ("_PlacementChanged at: " + ctrl);
		}

		void ClearContainers ()
		{
			_Cells.Clear ();
			_HexToCells.Clear ();
			_Hexes.Clear ();
			_NeighborLinks.Clear ();
			_HexPairToNbLinks.Clear ();
		}

		void CheckCellParent ()
		{
			if (_CellParent == null) {
				_CellParent = transform;
			}
		}

		static Vector3 GetLinkPos (
			KeyValuePair<Hex, GameObject> hgA, 
			KeyValuePair<Hex, GameObject> hgB)
		{
			Vector3 pos;
			Transform tfA = hgA.Value.transform;
			Transform tfB = hgB.Value.transform;
			pos = Vector3.Lerp (tfA.position, tfB.position, 0.5f);
			return pos;
		}

		void InitNewLinkObjTF (Vector3 pos, GameObject newLink)
		{
			newLink.transform.SetParent (_LinkParent);
			newLink.transform.position = pos;
		}


		void AddNewLinkObjToContainers (Vector2 pos, GameObject newLink)
		{
			_NeighborLinks.Add (newLink);
			_HexPairToNbLinks [pos] = newLink;
		}




		/*
		static void DispHexPairInfo(HexPair hp)
		{
			Hex h = hp.A;
			Hex l = hp.B;
			Debug.Log (
				"A:" + h.q + "," + h.r + "," + h.s + 
				" B:" + l.q + "," + l.r + "," + l.s);


		}
		*/

		void AddHexInfoToNewCell (Hex h, GameObject newObj)
		{
			HexCoord hc = newObj.AddComponent<HexCoord> ();
			hc._hex = h;

		}

		void AddHexNeighborInfoToNewCell(GameObject newObj)
		{
			HexCoord hc = newObj.GetComponent<HexCoord> ();
			for (int i = 0; i < 6; i++) {
				Hex hexDir = Hex.directions [i];
				Hex nbHex = Hex.Add (hc._hex, hexDir);
				if (_HexToCells.ContainsKey (nbHex)) {
					hc._Neighbors [i] = _HexToCells [nbHex].transform;
				}
				else {
					hc._Neighbors [i] = null;
				}
			}
		}

		public void ConfigAttackersForEveryCell()
		{
			foreach (GameObject ctrlObj in _Cells) {
				CellObjCtrl ctrl = ctrlObj.GetComponent<CellObjCtrl> ();

				MiroV1PlacementMgr.ConfigAttackersFor (ctrlObj.transform, true);
			}
				
		}

		public List<Hex> GetAllHexes()
		{
			return _Hexes;
		}

		public List<Hex> GetAllPlacableHexes()
		{
			List<Hex> lstHexes = new List<Hex> ();
			foreach (KeyValuePair<Hex,GameObject> item in _HexToCells) {
				CellObjCtrl ctrl  =
					item.Value.GetComponent<CellObjCtrl> ();
				bool bBlocked = ctrl.IsBlocked ();
				if (!bBlocked) {
					lstHexes.Add (item.Key);
				}
			}
			return lstHexes;
		}

		public List<Hex> GetAllPlacableEmptyHexes()
		{
			List<Hex> placableHexes = GetAllPlacableHexes ();
			List<Hex> emptyHexes = new List<Hex> ();
			foreach (Hex h in placableHexes) {
				CellObjCtrl ctrl = 
					_HexToCells [h].GetComponent<CellObjCtrl> ();
				if (ctrl._TgtObj == null) {
					emptyHexes.Add (h);
				}
			}
			return emptyHexes;
		}

		public List<Hex> GetAllOccupiedHexes()
		{
			List<Hex> allHexes = GetAllHexes ();
			List<Hex> occupiedHexes = new List<Hex> ();
			foreach (Hex h in allHexes) {
				CellObjCtrl ctrl = 
					_HexToCells [h].GetComponent<CellObjCtrl> ();
				if (ctrl._TgtObj != null) {
					occupiedHexes.Add (h);
				}
			}

			return occupiedHexes;
		}

		public void TurnHexDisp(bool bON)
		{
			foreach (var ctrlObj in _Cells) {
				MeshRenderer mrdr = 
					ctrlObj.GetComponentInChildren<MeshRenderer>();
				mrdr.enabled = bON;
			}
		}


		public void ResetPressedUpPlacers()
		{
			foreach (GameObject gb in _Cells) {
				if (gb != null) {
					ChessPressUpPlacer pp = gb.GetComponentInChildren<ChessPressUpPlacer> ();
					pp.ResetPressed ();
				}
			}
		}

		public void DisablePressedUpPlacers()
		{
			TurnPressedUpPlacers (false);
		}

		public void EnablePressedUpPlacers()
		{
			TurnPressedUpPlacers (true);
		}

		public bool DisablePressedUpPlacerAt(Hex pos)
		{
			return TurnPressedUpPlacerAt (pos,false);
		}

		public bool EnablePressedUpPlacerAt(Hex pos)
		{
			return TurnPressedUpPlacerAt (pos, true);
		}

		public bool TurnPressedUpPlacerAt(Hex pos, bool bEnable)
		{
			bool bExist = 
				_HexToCells.ContainsKey (pos);
			if (bExist) {
				GameObject cellobj = GetCellObjAt (pos);
				ChessPressUpPlacer pp = cellobj.GetComponentInChildren<ChessPressUpPlacer> ();
				pp.enabled = bEnable;
				return true;
			} else {
				return false;
			}
		}

		public void TurnPressedUpPlacers(bool bEnable)
		{
			foreach (GameObject gb in _Cells) {
				if (gb != null) {
					ChessPressUpPlacer pp = gb.GetComponentInChildren<ChessPressUpPlacer> ();
					pp.enabled = bEnable;
				}
			}
		}

		public bool EnableDragRotaterAt(Hex pos)
		{
			return TurnDragRotaterAt (pos, true);
		}

		public bool DisableDragRotaterAt(Hex pos)
		{
			return TurnDragRotaterAt (pos, false);
		}

		public bool TurnDragRotaterAt(Hex pos, bool bEnable)
		{
			bool bExist = 
				_HexToCells.ContainsKey (pos);
			if (bExist) {
				GameObject cellobj = GetCellObjAt (pos);
				ChessDragRotator dr = cellobj.GetComponentInChildren<ChessDragRotator> ();
				dr.enabled = bEnable;
				return true;
			} else {
				return false;
			}
		}

		public void TurnPressedUpPlacerOnOccupiedCells(bool bEnable)
		{
			List<Hex> hexes = GetAllOccupiedHexes ();
			foreach (Hex h in hexes) {
				var pressupPlacer = 
					_HexToCells [h].GetComponentInChildren<ChessPressUpPlacer> ();
				pressupPlacer.enabled = bEnable;
			}
		}

		public void TurnDragRotaterOnOccupiedCells(bool bEnable)
		{
			List<Hex> hexes = GetAllOccupiedHexes ();
			foreach (Hex h in hexes) {
				var dr = 
					_HexToCells [h].GetComponentInChildren<ChessDragRotator> ();
				dr.enabled = bEnable;
			}
		}

		public void TurnPressedUpPlacersOnEmptyPlacableCells(bool bEnable)
		{
			List<Hex> hexes = 
				GetAllPlacableEmptyHexes ();
			foreach (Hex h in hexes) {
				var pressupPlacer = 
					_HexToCells [h].GetComponentInChildren<ChessPressUpPlacer> ();
				pressupPlacer.enabled = bEnable;
			}
		}

		public void TurnDragRotatersOnEmptyPlacableCells(bool bEnable)
		{
			List<Hex> hexes = 
				GetAllPlacableEmptyHexes ();
			foreach (Hex h in hexes) {
				var dr = 
					_HexToCells [h].GetComponentInChildren<ChessDragRotator> ();
				dr.enabled = bEnable;
			}
		}



		public void EnableDragRotaters()
		{
			TurnDragRotaters (true);
		}

		public void DisableDragRotaters()
		{
			TurnDragRotaters (false);
		}

		public void TurnDragRotaters(bool bEnable)
		{
			foreach (GameObject gb in _Cells) {
				if (gb != null) {
					ChessDragRotator dr = gb.GetComponentInChildren<ChessDragRotator> ();
					dr.enabled = bEnable;
				}
			}
		}

		public List<CellObjCtrl> GetEmptyCellObjCtrls()
		{
			List<CellObjCtrl> epCtrls = new List<CellObjCtrl> ();

			foreach (GameObject ctrlObj in _Cells) {
				CellObjCtrl cctrl = ctrlObj.GetComponent<CellObjCtrl> ();
				if (cctrl._TgtObj == null) {
					epCtrls.Add (cctrl);
				}
			}

			return epCtrls;

		}

		public List<CellObjCtrl> GetEmptyPlacableCellObjCtrls()
		{
			List<CellObjCtrl> epCtrls = new List<CellObjCtrl> ();

			foreach (GameObject ctrlObj in _Cells) {
				CellObjCtrl cctrl = ctrlObj.GetComponent<CellObjCtrl> ();
				if (cctrl._TgtObj == null && !cctrl.IsBlocked()) {
					epCtrls.Add (cctrl);
				}
			}

			return epCtrls;
		}

		public void ConfirmOccupiedCells()
		{
			List<CellObjCtrl> occupiedCtrls = GetOccupiedCellObjCtrls ();
			foreach (CellObjCtrl ctrl in occupiedCtrls) {
				ChessDragRotator dragRotator = 
					ctrl.GetComponentInChildren<ChessDragRotator> ();
				dragRotator.enabled = false;
			}
		}

		public List<CellObjCtrl> GetOccupiedCellObjCtrls()
		{
			List<CellObjCtrl> occupiedCtrls = new List<CellObjCtrl> ();
			foreach (GameObject gb in _Cells) {
				CellObjCtrl ctrl = gb.GetComponent<CellObjCtrl> ();
				if (ctrl._TgtObj != null) {
					occupiedCtrls.Add (ctrl);
				}
			}
			return occupiedCtrls;

		}



	}
}
