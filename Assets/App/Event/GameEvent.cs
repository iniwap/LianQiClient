/**************************************/
//FileName: LianQiEventManager.cs
//Author: wtx
//Data: 13/04/2017
//Describe: 所有事件定义以及处理封装，请使用此来完成ui和c之间的交互，禁ui直接访问controller
/**************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class GameEvent : LianQiEventManager{
	public enum EVENT{
		// 因为不希望在界面体现网络通信相关代码，所以需要传出这两个事件
		GAME_EVENT_BEGIN = 30000,
		PLAY,//落子
		MOVE,//移动
		PASS,//过，回合结束
		ABANDON,//投降
		DRAW,//请和
		ACTION_FAIL,//操作失败，响应
		CLOCK,

		SHOW_GAME_START,//对局开始
		SHOW_FLAG,//
		SHOW_DRAW,// 显示谁请求和棋
		SHOW_DRAW_RESULT,//显示和棋请求结果//谁谁不同意或者同意之类

		SHOW_PLAY,//别人落子

		SHOW_MOVE,//别人落子
		SHOW_PASS,//结束回合响应

		SHOW_ABANDON,
		SHOW_ABANDON_PASS,//由于服务器自动pass，所以客户端需要处理此消息
		SHOW_TURN,//显示谁出
		SHOW_RESULT,//结算
		SHOW_LIANQI,// 重连时候的棋盘数据

		//游戏界面之间的消息定义

		//lianqi panel
		TO_LQP_ACTION_FAIL,
		TO_LQP_CLOCK,
		TO_LQP_SWITCH_HINT,//棋子信息提示

		//action panel
		ACTION_MOVE,//操作面板 移动
		ACTION_BAN_DIR,//操作面板 禁用方向
		ACTION_PASS,//操作面板 结束回合
		ACTION_PLAY,//操作面板 落子
		ACTION_STEP,//操作是否轮到自己


		//to gameview
		TO_GAMEVEIW_UPDATE_SCORE,
	};

	public enum LIANQI_DIRECTION_TYPE
	{
		LIANQI_DIRECTION_TYPE_NONE  = -1,//不使用禁止方向
		LIANQI_DIRECTION_TYPE_0 = 0,
		LIANQI_DIRECTION_TYPE_1,
		LIANQI_DIRECTION_TYPE_2,
		LIANQI_DIRECTION_TYPE_3,
		LIANQI_DIRECTION_TYPE_4,
		LIANQI_DIRECTION_TYPE_5,
	};


	//结构体用的蛋疼，还是用类型，虽然无法避免传递被修改
	//棋子基本数据结构
	public class Skill{
		public int healthChange;
		public int attackChange;
		public int absorbChange;
		public int applyPosX;
		public int applyPosY;
		public int applyPosZ; 
		public int basePosX;
		public int basePosY;
		public int basePosZ;
		public string type;
	};

	public class Buff{
		public int healthChange;
		public int attackChange;
		public int absorbChange;
		public string type;
	};

	public class Chess{
		public int health;
		public int attack;
		public int support;
		public int absorb;

		public int direction;
		public int playerID;

		public int x;
		public int y;
		public List<Skill> skillList;
		public List<Buff> buffList;
	};
	//-----------------------数据传递定义------------------
	public struct sV2C_Play{
		public int x;
		public int y;
		public LIANQI_DIRECTION_TYPE direction;
	};
	public struct sV2C_Move{
		public int x;
		public int y;
		public LIANQI_DIRECTION_TYPE direction;
	};

	public struct sC2V_Draw{
		public int local;
		public string name;//用于向其他用户提示谁请求和棋，是否同意
	}

	public struct sC2V_PlayOrMove{
		public int local;
		public int x;
		public int y;
		public LIANQI_DIRECTION_TYPE direction;
		public List<Chess> checkerBoard;
	}

	//结算需要更多详情定制 //诸如每个人的各种详情，不仅仅局限于最终的得分
	public struct sC2V_Result{
		
		public List<Chess> checkerBoard;
	}

	public struct sC2V_ShowTurn{
		public int local;
		public bool isPassTurn;
		public int round;
		public bool isTimeOut;
		public List<int> banDirList;
	}

	public struct sC2V_ShowClock{
		public int local;
		public int leftTime;
		public int step;
	}

	public struct sV2V_ShowGameResult{
		public struct sGameResult{
			public int seat;
			public bool isOwner;//只有房间模式有效
			public string head;
			public string name;
			public int area;//领地
			public int kill;//消灭
			public int score;//得分
			public int multi;//倍率
			public bool hasAbandon;//是否投降了
		}
		public CommonDefine.eCreateRoomType roomType;//用于是否显示房主标志,只有房间模式显示
		public CommonDefine.ePlazaLevelType level;
		public int baseScore;
		public int poolGold;//实际的奖池积分
		public List<sGameResult> gameResult;//根据积分 顺序
	}

	public struct sV2V_ActionBanDir{
		public int dir;
		public bool ban;
	};

	private static GameEvent _instance = null;

	public static GameEvent EM()
	{
		if(_instance == null)
		{
			_instance = new GameEvent();
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
