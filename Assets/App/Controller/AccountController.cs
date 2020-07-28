/**************************************/
//FileName: AccountController.cs
//Author: wtx
//Data: 04/13/2017
//Describe: 用户登陆逻辑
/**************************************/

using UnityEngine;
using System;
using System.Collections;
using Pomelo.DotNetClient;
using SimpleJson;
using System.IO;
using ProtocolDefine;
using CommonDefine;


public class AccountController{
	private static AccountController _instance = null;
	private NowAccount.eLoginType _loginType;
	private bool _autoLogin = false;

	private AccountController()
	{
		
	}

	public static AccountController getInstance()
	{
		if(_instance == null)
		{
			_instance = new AccountController();
		}
		return _instance;
	}

	public void addAllEvent(){
		AccountEvent.EM().AddEvent(AccountEvent.EVENT.CONNECT_SERVER,onConnectServer);	
		AccountEvent.EM().AddEvent(AccountEvent.EVENT.LOGIN,onLogin);
		AccountEvent.EM().AddEvent(AccountEvent.EVENT.LOGOUT,onLogout);
		AccountEvent.EM().AddEvent(AccountEvent.EVENT.THIRD_PARTY_LOGIN_RET,onThirdPartyLoginResult);

		AccountEvent.EM().AddEvent(AccountEvent.EVENT.NETWORK_DISCONNECT,onDisconnect);
		AccountEvent.EM().AddEvent(AccountEvent.EVENT.NETWORK_ERROR,onError);
	}
	public void removeAllEvent(){
		//AccountEvent.EM().RemoveEvent(AccountEvent.EVENT.CONNECT_SERVER);	
		//AccountEvent.EM().RemoveEvent(AccountEvent.EVENT.LOGIN);
		//AccountEvent.EM().RemoveEvent(AccountEvent.EVENT.LOGOUT);

		//AccountEvent.EM().RemoveEvent(AccountEvent.EVENT.NETWORK_DISCONNECT);
		//AccountEvent.EM().RemoveEvent(AccountEvent.EVENT.NETWORK_ERROR);
	}

