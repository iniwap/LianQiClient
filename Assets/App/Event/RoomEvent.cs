/**************************************/
//FileName: LianQiEventManager.cs
//Author: wtx
//Data: 13/04/2017
//Describe: 单独分开房间相关的事件，
/**************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class RoomEvent : LianQiEventManager{
	public enum EVENT{
		ROOM_EVENT_BEGIN = 40000,
		CREATE_ROOM,//请求 创建房间
		JOIN_ROOM,//请求加入房间
		LEAVE_ROOM,//请求离开房间
		ENTER_ROOM_FINISH,//游戏界面加载完成
		UPDATE_LEAVE_ROOM,//有人离开房间，自己的话收到响应的时候就直接退出了，不是这个处理
		UPDATE_DISSOLVE_ROOM,//解散房间
		GET_ROOM_LIST,//获取房间列表
		SHOW_ROOM_LIST,
		UPDATE_ROOM_STATE,
		PLAYER_ACT,
		TALK_MSG,
		SHOW_TALK_MSG,
		UPDATE_ROOMRULE,
		PLAER_ENTER,//用户进入
		UPDATE_PLAER_STATE,//用户状态发生变化
		START_GAME,//点击开局
	};

	//注意
	//为了避免界面访问网络相关消息结构，所以每个事件如果必要，请定义需要传递给controller的数据结构
	public struct sV2C_JoinRoom{
		public int playerNum;//由于一个场支持两种人数模式，所以需要客户端上传这个两个参数
		public int gridLevel;//
		public int plazaID;//根据plazalist得到，界面也是根据plazalisy生成
		public string pwd;//非用户创建无密码
		public int roomId;//非用户创建房间填0，即通过各种模式直接进入游戏的

		//未来方便，传递下name和tagid
		public string plazaName;
		public int tagId;
	};
	public struct sV2C_CreateRoom{
		public CommonDefine.eCreateRoomType roomType;
		public int baseScore;
		public int minScore;
		public int maxScore;
		public string roomName;
		public string roomPassword;
		//此处rule已经拼接好，rule也可以拆分为单字段，看具体实现
		public string rule;// "{\"playerNum\":2,\"gridLevel\":4,\"rule\":\"default\",\"gameTime\":\"360\"}";//规则json 或者字符串，非常重要

	};
	public struct sV2C_GetRoomList{
		public int currentPage;//当前第几页
		public int perCnt;//每页数量
	};
	public struct sV2C_PlayerAct{
		//这里有个麻烦的地方是需要 网络和事件定义需要一致
		public enum ACT_TYPE
		{
			ACT_SITDOWN,//坐下，之后才能准备
			ACT_STANDUP,//站起
			ACT_READY,//准备
			ACT_SEEING,//旁观
		};

		public ACT_TYPE act;
	}
	public struct sV2C_PlayerTalk{
		public string content;// 内容可以各种各样的json数据拼接，以丰富界面对消息对话的显示
	}
	public struct sV2C_PlayerTalentList{
		public int local;
		public List<int> talentList;
	}

	public struct sC2V_PlayerState{
		public int local;
		public bool ifAllReady;
		public CommonDefine.PLAYER_STATE state;
	}

	public struct sC2V_PlayerTalk{
		public int local;
		public string content;
	}

	public struct sC2V_RoomRule{
		public CommonDefine.eCreateRoomType type;

		public int playerNum;
		public int gridLevel;
		public string rule;
		public int gameTime;//限制总时长
		public int lmtRound;//限制回合
		public int lmtTurnTime;//限手时长
		public int roomLevel;
		public int roomID;

		//如果是场模式，需要以下参数
		public float star;
		public int tag;
		public string plazaName;
	}

	//etc ..

	private static RoomEvent _instance = null;

	public static RoomEvent EM()
	{
		if(_instance == null)
		{
			_instance = new RoomEvent();
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
