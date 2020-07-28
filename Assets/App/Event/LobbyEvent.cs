/**************************************/
//FileName: LianQiEventManager.cs
//Author: wtx
//Data: 13/04/2017
//Describe: 所有事件定义以及处理封装，请使用此来完成ui和c之间的交互，禁ui直接访问controller
/**************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class LobbyEvent : LianQiEventManager{
	public enum EVENT{
		// 因为不希望在界面体现网络通信相关代码，所以需要传出这两个事件
		LOBBY_EVENT_BEGIN = 20000,

		//大厅panel 之间的切换
		SHOW_LOBBY_PANEL,

		UPDATE_USER_INFO,//更新用户信息
		SHOW_USER_INFO,//主动请求刷新用户信息

		UPDATE_PLAZA,//更新场次显示
		SHOW_PLAZA,
		UPDATE_STORE,//更新商城显示,如果此时商城正在显示，以下类似
		SHOW_STORE,
		UPDATE_PACKAGE,//
		SHOW_PACKAGE,
		UPDATE_PRIVATEMSG,//个人邮件
		SHOW_PRIVATE_MSG,
		SHOW_SYSMSG,//显示某条系统公告－跑马灯
		UPDATE_SYSMSG,//系统公告，跑马灯走起
		UPDATE_SIGNIN,//更新签到界面
		SHOW_SIGNIN,
		UPDATE_LUCKDRAW,//抽奖
		SHOW_LUCKDRAW,
		UPDATE_FRIEND,//显示好友列表
		SHOW_FRIEND,//显示好友界面
		UPDATE_RANK,
		SHOW_RANK,//需要传榜单类型

		SHOW_POPUP,//弹窗界面之间的跳转

		SHOW_SIGNIN_RESULT,//显示签到结果，主要是是播放对应的动画
		SHOW_LUCKDRAW_RESULT,//显示抽奖结果

		REQ_UPDATE_EMAIL,
		SHOW_UPDATE_EMAIL_RESULT,

		SHOW_HAS_EMAIL,//有未读邮件标志

		REQ_FEEDBACK,
		REQ_OPEN_TALENTSLOT,
		RESP_OPEN_TALENTSLOT,
		UPDATE_USER_TALENT,//更新用户天赋列表
		//
		OPEN_CLOSE_EFFECT_SND,//开关音效
		OPEN_CLOSE_BG_SND,//开关背景音
	};

	public enum LOBBY_PANEL{
		LOBBY_NONE_PANEL,
		LOBBY_LOBBY_PANEL,
		LOBBY_PLAZAROOM_PANEL,
		LOBBY_PACKAGE_PANEL,
		LOBBY_STORE_PANEL,
		LOBBY_FRIEND_PANEL,
		LOBBY_RANK_PANEL,
		LOBBY_TASK_PANEL,//不一定是panel
		LOBBY_TALENT_PANEL,
	};

	public struct s_V2VShowLobbyPanel{
		public LOBBY_PANEL from;
		public LOBBY_PANEL to;
	};
		
	public struct sV2C_ReqUpdateEmail{
		public CommonDefine.eUpdateEmailType type;
		public int id;
	};

	public struct sV2C_Feedback{
		public CommonDefine.eFeedbackType type;
		public string content;
	};

	public struct sV2C_OpenTalentslot{
		public CommonDefine.eOpenByType type;
	};

	public struct s_V2C_UpdateUserTalent{
		public List<CommonDefine.eTalentType> talentList;
	};

	public struct s_C2V_UpdateTalentForOpenSlot{
		public CommonDefine.eRespResultType  ret;
		public long gold;
		public long diamond;
		public int currentTalentNum;
	};


	private static LobbyEvent _instance = null;

	public static LobbyEvent EM()
	{
		if(_instance == null)
		{
			_instance = new LobbyEvent();
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

	//--------------------数据结构定义-------------------
	//场次配置
	public class PlazaLevel{
		public int base_score;   // --底分
		public int minsr;      // --进入下限   
		public int maxsr;     // --进入上限
		public int levelid;     // --场等级
	};
	public class Plaza
	{
		public enum LMT_TYPE
		{
			LMT_BY_GOLD,//金币限制条件
			LMT_BY_LEVEL,//等级限制条件
		};

		public LMT_TYPE lmtType;//限制条件类型
		public int plazaid;   // --场次id
		public int roomType;      //场类型
		public float star;     // --难度等级
		public string rule ;//配置字符串
		public string name;
		public string des;
		public List<PlazaLevel> plazaLevel;
	};
	//商城配置
	public struct Store{
		public int id;
		public string point;//计费点
		public int price;
		public string name;
		public string des;
		public string pic;
		public string data;
	};
	//道具配置
	public struct Prop{
		public enum PROP_TYPE
		{
			NONE,
			PROP,
			SKIN,
			HERO,
			//etc...
		};
		public PROP_TYPE type;
		public int id;//道具id
		public int price;//道具价格
		public string name;//道具名字
		public string pic;//道具图片
		public string des;//道具描述
		public string data;//道具属性，待定
	};
	//背包
	public struct Package{
		public Prop prop;
		public int prop_cnt;
		public int end_time;
	};
	//个心消息
	public struct PrivateMsg{
		public int id;
		public int has_read;
		public string title;
		public string content;
		public string author;
		public string send_time;
		public string end_time;//过期时间
	};
	//系统公告
	public struct SysMsg{
		public int id;
		public string content;
	};
	//签到配置
	public struct SignIn{
		public enum SIGNIN_AWARD_TYPE
		{
			NONE,
			GOLD,
			PROP,
			//etc...
		};
		public SignIn.SIGNIN_AWARD_TYPE type;//签到奖励类型,请根据type来决定显示的奖励
		public Prop prop;
		public int gold_num;
	};
	public struct SignInData{
		public bool hasSigned;
		public int currentSignDay;
		public List<SignIn> signInList;
	};
	//抽奖配置
	public struct LuckDraw{
		public SignIn.SIGNIN_AWARD_TYPE type;
		public Prop prop;
		public int gold_num;
	};

	public struct LuckDrawData{
		public bool hasDrawed;
		public List<LuckDraw> luckDrawList;
	};

	public struct Friend{
		public int friend_id;
		public string name;
		public string head_url;
		public string lastlogin_time;
		public string des;
		public int friend_score;//亲密值等
	}

	//不再做分类，使用的时候查找筛选,有优化余地，to do
	public struct RankScopeType{
		public enum RANK_SCOPE_TYPE
		{
			RANK_ALL,//全世界
			RANK_FRIEND,//好友
			RANK_AREA,//区服
		};

		public enum RANK_TYPE
		{
			RANK_GOLD,//财富
			RANK_CHARM,//魅力
			RANK_DIAMOND,//钻石
			RANK_SCORE,//积分
			RANK_WINRATE,//胜率
			RANK_EXP,
		};
		public RANK_SCOPE_TYPE scope;
		public RANK_TYPE type;
	};
	public struct Rank{
		public RankScopeType rst;
		public int userID;
		public string name;
		public string headUrl;
		public int exp;
		public int score;
		public int charm;
		public int diamond;
		public int gold;
		public float win_rate;
	};
}
