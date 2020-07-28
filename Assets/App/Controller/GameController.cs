/**************************************/
//FileName: GameController.cs
//Author: wtx
//Data: 04/13/2017
//Describe: 游戏逻辑
/**************************************/

using UnityEngine;
using System;
using System.Collections;
using Pomelo.DotNetClient;
using SimpleJson;
using System.IO;
using ProtocolDefine;
using System.Collections.Generic;

public class GameController{
	private static GameController _instance = null;

	private GameController()
	{
	}

	public static GameController getInstance()
	{
		if(_instance == null)
		{
			_instance = new GameController();
		}
		return _instance;
	}

	public void addAllEvent(){
		//注册界面消息
		GameEvent.EM().AddEvent(GameEvent.EVENT.PLAY,onEventPlay);
		GameEvent.EM().AddEvent(GameEvent.EVENT.ABANDON,onEventAbandon);
		GameEvent.EM().AddEvent(GameEvent.EVENT.DRAW,onEventDraw);
		GameEvent.EM().AddEvent(GameEvent.EVENT.MOVE,onEventMove);
		GameEvent.EM().AddEvent(GameEvent.EVENT.PASS,onEventPass);

		//注册网络消息
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_START,OnLQStart);//联棋游戏开始 -播放相关动画
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_FLAG,OnLQFlag);//标志,诸如重连，特殊棋型出现
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_QI,OnLQ);//联棋棋盘数据
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_DRAW,OnRespDraw);//请和响应
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_MOVE,OnRespMove);//请求移动响应
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_PASS,OnRespPass);//请求过
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_PLAY,OnRespPlay);//请求落子
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESULT,OnLQResult);//结算
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_TURN,OnLQTurn);//该谁落子
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_ABANDON,OnLQAbandon);//联棋投降
		ProtocolManager.getInstance().addPushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_ABANDON_PASS,OnLQAbandonPass);//投降玩家自动pass

		Game.Game.reset();
	}
	public void removeAllEvent(){
		//注册界面消息
		GameEvent.EM().RemoveEvent(GameEvent.EVENT.PLAY);
		GameEvent.EM().RemoveEvent(GameEvent.EVENT.ABANDON);
		GameEvent.EM().RemoveEvent(GameEvent.EVENT.DRAW);
		GameEvent.EM().RemoveEvent(GameEvent.EVENT.MOVE);
		GameEvent.EM().RemoveEvent(GameEvent.EVENT.PASS);

		//注册网络消息
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_START);//联棋游戏开始 -播放相关动画
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_FLAG);//标志,诸如重连，特殊棋型出现
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_QI);//联棋棋盘数据
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_DRAW);//请和响应
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_MOVE);//请求移动响应
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_PASS);//请求过
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_PLAY);//请求落子
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESULT);//结算
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_TURN);//该谁落子
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_RESP_ABANDON);//联棋投降
		ProtocolManager.getInstance().removePushMsgEventListener(GameLianQiProtocol.P_GAME_LIANQI_ABANDON_PASS);//投降玩家自动pass



		Game.Game.reset();
	}
	//-----------------------------------------以下为界面消息处理--------------------------------------
	public void onEventPlay(object data){
		if (!checkSelfTurn ()) {
			//给予提示，不该自己操作
			CommonUtil.Util.showDialog ("温馨提示","当前不是您的回合阶段哟～");
			return;
		}

		if(Room.Room.getHasAbandon()){
			//已经投降了，不能操作
			CommonUtil.Util.showDialog ("温馨提示","您已经投降过了～");
			return;
		}

		GameEvent.sV2C_Play play = (GameEvent.sV2C_Play)data;

		msgLianQiReqPlay rplay = new msgLianQiReqPlay();
		rplay.direction = (LIANQI_DIRECTION_TYPE)play.direction;
		rplay.seat = Room.Room.selfSeat;
		rplay.x = play.x;
		rplay.y = play.y;

		ProtocolManager.getInstance().sendMsg(GameLianQiProtocol.P_GAME_LIANQI_REQ_PLAY,rplay,OnRespPlay);
	}
	public void onEventMove(object data){
		if (!checkSelfTurn ()) {
			//给予提示，不该自己操作
			CommonUtil.Util.showDialog ("温馨提示","当前不是您的回合阶段哟～");
			return;
		}

		if(Room.Room.getHasAbandon()){
			//已经投降了，不能操作
			return;
		}

		GameEvent.sV2C_Move move = (GameEvent.sV2C_Move)data;

		msgLianQiReqMove rmove = new msgLianQiReqMove();
		rmove.direction = (LIANQI_DIRECTION_TYPE)move.direction;
		rmove.seat = Room.Room.selfSeat;
		rmove.x = move.x;
		rmove.y = move.y;

		ProtocolManager.getInstance().sendMsg(GameLianQiProtocol.P_GAME_LIANQI_REQ_MOVE,rmove,OnRespMove);
	}
	public void onEventPass(object data){
		if (!checkSelfTurn ()) {
			CommonUtil.Util.showDialog ("温馨提示","当前不是您的回合阶段哟～");
			return;
		}
		if(Room.Room.getHasAbandon()){
			//已经投降了，不能操作
			return;
		}

		msgLianQiReqPass rpass = new msgLianQiReqPass();
		rpass.seat = Room.Room.selfSeat;

		ProtocolManager.getInstance().sendMsg(GameLianQiProtocol.P_GAME_LIANQI_REQ_PASS,rpass,OnRespPass);
	}
	public void onEventAbandon(object data){
		if (!checkSelfTurn ()) {
			CommonUtil.Util.showDialog ("温馨提示","当前不是您的回合阶段哟～");
			return;
		}

		if(Room.Room.getHasAbandon()){
			//已经投降了，不能操作
			return;
		}

		msgLianQiReqAbandon rb = new msgLianQiReqAbandon();
		rb.seat = Room.Room.selfSeat;

		ProtocolManager.getInstance().sendMsg(GameLianQiProtocol.P_GAME_LIANQI_REQ_ABANDON,rb,OnLQAbandon);
	}
	public void onEventDraw(object data){
		if (!checkSelfTurn ()) {
			CommonUtil.Util.showDialog ("温馨提示","当前不是您的回合阶段哟～");
			return;
		}

		if(Room.Room.getHasAbandon()){
			//已经投降了，不能操作
			return;
		}


		msgLianQiReqDraw rd = new msgLianQiReqDraw();
		rd.seat = Room.Room.selfSeat;

		ProtocolManager.getInstance().sendMsg(GameLianQiProtocol.P_GAME_LIANQI_REQ_DRAW,rd,OnRespDraw);
	}

	//------------------------------------------以下为网络消息处理---------------------------------------
	void OnLQStart(Message msg)
	{
		//联棋游戏开始 -播放相关动画
		msgLianQiStart resp = msgLianQiStart.deserialize(msg);
		if (resp.flag == 0) {
			Game.Game.firstHandSeat = resp.firstHandSeat;

			GameEvent.EM ().InvokeEvent (GameEvent.EVENT.SHOW_GAME_START, (object)Game.Game.firstHandSeat);
		}
	}
	void OnLQFlag(Message msg)
	{
		//标志,诸如重连，特殊棋型出现
		// 重连期间不播放任何动画，请根据标志判断
		msgLianQiFlag resp = msgLianQiFlag.deserialize(msg);
		if (resp.flag == msgLianQiFlag.FLAG_TYPE.RELINK_TYPE_BEGIN) {
			//
		} else if (resp.flag == msgLianQiFlag.FLAG_TYPE.RELINK_TYPE_END) {
			//
		}

		//用户动画以及重连标示控制
		GameEvent.EM().InvokeEvent (GameEvent.EVENT.SHOW_FLAG,(object)resp.flag);
	}
	void OnLQ(Message msg)
	{
		//联棋棋盘数据
		//此消息原则上只在重连的时候收到，以此更新棋盘
		msgLianQi resp = msgLianQi.deserialize(msg);

		//刷新棋盘
		List<GameEvent.Chess> checkerBoard = getCheckerBoard(resp.checkerBoard);
		GameEvent.EM ().InvokeEvent (GameEvent.EVENT.SHOW_LIANQI,(object)checkerBoard);

		/*
		// 设置当前下棋的人
		Game.Game.currentTurn = resp.turn;

		GameEvent.sC2V_ShowTurn st;
		st.isPassTurn = true;
		st.isTimeOut = false;
		st.round = 0;//原则上需要这个值，但是也就影响一轮，暂时不填
		st.local = Room.Room.getLocalBySeat (resp.turn);

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_TURN,(object)(st));
		*/

	}
	void OnRespDraw(Message msg)
	{
		//请和响	应
		msgLianQiRespDraw resp = msgLianQiRespDraw.deserialize(msg);

		if (resp.type == REQ_RESP_TYPE.REQ) {
			//别的玩家请求和棋，向大家显示请求和棋的提示
			GameEvent.sC2V_Draw dr;
			dr.local = Room.Room.getLocalBySeat(resp.seat);
			dr.name = Room.Room.getPlayerNameBySeat(resp.seat);
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_DRAW,(object)dr);
		}else if(resp.type == REQ_RESP_TYPE.REQ_RESP){
			//收到别人是否同意的和棋
			GameEvent.sC2V_Draw dr;
			dr.local = Room.Room.getLocalBySeat(resp.seat);
			dr.name = Room.Room.getPlayerNameBySeat(resp.seat);
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_DRAW_RESULT,(object)dr);

		}else if(resp.type == REQ_RESP_TYPE.RESP){
			// 自己请求或者响应请求的 响应，无需处理
		}
	}
	void OnRespPass(Message msg)
	{
		//请求过
		msgLianQiRespPass resp = msgLianQiRespPass.deserialize(msg);
		if (resp.flag == 0) {
			Game.Game.currentTurn = resp.turn;
			GameEvent.EM ().InvokeEvent (GameEvent.EVENT.SHOW_PASS, (object)(Room.Room.getLocalBySeat (resp.turn)));
		} else {
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_FAIL,(object)GameEvent.EVENT.PASS);
		}
	}
	void OnRespPlay(Message msg)
	{
		//请求落子
		msgLianQiRespPlay resp = msgLianQiRespPlay.deserialize(msg);

		if (resp.flag != GAME_OP_RESP_FLAG.SUCCESS) {
			//不成功
			//根据具体值给予错误提示
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_FAIL,(object)GameEvent.EVENT.PLAY);
			return;
		}

		List<GameEvent.Chess> checkerBoard = getCheckerBoard(resp.checkerBoard);

		GameEvent.sC2V_PlayOrMove pm;
		pm.checkerBoard = checkerBoard;
		pm.direction = (GameEvent.LIANQI_DIRECTION_TYPE)resp.direction;
		pm.local = Room.Room.getLocalBySeat(resp.seat);
		pm.x = resp.x;
		pm.y = resp.y;

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_PLAY,(object)pm);
	}
	void OnRespMove(Message msg)
	{
		//请求移动响应
		msgLianQiRespMove resp = msgLianQiRespMove.deserialize(msg);

		if (resp.flag != GAME_OP_RESP_FLAG.SUCCESS) {
			//不成功
			//根据具体值给予错误提示
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.ACTION_FAIL,(object)GameEvent.EVENT.MOVE);
			return;
		}

		List<GameEvent.Chess> checkerBoard = getCheckerBoard(resp.checkerBoard);

		GameEvent.sC2V_PlayOrMove pm;
		pm.checkerBoard = checkerBoard;
		pm.direction = (GameEvent.LIANQI_DIRECTION_TYPE)resp.direction;
		pm.local = Room.Room.getLocalBySeat(resp.seat);
		pm.x = resp.x;
		pm.y = resp.y;

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_MOVE,(object)pm);
	}
	void OnLQResult(Message msg)
	{
		//结算
		msgLianQiResult resp = msgLianQiResult.deserialize(msg);

		//游戏结果，较为复杂，待实现
		GameEvent.sV2V_ShowGameResult result;
		result.roomType = Room.Room.roomType;
		result.level = (CommonDefine.ePlazaLevelType)Room.Room.roomLevel;
		result.baseScore = Room.Room.baseScore;
		result.poolGold = resp.poolGold;

		result.gameResult = new List<GameEvent.sV2V_ShowGameResult.sGameResult>();
		for (int i = 0; i < resp.result.Count; i++) {
			Room.Player? player = Room.Room.getPlayerBySeat(resp.result[i].seat);

			if (player == null)
				//error
				return;

			GameEvent.sV2V_ShowGameResult.sGameResult  gr;
			gr.seat = resp.result[i].seat;
			gr.isOwner = player.Value.isOwner;
			gr.head = player.Value.head;
			gr.name = player.Value.name;
			gr.area = resp.result[i].area;
			gr.kill = resp.result[i].kill;
			gr.score = resp.result[i].score;
			gr.multi = resp.result[i].multi;
			gr.hasAbandon = resp.result[i].hasAbandon;
			result.gameResult.Add (gr);

			//暂时在此处进行修改用户金币信息，后续新增更新用户基本信息消息
			if (gr.seat == Room.Room.selfSeat) {
				Account.updateUserGold( gr.score - result.baseScore * resp.result[i].multi);
				//LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.UPDATE_USER_INFO, (object)self);
			}
		}

		GameEvent.EM ().InvokeEvent (GameEvent.EVENT.SHOW_RESULT,(object)result);
	}
	void OnLQTurn(Message msg)
	{
		//该谁落子
		msgLianQiTurn resp = msgLianQiTurn.deserialize(msg);

		Game.Game.currentTurn = resp.seat;

		GameEvent.sC2V_ShowTurn st;
		st.isPassTurn = resp.isPassTurn;
		st.isTimeOut = resp.isTimeOut;
		st.round = resp.round;
		st.local = Room.Room.getLocalBySeat (resp.seat);
		st.banDirList = new List<int> ();

		for (int i = 0; i < resp.lmt.Count; i++) {
			st.banDirList.Add ((int)resp.lmt[i]);
		}

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_TURN,(object)(st));
	}
	void OnLQAbandonPass(Message msg)
	{
		msgLianQiAbandonPass resp = msgLianQiAbandonPass.deserialize(msg);

		GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_ABANDON_PASS,(object)(Room.Room.getLocalBySeat(resp.seat)));
	}
	void OnLQAbandon(Message msg)
	{
		msgLianQiRespAbandon resp = msgLianQiRespAbandon.deserialize(msg);
		if (resp.seat != Room.Room.selfSeat) {
			GameEvent.EM().InvokeEvent(GameEvent.EVENT.SHOW_ABANDON,(object)(Room.Room.getLocalBySeat(resp.seat)));
		} else {
			//自己投降的响应
			Room.Room.setHasAbandon ();
		}
	}

	//-------------------------一些封装------------------------------
	bool checkSelfTurn(){
		return Game.Game.currentTurn == Room.Room.selfSeat;
	}
	List<GameEvent.Chess> getCheckerBoard(List<ProtocolDefine.Chess> checkerBoard){

		//首选全部清空
		List<GameEvent.Chess> gameBord = new List<GameEvent.Chess>();

		for(int i = 0;i <checkerBoard.Count;i++){
			GameEvent.Chess ch = new GameEvent.Chess ();
			ch.buffList = new List<GameEvent.Buff> ();

			for (int j = 0; j < checkerBoard [i].buffList.Count; j++) {
				GameEvent.Buff bf = new GameEvent.Buff();
				bf.absorbChange = checkerBoard [i].buffList [j].absorbChange;
				bf.attackChange = checkerBoard [i].buffList [j].attackChange;
				bf.healthChange = checkerBoard [i].buffList [j].healthChange;
				bf.type = checkerBoard [i].buffList [j].type;

				ch.buffList.Add(bf);
			}
			ch.skillList = new List<GameEvent.Skill> ();
			for (int j = 0; j < checkerBoard [i].skillList.Count; j++) {
				GameEvent.Skill sk = new GameEvent.Skill (); 
				sk.absorbChange = checkerBoard [i].skillList [j].absorbChange;
				sk.applyPosX = checkerBoard [i].skillList [j].absorbChange;
				sk.applyPosY = checkerBoard [i].skillList [j].absorbChange;
				sk.applyPosZ = checkerBoard [i].skillList [j].absorbChange;
				sk.attackChange = checkerBoard [i].skillList [j].absorbChange;
				sk.basePosX = checkerBoard [i].skillList [j].absorbChange;
				sk.basePosY = checkerBoard [i].skillList [j].absorbChange;
				sk.basePosZ = checkerBoard [i].skillList [j].absorbChange;
				sk.healthChange = checkerBoard [i].skillList [j].absorbChange;
				sk.type = checkerBoard [i].skillList [j].type;

				ch.skillList.Add(sk);
			}
		
			ch.absorb = checkerBoard [i].absorb;
			ch.attack = checkerBoard [i].attack;
			ch.direction = checkerBoard [i].direction;
			ch.health = checkerBoard [i].health;
			ch.playerID = checkerBoard [i].playerID;
			ch.support = checkerBoard [i].support;
			ch.x = checkerBoard [i].x;
			ch.y = checkerBoard [i].y;

			gameBord.Add(ch);
		}
		return gameBord;
	}
}