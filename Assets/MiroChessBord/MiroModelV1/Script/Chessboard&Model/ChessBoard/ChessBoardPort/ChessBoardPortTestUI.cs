using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiroV1
{
	public class ChessBoardPortTestUI : MonoBehaviour {
		public MiroChessBoardPort _Port;
		public HexCoordInput _HexCoordInput;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			Update_OperatingCampIdDisp ();
			Update_UserOperatingHex ();
			Update_DispSelectedHexState ();
			Update_DispSelHexMovable ();
			Update_SelHexRotable ();
			Update_BanDirsText ();
			UpdateChessPropDisp ();
		}
			
		public void SetCampsCount(float count)
		{
			_Port.SetCampsCount ((int)count);
			PrintCampsCount ();
		}

		public void PrintCampsCount()
		{
			int campsCount = 
				_Port.GetCampsCount ();
			print ("Camps Count is " + campsCount);
		}

		public Text _CampIdAtHexDisp;
		public void DispCampIdAtSelectedHex()
		{
			int campId = 
				_Port.GetCampIdAt (_HexCoordInput._coord);

			Hex coord = _HexCoordInput._coord;
			string hs = HexToString (coord);

			_CampIdAtHexDisp.text = "Camp Id At " + hs + ":" + campId.ToString ();
		}

		private string HexToString(Hex h)
		{
			string hs = "(";
			hs += h.q;
			hs += ",";
			hs += h.r;
			hs += ",";
			hs += h.s;
			hs += ")";
			return hs;
		}

		public Text _OperatingCampIdDisp;
		private void Update_OperatingCampIdDisp()
		{
			string opCampIdTxt = _Port.GetOperatingCampId ().ToString ();
			_OperatingCampIdDisp.text = "Operating Camp Id:" + opCampIdTxt;
		}

		public void ChooseOperatingCamp(float id)
		{
			_Port.ChooseOperatingCamp ((int)id);
		}

		public void PrintAllHexes()
		{
			List<Hex> allHexes = 
				_Port.GetAllHexes ();
			foreach (Hex h in allHexes) {
				string hexTxt = HexToString (h);
				print (hexTxt);
			}
		}

		public void PrintAllPlacableHexes()
		{
			List<Hex> placableHexes = _Port.GetAllPlacableHexes ();
			foreach (Hex h in placableHexes) {
				string hexTxt = HexToString (h);
				print (hexTxt);
			}
		}

		public void PrintAllPlacableEmptyHexes()
		{
			List<Hex> emptyHexes = _Port.GetAllPlacableEmptyHexes ();
			foreach (Hex h in emptyHexes) {
				string hexTxt = HexToString (h);
				print (hexTxt);
			}
		}

		public void PrintAllOccupiedHexes()
		{
			List<Hex> occupiedHexes = _Port.GetAllOccupiedHexes ();
			foreach (Hex h in occupiedHexes) {
				string hexTxt = HexToString (h);
				print (hexTxt);
			}
		}

		public void PrintAllHexesOccupiedByOperatingCamp()
		{
			int campId = _Port.GetOperatingCampId ();
			List<Hex> hexes =
				_Port.GetAllHexesOccupiedByCamp (campId);
			foreach (Hex h in hexes) {
				string hexTxt = HexToString (h);
				print (hexTxt + " is occupied by camp " + campId.ToString());
			}
		}

		public Text _UserOperatingHex;
		public void Update_UserOperatingHex()
		{
			Hex h = _Port.GetUserOperatingHex ();
			string coordTxt = HexToString (h);

			_UserOperatingHex.text = "Operating at " + coordTxt;
		}

		public Text _SelectedHexState;
		public void Update_DispSelectedHexState()
		{
			Hex h = _HexCoordInput._coord;
			string hexTxt = HexToString (h);

			bool bPlacable = _Port.IsPlacableAt (h);
			bool bEmpty = _Port.IsEmptyAt (h);

			string txt = 
				hexTxt + " Placable? " + 
				bPlacable.ToString () + "; Empty? " + bEmpty;
			_SelectedHexState.text = txt;
		}

		public void TurnMoveFwdActionAtHex(bool bON)
		{
			Hex h = 
				_HexCoordInput._coord;
			if (bON) {
				_Port.EnableMoveFwdActionUIAt (h);
			} else {
				_Port.DisableMoveFwdActionUIAt (h);
			}
		}

		public Text _SelHexMovable;
		public void Update_DispSelHexMovable()
		{
			Hex h = 
				_HexCoordInput._coord;

			string hexTxt = HexToString (h);
			bool bMovable =	_Port.IsMoveFwdUIONAt (h);
			string dispText = hexTxt;
			if (bMovable) {
				dispText += " is movable";
			} else {
				dispText += " is NOT movable";	
			}
			_SelHexMovable.text = dispText;

		}

		public Text _SelHexRotable;
		public void Update_SelHexRotable()
		{
			Hex h = 
				_HexCoordInput._coord;

			string hexTxt = HexToString (h);
			bool bTurnable =	_Port.IsTurnDirUION (h);
			string dispText = hexTxt;
			if (bTurnable) {
				dispText += " is rotable";
			} else {
				dispText += " is NOT rotable";
			}
			_SelHexRotable.text = dispText;
		}

		public void TurnDragRotatorUI(bool bON)
		{
			Hex h = 
				_HexCoordInput._coord;
			if (bON) {
				_Port.EnableDragRotatorUI (h);
			} else {
				_Port.DisableDragRotatorUI (h);
			}
			Update_SelHexRotable ();
		}

		public Text _BanDirsText;
		public void Update_BanDirsText()
		{
			List<int> banDirs = 
				_Port.GetBanDirs ();
			string banDirsTxt = "";
			for (int i = 0; i < banDirs.Count; i++) {
				banDirsTxt += banDirs [i].ToString ();
			}
			_BanDirsText.text = "Ban Dirs: " + banDirsTxt;
		}
			
		public Toggle [] TgBanDirs;
		public void SetBanDirs()
		{
			List<int> banDirs = new List<int> ();
			for (int i = 0; i < TgBanDirs.Length; i++) {
				if (!TgBanDirs [i].isOn) {
					banDirs.Add (i);
				}
			}
			_Port.SetBanDirs (banDirs);

		}

		public void ConfirmChessPlacement()
		{
			Hex coord = new Hex ();
			int dir = -1;
			int campId = -1;
			_Port.ConfirmOperatingChess (out coord, out dir, out campId);

			//print ("Confirm Chess At " + HexToString(coord)+ " in dir " + dir + " of camp " + campId);
		}

		public void CancelChessPlacement()
		{
			Hex coord = new Hex();
			int dir = -1;
			int campId = -1;
			_Port.CancelOperatingChess (out coord, out dir, out campId);
		}

		public void TurnChessboardOperation(bool bEnable)
		{
			if (bEnable) {
				_Port.EnableChessboardOperation ();
			} else {
				_Port.DisableChessboardOperation ();
			}
		}
		/*
		public void TurnAllChessOperation(bool bEnable)
		{
			if (!bEnable) {
				_Port.DisableChessboardOperation ();
				_Port.EnableChessboardOpearionOnEmptyPlacableHex ();
			} else {
				_Port.EnableChessboardOperation ();
			}
		}*/

		public void TurnChessOperationAtSelHex(bool bEnable)
		{
			Hex pos = 
				_HexCoordInput._coord;
			if (bEnable) {
				_Port.EnableOperationAt (pos);
			} else {
				_Port.DisableOperationAt (pos);
			}
		}

		public void TurnAllChessOperation(bool bEnable)
		{
			if (bEnable) {
				_Port.EnableAllChessOperation ();
			} else {
				_Port.DisableAllChessOperation ();
			}
		}

		// 棋子操作
		public Text _NewChessDirTxt;
		public Slider _NewChessDirSlider;
		public void Update_NewChessDir(float dir)
		{
			int idir = (int)dir;
			_NewChessDirTxt.text = "Dir " + idir.ToString ();
		}

		public void PlaceChess()
		{
			int dir = (int)_NewChessDirSlider.value;
			int campId = _Port.GetOperatingCampId ();

			Hex coord = _HexCoordInput._coord;
			bool bPlaced = _Port.TryPlaceChessAt (campId, dir, coord);
			if (bPlaced) {
				print ("Placed new chess at:" + coord);
			} else {
				print ("Cannot place new chess at:" + coord);
			}
		}

		public void MoveFwdChess()
		{
			Hex coord = _HexCoordInput._coord;
			bool bSuc = _Port.TryMoveFwdAt (coord);
			if (bSuc) {
				print ("move forward at:" + HexToString (coord));
			} else {
				print("can not move forwart at:" + HexToString (coord));
			}
		}


		public void MoveChessInDir()
		{
			int dir = (int)_NewChessDirSlider.value;
			//int campId = _Port.GetOperatingCampId ();
			Hex coord = _HexCoordInput._coord;

			bool bSuc = _Port.TryMoveAlongDirAt (coord, dir);
			if (bSuc) {
				print ("move along dir at:" + HexToString (coord));
			} else {
				print("can not move along dir at:" + HexToString (coord));
			}
		}

		public void MoveChessFromAToB()
		{
			Hex h = _Port.GetUserOperatingHex ();
			Hex h2 = _HexCoordInput._coord;
			int dir = (int)_NewChessDirSlider.value;

			bool bSuc = _Port.TryMoveFromA2B (h, h2, dir);
			if (bSuc) {
				print ("Move from "  + HexToString (h) + 
					" to " + HexToString(h2));
			} else {
				print ("Can not move from "  + HexToString (h) + 
					" to " + HexToString(h2));
			}
		}

		public void TurnRightChess()
		{
			Hex coord = _HexCoordInput._coord;

			bool bSuc = _Port.TurnRightAt (coord);
			if (bSuc) {
				print ("turn right at:" + HexToString (coord));
			} else {
				print("can not turn right at:" + HexToString (coord));
			}

		}

		public void TurnLeftChess()
		{
			Hex coord = _HexCoordInput._coord;

			bool bSuc = _Port.TurnLeftAt (coord);
			if (bSuc) {
				print ("turn left at:" + HexToString (coord));
			} else {
				print("can not turn left at:" + HexToString (coord));
			}
		}

		public void KillChess()
		{
			Hex coord = _HexCoordInput._coord;

			bool bSuc = _Port.KillAt (coord);
			if (bSuc) {
				print ("Kill at:" + HexToString (coord));
			} else {
				print("can not kill at:" + HexToString (coord));
			}

		}

		public void DisappearChess()
		{
			Hex coord = _HexCoordInput._coord;

			bool bSuc = _Port.DisappearAt (coord);
			if (bSuc) {
				print ("Disappear at:" + HexToString (coord));
			} else {
				print("can not disappear at:" + HexToString (coord));
			}
		}

		public void KillAllMaxHurtChesses()
		{
			_Port.KillAllMaxHurtMiros ();
			print ("Kill max hurt chesses");
		}


		[System.Serializable]
		public struct ChessInfo
		{
			public int campId;
			public int q,r;
			public int dir;
		}

		public List<ChessInfo> _Chesses;
		public void PlaceChessBatch()
		{
			List<MiroChessBoardPort.ChessInfo> infos =
				new List<MiroChessBoardPort.ChessInfo> ();
			foreach(ChessInfo ch in _Chesses)
			{
				MiroChessBoardPort.ChessInfo info = new MiroChessBoardPort.ChessInfo ();
				info.campId = ch.campId;
				info.dir = ch.dir;
				info.pos = new Hex (
					ch.q, ch.r, 0 - ch.q - ch.r);
				infos.Add (info);
			}
			_Port.PlaceChesses (infos);
		}



		// --------- chess props access -------------//
		// Coord, MaxHP, HP, DispHP, MaxAT, AT, DispAT
		// AbsorbPwr, AbsorbAmt, En, AssistAT
		public Text _ChessPropsTitle;
		public Text _ChessPropMaxHP;
		public Text _ChessPropHP;
		public Text _ChessPropDispHP;
		public Text _ChessPropMaxAT;
		public Text _ChessPropAT;
		public Text _ChessPropDispAT;
		public Text _ChessPropAbsorbPwr;
		public Text _ChessPropAbsorbAmt;
		public Text _ChessPropEN;
		public Text _ChessPropAssistAT;
		public void UpdateChessPropDisp()
		{
			Update_ChessPropsTitle ();
			Update_ChessPropMaxHP();
			Update_ChessPropHP();
			Update_ChessPropDispHP();
			Update_ChessPropMaxAT();
			Update_ChessPropAT();
			Update_ChessPropDispAT();
			Update_ChessPropAbsorbPwr();
			Update_ChessPropAbsorbAmt();
			Update_ChessPropEN();
			Update_ChessPropAssistAT();
		}

		private int _HexPickMode = 0;
		public void SetHexPickMode(int modeId)
		{
			_HexPickMode = modeId;
		}
		private Hex PickHexForPropDisp()
		{
			Hex h = new Hex ();
			if (_HexPickMode == 0) {
				h = _HexCoordInput._coord;
			} else if (_HexPickMode == 1) {
				h = _Port.GetUserOperatingHex ();
			}
			return h;
		}

		public void Update_ChessPropsTitle()
		{
			Hex h = PickHexForPropDisp();
			string hexTxt = HexToString (h);
			string title = "Chess Props At " + hexTxt;
			_ChessPropsTitle.text = title;
		}

		public void Update_ChessPropMaxHP(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetMaxHPAt (h);
			_ChessPropMaxHP.text = value.ToString ();
		}
		public void Update_ChessPropHP(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetHPAt (h);
			_ChessPropHP.text = value.ToString ();
		}
		public void Update_ChessPropDispHP(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetDispHPAt (h);
			_ChessPropDispHP.text = value.ToString ();
		}
		public void Update_ChessPropMaxAT(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetMaxATAt (h);
			_ChessPropMaxAT.text = value.ToString ();
		}
		public void Update_ChessPropAT(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetATAt (h);
			_ChessPropAT.text = value.ToString ();
		}
		public void Update_ChessPropDispAT(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetDispATAt (h);
			_ChessPropDispAT.text = value.ToString ();
		}
		public void Update_ChessPropAbsorbPwr(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetAbsorbPowerMaxAt (h);
			_ChessPropAbsorbPwr.text = value.ToString ();
		}
		public void Update_ChessPropAbsorbAmt(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetAbsorbingAmtAt (h);
			_ChessPropAbsorbAmt.text = value.ToString ();
		}
		public void Update_ChessPropEN(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetEnergyAt (h);
			_ChessPropEN.text = value.ToString ();
		}
		public void Update_ChessPropAssistAT(){
			Hex h = PickHexForPropDisp();
			int value = _Port.GetAssistingATAt (h);
			_ChessPropAssistAT.text = value.ToString ();
		}

		public InputField _DispHPInput, _DispATInput;
		public void SetDispHPTest()
		{
			Hex coord = PickHexForPropDisp();
			int dhp = _Port.GetDispHPAt (coord);
			string dhpTxt = _DispHPInput.text;

			bool bSuc = int.TryParse (dhpTxt, out dhp);
			if (bSuc) {
				_Port.SetDispHPAt (coord, dhp);
			}
		}

		public void SetDispATTest()
		{
			Hex coord = PickHexForPropDisp();
			int dispAt= _Port.GetDispATAt (coord);
			string dispAtTxt = _DispATInput.text;

			bool bSuc = int.TryParse (dispAtTxt, out dispAt);
			if (bSuc) {
				_Port.SetDispATAt (coord, dispAt);
			}
		}

		public MiroV1PlacementMgr _placeMgr;
		public void PrintAllChessInfo()
		{
			var birthIDs = new List<int> ();
			var positions = new List<Hex> ();
			var directions = new List<int> ();
			var campIds = new List<int> ();

			_Port.GetAllChessesInfo (
				out birthIDs, 
				out positions, 
				out directions,
				out campIds);
			List<string> campNames = 
				_placeMgr.GetMiroPrefabsCampNames ();
			
			for (int i = 0; i < birthIDs.Count; i++) {
				string campName = campNames [campIds [i]];
				string info = campName + " " + birthIDs [i].ToString ();
				info += " at " + HexToString( positions [i]);
				info += " in dir " + directions [i].ToString();
				print (info);
			}
		}

		public void ResetChessboard()
		{
			_Port.ResetChessboard ();
		}

		public int _NewChessBoardSize = 4;
		public Text _NewChessBoardSizeTxt;
		public void SetNewChessBoardSize(float s)
		{
			int edgesize = (int)s;
			_NewChessBoardSize = edgesize;
			_NewChessBoardSizeTxt.text = edgesize.ToString ();
			_Port.SetChessBoardSizePreset (edgesize);
		}

		public void NewChessBoard()
		{
			_Port.ResetChessBoard (_NewChessBoardSize);
		}



	}
}
