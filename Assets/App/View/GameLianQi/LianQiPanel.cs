using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LianQiPanel : MonoBehaviour {

    public int _PlayerNum;          //玩家人数
    public int _BoardLevel;         //棋盘阶数

	public int _firstHandSeat;    //先手玩家

	public MiroV1.MiroChessBoardPort _Port;

	private bool _ifOpenHint;// 是否开启棋子信息提示

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
    }

	void OnDestroy(){
		//remove all event
		removeAllEvent();
	}
	void OnEnable(){
		addAllEvent ();
	}
	void OnDisable(){
		removeAllEvent();
	}

	void addAllEvent(){
		GameEvent.EM ().AddEvent (GameEvent.EVENT.ACTION_PLAY, onActionPlay);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.ACTION_PASS, onActionPass);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.ACTION_MOVE, onActionMove);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.ACTION_STEP, onActionStep);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.ACTION_BAN_DIR, onActionBanDir);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.TO_LQP_SWITCH_HINT, onSwitchHint);


		GameEvent.EM ().AddEvent (GameEvent.EVENT.TO_LQP_ACTION_FAIL, onActionFail);
		GameEvent.EM ().AddEvent (GameEvent.EVENT.TO_LQP_CLOCK, onShowClock);
	}
	void removeAllEvent(){
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.ACTION_PLAY);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.ACTION_PASS);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.ACTION_MOVE);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.ACTION_STEP);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.ACTION_BAN_DIR);

		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.TO_LQP_SWITCH_HINT);
		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.TO_LQP_ACTION_FAIL);


		GameEvent.EM ().RemoveEvent (GameEvent.EVENT.TO_LQP_CLOCK);
	}

	//-------------------------- 界面事件-----------------------------
	public void onInit(object data){
		RoomEvent.sC2V_RoomRule rr = (RoomEvent.sC2V_RoomRule)data;
		_PlayerNum = rr.playerNum;
		_BoardLevel = rr.gridLevel;

		_Port.ResetChessBoard (_BoardLevel);

		_Port.SetCampsCount (_PlayerNum);

		_ifOpenHint = true;//默认开启，该类设置信息，需要后续根据用户习惯，从本地读取配置

		// 代码里不能修改prefab，否则实际的prefab也会发生变化
		/*
		_GridPrefab.transform.localScale =  new Vector3(6.0f/_BoardLevel, 6.0f/_BoardLevel,1.0f);
		for (int i = 0; i < _ChessPrefab.Length; i++) {
			_ChessPrefab[i].transform.localScale = new Vector3(6.0f/_BoardLevel, 6.0f/_BoardLevel,1.0f);
		}
		*/
	}

	// 切换显示棋子信息提示
	public void onSwitchHint(object data){

		_ifOpenHint = !_ifOpenHint;
	}

	public void onActionPlay(object data){

		Hex coord;
		int dir;
		int campId;

		if (!_Port.GetOperatingChessInfo (out coord, out dir, out campId)) {

			CommonUtil.Util.showDialog ("温馨提示","您还没有落子，请落子");

			return;
		}

		GameEvent.sV2C_Play pdata;

		pdata.direction = (GameEvent.LIANQI_DIRECTION_TYPE)dir;
		pdata.x = CommonUtil.Util.Hex2Point(coord).x;
		pdata.y = CommonUtil.Util.Hex2Point(coord).y;
		GameEvent.EM().InvokeEvent(GameEvent.EVENT.PLAY,(object)pdata);
	}
	public void onActionPass(object data){
		GameEvent.EM().InvokeEvent(GameEvent.EVENT.PASS,null);
	}
	public void  onActionMove(object data){

		GameEvent.sV2C_Move mdata;

		//请根据实际数据填写
		mdata.direction = GameEvent.LIANQI_DIRECTION_TYPE.LIANQI_DIRECTION_TYPE_0;
		mdata.x = 1;
		mdata.y = 1;
		GameEvent.EM().InvokeEvent(GameEvent.EVENT.MOVE,(object)mdata);
	}

	public void onActionBanDir(object data){

		GameEvent.sV2V_ActionBanDir abd = (GameEvent.sV2V_ActionBanDir )data;;

		_Port.SetBanDir (abd.ban,abd.dir);
	}

	public void onActionStep(object data){
		ActionPanel.eActionStep step = (ActionPanel.eActionStep)data;

		if (step == ActionPanel.eActionStep.STEP_NOT_TURN) {
			_Port.DisableChessboardOperation ();
			//_Port.DisableChessboardOpearionOnEmptyPlacableHex ();
			//_Port.DisableAllChessOperation();

			//_Port.TurnDynamics (false);

		} else {
			//_Port.EnableChessboardOperation ();
			_Port.EnableChessboardOpearionOnEmptyPlacableHex ();
		}
	}

	public void  onLQShowPass(object data){

		//end turn next
		_Port.ChooseOperatingCamp ((_Port.GetOperatingCampId()+1)%_PlayerNum);

	}
	public void onShowAbandonPass(object data){
		onLQShowPass (data);
	}
	public void  onLQShowTurn(object data){

		GameEvent.sC2V_ShowTurn st = (GameEvent.sC2V_ShowTurn)data;

		// 如果是别人pass导致的turn变化，则切换手，否则是第一次发turn，不需要切换
		if (st.isPassTurn) {
			_Port.ChooseOperatingCamp ((_Port.GetOperatingCampId()+1)%_PlayerNum);
		}


		//超时的换手，则清除试棋的棋子
		if (st.isTimeOut) {
			//如果存在试棋的棋子则删除
			Hex coord;
			int dir;
			int campId;
			if (_Port.GetOperatingChessInfo (out coord, out dir, out campId)) {

				_Port.CancelOperatingChess (out coord, out dir, out campId);
			}
		}
	}
	public void  onLQShowPlay(object data){

		//这里要判断是否是自己在下棋，如果不是则仅仅刷新
		GameEvent.sC2V_PlayOrMove pm = (GameEvent.sC2V_PlayOrMove)data;

		//调用下棋接口
		Hex pos = CommonUtil.Util.Point2Hex(new Vector3i(pm.x,pm.y,0));

		if (pm.local == (int)CommonDefine.SEAT.SELF) {
			Hex coord;
			int dir;
			int campId;

			if (!_Port.ConfirmOperatingChess (out coord, out dir, out campId)) {

				CommonUtil.Util.showDialog ("温馨提示", "操作出错了。。。");

				//原则上不应该出现
				if (!_Port.IsEmptyAt (pos)) {
					//删除已有棋子
					_Port.DisappearAt(pos);
				}

				_Port.TryPlaceChessAt (getPlayerIDByLocal (pm.local), (int)pm.direction, pos);

			}

			//此时关闭
			//_Port.DisableAllChessOperation();

		} else {

			//原则上不应该出现
			if (!_Port.IsEmptyAt (pos)) {
				//删除已有棋子
				_Port.DisappearAt(pos);
			}

			_Port.TryPlaceChessAt (getPlayerIDByLocal (pm.local), (int)pm.direction, pos);
		}

		//更新各个玩家的棋子数
		updatePlayerChessNum ();
	}
	public void  onLQShowMove(object data){
		
	}

	public void onUpdateFirstHandSeat(object data){
		_firstHandSeat = (int)data;
	}

	//断线重连
	public void onShowLianQi(List<GameEvent.Chess> cb){

		//清空棋盘
		_Port.ResetChessboard();

		foreach(GameEvent.Chess chess in cb){

			Hex pos = CommonUtil.Util.Point2Hex(new Vector3i(chess.x,chess.y,0));

			_Port.TryPlaceChessAt (getPlayerIDByLocal (getLocalByPlayID(chess.playerID)), (int)chess.direction, pos);	
		}
	}
	//------------------------------网络事件-----------------------------
	public void onActionFail(object data){
		switch ((GameEvent.EVENT)data) {
		case GameEvent.EVENT.ABANDON:
			break;
		case GameEvent.EVENT.DRAW:
			break;
		case GameEvent.EVENT.PLAY:
			break;
		case GameEvent.EVENT.PASS:
			break;
		case GameEvent.EVENT.MOVE:
			break;
		}
	}

	public void onShowClock(object data){
		
	}
	//-----------------------------内部处理------------------------------
	private void updatePlayerChessNum(){
		//这里更新下当前各自玩家的棋子数，只有收到落子才更新
		int[] count = new int[4] ;//最多4人，这里全用
		for (int i = 0; i < 4; i++) {
			count [i] = 0;
		}

		//此处可以自己记录值，也可以通过接口获取
		/*
		foreach (LianQiChess cc in _ChessList) {
			count [cc._PlayerID]++;
		}
		*/

		//更新界面
		GameEvent.EM().InvokeEvent(GameEvent.EVENT.TO_GAMEVEIW_UPDATE_SCORE,(object)count);
	}
		
	private int getLocalByPlayID(int playerId){

		return CommonUtil.Util.getLocalBySeat ((_firstHandSeat + playerId)%_PlayerNum);
	}

	private int getPlayerIDByLocal(int local){
		int seat = CommonUtil.Util.getSeatByLocal(local);
		return (_firstHandSeat + seat)%_PlayerNum;
	}
}
