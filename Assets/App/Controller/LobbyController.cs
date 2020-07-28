/**************************************/
//FileName: LobbyController.cs
//Author: wtx
//Data: 04/13/2017
//Describe:  大厅逻辑
/**************************************/

using UnityEngine;
using System;
using System.Collections;
using Pomelo.DotNetClient;
using SimpleJson;
using System.IO;
using ProtocolDefine;
using System.Collections.Generic;

public class LobbyController{
	private static LobbyController _instance = null;

	private LobbyController()
	{
		Lobby.Lobby.initData ();
	}

	public static LobbyController getInstance()
	{
		if(_instance == null)
		{
			_instance = new LobbyController();
		}
		return _instance;
	}

	public void addAllEvent(){
		AccountEvent.EM().AddEvent(AccountEvent.EVENT.LOGIN_SUCCESS,onAccountLoginSuccess);
		//子界面或独立界面刷新事件，请参考store
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_USER_INFO,onEventShowUserInfo);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_PLAZA,onEventShowPlaza);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_LUCKDRAW,onEventShowLuckDraw);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_PACKAGE,onEventShowPackage);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_PRIVATE_MSG,onEventShowPrivateMsg);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_SIGNIN,onEventShowSignIn);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_STORE,onEventShowStore);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_FRIEND,onEventShowFriend);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.SHOW_RANK,onEventShowRank);

		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.REQ_UPDATE_EMAIL,onEventReqUpdateEmail);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.REQ_FEEDBACK,onEventReqFeedback);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.REQ_OPEN_TALENTSLOT,onEventReqOpenTalentslot);
		LobbyEvent.EM().AddEvent(LobbyEvent.EVENT.UPDATE_USER_TALENT,onEventUpdateUserTalent);

		//声音
		LobbyEvent.EM ().AddEvent (LobbyEvent.EVENT.OPEN_CLOSE_BG_SND, onEventBgSndSwitch);
		LobbyEvent.EM ().AddEvent (LobbyEvent.EVENT.OPEN_CLOSE_EFFECT_SND, onEventEffectSndSwitch);


		ProtocolManager.getInstance().addPushMsgEventListener(LobbyProtocol.P_LOBBY_AWARD_EMAIL,OnAwardEmail);

	}

	public void removeAllEvent(){
		//AccountEvent.EM().RemoveEvent(AccountEvent.EVENT.LOGIN_SUCCESS);
		//子界面或独立界面刷新事件，请参考store
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_USER_INFO);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_PLAZA);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_LUCKDRAW);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_PACKAGE);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_PRIVATE_MSG);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_SIGNIN);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_STORE);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_FRIEND);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.SHOW_RANK);

		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.REQ_UPDATE_EMAIL);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.REQ_FEEDBACK);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.REQ_OPEN_TALENTSLOT);
		LobbyEvent.EM().RemoveEvent(LobbyEvent.EVENT.UPDATE_USER_TALENT);

		LobbyEvent.EM ().RemoveEvent (LobbyEvent.EVENT.OPEN_CLOSE_BG_SND);
		LobbyEvent.EM ().RemoveEvent (LobbyEvent.EVENT.OPEN_CLOSE_EFFECT_SND);

		ProtocolManager.getInstance().removePushMsgEventListener(LobbyProtocol.P_LOBBY_AWARD_EMAIL);

	}
	//-------------------------------以下为界面消息------------------------------------
	public void onEventShowStore(object data){
		//通知更新界面
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_STORE,(object)(Lobby.Lobby.storeList));
	}
	public void onEventShowPrivateMsg(object data){
		if (Lobby.Lobby.privateMsgList.Count != 0) {
			//此处把系统公告也插进去
			LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.UPDATE_PRIVATEMSG, (object)(Lobby.Lobby.privateMsgList));
		} /*else {
			msgReqPrivateMsgList primsg = new msgReqPrivateMsgList ();
			primsg.game = GameType.GAME_LIANQI;
			primsg.begin = 0;
			primsg.cnt = 20;
			ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_PRIVATEMSG, primsg, OnRespPrivateMsgList);
		}*/
	}
	public void onEventShowPackage(object data){
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_PACKAGE,(object)(Lobby.Lobby.packageList));
	}
	public void onEventShowSignIn(object data){
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_SIGNIN,(object)(Lobby.Lobby.signInData));
	}
	public void onEventShowLuckDraw(object data){
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_LUCKDRAW,(object)(Lobby.Lobby.luckDrawData));
	}
	public void onEventShowUserInfo(object data){
		SelfData self = Account.getSelfData();

		//已经可以更新用户相关界面信息
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_USER_INFO,(object)self);
	}
	public void onEventShowPlaza(object data){
		//过滤掉经典模式
		List<LobbyEvent.Plaza> plazaList = new List<LobbyEvent.Plaza> ();
		for (int i = 0; i < Lobby.Lobby.plazaList.Count; i++) {
			if(Lobby.Lobby.plazaList[i].roomType == (int)CommonDefine.eCreateRoomType.ROOM_PLAZA){
				plazaList.Add (Lobby.Lobby.plazaList[i]);
			}
		}
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_PLAZA,(object)plazaList);
	}
	public void onEventShowFriend(object data){
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_FRIEND,(object)(Lobby.Lobby.friendList));
	}
	public void onEventShowRank(object data){
		LobbyEvent.RankScopeType rst = (LobbyEvent.RankScopeType)data;
		rst.scope = LobbyEvent.RankScopeType.RANK_SCOPE_TYPE.RANK_AREA;
		rst.type = LobbyEvent.RankScopeType.RANK_TYPE.RANK_GOLD;

		List<LobbyEvent.Rank> rankList = new List<LobbyEvent.Rank>();
		for (int i = 0; i < Lobby.Lobby.rankList.Count; i++) {
			if (rst.type == Lobby.Lobby.rankList [i].rst.type
			   && rst.scope == Lobby.Lobby.rankList [i].rst.scope) {
				rankList.Add (Lobby.Lobby.rankList [i]);
			}
		}

		if (rankList.Count == 0) {
			//说明还没有请求过，刷新一次

			msgReqRankList rank = new msgReqRankList ();
			rank.game = GameType.GAME_LIANQI;
			rank.areaID = Account.getSelfData().area;
			rank.rankNum = 50;// 只取前50
			rank.scope = msgReqRankList.RANK_SCOPE_TYPE.RANK_AREA;//区排行
			rank.type = msgReqRankList.RANK_TYPE.RANK_GOLD;//财富排行
			ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_RANK_LIST, rank, OnRespRankList);

			ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);


		} else {
			LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.UPDATE_RANK, (object)(rankList));
		}
	}

	public void onEventReqUpdateEmail(object data){
		LobbyEvent.sV2C_ReqUpdateEmail re = (LobbyEvent.sV2C_ReqUpdateEmail)data;
		msgReqUpdateEmail msg = new msgReqUpdateEmail ();
		msg.type = re.type;
		msg.awardEmailId = re.id;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_UPDATE_EMAIL, msg, OnRespUpdateEmail);

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);

	}
	public void onEventReqFeedback(object data){
		LobbyEvent.sV2C_Feedback fb = (LobbyEvent.sV2C_Feedback)data;
		msgReqFeedback msg = new msgReqFeedback();
		msg.type = fb.type;
		msg.content = fb.content;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_FEEDBACK, msg, OnRespFeedback);

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);

	}

	public void onEventReqOpenTalentslot(object data){
		LobbyEvent.sV2C_OpenTalentslot ot = (LobbyEvent.sV2C_OpenTalentslot)data;

		//需要判断是否有足够的金币或者钻石开槽，避免多余的网络请求

		msgReqOpenTalentslot msg = new msgReqOpenTalentslot();
		msg.game = GameType.GAME_LIANQI;
		msg.openBy = ot.type;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_OPENTALENTSLOT, msg, OnRespOpenTalentslot);

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);

	}

	public void onEventUpdateUserTalent(object data){
		LobbyEvent.s_V2C_UpdateUserTalent uut = (LobbyEvent.s_V2C_UpdateUserTalent)data;

		Account.updateUserTalent (uut.talentList);
	}

	public void onEventBgSndSwitch(object data){
		CommonUtil.Util.setSetting("BgSnd",(bool)data == true?1:0);
	}
	public void onEventEffectSndSwitch(object data){
		CommonUtil.Util.setSetting("EffectSnd",(bool)data == true?1:0);
	}
	//-------------------------------以下为网络消息处理---------------------------------
	public void onAccountLoginSuccess(object data){
		//此时需要加载动画，大厅界面不可点击，数据全部收到或者部分收到时，可以点击。
		SelfData self = Account.getSelfData();

		onEventShowUserInfo (null);

		//为了节省服务器压力，以下数据后续需要实现md5方式请求，如果数据未发生变动，则不需要请求。to do
		//请求大厅数据
		msgReqPlazaList plaza = new msgReqPlazaList ();
		plaza.game = GameType.GAME_LIANQI;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_PLAZA_LIST, plaza, OnRespPlazaList);

		msgReqPropList prop = new msgReqPropList ();
		prop.game = GameType.GAME_LIANQI;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_PROP_LIST, prop, OnRespPropList);

		msgReqPackageList package = new msgReqPackageList ();
		package.game = GameType.GAME_LIANQI;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_PACKAGE_LIST, package, OnRespPackageList);

		msgReqSysMsgList sysmsg = new msgReqSysMsgList ();
		sysmsg.game = GameType.GAME_LIANQI;
		sysmsg.channelId = ChannelType.CHANNEL_APPSTORE;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_SYSMSG, sysmsg, OnRespSysMsgList);

		//
		msgReqPrivateMsgList primsg = new msgReqPrivateMsgList ();
		primsg.game = GameType.GAME_LIANQI;
		primsg.begin = 0;
		primsg.cnt = 20;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_PRIVATEMSG, primsg, OnRespPrivateMsgList);

		msgReqStoreList store = new msgReqStoreList ();
		store.game = GameType.GAME_LIANQI;
		store.channelID = ChannelType.CHANNEL_APPSTORE;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_STORE_LIST, store, OnRespStoreList);

		msgReqFriendList friend = new msgReqFriendList ();
		friend.game = GameType.GAME_LIANQI;
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_FRIEND_LIST, friend, OnRespFriendList);


		/////新增 2017－04－05
		//请求签到和抽奖数据
		msgReqSignInLuckDrawData sild = new msgReqSignInLuckDrawData ();
		sild.game = GameType.GAME_LIANQI;
		sild.areaID = self.area;
		sild.deviceID = "deviceID";
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_SIGNIN_LUCKDRAW_DATA, sild, OnRespSignInLuckDrawList);
		//请求签到
		msgReqSignIn signin = new msgReqSignIn ();
		signin.game = GameType.GAME_LIANQI;
		signin.areaID = self.area;
		signin.deviceID = "deviceID";
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_SIGNIN, signin, OnRespSignIn);
		//请求抽奖
		msgReqLuckDraw luckdraw = new msgReqLuckDraw ();
		luckdraw.game = GameType.GAME_LIANQI;
		luckdraw.areaID = self.area;
		luckdraw.deviceID = "deviceID";
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_LUCKDRAW, luckdraw, OnRespLuckDraw);
	}

	void OnRespPlazaList(Message msg){
		//
		msgRespPlazaList plazaList = msgRespPlazaList.deserialize(msg);

		Lobby.Lobby.plazaList.Clear ();//首先清空
		// 转换存数据
		for (int i = 0; i < plazaList.plazaList.Count; i++) {
			LobbyEvent.Plaza plaza = new LobbyEvent.Plaza();
			plaza.lmtType = (LobbyEvent.Plaza.LMT_TYPE)plazaList.plazaList[i].lmt_type;
			plaza.des = plazaList.plazaList[i].des;
			plaza.star = plazaList.plazaList[i].star;
			plaza.name = plazaList.plazaList[i].name;
			plaza.plazaid = plazaList.plazaList[i].plazaid;
			plaza.rule = plazaList.plazaList[i].rule;
			plaza.roomType = plazaList.plazaList[i].room_type;
			plaza.plazaLevel = new List<LobbyEvent.PlazaLevel> ();
			for (int j = 0; j < plazaList.plazaList [i].plazaLevel.Count; j++) {
				LobbyEvent.PlazaLevel pl = new LobbyEvent.PlazaLevel ();
				pl.base_score = plazaList.plazaList [i].plazaLevel[j].base_score;
				pl.levelid = plazaList.plazaList [i].plazaLevel[j].levelid;
				pl.minsr = plazaList.plazaList [i].plazaLevel[j].minsr;
				pl.maxsr = plazaList.plazaList [i].plazaLevel[j].maxsr;

				plaza.plazaLevel.Add (pl);
			}

			Lobby.Lobby.plazaList.Add(plaza);
		}

		//刷新界面
		onEventShowPlaza(null);
	}
	void OnRespPropList(Message msg){
		//
		msgRespPropList propList = msgRespPropList.deserialize(msg);

		Lobby.Lobby.propList.Clear ();
		for (int i = 0; i < propList.propList.Count; i++) {
			LobbyEvent.Prop prop;
			prop.data = propList.propList [i].data;
			prop.des = propList.propList [i].des;
			prop.id = propList.propList [i].id;
			prop.name = propList.propList [i].name;
			prop.pic = propList.propList [i].pic;
			prop.price = propList.propList [i].price;
			prop.type = (LobbyEvent.Prop.PROP_TYPE)propList.propList [i].type;
			Lobby.Lobby.propList.Add (prop);
		}
	}
	void OnRespPackageList(Message msg){
		//
		msgRespPackageList packageList = msgRespPackageList.deserialize(msg);

		Lobby.Lobby.packageList.Clear ();
		for (int i = 0; i < packageList.packageList.Count; i++) {
			LobbyEvent.Package pk;
			pk.end_time = packageList.packageList[i].end_time;
			pk.prop_cnt = packageList.packageList[i].prop_cnt;

			pk.prop.data = "";
			pk.prop.des = "";
			pk.prop.id = 0;
			pk.prop.name = "";
			pk.prop.pic = "";
			pk.prop.price = 0;
			pk.prop.type = 0;
			// 查找对应的道具
			if (!getProp (packageList.packageList [i].prop_id,ref pk.prop)) {
				//....??
			}

			Lobby.Lobby.packageList.Add (pk);
		}
		onEventShowPackage (null);
	}
	void OnRespSysMsgList(Message msg){
		//
		msgRespSysMsgList sml = msgRespSysMsgList.deserialize(msg);

		List<LobbyEvent.SysMsg> sysMsgList = new List<LobbyEvent.SysMsg>();
		for (int i = 0; i < sml.sysMsgList.Count; i++) {
			LobbyEvent.SysMsg sm;
			sm.content = sml.sysMsgList [i].content;
			sm.id = sml.sysMsgList [i].id;

			sysMsgList.Add (sm);
		}

		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.UPDATE_SYSMSG,(object)(sysMsgList));
	}
	void OnRespPrivateMsgList(Message msg){
		//
		msgRespPrivateMsgList privateMsgList = msgRespPrivateMsgList.deserialize(msg);

		Lobby.Lobby.privateMsgList.Clear ();
		for (int i = 0; i < privateMsgList.privateMsgList.Count; i++) {
			LobbyEvent.PrivateMsg psm;
			psm.author = privateMsgList.privateMsgList[i].author;
			psm.content = privateMsgList.privateMsgList[i].content;
			psm.end_time = privateMsgList.privateMsgList[i].end_time;
			psm.has_read = privateMsgList.privateMsgList[i].has_read;
			psm.id = privateMsgList.privateMsgList[i].id;
			psm.send_time = privateMsgList.privateMsgList[i].send_time;
			psm.title = privateMsgList.privateMsgList[i].title;

			Lobby.Lobby.privateMsgList.Add (psm);
		}

		//此时不显示界面
		onEventShowPrivateMsg (null);

		checkIfHasUnReadEmail ();
	}
	void OnRespStoreList(Message msg){
		//
		msgRespStoreList storeList = msgRespStoreList.deserialize(msg);

		Lobby.Lobby.storeList.Clear ();
		for (int i = 0; i < storeList.storeList.Count; i++) {
			LobbyEvent.Store store;
			store.data = storeList.storeList[i].data;
			store.des = storeList.storeList[i].des;
			store.id = storeList.storeList[i].id;
			store.name = storeList.storeList[i].name;
			store.pic = storeList.storeList[i].pic;
			store.point = storeList.storeList[i].point;
			store.price = storeList.storeList[i].price;

			Lobby.Lobby.storeList.Add (store);
		}

		onEventShowStore (null);
	}

	void OnRespSignInLuckDrawList(Message msg){
		msgRespSignInLuckDrawData resp = msgRespSignInLuckDrawData.deserialize(msg);

		Lobby.Lobby.signInData.hasSigned = resp.hasSigned;
		Lobby.Lobby.signInData.currentSignDay = resp.signInDay;
		Lobby.Lobby.luckDrawData.hasDrawed = resp.hasDrawed;

		Lobby.Lobby.signInData.signInList.Clear ();
		for (int i = 0; i < resp.signData.Count; i++) {
			LobbyEvent.SignIn sn;
			sn.gold_num = resp.signData [i].gold_num;
			sn.prop.data = "";
			sn.prop.des = "";
			sn.prop.id = 0;
			sn.prop.name = "";
			sn.prop.pic = "";
			sn.prop.price = 0;
			sn.prop.type = 0;
			if (!getProp (resp.signData [i].prop_id,ref sn.prop)) {
				//....??
			}

			sn.type = (LobbyEvent.SignIn.SIGNIN_AWARD_TYPE)resp.signData [i].type;
			Lobby.Lobby.signInData.signInList.Add(sn);
		}

		onEventShowSignIn (null);

		Lobby.Lobby.luckDrawData.luckDrawList.Clear ();
		for (int i = 0; i < resp.luckData.Count; i++) {
			LobbyEvent.LuckDraw ld;
			ld.gold_num = resp.luckData[i].gold_num;
			ld.prop.data = "";
			ld.prop.des = "";
			ld.prop.id = 0;
			ld.prop.name = "";
			ld.prop.pic = "";
			ld.prop.price = 0;
			ld.prop.type = 0;
			if (!getProp (resp.luckData[i].prop_id,ref ld.prop)) {
				//....??
			}

			ld.type = (LobbyEvent.SignIn.SIGNIN_AWARD_TYPE)resp.luckData[i].type;
			Lobby.Lobby.luckDrawData.luckDrawList.Add (ld);
		}
		onEventShowLuckDraw (null);

		if (Account.inRoomId == 0) {
			//这里假设这是最后一条收到，可以隐藏loading
			ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI, false);
		} else {
			//自动请求进入房间
			RoomEvent.sV2C_JoinRoom data;

			data.playerNum = 0;
			data.gridLevel = 0;
			data.plazaID = 0;
			data.pwd = "";
			data.roomId = Account.inRoomId;
			data.tagId = -1;
			data.plazaName = "";

			RoomEvent.EM().InvokeEvent(RoomEvent.EVENT.JOIN_ROOM,(object)data);
		}
	}
	void OnRespSignIn(Message msg){
		msgRespSignIn resp = msgRespSignIn.deserialize(msg);

		//to do  此类事件请定义到 LobbyEvent
	}
	void OnRespLuckDraw(Message msg){
		msgRespLuckDraw resp = msgRespLuckDraw.deserialize(msg);

		//to do  此类事件请定义到 LobbyEvent
	}

	//以下消息类似
	void OnRespRankList(Message msg){
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);

		//
		msgRespRankList rankList = msgRespRankList.deserialize(msg);

		//此条有较大问题，后续优化
		Lobby.Lobby.rankList.Clear ();
		for (int i = 0; i < rankList.rankList.Count; i++) {
			LobbyEvent.Rank rk;
			rk.headUrl = rankList.rankList[i].headUrl;
			rk.exp = rankList.rankList[i].exp;
			rk.name = rankList.rankList[i].name;
			rk.score = rankList.rankList[i].score;
			rk.userID = rankList.rankList[i].userID;
			rk.charm = rankList.rankList [i].charm;
			rk.diamond = rankList.rankList [i].diamond;
			rk.gold = rankList.rankList [i].gold;
			rk.win_rate = rankList.rankList [i].win_rate;

			rk.rst.scope = (LobbyEvent.RankScopeType.RANK_SCOPE_TYPE)rankList.scope;
			rk.rst.type = (LobbyEvent.RankScopeType.RANK_TYPE)rankList.type;

			Lobby.Lobby.rankList.Add (rk);
		}

		//由于这个消息的特殊性，可以不在此处刷新，需要的展示界面的时候再请求，刷新
		LobbyEvent.RankScopeType rst;
		rst.scope = LobbyEvent.RankScopeType.RANK_SCOPE_TYPE.RANK_AREA;
		rst.type = LobbyEvent.RankScopeType.RANK_TYPE.RANK_GOLD;
		onEventShowRank ((object)(rst));//此为默认显示的榜单
	}
	void OnRespFriendList(Message msg){
		//
		msgRespFriendList resp = msgRespFriendList.deserialize(msg);

		Lobby.Lobby.friendList.Clear ();
		for (int i = 0; i < resp.friendList.Count; i++) {
			LobbyEvent.Friend fd;
			fd.des = resp.friendList [i].des;
			fd.friend_id = resp.friendList [i].friend_id;
			fd.friend_score = resp.friendList [i].friend_score;
			fd.head_url = resp.friendList [i].head_url;
			fd.lastlogin_time = resp.friendList [i].lastlogin_time;
			fd.name = resp.friendList [i].name;
			Lobby.Lobby.friendList.Add(fd);
		}
		onEventShowFriend (null);
	}
	void OnAwardEmail(Message msg){
		msgAwardEmail ae = msgAwardEmail.deserialize(msg);
		//
		if(ae.type == SYS_OR_PRIVATE_MSG_TYPE.TYPE_MSG_PRIVATE){
			//个人消息，这种是私人消息，比如队伍之间，非系统下发
			//待完成
		}else if(ae.type == SYS_OR_PRIVATE_MSG_TYPE.TYPE_MSG_SYS){
			//系统下发消息，多是奖励信息，如果有必要则添加到邮件
			if (ae.needAdd2Email) {
				LobbyEvent.PrivateMsg psm;
				psm.author = ae.author;
				psm.content = ae.content;
				psm.end_time = ae.end_time;
				psm.has_read = ae.has_read;
				psm.id = ae.id;
				psm.send_time = ae.send_time;
				psm.title = ae.title;

				Lobby.Lobby.privateMsgList.Insert (0,psm);

				onEventShowPrivateMsg (null);

				checkIfHasUnReadEmail ();

			} else {
				//如果不需要添加到邮件，则仅仅弹窗提示即可
				//show dialog
			}
		}else if(ae.type == SYS_OR_PRIVATE_MSG_TYPE.TYPE_MSG_NOTICE){
			//系统公告性质的消息，也就是要走跑马灯
			LobbyEvent.SysMsg sm;
			sm.id = 0;
			sm.content = ae.content;
			LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.SHOW_SYSMSG,(object)sm);
		}
	}
	void  OnRespUpdateEmail(Message msg){

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);


		msgReqUpdateEmail resp = msgReqUpdateEmail.deserialize (msg);

		int find = -1;
		for (int i = 0; i < Lobby.Lobby.privateMsgList.Count; i++) {
			if (Lobby.Lobby.privateMsgList [i].id == resp.awardEmailId) {
				find = i;
				break;
			}
		}
		if (find == -1) {
			//邮件不存在
			return;
		}
		LobbyEvent.PrivateMsg psm;
		psm.author = Lobby.Lobby.privateMsgList [find].author;
		psm.content = Lobby.Lobby.privateMsgList [find].content;
		psm.end_time = Lobby.Lobby.privateMsgList [find].end_time;
		psm.has_read = Lobby.Lobby.privateMsgList [find].has_read;
		psm.id = Lobby.Lobby.privateMsgList [find].id;
		psm.send_time = Lobby.Lobby.privateMsgList [find].send_time;
		psm.title = Lobby.Lobby.privateMsgList [find].title;

		if (resp.type == CommonDefine.eUpdateEmailType.READ) {
			//更新邮件缓存列表
			psm.has_read = 1;
			Lobby.Lobby.privateMsgList [find] = psm;

			checkIfHasUnReadEmail ();

		} else if (resp.type == CommonDefine.eUpdateEmailType.DEL) {
			//删除
			Lobby.Lobby.privateMsgList.RemoveAt(find);

			checkIfHasUnReadEmail ();

		} else if (resp.type == CommonDefine.eUpdateEmailType.GET_AWARD) {
			CommonUtil.EmailContent ec = CommonUtil.EmailContent.deserialize (Lobby.Lobby.privateMsgList[find].content);

			if (ec.hasGottenAward) {
				//已经领取过奖励了
				CommonUtil.Util.showDialog ("温馨提示","您已经领取过奖励了 :)");

				return;
			} else {

				ec.hasGottenAward = true;
				psm.content = CommonUtil.EmailContent.serialize (ec);
				Lobby.Lobby.privateMsgList [find] = psm;

				// 奖励需要同时更新 用户金币以及包裹信息
				if (ec.type == CommonUtil.EmailContent.AWARD_TYPE.GOLD) {
					Account.updateUserGold( ec.awardCnt);
					//已经可以更新用户相关界面信息
					onEventShowUserInfo(null);
				} else if (ec.type == CommonUtil.EmailContent.AWARD_TYPE.PROP) {
					//重新请求一遍
					msgReqPackageList package = new msgReqPackageList ();
					package.game = GameType.GAME_LIANQI;
					ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_PACKAGE_LIST, package, OnRespPackageList);
				}
			}
		}

		LobbyEvent.sV2C_ReqUpdateEmail re;
		re.id = resp.awardEmailId;
		re.type = resp.type;
		LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.SHOW_UPDATE_EMAIL_RESULT,(object)re);
	}

	void OnRespFeedback(Message msg){
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);

		msgReqFeedback resp = msgReqFeedback.deserialize (msg);
		if (resp.type == CommonDefine.eFeedbackType.DANMU) {
			//显示弹幕
		}else{
			CommonUtil.Util.showDialog ("温馨提示",resp.content);
		}
	}
	void OnRespOpenTalentslot(Message msg){
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);

		msgRespOpenTalentslot resp = msgRespOpenTalentslot.deserialize (msg);
		if (resp.result == msgRespOpenTalentslot.eOpenTalentslotResultType.OPEN_SUCCESS) {
			//开槽成功
			//refresh ui
			//CommonUtil.Util.showDialog ("温馨提示","开槽成功，当前槽数为："+resp.currentOpenedCnt);

			Account.setUserGold (resp.currentGold);
			Account.setUserDiamond (resp.currentDiamond);

			LobbyEvent.s_C2V_UpdateTalentForOpenSlot utfos;
			utfos.gold = resp.currentGold;
			utfos.diamond = resp.currentDiamond;
			utfos.ret = CommonDefine.eRespResultType.SUCCESS;
			utfos.currentTalentNum = resp.currentOpenedCnt;
			LobbyEvent.EM ().InvokeEvent (LobbyEvent.EVENT.RESP_OPEN_TALENTSLOT,(object)utfos);

		}else if(resp.result == msgRespOpenTalentslot.eOpenTalentslotResultType.OPEN_FAIL_LESS_DIAMOND){
			CommonUtil.Util.showDialog ("温馨提示","开槽失败，您的晶核不足，请充值");
		}else if(resp.result == msgRespOpenTalentslot.eOpenTalentslotResultType.OPEN_FAIL_LESS_GOLG){
			CommonUtil.Util.showDialog ("温馨提示","开槽失败，您的金币不足，请充值");
		}else if(resp.result == msgRespOpenTalentslot.eOpenTalentslotResultType.OPEN_FAIL_MAX_SLOT){
			CommonUtil.Util.showDialog ("温馨提示","您的天赋槽已全部打开！");
		}
	}
	//-------------------------一些封装----------------------------------

	bool getProp(int propID,ref LobbyEvent.Prop prop){
		for(int j = 0;j < Lobby.Lobby.propList.Count;j++){
			if (propID == Lobby.Lobby.propList [j].id) {
				prop.data = Lobby.Lobby.propList [j].data;
				prop.des = Lobby.Lobby.propList [j].des;
				prop.id = Lobby.Lobby.propList [j].id;
				prop.name = Lobby.Lobby.propList [j].name;
				prop.pic = Lobby.Lobby.propList [j].pic;
				prop.price = Lobby.Lobby.propList [j].price;
				prop.type = Lobby.Lobby.propList [j].type;

				return true;
			} else {
				//此时可能是背包数据先到，道具数据没有收到还，此类情况基本不可能出现
				//后续做容错处理
			}
		}
		return false;
	}

	void checkIfHasUnReadEmail(){
		for (int i = 0; i < Lobby.Lobby.privateMsgList.Count; i++) {
			if (Lobby.Lobby.privateMsgList [i].has_read == 0) {
				//通知显示提示标示
				LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_HAS_EMAIL,true);
				return;
			}
		}
		LobbyEvent.EM().InvokeEvent(LobbyEvent.EVENT.SHOW_HAS_EMAIL,false);
	}
}