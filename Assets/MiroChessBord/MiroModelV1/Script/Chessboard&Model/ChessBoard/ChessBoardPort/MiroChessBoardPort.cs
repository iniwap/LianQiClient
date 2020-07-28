using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiroV1
{
	public class MiroChessBoardPort : MonoBehaviour {

		public HexGridCtrl _GridCtrl;
		public MiroV1PlacementMgr _MiroMgr;
		public ChessPlacerUI _ChessPlacerUI;
		public StdChessSpawner _StdChessSpawer;

		// ------------- 阵营 --------------------------
		// 设置阵营数量：1~4
		public void SetCampsCount(int campsCount=2)
		{
			_MiroMgr.SetCampsCount (campsCount);
			_ChessPlacerUI.InvokeBanDirsChanged ();
		}

		// 获得阵营数量：1~4玩家
		public int GetCampsCount() 
		{
			int cnt = _MiroMgr._MiroPrefabs.Count;
			return cnt;
		}

		// 获得棋格坐标处的阵营id： -1~3
		// -1: 无棋子 
		// 0~3: 四个阵营的编号
		public int GetCampIdAt(Hex coord) 
		{
			MiroV1ModelSetting miroSetting = 
				_GridCtrl.GetMiroModelSettingAt (coord);
			if (miroSetting == null) {
				return -1;
			}
			int id = _MiroMgr.GetIdOfMiroModelSetting (miroSetting);
			return id;
		}



		// ----------------- 棋盘格 ---------------------
		// Hex: 以 q,r,s三数值表达的棋格坐标
		// 操作下列接口，需要进行坐标转换，
		// 蜂窝网格坐标表达源自于: http://www.redblobgames.com/grids/hexagons/
		// 当前棋盘中，最中心的qrs坐标为0，0，0

		// 获得所有棋格坐标
		public List<Hex> GetAllHexes()
		{
			List<Hex> allHexes = _GridCtrl.GetAllHexes ();
			return allHexes;
		}
		// 所有允许落子的坐标，里面可能为空，也可能已经有棋子
		public List<Hex> GetAllPlacableHexes()
		{
			List<Hex> allHexes = _GridCtrl.GetAllPlacableHexes ();
			return allHexes;
		}
		// 所有空格坐标，里面可以落入棋子且当前为空
		public List<Hex> GetAllPlacableEmptyHexes()
		{
			List<Hex> allHexes = _GridCtrl.GetAllPlacableEmptyHexes ();
			return allHexes;
		}

		// 所有被占格子，里面已经有棋子
		public List<Hex> GetAllOccupiedHexes()
		{
			List<Hex> occupiedHexes = _GridCtrl.GetAllOccupiedHexes ();
			return occupiedHexes;
		}

		// 所有被某一阵营占据的格子
		public List<Hex> GetAllHexesOccupiedByCamp(int campId)
		{
			List<Hex> occupiedHexes = _GridCtrl.GetAllOccupiedHexes ();
			List<Hex> campHexes = new List<Hex> ();
			foreach (Hex h in occupiedHexes) {
				int id = GetCampIdAt (h);
				if (id == campId) {
					campHexes.Add (h);
				}
			}
			return campHexes;
		}

		// 获得所有棋子布局信息
		public void GetAllChessesInfo(
			out List<int> birthIDs,
			out List<Hex> positions, 
			out List<int> directions,
			out List<int> campIds)
		{
			birthIDs = new List<int> ();
			positions = new List<Hex> ();
			directions = new List<int> ();
			campIds = new List<int> ();

			var ccmgr = 
				_MiroMgr.GetComponent<CCManager> ();
			List<MiroModelV1> models = _MiroMgr.GetMiroModels();

			foreach (MiroModelV1 m in models) {
				int birthId = m.GetBirthID ();
				Hex pos = ccmgr.GetHexOfModel (m);
				int dir = ccmgr.GetDirOfModel (m);
				int campid = 
					_MiroMgr.GetIdOfMiroModelSetting (
						m.GetComponent<MiroV1ModelSetting> ());

				birthIDs.Add (birthId);
				positions.Add (pos);
				directions.Add (dir);
				campIds.Add (campid);
			}
		}

		// 获得当前用户正在操作的棋格坐标
		public Hex GetUserOperatingHex()
		{
			Hex h = _GridCtrl.GetLastOperatedCCtrlHex ();
			return h;
		}

		// 获得最后发生操作的棋格坐标

		/*
		public Hex GetLastChangedHex()
		{
			Hex h = new Hex ();
			return h;
			//_GridCtrl.GetLastOperatedCCtrlHex ();	
		}
		*/

		// 判断棋格是否允许落子
		// 棋格可以被设置为无法落子，则它无法落入任意一方棋子，必然为空格
		public bool IsPlacableAt(Hex coord)
		{
			bool bBlocked = 
				_GridCtrl.IsBlockedAtHex (coord);
			bool bPlacable = !bBlocked;
			
			return bPlacable;
		}

		// 判断棋格是否为空格
		public bool IsEmptyAt(Hex coord)
		{
			bool bEmpty = _GridCtrl.IsEmptyAtHex (coord);
			return bEmpty;
		}

		// 棋格方向: 0~5 正常方向； -1：棋格不存在
		public int GetDirAt(Hex coord)
		{
			CellObjCtrl cctrl = 
				_GridCtrl.GetCellCtrlAt (coord);
			if (cctrl != null) {
				return cctrl.GetDir();
			} else {
				return -1;
			}
		}

		// -----------  对当前用户UI进行调整的接口 --------------
		// 当前用户操作阵营id
		public int GetOperatingCampId ()
		{
			int id = _MiroMgr._MiroPrefID;
			return id;
		}

		// 选择当前操作的阵营
		public void ChooseOperatingCamp(int campId)
		{
			//_ChessPlacerUI.SetBanDirs ();
			_MiroMgr.SetMiroPrefabId (campId);
			_ChessPlacerUI.InvokeBanDirsChanged ();
		}

		// 开关移动操作UI
		public void EnableMoveFwdActionUIAt(Hex coord)
		{
			CellObjCtrl ctrl = _GridCtrl.GetCellCtrlAt (coord);
			int movePwr = ctrl.GetMovePwr ();
			if (movePwr == 0) {
				ctrl.IncMovePwr ();
			}
		}
		public void DisableMoveFwdActionUIAt(Hex coord)
		{
			GameObject ctrlObj = _GridCtrl.GetCellObjAt (coord);
			CellObjCtrl ctrl = ctrlObj.GetComponent<CellObjCtrl> ();
			ctrl.ClearMovePwr ();
		}

		public bool IsMoveFwdUIONAt(Hex coord)
		{
			CellObjCtrl ctrl = _GridCtrl.GetCellCtrlAt (coord);
			int movePwr = ctrl.GetMovePwr ();
			if (movePwr > 0) {
				return true;
			} else {
				return false;
			}
		}

		// 开关转向操作UI
		// 目前，只允许刚落入的棋子能够随意转向
		public bool IsTurnDirUION(Hex coord)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null) {
				return false;
			}
			ChessDragRotator dragRoter = 
				cctrl.GetComponentInChildren<ChessDragRotator> ();
			return dragRoter.enabled;
		}

		public bool EnableDragRotatorUI(Hex coord)
		{
			return TurnDragRotatorUI (coord, true);
		}

		public bool DisableDragRotatorUI(Hex coord)
		{
			return TurnDragRotatorUI(coord,false);
		}

		private bool TurnDragRotatorUI(Hex coord, bool bEnable)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null) {
				return false;
			}

			ChessDragRotator dragRoter = 
				cctrl.GetComponentInChildren<ChessDragRotator> ();

			dragRoter.enabled = bEnable;
			return true;
		}

		// 禁手
		public List<int> GetBanDirs()
		{
			List<int> banDirs = _ChessPlacerUI.GetBanDirs ();
			return banDirs;
		}

		public void SetBanDirs(List<int> banDirs)
		{
			_ChessPlacerUI.SetBanDirs (banDirs);	
		}

		public void SetBanDir(bool ban,int banDir)
		{
			_ChessPlacerUI.SetBanDir (ban,banDir);	
		}
		// 获得最新用户操作棋子的布局信息
		public bool GetOperatingChessInfo(
			out Hex pos, out int dir, out int campId)
		{
			pos = _GridCtrl.GetLastOperatedCCtrlHex ();
			dir = GetDirAt (pos);
			campId = GetCampIdAt (pos);
			if (dir < 0) {
				return false;
			} else {
				return true;
			}
		}

		// 棋子交互
		// 将当前正在操作的棋子确认（固定下来）
		public bool ConfirmOperatingChess(
			out Hex coord, 
			out int dir, 
			out int campId)
		{
			CellObjCtrl newestCtrl = 
				_StdChessSpawer.Confirm ();

			_GridCtrl.ConfirmOccupiedCells ();
			//DisableAllChessOperation ();

			if (newestCtrl == null) {
				coord = new Hex ();
				dir = -1;
				campId = -1;
				return false;
			}

			coord = newestCtrl.GetComponent<HexCoord> ()._hex;
			dir = newestCtrl.GetDir ();
			MiroV1ModelSetting msetting = CellObjCtrlUtils.GetModelSettingFromCtrl (newestCtrl);
			campId = _MiroMgr.GetIdOfMiroModelSetting (msetting);
			return true;
		}


		// 将当前正在操作的棋子取消
		public bool CancelOperatingChess(
			out Hex coord, 
			out int dir, 
			out int campId)
		{
			CellObjCtrl newestCtrl = _StdChessSpawer.GetSpawningCellObjCtrl ();
			if (newestCtrl == null) {
				coord = new Hex ();
				dir = -1;
				campId = -1;
				return false;
			}

			coord = newestCtrl.GetComponent<HexCoord> ()._hex;
			dir = newestCtrl.GetDir ();
			MiroV1ModelSetting msetting = CellObjCtrlUtils.GetModelSettingFromCtrl (newestCtrl);
			campId = _MiroMgr.GetIdOfMiroModelSetting (msetting);


			_StdChessSpawer.Cancel ();
			//_GridCtrl.ConfirmOccupiedCells ();
			//DisableAllChessOperation ();
			_GridCtrl.TurnDragRotatersOnEmptyPlacableCells(true);
			return true;
		}

		// 对单个棋格禁止、开启操作
		public void EnableOperationAt(Hex coord)
		{
			_GridCtrl.EnableDragRotaterAt (coord);
			_GridCtrl.EnablePressedUpPlacerAt (coord);
		}

		public void DisableOperationAt(Hex coord)
		{
			_GridCtrl.DisablePressedUpPlacerAt (coord);
			_GridCtrl.DisableDragRotaterAt (coord);
		}

		// 对整个棋盘禁止、开启操作
		public void EnableChessboardOperation()
		{
			_GridCtrl.EnablePressedUpPlacers ();
			_GridCtrl.EnableDragRotaters ();
		}

		public void DisableChessboardOperation()
		{
			_GridCtrl.DisablePressedUpPlacers ();
			_GridCtrl.DisableDragRotaters ();
		}

		// 对所有被棋子占据的棋格禁止、开启操作
		public void EnableAllChessOperation()
		{
			_GridCtrl.TurnPressedUpPlacerOnOccupiedCells (true);
			_GridCtrl.TurnDragRotaterOnOccupiedCells (true);
		}

		public void DisableAllChessOperation()
		{
			_GridCtrl.TurnPressedUpPlacerOnOccupiedCells (false);
			_GridCtrl.TurnDragRotaterOnOccupiedCells (false);
		}

		// 对所有空棋格进行开启、关闭操作
		public void EnableChessboardOpearionOnEmptyPlacableHex()
		{
			_GridCtrl.TurnDragRotatersOnEmptyPlacableCells (true);
			_GridCtrl.TurnPressedUpPlacersOnEmptyPlacableCells (true);
		}

		public void DisableChessboardOpearionOnEmptyPlacableHex()
		{
			_GridCtrl.TurnDragRotatersOnEmptyPlacableCells (false);
			_GridCtrl.TurnPressedUpPlacersOnEmptyPlacableCells (false);
		}

		// ------------------- 棋子操作 ------------------------
		// 落子
		public bool TryPlaceChessAt(int campId, int dir, Hex coord)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null) {
				return false;
			}
			if (cctrl.IsBlocked ()) {
				return false;
			}
			if (cctrl._TgtObj != null) {
				return false;
			}

			cctrl.SetDir (dir);
			_MiroMgr.SetMiroPrefabID (campId);

			_MiroMgr.SpawnAtCellCtrl (cctrl);
			cctrl.SetDir (dir);
			cctrl.PointToCurrentDir ();

			//int direction = cctrl.GetDir ();
			return true;
		}

		// 移动
		public bool TryMoveFwdAt(Hex coord)
		{
			bool bMoved = TryMoveAlongDirAt (coord, -1);
			return bMoved;
		}

		public bool TryMoveAlongDirAt(Hex coord, int dir = -1)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null) {
				return false;
			}
			if (cctrl.IsBlocked()) {
				return false;
			}
			if (cctrl._TgtObj == null) {
				return false;
			}

			if (dir >= 0 && dir <= 5) {
				cctrl.TurnToDir (dir);
			}

			CellObjCtrl fwdCCtrl = CellObjCtrlUtils.GetFwdCellObjCtrl (cctrl);
			if (fwdCCtrl == null) {
				return false;
			}
			if (fwdCCtrl.IsBlocked ()) {
				return false;
			}
			if (fwdCCtrl._TgtObj != null) {
				return false;
			}

			_MiroMgr.MoveFwdInTF (cctrl.transform);

			return false;
		}

		public bool TryMoveFromA2B(Hex A, Hex B, int dirB = -1)
		{
			CellObjCtrl ctrlA = _GridCtrl.GetCellCtrlAt (A);
			CellObjCtrl ctrlB = _GridCtrl.GetCellCtrlAt (B);

			if (ctrlA == null ) {
				return false;
			}
			if (ctrlA._TgtObj == null) {
				return false;
			}

			bool bTeleported = ctrlA.TeleportTo (ctrlB, dirB);
			return bTeleported;
		}

		// 转向
		public bool TurnRightAt(Hex coord)
		{
			bool bTurned = TurnDirAt (coord, 1);
			return bTurned;
		}

		public bool TurnLeftAt(Hex coord)
		{
			bool bTurned = TurnDirAt (coord, -1);
			return bTurned;
		}

		public bool TurnDirAt(Hex coord, int DirChange = 1)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null) {
				return false;
			}
			if (cctrl.IsBlocked()) {
				return false;
			}
			if (cctrl._TgtObj == null) {
				return false;
			}

			cctrl.TurnToDir(cctrl.GetDir () + DirChange);
			return true;
		}

		public bool SetDirAt(Hex coord, int dir)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null) {
				return false;
			}
			if (cctrl.IsBlocked()) {
				return false;
			}
			if (cctrl._TgtObj == null) {
				return false;
			}

			cctrl.SetDir (dir);
			return true;
		}

		// 移除
		// Kill:  棋子死亡，播放死亡动画
		public bool KillAt(Hex coord)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			bool bKilled = 
				_MiroMgr.KillMiroAtCellCtrl (cctrl);
			return bKilled;
		}

		// Disappear: 直接消失，无动画
		public bool DisappearAt(Hex coord)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			bool bDisappeared = _MiroMgr.DisappearMiroAtCellCtrl (cctrl);
			return bDisappeared;
		}

		// 杀死所有HP==0的棋子
		public void KillAllMaxHurtMiros()
		{
			_MiroMgr.DeleteMaxHurtMiros ();
		}

		// 生成一堆棋子
		public struct ChessInfo
		{
			public int campId;
			public Hex pos;
			public int dir;
		}
		public void PlaceChesses(
			List<ChessInfo> chesses)
		{
			foreach (ChessInfo chInfo in chesses) {
				CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (chInfo.pos);
				if (cctrl == null) {
					continue;
				}

				cctrl.SetDir (chInfo.dir);
				_MiroMgr.SetMiroPrefabID (chInfo.campId);
				_MiroMgr.SpawnAtCellCtrl (cctrl);
			}
		}


		// ----------------- 棋子属性 ------------------------
		// 获得属性值
		// 最大HP：所有生命Buff累加
		// 若为空格，则返回-1
		public int GetMaxHPAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}
			int maxHp = model.GetMaxHP ();

			return maxHp;
		}

		// 剩余HP：==最大HP-受损
		public int GetHPAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}

			int hp = model.GetHP ();
			return hp;
		}

		// 显示HP: 在画面上显示的HP数值，一般是等于剩余HP，
		// 但也可以被强制设为为某数值
		public int GetDispHPAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}
			int dhp = model.GetDispHP ();
			return dhp;
		}

		// 最大攻击力： 所有攻击Buff累加
		// 只有当棋子面对空格或敌人时，此数值才>0
		// 面对友军时，此数值==0
		// 空格时，返回-1
		public int GetMaxATAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}

			int maxAT = 
				model.GetMaxAT ();

			return maxAT;
		}

		// 当前攻击力: 最大攻击力-被敌方吸收的攻击力
		// 若为空格，返回-1
		public int GetATAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}

			int AT = 
				model.GetAT ();

			return AT;
		}

		public int GetDispATAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}

			int dispAT = 
				model.GetDispAT ();

			return dispAT;
		}

		// 吸收能力：当处于><阵型时才有此数值，为2，否则为0
		// 若为空格，返回-1
		public int GetAbsorbPowerMaxAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}
			int abPwr = 
				model.GetAbsorbingPwr ();
			return abPwr;
		}

		// 当前吸收量：当前正在吸收到敌方的攻击力的量
		// 只有当自己处于><阵型，且背对正在攻击或支援攻击的地方时，
		// 才能吸收到敌方攻击力，该数值<=吸收能力
		// 若为空格，返回-1
		public int GetAbsorbingAmtAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}
			int amt = 0;
			bool bHoleGrown = model._BlackHole.IsGrown ();
			if (bHoleGrown) {
				amt = model.GetAbsorbingAmt ();
			}
			return amt;
		}

		// 能量：表示棋子攻击的潜能，形态上==棋子“眼睛”中彩色点的数量
		// 若为空格，返回-1
		public int GetEnergyAt(Hex coord)
		{
			MiroModelV1 model = _GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return -1;
			}
			int en = 
				model._ENGenerator._EN;

			return en;
		}

		// 支援攻击力： 若棋子正在支援友军攻击，
		// 则本数值==增强友军攻击力的加成数值==眼睛中彩色点数量-被敌方吸收掉的攻击
		// 若为空格，返回-1
		public int GetAssistingATAt(Hex coord)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null) {
				return -1;
			}

			bool bAssisting = 
				CellObjCtrlUtils.ShouldAssistingAttacking (cctrl);

			if (bAssisting) {
				MiroModelV1 model = CellObjCtrlUtils.GetMiroModelFromCell (cctrl);
				int assistingAT = model.GetAssistingAT ();
				return assistingAT;
			} else {
				return 0;	
			}
		}

		// 强制设置属性显示数值
		// 注意：一般情况，棋盘会自行计算攻击力和生命值的数值，
		// 但如果算不正确，可以用这些接口来强制设置数值，
		// 不过只会影响显示出来数值，棋子形态并不会因为强制设置的数值而自动调整正确
		// 最好让棋子形态重新计算一遍，一般能够纠正正确
		public bool SetDispATAt(Hex coord, int dispAT)
		{
			CellObjCtrl cctrl = 
				_GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null && cctrl._TgtObj==null) {
				return false;
			}

			MiroModelV1 model = 
				CellObjCtrlUtils.GetMiroModelFromCell (cctrl);
			model.SetDispAT (dispAT);
			return true;
		}

		public bool SetDispHPAt(Hex coord, int dispHP)
		{
			CellObjCtrl cctrl = 
				_GridCtrl.GetCellCtrlAt (coord);
			if (cctrl == null && cctrl._TgtObj==null) {
				return false;
			}

			MiroModelV1 model = 
				CellObjCtrlUtils.GetMiroModelFromCell (cctrl);
			model.SetDispHP (dispHP);
			return true;
		}

		// 让所有棋子形态重新计算一遍
		public void UpdateMirosBuffs()
		{
			CCManager ccmgr = _MiroMgr.GetComponent<CCManager> ();
			ccmgr.Caculate ();
		}

		// ------------------- 棋子阵型 -----------------------
		// 下列接口主要返回视觉上显示出来的各种特殊阵型
		public List< List<Hex> > GetRings(int campId)
		{
			List< List<Hex> > rings = CCRing.GetRings ();
			return rings;
		}

		public List< List<Hex> > GetBackToBack(int campId)
		{
			List< List<Hex> > BB = CCFarm2.GetBackToBacks ();
			return BB;
		}

		public List< List<Hex> > GetFaceToFace(int campId)
		{
			List< List<Hex> > FF = CCHole2.GetFaceToFaces ();
			return FF;
		}

		public bool InRing(Hex coord)
		{
			List<List<Hex>> rings = 
				CCRing.GetRings ();
			foreach(var lstR in rings)
			{
				foreach (Hex h in lstR) {
					if (h.q == coord.q &&
						h.r == coord.r && 
						h.s == coord.s) {
						return true;
					}
				}
			}

			return false;
		}

		public bool InBackToBack(Hex coord)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			bool bIN = CellObjCtrlUtils.IsBackToBack (cctrl);
			return bIN;
		}

		public bool InFaceToFace(Hex coord)
		{
			CellObjCtrl cctrl = _GridCtrl.GetCellCtrlAt (coord);
			bool bIN = CellObjCtrlUtils.IsFaceToFace (cctrl);
			return bIN;
		}

		// ------------------ other --------------------//
		public void ResetChessboard()
		{
			_MiroMgr.ClearAll ();
			_GridCtrl.ResetPressedUpPlacers ();
		}

		public void ResetChessBoard(int edgeSize)
		{
			SetChessBoardSizePreset (edgeSize);

			_MiroMgr.ClearAll ();
			//_GridCtrl.ResetPressedUpPlacers ();
			_GridCtrl.ClearGrid ();
			_GridCtrl.GenGrid ();

		}

		public void SetChessBoardSizePreset(int edgeSize)
		{
			//_GridCtrl._GridLevel = edgeSize + 1;
			_GridCtrl.setGridLevel (edgeSize + 1);
		}

		// ------------------ Dynamics -------------------//
		public MiroDynamicsMgr _dynamicsMgr;
		public void TurnDynamics(bool bON)
		{
			_dynamicsMgr.TurnAllDynamics (bON);
		}

		public bool TurnDynamicsAt(Hex coord, bool bON)
		{
			MiroModelV1 model = 
				_GridCtrl.GetMiroModelAt (coord);
			if (model == null) {
				return false;
			} else {
				MiroModelDynamicsCtrl dynCtrl = model.GetComponent<MiroModelDynamicsCtrl> ();
				if (dynCtrl == null) {
					return false;
				} else {
					dynCtrl.TurnAllDynamics (bON);
					return true;
				}
			}


		}




	}
}
