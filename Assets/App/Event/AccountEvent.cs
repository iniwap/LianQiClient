/**************************************/
//FileName: LianQiEventManager.cs
//Author: wtx
//Data: 13/04/2017
//Describe: 所有事件定义以及处理封装，请使用此来完成ui和c之间的交互，禁ui直接访问controller
/**************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class AccountEvent : LianQiEventManager{
	public enum EVENT{
		// 因为不希望在界面体现网络通信相关代码，所以需要传出这两个事件
		ACCOUNT_EVENT_BEGIN = 10000,
		CONNECT_SERVER,//首次连接服务器
		LOGIN,//登陆按钮点击//传递登陆类型
		THIRD_PARTY_LOGIN,
		THIRD_PARTY_LOGIN_RET,
		LOGOUT,//请求退出
		LOGIN_SUCCESS,//登陆成功，用于通知其他
		NETWORK_ERROR,
		NETWORK_DISCONNECT,
	};

	public struct ThirdPartyLoginResult{
		public string openId;
		public string token;
		public string name;
		public string head;
		public int sex;
		public int expireTime;
	};


	private static AccountEvent _instance = null;

	public static AccountEvent EM()
	{
		if(_instance == null)
		{
			_instance = new AccountEvent();
		}
		return _instance;
	}

	public void AddEvent(EVENT eventID, Action<object> callback)
	{
		AddEvent((int)eventID,callback);
	}
	public void RemoveEvent(EVENT eventID)
	{
		RemoveEvent((int)eventID);
	}
	public void InvokeEvent(EVENT eventID, object data)
	{
		InvokeEvent((int)eventID,data);
	}
}
