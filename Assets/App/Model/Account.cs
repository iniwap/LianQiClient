/**************************************/
//FileName: Account.cs
//Author: wtx
//Data: 04/13/2017
//Describe:  用户数据
/**************************************/

using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public struct NowAccount{
	public enum eLoginType
	{
		LOGIN_TYPE_NONE = 0,
		LOGIN_TYPE_YK = 1,
		LOGIN_TYPE_QQ = 2,
		LOGIN_TYPE_WX = 3,
	};

	public int area;
	public int userID;
	public string pwd;
	// 第三方登陆使用
	public string token;
	public string openid;

	public eLoginType lastLoginType;
};

public struct SelfData{
	public int userID; 
	public int area;
	public string name;
	public int sex;
	public string head;
	public long gold;
	public int win;//赢
	public int lose;//输
	public int draw;//平－和
	public int escape;//逃跑
	public int talent;//天赋槽数
	public long gameTime;//总游戏时长
	public long exp;//经验
	public int roomID;//是否在房间中,0不在
	public bool adult;//是否成年
	public int charm;//魅力
	public long lastloginTime;//最后登陆时间
	public int score;//得分-对应段位信息，是总数字，每个段位计算得到
	public long diamond;//钻石
	public long energy;//能量

	public List<CommonDefine.eTalentType> talentList;
};

public static class Account{
	public static string thirdToken { get; set; }                     //第三方token
	public static string thirdOpenID { get; set; }                            //第三方openid


	//
	public static string thirdLoginNickName{ get; set; }
	public static int 	  thirdLoginSex{ get; set; }
	public static string thirdHeadUrl { get; set; }


	public static int inRoomId { get; set; }

	private static SelfData selfData;
	private static NowAccount _nowAccount;

	public static SelfData getSelfData(){
		return selfData;
	}

	public static NowAccount getNowAccount(){
		return _nowAccount;
	}
	//自动登陆使用，启动后从本地读取已经存储的用户登陆信息,区服，帐号等
	public static bool loadNowAccout(){
		_nowAccount.area = CommonUtil.Util.getPlayerPrefs("AccountArea",0);
		_nowAccount.lastLoginType = (NowAccount.eLoginType)CommonUtil.Util.getPlayerPrefs("AccountType",0);
		_nowAccount.openid = CommonUtil.Util.getPlayerPrefs("AccountOpenId","");
		_nowAccount.pwd = CommonUtil.Util.getPlayerPrefs("AccountPwd","");
		_nowAccount.token = CommonUtil.Util.getPlayerPrefs("AccountToken","");
		_nowAccount.userID = CommonUtil.Util.getPlayerPrefs("AccountUserId",0);

		return _nowAccount.lastLoginType != NowAccount.eLoginType.LOGIN_TYPE_NONE;
	}
	public static void saveNowAccount(){
		CommonUtil.Util.setPlayerPrefs("AccountArea",_nowAccount.area);
		CommonUtil.Util.setPlayerPrefs("AccountType",(int)_nowAccount.lastLoginType);
		CommonUtil.Util.setPlayerPrefs("AccountOpenId",_nowAccount.openid);
		CommonUtil.Util.setPlayerPrefs("AccountPwd",_nowAccount.pwd);
		CommonUtil.Util.setPlayerPrefs("AccountToken",_nowAccount.token);
		CommonUtil.Util.setPlayerPrefs("AccountUserId",_nowAccount.userID);
	}
	public static void onLoginSuccess(SelfData playerData,NowAccount.eLoginType loginBy){
		selfData = playerData;

		_nowAccount.area = selfData.area;
		_nowAccount.userID = selfData.userID;
		//_nowAccount.pwd = selfData.pwd;

		_nowAccount.lastLoginType = loginBy;

		if (_nowAccount.lastLoginType == NowAccount.eLoginType.LOGIN_TYPE_QQ
			|| _nowAccount.lastLoginType == NowAccount.eLoginType.LOGIN_TYPE_WX) {
			_nowAccount.token = thirdToken;
			_nowAccount.openid = thirdOpenID;
		}
		//填写now account，并保存到本地
		saveNowAccount();
	}
	public static void updateUserGold(int changeGold){
		selfData.gold += changeGold;
	}
	public static void setUserGold(long gold){
		selfData.gold = gold;
	}
	public static void setUserDiamond(long diamond){
		selfData.diamond = diamond;
	}
	public static void updateUserGoldForGameResult(int changeGold){
		updateUserGold (changeGold);
		if (changeGold > 0) {
			selfData.win += 1;
		}else if(changeGold < 0){
			selfData.lose += 1;
		}else{
			selfData.draw += 1;
		}
	}

	public static void updateUserTalent(List<CommonDefine.eTalentType> tl){
		for(int i = 0;i < selfData.talentList.Count;i++){
			selfData.talentList [i] = CommonDefine.eTalentType.TALENT_NONE;
		}

		for(int i = 0; i < tl.Count;i++){
			selfData.talentList [i] = tl [i];
		}
	}
};