	public void onConnectServer(object data){
		bool selectServer = (bool)data;

		ProtocolManager.getInstance ().Start (selectServer,OnError,OnDisconnect,OnConnect);//此处只能调用一次，位置是在登陆界面显示的时候首次连接服务器

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);
	}

	public void onLogin(object data){
		_loginType =  (NowAccount.eLoginType)data;//data as eLoginBtnType;

		//首先获取本地保存的数据
		if (Account.loadNowAccout ()) {
			//已经登陆过
			NowAccount nc = Account.getNowAccount ();
			if (nc.lastLoginType == _loginType) {

				//需要保存
				Account.thirdOpenID = nc.openid;
				Account.thirdToken = nc.token;

				//不必启动第三方
				loginServer (nc.userID, nc.openid, nc.pwd, nc.lastLoginType, nc.area);

				return;
			}
		}

		switch (_loginType) {
		case NowAccount.eLoginType.LOGIN_TYPE_YK:
			loginByYK ();
			break;
		case NowAccount.eLoginType.LOGIN_TYPE_WX:
			loginByQQWX (msgLogin.eLoginType.LOGIN_TYPE_WX);
			break;
		case NowAccount.eLoginType.LOGIN_TYPE_QQ:
			loginByQQWX (msgLogin.eLoginType.LOGIN_TYPE_QQ);
			break;
		}
	}

	public void onLogout(object data){
		ProtocolManager.getInstance().Disconnect();//断开，即退出的时候调用

		//这个时候其实应该转到登陆界面
		//to do
	}
	public void loginByQQWX(msgLogin.eLoginType type){
		//这里需要回调到ui界面处理三方登陆
		AccountEvent.EM ().InvokeEvent (AccountEvent.EVENT.THIRD_PARTY_LOGIN,(int)type);
	}
	private void loginServer(int userId,string openId,string pwd,NowAccount.eLoginType loginType,int area){
		msgLogin msg = new msgLogin();
		msg.userID = userId;
		msg.area = area;//用户所选服务器
		msg.appVersion = CommonUtil.Util.getAppVer();
		msg.channelID = CommonUtil.Util.getChanelId();

		msg.deviceID = SystemInfo.deviceUniqueIdentifier;
		msg.ipAddr = 1111;
		msg.loginType = (msgLogin.eLoginType)loginType;
		msg.netWorkType = 1;
		msg.osVersion = 10000;
		msg.password = pwd;

		//
		msg.openID = openId;
		//.. etc.
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_LOGIN, msg, OnLoginSuccess);

		//显示加载，禁止点击
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);
	}
	//登陆
	public void loginByYK(){
		loginServer (0,"","",NowAccount.eLoginType.LOGIN_TYPE_YK,1);
	}

	//三方登陆响应
	public void onThirdPartyLoginResult(object data){
		AccountEvent.ThirdPartyLoginResult ret = (AccountEvent.ThirdPartyLoginResult)data;

		//设置nowaccount的 third
		Account.thirdOpenID = ret.openId;
		Account.thirdToken = ret.token;

		//登陆
		msgLogin msg = new msgLogin();
		msg.userID = 0;
		msg.area = 1;//用户所选服务器
		msg.appVersion = CommonUtil.Util.getAppVer();
		msg.channelID = CommonUtil.Util.getChanelId();
		msg.deviceID = SystemInfo.deviceUniqueIdentifier;
		msg.ipAddr = 1111;
		msg.loginType = (msgLogin.eLoginType)_loginType;
		msg.netWorkType = 1;
		msg.osVersion = 10000;
		msg.password = "lianqi";

		//
		msg.openID = ret.openId;
		msg.token = ret.token;
		msg.nickName = ret.name;
		msg.head = ret.head;
		msg.sex = ret.sex;
		msg.expireTime = ret.expireTime;
		//.. etc.
		ProtocolManager.getInstance ().sendMsg (LobbyProtocol.P_LOBBY_REQ_LOGIN, msg, OnLoginSuccess);

		//显示加载，禁止点击
		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,true);
	}
	//登陆成功
	public void OnLoginSuccess(Message msg){
		msgUserData userData = msgUserData.deserialize(msg);
		if (userData.flag == msgUserData.eLoginResultFlag.LOGIN_SUCCESS) {
			//登陆成功
			SelfData selfData;
			selfData.adult = userData.adult;
			selfData.area = userData.area;
			selfData.charm = userData.charm;
			selfData.diamond = userData.diamond;
			selfData.draw = userData.draw;
			selfData.energy = userData.energy;
			selfData.escape = userData.escape;
			selfData.exp = userData.exp;
			selfData.gameTime = userData.gameTime;
			selfData.gold = userData.gold;
			selfData.head = userData.head;
			selfData.lastloginTime = userData.lastlogin_time;
			selfData.lose = userData.lose;
			selfData.name = userData.name;
			selfData.talent = userData.talent;
			selfData.roomID = userData.room_id;
			selfData.score = userData.score;
			selfData.sex = userData.sex;
			selfData.userID = userData.user_id;
			selfData.win = userData.win;

			selfData.talentList = new System.Collections.Generic.List<eTalentType>();


			string btnStateStr = CommonUtil.Util.getPlayerPrefs (CommonDefine.CONST.TALENT_SLOT_STATE, "");
			if (btnStateStr != "") {
				string [] btnStates = btnStateStr.Split (',');
				foreach (string st in btnStates) {
					string[] data = st.Split ('#');

					CommonDefine.eTalentType ttp = CommonDefine.eTalentType.TALENT_NONE;

					if ((CommonDefine.TalentSlotState)(int.Parse (data [2])) == CommonDefine.TalentSlotState.TALENT_INSTALLED) {
						//已配置
						ttp = (CommonDefine.eTalentType)int.Parse (data [0]);
					}

					selfData.talentList.Add (ttp);

				}
			} else {
				//如果还没有配置，则默认不配
				for (int i = 0; i < CommonDefine.CONST.MAX_TALENT_CFG_NUM; i++) {
					selfData.talentList.Add (CommonDefine.eTalentType.TALENT_NONE);
				}
			}

			//设置用户数据
			Account.onLoginSuccess(selfData,_loginType);
			Account.inRoomId = userData.room_id;

			//切换界面并通知 登陆成功
			ViewManagerEvent.sShowView showView;
			showView.fromView = ViewManagerEvent.VIEW_TYPE.ACCOUNT_VIEW;
			showView.toView = ViewManagerEvent.VIEW_TYPE.LOBBY_VIEW;
			ViewManagerEvent.EM().InvokeEvent(ViewManagerEvent.EVENT.SHOW_VIEW,(object)showView);


			AccountEvent.EM().InvokeEvent(AccountEvent.EVENT.LOGIN_SUCCESS,null);//此处不需要传入数据，将用户数据写入account，大厅自取
		

		} else {
			//登陆失败，给予提示
		}
	}
	void OnError(Message msg){
		AccountEvent.EM ().InvokeEvent (AccountEvent.EVENT.NETWORK_ERROR,msg.jsonObj["reason"]);
	}
	void OnDisconnect(Message msg){
		AccountEvent.EM ().InvokeEvent (AccountEvent.EVENT.NETWORK_DISCONNECT,msg.jsonObj["reason"]);
	}
	//服务器列表，以此显示服务器列表，供用户选择服务器
	void OnConnect(bool canLogin,Message msg){
		if (!canLogin) {
			msgRespServerList resp = msgRespServerList.deserialize (msg);
			//选择服务器后需要调用，然后再去请求登陆到游戏(和自动选择模式相同)
			//ProtocolManager.getInstance().SelectServer(host,port);
		} else {
			//后续 自动登录
			//第一次需要点击登录按钮

			if (_autoLogin) {
				_autoLogin = false;

				//重新登陆
				NowAccount nc = Account.getNowAccount ();
				loginServer (nc.userID, nc.openid, nc.pwd, nc.lastLoginType, nc.area);

			} else {
				ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_LOADING_ANI,false);
			}
		}
	}
	void onError(object data){
		ViewManagerEvent.s_ShowDialog d;
		d.callBack = onClickDialogBtn;
		d.hasCancel = true;
		d.hasClose = false;
		d.hasOk = true;
		d.tip = "系统提示";
		d.tip = "您的网络存在异常，请检查网络设置~";	
		d.type = CommonDefine.eDialogEventType.NETWORK_ERROR;

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_DIALOG,(object)d);
	}
	void onDisconnect(object data){
		ViewManagerEvent.s_ShowDialog d;
		d.callBack = onClickDialogBtn;
		d.hasCancel = true;
		d.hasClose = false;
		d.hasOk = true;
		d.tip = "系统提示";
		d.tip = "您已经与服务器断开连接，是否重连？";	
		d.type = CommonDefine.eDialogEventType.NETWORK_DISCONNECT;

		ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_DIALOG,(object)d);

		// 自动重连模式

	}

	void onClickDialogBtn(CommonDefine.eDialogBtnType btn,CommonDefine.eDialogEventType type){
		if (type == CommonDefine.eDialogEventType.NETWORK_DISCONNECT) {
			if (btn == eDialogBtnType.DIALOG_BTN_OK) {
				//重连
				reLogin();
			}else{
				//不操作

			}
		}else if (type == CommonDefine.eDialogEventType.NETWORK_ERROR) {
			if (btn == eDialogBtnType.DIALOG_BTN_OK) {
				//可以去网络设置
			}else{
				// 重试
				reLogin();
			}
		}
	}

	void reLogin(){
		_autoLogin = true;
		ProtocolManager.getInstance ().Restart ();
	}
}