/**************************************/
//FileName: RoomController.cs
//Author: wtx
//Data: 04/13/2017
//Describe: 房间相关逻辑控制
/**************************************/

using UnityEngine;
using System;
using System.Collections;
using Pomelo.DotNetClient;
using SimpleJson;
using System.IO;
using ProtocolDefine;
using CommonDefine;
using System.Collections.Generic;


public class RoomController{
	private static RoomController _instance = null;

	private RoomController()
	{
	}

	public static RoomController getInstance()
	{
		if(_instance == null)
		{
			_instance = new RoomController();
		}
		return _instance;
	}
	public void addRoomLobbyEvent(){
		//注册网络消息
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_ROOMSTATE_CHANGE,OnRoomStateChange);//房间状态变化
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_PLAYER_INFO,OnPlayerEnter);//玩家进入房间
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_PLAYER_STATE,OnPlayerState);//玩家准备等
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_PLAYER_LEAVE,OnPlayerLeave);//玩家离开房间
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_START_GAME,OnGameStart);//收到游戏开始通知
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_TALENT_LIST,OnTalentList);//收到天赋列表

		//注册界面消息
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.CREATE_ROOM,onEventCreateRoom);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.JOIN_ROOM,onEventJoinRoom);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.GET_ROOM_LIST,onEventGetRoomList);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.LEAVE_ROOM,onEventLeaveRoom);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.PLAYER_ACT,onEventPlayerAct);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.SHOW_ROOM_LIST,onEventShowRoomList);//界面自己请求房间列表数据，即该界面显示的时候invoke
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.START_GAME,onEventStartGame);
	}
	public void removeRoomLobbyEvent(){
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_ROOMSTATE_CHANGE);//房间状态变化
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_PLAYER_INFO);//玩家进入房间
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_PLAYER_STATE);//玩家准备等
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_PLAYER_LEAVE);//玩家离开房间
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_START_GAME);//收到游戏开始通知
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_TALENT_LIST);//收到天赋列表

		//
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.CREATE_ROOM);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.JOIN_ROOM);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.GET_ROOM_LIST);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.LEAVE_ROOM);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.PLAYER_ACT);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.SHOW_ROOM_LIST);//界面自己请求房间列表数据，即该界面显示的时候invoke
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.START_GAME);

	}
	public void addRoomGameEvent(){
		/// reqJoinRoom->respJoinRoom->playerEnter->reqPlayerAct(非自动准备情况)->respPlayerAct/OnPlayerState->start->leave
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_PLAYER_INFO,OnPlayerEnter);//玩家进入房间
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_PLAYER_STATE,OnPlayerState);//玩家准备等
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_PLAYER_TALK_MSG,OnPlayerTalk);//玩家聊天
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_PLAYER_LEAVE,OnPlayerLeave);//玩家离开房间
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_CLOCK,OnPlayerClock);//玩家步骤剩余时间
		ProtocolManager.getInstance().addPushMsgEventListener(GameProtocol.P_GAME_TALENT_LIST,OnTalentList);//收到天赋列表


		RoomEvent.EM().AddEvent(RoomEvent.EVENT.ENTER_ROOM_FINISH,onEnterRoomFinish);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.LEAVE_ROOM,onEventLeaveRoom);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.PLAYER_ACT,onEventPlayerAct);
		RoomEvent.EM().AddEvent(RoomEvent.EVENT.TALK_MSG,onEventPlayerTalk);
	}
	public void removeRoomGameEvent(){

		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_PLAYER_INFO);//玩家进入房间
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_PLAYER_STATE);//玩家准备等
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_PLAYER_TALK_MSG);//玩家聊天
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_PLAYER_LEAVE);//玩家离开房间
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_CLOCK);//玩家步骤剩余时间
		ProtocolManager.getInstance().removePushMsgEventListener(GameProtocol.P_GAME_TALENT_LIST);//收到天赋列表


		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.ENTER_ROOM_FINISH);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.LEAVE_ROOM);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.PLAYER_ACT);
		RoomEvent.EM().RemoveEvent(RoomEvent.EVENT.TALK_MSG);
		//清除数据
		Room.Room.reset();
	}
	//------------------------------------以下是界面消息--------------------------------------------
	void onEventCreateRoom(object data){
		
		RoomEvent.sV2C_CreateRoom roomData = (RoomEvent.sV2C_CreateRoom)data;

		msgReqCreateRoom cr = new msgReqCreateRoom();
		cr.game =  GameType.GAME_LIANQI;
		cr.roomType = roomData.roomType;
		cr.baseScore = roomData.baseScore;
		cr.minScore = roomData.minScore;
		cr.maxScore = roomData.maxScore;
		cr.roomName = Account.getSelfData().name;
		cr.roomPassword = roomData.roomPassword;
		cr.rule = roomData.rule;

		ProtocolManager.getInstance().sendMsg(GameProtocol.P_GAME_REQ_CREATEROOM,cr,OnRespCreateRoom);

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);

	}
	void onEventJoinRoom(object data){

		RoomEvent.sV2C_JoinRoom roomData = (RoomEvent.sV2C_JoinRoom)data;

		msgReqJoinRoom jr = new msgReqJoinRoom();
		jr.game  = GameType.GAME_LIANQI;
		jr.playerNum = roomData.playerNum;
		jr.gridLevel = roomData.gridLevel;
		jr.pwd = roomData.pwd;//非用户创建无密码
		jr.plazaID = roomData.plazaID;//根据plazalist得到，界面也是根据plazalisy生成
		jr.roomId = roomData.roomId;//非用户创建房间填0，即通过各种模式直接进入游戏的

		//如果plazaid和roomid同时为0 则认为是经典快速开始模式
		if(roomData.plazaID == 0 && roomData.roomId == 0){
			//需要从plazalist查找plazaid
			for (int i = 0; i < Lobby.Lobby.plazaList.Count; i++) {
				CommonUtil.RoomRule roomRule = CommonUtil.RoomRule.deserialize(Lobby.Lobby.plazaList[i].rule);

				if(Lobby.Lobby.plazaList[i].roomType == (int)CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA
					&& roomRule.playerNum == roomData.playerNum
					&& roomRule.gridLevel == roomData.gridLevel){
					jr.plazaID = Lobby.Lobby.plazaList[i].plazaid;
					jr.roomId = 0;
				}
			}
		}
		//保存下如果是场模式的信息
		Room.Room.setPlazaData(roomData.plazaName,roomData.tagId);

		ProtocolManager.getInstance().sendMsg(GameProtocol.P_GAME_REQ_JOINROOM,jr,OnRespJoinRoom);

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);

	}
	void onEventLeaveRoom(object data){
		bool mustLeave = (bool)data;
		if (mustLeave) {
			Room.Room.reset ();

			ViewManagerEvent.sShowView s;
			s.fromView = ViewManagerEvent.VIEW_TYPE.GAME_VIEW;
			s.toView = ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW;
			ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_VIEW, (object)s);

			RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_DISSOLVE_ROOM,null);

		} else {
			msgReqLeaveRoom lr = new msgReqLeaveRoom ();
			lr.game = GameType.GAME_LIANQI;
			ProtocolManager.getInstance ().sendMsg (GameProtocol.P_GAME_REQ_LEAVEROOM, lr, OnRespLeaveRoom);
		}
	}
	void onEventStartGame(object data){
		//此时可以启动游戏开始
		msgNotifyStartGame sgmsg = new msgNotifyStartGame ();
		sgmsg.game = GameType.GAME_LIANQI;
		sgmsg.isEnterRoomFinsh = (bool)data;
		ProtocolManager.getInstance ().sendMsg (GameProtocol.P_GAME_START_GAME, sgmsg);
	}
	void onEnterRoomFinish(object data){
		//刷新房间规则显示
		RoomEvent.sC2V_RoomRule rr;
		rr.playerNum = Room.Room.roomRule.playerNum;
		rr.gameTime = Room.Room.roomRule.gameTime;
		rr.gridLevel = Room.Room.roomRule.gridLevel;
		rr.rule = Room.Room.roomRule.rule;
		rr.lmtRound = Room.Room.roomRule.lmtRound;
		rr.lmtTurnTime = Room.Room.roomRule.lmtTurnTime;
		rr.roomLevel = Room.Room.roomLevel;
		rr.roomID = 0;//此处不需要，填0
		rr.plazaName = Room.Room.plazaName;
		rr.tag = Room.Room.tagId;
		rr.type = (CommonDefine.eCreateRoomType)Room.Room.roomType;

		if (Room.Room.plazaid != 0) {
			LobbyEvent.Plaza plaza = getPlazaById(Room.Room.plazaid);
			rr.star = plaza.star;
		} else {
			rr.star = 0;
		}

		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.UPDATE_ROOMRULE,(object)rr);

		if (Room.Room.roomType == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA
			||Room.Room.roomType == CommonDefine.eCreateRoomType.ROOM_PLAZA
			|| Room.Room.isRelink) {
			//完全进入游戏界面后，需要通知服务器可以发用户enter信息了,原则上来说，需要界面真正加载完成后再发,这里暂时直接发送
			msgNotifyEnterRoomFinish efmsg = new msgNotifyEnterRoomFinish ();
			efmsg.isRelink = Room.Room.isRelink;
			ProtocolManager.getInstance ().sendMsg (GameProtocol.P_GAME_ENTER_ROOM_FINISH, efmsg);
		} else {
			// 因为房间和组队模式，用户相关信息已经下发，所以需要手动驱动相关消息刷新游戏界面
			for(int i = 0;i<Room.Room.playerList.Count;i++){
				/////////////////////////////enter/////////////////////////////////
				RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.PLAER_ENTER,Room.Room.playerList[i]);

				///////////////////////state/////////////////////////////////
				RoomEvent.sC2V_PlayerState ps;
				ps.state = (CommonDefine.PLAYER_STATE)Room.Room.playerList[i].state;
				ps.local = Room.Room.getLocalBySeat(Room.Room.playerList[i].seat);
				ps.ifAllReady = true;

				RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_PLAER_STATE,(object)ps);
			}

			//进入游戏后的start game
			onEventStartGame (true);
		}
	}
	void onEventGetRoomList(object data){
		SelfData self = Account.getSelfData ();

		RoomEvent.sV2C_GetRoomList getRoom = (RoomEvent.sV2C_GetRoomList)data;

		msgReqRoomList rl = new msgReqRoomList();
		rl.areaID =  self.area;

		//原则上来说，以后这两个数据需要传过来
		rl.begin = getRoom.currentPage * getRoom.perCnt;
		rl.reqCnt = getRoom.perCnt;//所有

		rl.game = GameType.GAME_LIANQI;
		ProtocolManager.getInstance().sendMsg(GameProtocol.P_GAME_REQ_ROOMLIST,rl,OnRespRoomList);

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);

	}
	//用户点击准备或者自动准备
	void onEventPlayerAct(object data){

		RoomEvent.sV2C_PlayerAct pact = (RoomEvent.sV2C_PlayerAct)data;

		Room.Player? player = Room.Room.getPlayerBySeat (Room.Room.selfSeat);
		if (player.Value.talentList.Count == 0 
			&& Room.Room.roomType == CommonDefine.eCreateRoomType.ROOM_ROOM /*只有房间模式才提示*/) {

			//天赋未配置，作出提示，这里可以直接到请求准备到时候提示
			CommonUtil.Util.showDialog("温馨提示","您未配置天赋，请配置后再准备");
			return;
		}


		msgPlayerAct pa = new msgPlayerAct ();
		pa.act = (msgPlayerAct.ACT_TYPE)(pact.act);
		pa.seat = Room.Room.selfSeat;

		ProtocolManager.getInstance().sendMsg(GameProtocol.P_GAME_PLAYER_ACT,pa,OnRespPlayerAct);
	}
	void onEventPlayerTalk(object data){
		RoomEvent.sV2C_PlayerTalk ptalk = (RoomEvent.sV2C_PlayerTalk)data;

		msgPlayerTalkMsg pk = new msgPlayerTalkMsg();
		pk.seat = Room.Room.selfSeat;
		pk.content = ptalk.content;

		ProtocolManager.getInstance().sendMsg(GameProtocol.P_GAME_PLAYER_TALK_MSG,pk,OnRespPlayerTalkMsg);
	}
	public void onEventShowRoomList(object data){
		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.SHOW_ROOM_LIST,(object)(Room.Room.roomList));
	}

	void onEventReportTalent(object data){
		RoomEvent.sV2C_PlayerTalentList ptl = (RoomEvent.sV2C_PlayerTalentList)data;

		msgTalentList ot = new msgTalentList();
		ot.seat = Room.Room.selfSeat;
		ot.talentList = "";
		//ot.talentList = string.Join(",",ptl.talentList);
		for(int i = 0 ;i < ptl.talentList.Count;i++){
			if (i < ptl.talentList.Count - 1) {
				ot.talentList += ptl.talentList [i] + ",";
			} else {
				ot.talentList += ptl.talentList [i];
			}
		}

		if (ptl.talentList.Count == 0) {
			ot.talentList = "0,0,0,0";
		}

		ProtocolManager.getInstance().sendMsg(GameProtocol.P_GAME_REPORT_TALENT_LIST,ot,OnTalentList);
	}
	//-----------------------------------以下是网络消息----------------------------------------------
	void OnTalentList(Message msg){
		msgTalentList resp = msgTalentList.deserialize(msg);

		if (resp.seat == Room.Room.selfSeat) {
			//自己的天赋列表，原则上可以不用刷新了，因为本地知道

			//可刷新缓存，也可以不刷新
		} else {
			//别人的天赋列表，刷新别人天赋列表，并记录

			Room.Player? player = Room.Room.getPlayerBySeat (resp.seat);
			player.Value.talentList.Clear ();

			string[] tl = resp.talentList.Split(',');

			for(int i = 0;i < tl.Length;i++){
				player.Value.talentList.Add ((CommonDefine.eTalentType)(Convert.ToInt32(tl[i])));
			}
		}

		//刷新天赋显示界面
	}
	void OnGameStart(Message msg){
		//
		ViewManagerEvent.sShowView data;
		data.fromView = ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW;
		data.toView = ViewManagerEvent.VIEW_TYPE.GAME_VIEW;
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_VIEW, (object)data);
	}
	//玩家进入房间
	void OnPlayerEnter(Message msg)
	{
		msgPlayerInfo resp = msgPlayerInfo.deserialize(msg);

		SelfData self = Account.getSelfData ();
		if(resp.userID != self.userID && Room.Room.playerList.Count == 0){
			Debug.Log("第一个进来的玩家不是自己，这会导致座位错误，无法处理，出错！");
			return;
		}

		//将数据写入游戏房间数据
		Room.Player player = new Room.Player();
		player.charm = resp.charm;
		player.draw = resp.draw;
		player.escapse = resp.escape;
		player.vip = resp.vip;
		player.gold = resp.gold;
		player.head = resp.headUrl;
		player.lose = resp.lose;
		player.name = resp.name;
		player.score = resp.score;
		player.seat = resp.seat;
		player.sex = resp.sex;
		player.state = CommonDefine.PLAYER_STATE.STATE_TYPE_SITDOWN;// 默认坐下状态，接下来要准备
		player.userID = resp.userID;
		player.win = resp.win;
		player.isOwner = Room.Room.owner == resp.userID;

		player.talentList = new List<eTalentType> ();
		//进来的如果是自己，则初始化天赋列表
		//从自己的数据中获取
		if(resp.userID == self.userID){
			//self
			player.talentList.AddRange(self.talentList);
		}

		//原则上来说，第一个进入的必须是自己，否则不应该处理
		Room.Room.addPlayer(player);

		//显示用户， 此处需要将seat转为local
		RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.PLAER_ENTER,player);
	}
	//玩家准备等
	void OnPlayerState(Message msg)
	{
		msgPlayerState resp = msgPlayerState.deserialize(msg);

		if (Room.Room.updatePlayerStateByUserID (resp.userID,(CommonDefine.PLAYER_STATE)resp.state)) {
			//用户存在，则通知更新用户状态
			//诸如已准备、离线等
			//纯界面数据定义，请定义在roomevent里
			RoomEvent.sC2V_PlayerState ps;
			ps.state = (CommonDefine.PLAYER_STATE)resp.state;
			ps.local = Room.Room.getLocalBySeat(resp.seat);
			ps.ifAllReady = Room.Room.getIfAllPlayerReady ();

			RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_PLAER_STATE,(object)ps);
		}
	}
	//玩家聊天
	void OnPlayerTalk(Message msg)
	{
		msgPlayerTalkMsg resp = msgPlayerTalkMsg.deserialize(msg);

		if (resp.flag == msgSimpleResp.eFlag.SUCCESS) {

			RoomEvent.sC2V_PlayerTalk pt;
			pt.local = Room.Room.getLocalBySeat (resp.seat);
			pt.content = resp.content;
			RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.SHOW_TALK_MSG, (object)pt);
		}
	}
	//玩家离开房间
	void OnPlayerLeave(Message msg)
	{
		//自己离开收到的是另外一个消息：msgRespLeaveRoom，如果是非请求离开，则会收到这个，需要根据id判断是否是自己。
		//目前不支持t人，所以自己一般不会收到这个消息
		msgPlayerLeave resp = msgPlayerLeave.deserialize(msg);

		SelfData self = Account.getSelfData ();
		if (resp.userID == self.userID) {
			if (resp.type == msgPlayerLeave.PLAYER_LEAVE_ROOM_TYPE.LEAVE_ROOM_GAMEEND) {
				//此时清空房间缓存数据
				Room.Room.reset();
			
			}else if(resp.type == msgPlayerLeave.PLAYER_LEAVE_ROOM_TYPE.LEAVE_ROOM_REMOVED){
				
			}

		}else{
			if (msgPlayerLeave.PLAYER_LEAVE_ROOM_TYPE.LEAVE_ROOM_OWNER_DISSOLVE == resp.type) {
				//解散房间 相当于自己退出
				////切换面板
				RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_DISSOLVE_ROOM, null);
				Room.Room.reset ();
			}else{
				//别人退出
				if (Room.Room.removePlayerBySeat (resp.seat)) {
					RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_LEAVE_ROOM, (object)(Room.Room.getLocalBySeat (resp.seat)));	
				}
			}
		}
	}
		
	void OnPlayerClock(Message msg){
		msgClock resp = msgClock.deserialize(msg);

		GameEvent.sC2V_ShowClock sc;
		sc.local = resp.seat;
		sc.leftTime = resp.leftTime;
		sc.step = (int)resp.step;

		GameEvent.EM ().InvokeEvent (GameEvent.EVENT.CLOCK,(object)sc);

	}
	void OnRespJoinRoom(Message msg)
	{
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);

		msgRespJoinRoom resp = msgRespJoinRoom.deserialize(msg);
		//此处需要根据各种响应值，来给出友好提示
		string tip = "加入房间失败";
		//RESP_JOINROOM_FLAG
		if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_SUCCESS
			|| resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINGROOM_ALREADY_IN_ROOM){

			//规则应该一定不会为空的
			if(resp.rule == null || resp.rule.Length == 0){
				//
				return;
			}

			if (resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINGROOM_ALREADY_IN_ROOM) {
				//原则上此处可以直接进入房间，也可以是弹窗提示用户是否重新进入房间
				//
			}

			// 设置相关数据
			Room.Room.setRoomData(resp.roomId,resp.levelId,resp.plazaid,resp.roomType,resp.owner,resp.rule,resp.baseScore);
			Room.Room.isRelink = resp.isRelink;

			//这里需要判断是哪种类型的房间
			if (Room.Room.roomType == CommonDefine.eCreateRoomType.ROOM_ROOM) {
				msgNotifyEnterRoomFinish efmsg = new msgNotifyEnterRoomFinish ();
				efmsg.isRelink = Room.Room.isRelink;
				ProtocolManager.getInstance ().sendMsg (GameProtocol.P_GAME_ENTER_ROOM_FINISH,efmsg);


				if(!Room.Room.isRelink){
					//刷新房间规则显示
					RoomEvent.sC2V_RoomRule rr;
					rr.playerNum = Room.Room.roomRule.playerNum;
					rr.gameTime = Room.Room.roomRule.gameTime;
					rr.gridLevel = Room.Room.roomRule.gridLevel;
					rr.rule = Room.Room.roomRule.rule;
					rr.lmtRound = Room.Room.roomRule.lmtRound;
					rr.lmtTurnTime = Room.Room.roomRule.lmtTurnTime;
					rr.roomLevel = resp.levelId;
					rr.roomID = resp.roomId;
					rr.plazaName = Room.Room.plazaName;

					if (Room.Room.plazaid != 0) {
						LobbyEvent.Plaza plaza = getPlazaById(Room.Room.plazaid);
						rr.star = plaza.star;
					} else {
						rr.star = 0;
					}

					rr.tag = Room.Room.tagId;
					rr.type = Room.Room.roomType;

					RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.UPDATE_ROOMRULE,(object)rr);
				}else{
					//////
					ViewManagerEvent.sShowView data;
					data.fromView = ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW;
					data.toView = ViewManagerEvent.VIEW_TYPE.GAME_VIEW;
					ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_VIEW, (object)data);
				}


			} else if (Room.Room.roomType == CommonDefine.eCreateRoomType.ROOM_TEAM) {
				
			} else {
				//另外需要显示房间规则相关的信息在房间某个位置
				//进入房间成功，切换到游戏场景,这里应为主动操作，不需要事件通知请访问GameController来切换
				//或者这里直接切换也行
				//也可以事件通知，为了统一起见
				ViewManagerEvent.sShowView data;
				data.fromView = ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW;
				data.toView = ViewManagerEvent.VIEW_TYPE.GAME_VIEW;
				ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_VIEW, (object)data);
			}

			//结束处理
			return;

		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINGROOM_FAIL_ROOM_NOT_EXIST){
			tip = "房间不存在，请重试";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_ACCOUNT_ERR){
			tip = "加入失败，用户异常";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_FAIL_NO_FREE_ROOM){
			tip = "加入失败，没有可用的房间";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_FIAL_SYSERR){
			tip = "加入失败，系统错误";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_GOLD_LESS){
			tip = "加入失败，您的积分不足，请充值";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_GOLD_MORE){
			tip = "加入失败，您太有钱了，请到其他模式游戏";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_LEVEL_LESS){
			tip = "加入失败，您的级别太高了";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_LEVEL_MORE){
			tip = "加入失败，您的级别太低了";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_PLAZA_ERR){
			tip = "加入失败，参数错误";
		}else if(resp.flag == msgRespJoinRoom.RESP_JOINROOM_FLAG.JOINROOM_PWD_ERR){
			tip = "加入失败，密码错误";
		}

		CommonUtil.Util.showDialog ("系统提示",tip);
	}
	void OnRespPlayerAct(Message msg)
	{
		// 自己只收到一个简单的响应,别人准备收到的是 playerstate
		//ret:
		msgSimpleResp resp = msgSimpleResp.deserialize(msg);
		SelfData self = Account.getSelfData ();

		if (resp.ret == 0) {
			//自己准备成功
			if (Room.Room.updatePlayerStateByUserID (self.userID,CommonDefine.PLAYER_STATE.STATE_TYPE_ROOMREADY)) {

				RoomEvent.sC2V_PlayerState ps;
				ps.state = CommonDefine.PLAYER_STATE.STATE_TYPE_ROOMREADY;
				ps.local = Room.Room.getLocalBySeat(Room.Room.selfSeat);
				ps.ifAllReady = Room.Room.getIfAllPlayerReady ();

				RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_PLAER_STATE,(object)ps);

				//准备成功，上报天赋列表
				RoomEvent.sV2C_PlayerTalentList ptl;
				ptl.local = ps.local;

				Room.Player? player = Room.Room.getPlayerBySeat (Room.Room.selfSeat);

				ptl.talentList = new List<int>();
				for(int i = 0;i < player.Value.talentList.Count;i++){
					ptl.talentList.Add ((int)player.Value.talentList[i]);
				}

				onEventReportTalent((object)ptl);
			}
		}
	}
	void OnRespPlayerTalkMsg(Message msg)
	{
		// 自己只收到一个简单的响应
		//ret = 0成功
		//msgSimpleResp resp = msgSimpleResp.deserialize(msg);
		//自己发聊天消息，可以在发送的时候就显示，不用等这个响应

		OnPlayerTalk (msg);
	}
	void OnRespLeaveRoom(Message msg)
	{
		msgRespLeaveRoom resp = msgRespLeaveRoom.deserialize(msg);

		if (resp.type == msgRespLeaveRoom.LEAVE_TYPE.LEAVE_CANT_LEAVE) {
			//不能离开，目前是游戏不能离开，此消息在游戏中的时候离开会收到
		}else if(resp.type == msgRespLeaveRoom.LEAVE_TYPE.LEAVE_ESCAPE){
			//逃跑，目前不支持
		}else if(resp.type == msgRespLeaveRoom.LEAVE_TYPE.LEAVE_KICK){
			//被t了，目前不支持
		}else if(resp.type == msgRespLeaveRoom.LEAVE_TYPE.LEAVE_NORMAL){
			//正常离开，切到大厅界面
			if (Room.Room.roomType == CommonDefine.eCreateRoomType.ROOM_PLAZA
				||Room.Room.roomType == CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
				ViewManagerEvent.sShowView s;
				s.fromView = ViewManagerEvent.VIEW_TYPE.GAME_VIEW;
				s.toView = ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW;
				ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_VIEW, (object)s);
			} else {
				//此处为离开队伍或者离开房间，对于自己而言，这就是解散同样的处理
				RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_DISSOLVE_ROOM,null);
			}
		}else if(resp.type == msgRespLeaveRoom.LEAVE_TYPE.LEAVE_NOT_IN_ROOM){
			//不在房间中，直接切到大厅界面
			ViewManagerEvent.sShowView s;
			s.fromView = ViewManagerEvent.VIEW_TYPE.GAME_VIEW;
			s.toView = ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW;
			ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_VIEW, (object)s);

		}else if(resp.type == msgRespLeaveRoom.LEAVE_TYPE.LEAVE_DISSOLVE){
			// 房间解散
			//切换面板
			RoomEvent.EM ().InvokeEvent (RoomEvent.EVENT.UPDATE_DISSOLVE_ROOM,null);
		}

		Room.Room.reset ();
	}
	void OnRespCreateRoom(Message msg)
	{
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);

		msgRespCreateRoom resp = msgRespCreateRoom.deserialize(msg);

		if (resp.flag == 0) {

			CommonUtil.RoomRule roomRule = CommonUtil.RoomRule.deserialize(resp.rule);
			RoomEvent.sV2C_JoinRoom data;

			data.playerNum = roomRule.playerNum;
			data.gridLevel = roomRule.gridLevel;
			data.plazaID = 0;
			data.pwd = resp.roomPassword;
			data.roomId = resp.roomId;
			data.plazaName = "";
			data.tagId = -1;

			onEventJoinRoom((object)data);

		} else {
			CommonUtil.Util.showDialog ("系统提示","创建房间失败，参数有误或不满足开房条件");
		}
	}
	void OnRespRoomList(Message msg)
	{
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);


		msgRespRoomList resp = msgRespRoomList.deserialize(msg);
		//
		Room.Room.roomList.Clear ();//首先清空
		for (int i = 0; i < resp.roomList.Count; i++) {
			Room.RoomData rd;
			rd.isFull = resp.roomList[i].isFull;
			rd.roomDes = resp.roomList[i].roomDes;
			rd.roomId = resp.roomList[i].roomId;
			rd.roomName = resp.roomList[i].roomName;
			rd.roomPersonCnt = resp.roomList[i].roomPersonCnt;
			rd.roomPwd = resp.roomList[i].roomPwd;
			rd.rule = resp.roomList [i].rule;

			Room.Room.roomList.Add(rd);
		}

		//刷新界面,如果正在显示。一般这条消息是显示房间列表的时候再请求的，所以一般会正在显示。请求的时候加loading
		onEventShowRoomList(null);
	}
	void OnRoomStateChange(Message msg){
		msgRoomStateChange resp = msgRoomStateChange.deserialize(msg);
		Room.RoomData rd;
		rd.roomId = resp.roomId;
		rd.roomDes = "";
		rd.isFull = 0;
		rd.roomName = "";
		rd.roomPersonCnt = 0;
		rd.rule = "";
		rd.roomPwd = "";

		if (resp.type == msgRoomStateChange.ROOMSTATE_CHANGE_TYPE.ROOMSTATE_ADD) {
			rd.isFull = 0;
			rd.roomName = resp.roomName;
			rd.roomPersonCnt = 0;
			rd.rule = resp.rule;
			rd.roomPwd = resp.roomPassword;
			rd.roomDes = "add";

			Room.Room.addRoomListByRoomID(resp.roomId,rd);
		} else if (resp.type == msgRoomStateChange.ROOMSTATE_CHANGE_TYPE.ROOMSTATE_REMOVE) {
			Room.Room.removeRoomListByRoomID(resp.roomId);
			rd.roomDes = "remove";
			Room.Room.addRoomListByRoomID(resp.roomId,rd);
		} else if (resp.type == msgRoomStateChange.ROOMSTATE_CHANGE_TYPE.ROOMSTATE_UPDATE) {
			rd.roomDes = "update";
			rd.roomPersonCnt = resp.personCnt;

			Room.Room.updateRoomListByRoomID(resp.roomId,resp.personCnt);
		}

		//  这里借用以下房间描述暂时
		RoomEvent.EM ().InvokeEvent(RoomEvent.EVENT.UPDATE_ROOM_STATE,(object)rd);
	}
	//-------------------------------------------------------------
	LobbyEvent.Plaza getPlazaById(int plazaID){
		for (int i = 0; i < Lobby.Lobby.plazaList.Count; i++) {
			if (Lobby.Lobby.plazaList [i].plazaid == plazaID) {
				return Lobby.Lobby.plazaList [i];
			}
		}
		return null;
	}
}