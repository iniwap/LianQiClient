/**************************************/
//FileName: ProtocolDefine.cs
//Author: wtx
//Data: 20/03/2017
//Describe: class for the all server-client protocol define
/**************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Pomelo.DotNetClient;

namespace ProtocolDefine
{
	#region 游戏类型定义
	public enum GameType
	{
		GAME_LIANQI = 1,
		GAME_OTHER = 2,
	};

	#endregion

	#region 渠道列表
	public enum ChannelType{
		CHANNEL_APPSTORE,
		CHANNEL_YYB,
		CHANNEL_BAIDU,
		CHANNEL_360,
		CHANNEL_XIAOMI,
		CHANNEL_HUAWEI,
		CHANNEL_OPPO,
		CHANNEL_VIVO,
		CHANNEL_MEIZU,
		CHANNEL_JINLI,
		CHANNEL_PPZHUSHOU,
		CHANNEL_WANDOUJIA,
		CHANNEL_GOOGLEPLAY,
		//ETC...
	};
	#endregion

	#region 大厅协议
	public enum LobbyProtocol
	{
		P_LOBBY_BEGIN = 10000,//大厅协议相关起止协议号
		P_LOBBY_REQ_CONNECT,
		P_LOBBY_RESP_CONNECT,
		//登陆
		P_LOBBY_REQ_LOGIN,
		P_LOBBY_RESP_LOGIN,
		//在线人数
		P_LOBBY_REQ_ONLINE_CNT,
		P_LOBBY_RESP_ONLINE_CNT,
		//场次
		P_LOBBY_REQ_PLAZA_LIST,
		P_LOBBY_RESP_PLAZA_LIST,
		//道具信息
		P_LOBBY_REQ_PROP_LIST,
		P_LOBBY_RESP_PROP_LIST,
		//包裹信息
		P_LOBBY_REQ_PACKAGE_LIST,
		P_LOBBY_RESP_PACKAGE_LIST,
		//系统公告
		P_LOBBY_REQ_SYSMSG,
		P_LOBBY_RESP_SYSMSG,
		//个人消息（邮件）
		P_LOBBY_REQ_PRIVATEMSG,
		P_LOBBY_RESP_PRIVATEMSG,
		//修改昵称
		P_LOBBY_REQ_CHANGE_NICKNAME,
		P_LOBBY_RESP_CHANGE_NICKNAME,
		//排行榜
		P_LOBBY_REQ_RANK_LIST,
		P_LOBBY_RESP_RANK_LIST,
		//商品列表
		P_LOBBY_REQ_STORE_LIST,
		P_LOBBY_RESP_STORE_LIST,

		//好友列表
		P_LOBBY_REQ_FRIEND_LIST,
		P_LOBBY_RESP_FRIEND_LIST,

		//签到、抽奖数据，由此生成用户签到、抽奖信息
		P_LOBBY_REQ_SIGNIN_LUCKDRAW_DATA,
		P_LOBBY_RESP_SIGNIN_LUCKDRAW_DATA,

		//请求签到,需要手动点击签到
		P_LOBBY_REQ_SIGNIN,
		P_LOBBY_RESP_SIGNIN,
		//抽奖
		P_LOBBY_REQ_LUCKDRAW,
		P_LOBBY_RESP_LUCKDRAW,

		P_LOBBY_AWARD_EMAIL,
		P_LOBBY_REQ_UPDATE_EMAIL,//删除、已读、领奖等
		//..
		P_LOBBY_REQ_FEEDBACK,//反馈
		P_LOBBY_RESP_FEEDBACK,

		P_LOBBY_REQ_OPENTALENTSLOT,//打开天赋槽
		P_LOBBY_RESP_OPENTALENTSLOT,//打开槽响应

		P_LOBBY_END = 19999,//大厅协议相关起止协议号
	};
	#endregion

	#region 游戏协议，即房间相关协议，同时包括非具体游戏本身协议
	public enum GameProtocol //or RoomProtocol
	{
		P_GAME_BEGIN  = 20000,//游戏房间相关起止协议号
		//加入房间
		P_GAME_REQ_JOINROOM,
		P_GAME_RESP_JOINROOM,
		//创建房间
		P_GAME_REQ_CREATEROOM,
		P_GAME_RESP_CREATEROOM,
		//离开房间
		P_GAME_REQ_LEAVEROOM,
		P_GAME_RESP_LEAVEROOM,
		//玩家信息、玩家动作
		P_GAME_PLAYER_ACT,
		P_GAME_PLAYER_INFO,
		P_GAME_PLAYER_STATE,
		P_GAME_PLAYER_TALK_MSG,
		//心跳
		P_GAME_REQ_HEART,
		P_GAME_RESP_HEART,
		//托管
		P_GAME_REQ_TRUST,
		P_GAME_RESP_TRUST,
		//时钟
		P_GAME_CLOCK,
		//房间列表
		P_GAME_REQ_ROOMLIST,
		P_GAME_RESP_ROOMLIST,
		P_GAME_ROOMSTATE_CHANGE,

		//通知玩家离开房间
		P_GAME_PLAYER_LEAVE,

		//通知服务器进入游戏界面完成
		P_GAME_ENTER_ROOM_FINISH,
		P_GAME_START_GAME,//通知服务器开始游戏

		P_GAME_REPORT_TALENT_LIST,//上报天赋列表
		P_GAME_TALENT_LIST,//推送天赋列表,包括自己上报的响应

		P_GAME_END = 29999,//游戏房间相关起止协议号
	};
	#endregion

	#region 联棋游戏
	public enum GameLianQiProtocol
	{
		P_GAME_LIANQI_BEGIN = 30000,
		//游戏开始
		P_GAME_LIANQI_START,
		//玩家棋盘，需要在每次落子或者移动后发送
		P_GAME_LIANQI_QI,
		//执子方
		P_GAME_LIANQI_TURN,
		//弃权
		P_GAME_LIANQI_REQ_PASS,
		P_GAME_LIANQI_RESP_PASS,
		//落子
		P_GAME_LIANQI_REQ_PLAY,
		P_GAME_LIANQI_RESP_PLAY,//此处返回逻辑运算结果，执行动画等
		//吃掉子后是否移动
		P_GAME_LIANQI_REQ_MOVE,
		P_GAME_LIANQI_RESP_MOVE,//MOVE之后触发新棋局形式，如果可移动携带计算结果
		//请求和棋
		P_GAME_LIANQI_REQ_DRAW,
		P_GAME_LIANQI_RESP_DRAW,
		//结束，带结算
		P_GAME_LIANQI_RESULT,
		//断线重连
		P_GAME_LIANQI_FLAG,
		P_GAME_LIANQI_REQ_ABANDON,// 投降
		P_GAME_LIANQI_RESP_ABANDON,// 投降

		P_GAME_LIANQI_ABANDON_PASS,// 自动切换手
		//更多待定..

		P_GAME_LIANQI_END = 39999,//游戏协议起止号，这里只有联棋
	};
	#endregion
	//other game


	#region 所有消息类定义，请使用以下来解析消息，切勿直接使用json数据
	[Serializable]
	public class msgSimpleResp{
		public enum eFlag{
			ERROR = -1,
			SUCCESS = 0,
		};
		public int ret;//0 成功，其他失败
		public static msgSimpleResp deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgSimpleResp>(msg.jsonObj.ToString());
		}
	}
	/// <summary>
	/// 请求登陆消息
	/// </summary>
	public class msgLogin
	{
		public enum eLoginType
		{
			LOGIN_TYPE_YK = 1,
			LOGIN_TYPE_QQ = 2,
			LOGIN_TYPE_WX = 3,
		};

		public enum eClientType
		{
			CLIENT_TYPE_IPHONE = 1,
			CLIENT_TYPE_IPAD = 2,
			CLIENT_TYPE_ANDROID = 3,
			CLIENT_TYPE_PC = 4,
			//..MORE
		};
		public enum eNetWorkType
		{
			NETWORK_TYPE_WIFI = 1,
			NETWORK_TYPE_4G = 2,
			// MORE
		};

		public int userID { get; set; }  //游客第一次登陆请填0//如果有值则认为是后续登陆
		public eLoginType loginType { get; set; }
		public int area;//区服id
		public string password{ get; set; }
		public string deviceID{ get; set; }//设备码
		public int osVersion{ get; set; }//操作系统版本号
		public int ipAddr{ get; set; }
		public int channelID{ get; set; } //渠道号
		public int appVersion{ get; set;}
		public int netWorkType{ get; set;}

		//三方登陆数据
		public string openID{ get; set;}
		public string token{ get; set;}
		public string nickName{ get; set;}
		public string head{ get; set;}
		public int sex{ get; set;}
		public int expireTime{ get; set;}

		public msgLogin(){
			userID = 0;
			loginType = eLoginType.LOGIN_TYPE_YK;
			area = 1;
			password = "";
			deviceID = "";
			osVersion = 0;
			ipAddr = 0;
			channelID = 0;
			appVersion = 0;
			netWorkType = (int)eNetWorkType.NETWORK_TYPE_WIFI;

			openID = "";
			token = "";
			nickName = "";
			head = "";
			sex = 0;
			expireTime = 0;
		}
	};
	/// <summary>
	/// 用户登陆成功数据返回
	/// </summary>
	[Serializable]
	public class msgUserData
	{
		public enum eLoginResultFlag
		{
			LOGIN_SUCCESS = 0,
			LOGIN_FAIL_INCORRECT_PWD = 1,//密码错误
			LOGIN_FAIL_NOT_EXIST_ACCOUT = 2,//账号不存在
			LOGIN_FAIL_NOT_SUPPORT_TYPE = 3,
			LOGIN_FAIL_OPENID_ERROR = 4,
			LOGIN_SUCCESS_HAS_LOGIN = 5,
		};

		public eLoginResultFlag flag;
		public int user_id; 
		public int area;
		public string name;
		public int sex;
		public string head;
		public int vip;
		public long gold;
		public int win;//赢
		public int lose;//输
		public int draw;//平－和
		public int escape;//逃跑
		public int talent;//天赋槽数，已打开的天赋槽
		public long gameTime;//总游戏时长
		public long exp;//经验
		public int room_id;//是否在房间中,0不在
		public bool adult;//是否成年
		public int charm;//魅力
		public long lastlogin_time;//最后登陆时间
		public int score;//得分-对应段位，是总星数，每个段位分多少星
		public long diamond;//钻石
		public long energy;//能量

		public static msgUserData deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgUserData>(msg.jsonObj.ToString());
		}

	};
	[Serializable]
	public class Server
	{
		public enum SERVER_STATE
		{
			SERVER_STATE_NOT_AVAILABLE,// 服务器不可用 
			SERVER_STATE_IDLE,//空闲
			SERVER_STATE_CROWD,//拥挤
			SERVER_STATE_FULL,//爆满
		};
		public SERVER_STATE state;
		public string host;
		public int port;
		public string serverName;
	};
	[Serializable]
	public class msgRespServerList
	{
		public List<Server> serverList;
		public static msgRespServerList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespServerList>(msg.jsonObj.ToString());
		} 
	};
	/// <summary>
	/// 场列表
	/// </summary>
	public class msgReqPlazaList
	{
		public GameType game;
	};
	[Serializable]
	public class PlazaLevel{
		public int base_score;   // --底分
		public int minsr;      // --进入下限   
		public int maxsr;     // --进入上限
		public int levelid;     // --场等级
	};
	[Serializable]
	public class Plaza
	{
		public enum LMT_TYPE
		{
			LMT_BY_GOLD,//金币限制条件
			LMT_BY_LEVEL,//等级限制条件
		};

		public LMT_TYPE lmt_type;//限制条件类型
		public int plazaid;   // --场次id
		public int room_type;      // --底注    
		public float star;     // --进入下限
		public string rule ;//配置字符串
		public string name;
		public string des;
		public List<PlazaLevel> plazaLevel;
	};

	[Serializable]
	public class msgRespPlazaList
	{
		//[NonSerialized]
		public List<Plaza> plazaList;
		public static msgRespPlazaList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespPlazaList>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 道具列表
	/// </summary>
	public class msgReqPropList
	{
		public GameType game;
	};
	[Serializable]
	public class Prop
	{
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

	[Serializable]
	public class msgRespPropList
	{
		public List<Prop> propList;
		public static msgRespPropList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespPropList>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 背包信息
	/// </summary>
	public class msgReqPackageList
	{
		public GameType game;
	};
	[Serializable]
	public class Package
	{
		public int prop_id;
		public int prop_cnt;
		public int end_time;
	};

	[Serializable]
	public class msgRespPackageList
	{
		public List<Package> packageList;
		public static msgRespPackageList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespPackageList>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 系统公告
	/// </summary>
	public enum SYS_OR_PRIVATE_MSG_TYPE
	{
		TYPE_MSG_SYS,//系统消息
		TYPE_MSG_PRIVATE,//个人消息
		TYPE_MSG_NOTICE,//系统公告
	};
	public class msgReqSysMsgList
	{
		public GameType game;
		public ChannelType channelId;
	};
	[Serializable]
	public class SysAndPrivateMsg
	{
		public SYS_OR_PRIVATE_MSG_TYPE type;
		public int id;
		public int has_read;
		public string title;
		public string content;
		public string author;
		public string send_time;
		public string end_time;//过期时间
	};
	[Serializable]
	public class msgRespSysMsgList
	{
		public List<SysAndPrivateMsg> sysMsgList;
		public static msgRespSysMsgList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespSysMsgList>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 个人消息
	/// </summary>
	public class msgReqPrivateMsgList
	{
		public GameType game;
		public int cnt;//请求几条
		public int begin;//开始的索引
	};

	[Serializable]
	public class msgRespPrivateMsgList
	{
		public List<SysAndPrivateMsg> privateMsgList;
		public static msgRespPrivateMsgList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespPrivateMsgList>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 排行榜
	/// </summary>
	public class msgReqRankList
	{
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
		};
		public GameType game;
		public RANK_SCOPE_TYPE scope;
		public RANK_TYPE type;

		public int areaID;//区服id
		public int rankNum;//多少人
	};
	[Serializable]
	public class Rank
	{
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

	[Serializable]
	public class msgRespRankList
	{
		public msgReqRankList.RANK_TYPE type;
		public msgReqRankList.RANK_SCOPE_TYPE scope;
		public List<Rank> rankList;//是否需要分条发送，待测试
		public static msgRespRankList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespRankList>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 商品列表
	/// </summary>
	public class msgReqStoreList
	{
		public GameType game;
		public ChannelType channelID;
	};
	[Serializable]
	public class Product
	{
		public int id;
		public string point;//计费点
		public int price;
		public string name;
		public string des;
		public string pic;
		public string data;
	};
	[Serializable]
	public class msgRespStoreList
	{
		public List<Product> storeList;
		public static msgRespStoreList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespStoreList>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 好友列表
	/// </summary>
	public class msgReqFriendList
	{
		public GameType game;
		//是否加入一次请求多少条？
	};
	[Serializable]
	public class Friend
	{
		public int friend_id;
		public string name;
		public string head_url;
		public string lastlogin_time;
		public string des;
		public int friend_score;//亲密值等
	};
	[Serializable]
	public class msgRespFriendList
	{
		public List<Friend> friendList;
		public static msgRespFriendList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespFriendList>(msg.jsonObj.ToString());
		}
	};

	[Serializable]
	public class msgReqFeedback{
		public CommonDefine.eFeedbackType type;
		public string content;
		public static msgReqFeedback deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgReqFeedback>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 签到抽奖数据，以此生成签到、抽奖界面
	/// </summary>
	public class msgReqSignInLuckDrawData
	{
		public GameType game;
		public int areaID;
		public string deviceID;

	};
	[Serializable]
	public class SignIn
	{
		public enum SIGNIN_AWARD_TYPE
		{
			NONE,
			GOLD,
			PROP,
			//etc...
		};
		public SIGNIN_AWARD_TYPE type;//签到奖励类型,请根据type来决定显示的奖励
		public int day;//第几天，也可以更加下标来用，一般按照天顺序
		public int prop_id;//请到proplist查询相关道具信息用于显示
		public int gold_num;
		//etc...
	};
	[Serializable]
	public class LuckDraw
	{
		public SignIn.SIGNIN_AWARD_TYPE type;
		public int prop_id;
		public int gold_num;
	};

	[Serializable]
	public class msgRespSignInLuckDrawData
	{
		public bool hasDrawed;
		public bool hasSigned;
		public int signInDay;// 当前签到第几天
		public List<SignIn> signData;//长度代表有多少天可以签到,一般是7日或者30日。实际用途为提高7日/30日留存率
		public List<LuckDraw> luckData;//长度代表有多少抽奖项，抽奖为日抽，或者不定期抽奖，此项需要判断有无来显示界面
		public static msgRespSignInLuckDrawData deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespSignInLuckDrawData>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	///  请求签到
	/// </summary>
	public class msgReqSignIn
	{
		public GameType game;
		public int areaID;
		public string deviceID;
	};
	[Serializable]
	public class msgRespSignIn
	{
		public enum SIGNIN_FLAG
		{
			SIGNIN_FLAG_SUCESS,//签到成功
			SIGNIN_FLAG_HAS_SIGNIN,//已经签到过
			SIGNIN_FLAG_INVLID,//签到失败
		};
		public SIGNIN_FLAG flag;
		public SignIn.SIGNIN_AWARD_TYPE type;//签到奖励类型,请根据type来决定显示的奖励
		public int prop_id;
		public int gold_num;

		public static msgRespSignIn deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespSignIn>(msg.jsonObj.ToString());
		}
	};

	public class msgReqLuckDraw
	{
		public GameType game;
		public int areaID;
		public string deviceID;
	};
	[Serializable]
	public class msgRespLuckDraw
	{
		public enum LUCKDRAW_FLAG
		{
			LUCKDRAW_FLAG_SUCESS,//抽奖成功
			LUCKDRAW_FLAG_HAS_DRAW,//已抽过
			LUCKDRAW_FLAG_INVLID,//抽奖失败
		};
		public LUCKDRAW_FLAG flag;
		public SignIn.SIGNIN_AWARD_TYPE type;
		public int prop_id;
		public int gold_num;

		public static msgRespLuckDraw deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespLuckDraw>(msg.jsonObj.ToString());
		}
	};

	//奖励邮件信息 服务器推送 所有奖励都必须走这条消息，服务端会同时插入到个人邮件
	[Serializable]
	public class msgAwardEmail{
		public SYS_OR_PRIVATE_MSG_TYPE type;
		public bool needAdd2Email;//是否需要添加到邮件列表，比如仅仅单纯发奖的，就不需要添加到邮件
		public int id;
		public int has_read;
		public string title;
		public string content;
		public string author;
		public string send_time;
		public string end_time;//过期时间
		public static msgAwardEmail deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgAwardEmail>(msg.jsonObj.ToString());
		}
	};
	//请求更新邮件信息 和响应 同一条
	[Serializable]
	public class msgReqUpdateEmail{
		public CommonDefine.eUpdateEmailType type;
		public int awardEmailId;//奖励邮件的id
		public static msgReqUpdateEmail deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgReqUpdateEmail>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 打开天赋槽
	/// </summary>
	public class msgReqOpenTalentslot{
		public GameType game;
		public CommonDefine.eOpenByType openBy;
	}
	/// <summary>
	/// 开槽响应
	/// </summary>
	[Serializable]
	public class msgRespOpenTalentslot{
		public enum eOpenTalentslotResultType{
			OPEN_SUCCESS,
			OPEN_FAIL,//
			OPEN_FAIL_LESS_GOLG,
			OPEN_FAIL_LESS_DIAMOND,
			OPEN_FAIL_MAX_SLOT,//达到最大槽数，即已经全打开过了，属于非法操作
		};
		public eOpenTalentslotResultType result;
		public int currentOpenedCnt;//当前已打开的个数//可以得到当前打开的是currentOpenedCnt-1，客户端限制必须依次打开

		public CommonDefine.eOpenByType openBy;
		public long currentGold;
		public long currentDiamond;

		public static msgRespOpenTalentslot deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespOpenTalentslot>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 上报天赋列表/天赋列表响应
	/// </summary>
	[Serializable]
	public class msgTalentList{
		public int seat;// 如果是自己，则是上报响应，不是自己则为其他玩家技能列表

		public string talentList;
		public static msgTalentList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgTalentList>(msg.jsonObj.ToString());
		} 
	}

	/// <summary>
	/// 加入房间
	/// </summary>
	public class msgReqJoinRoom
	{
		public GameType game;
		public int playerNum;//由于一个场支持两种人数模式，所以需要客户端上传这个两个参数
		public int gridLevel;//
		public int roomId;//用于加入房间，即房间列表进入，通过场进入的请务必填roomid=0
		public int plazaID;//用于加入场id，即需要分配roomid
		public string pwd;//房间密码，如果非自建房间，不需要传递密码。自建房间默认也是无密码的
	};

	[Serializable]
	public class msgRespJoinRoom
	{
		public enum RESP_JOINROOM_FLAG
		{
			JOINROOM_SUCCESS,//成功
			JOINGROOM_FAIL_ROOM_NOT_EXIST,//房间不存在即房间号错误
			JOINROOM_FAIL_NO_FREE_ROOM,//没有可用房间
			JOINROOM_GOLD_LESS,//金币不足
			JOINROOM_GOLD_MORE,//金币过高
			JOINROOM_LEVEL_LESS,//等级过低
			JOINROOM_LEVEL_MORE,//等级过高
			JOINROOM_PWD_ERR,//密码错误
			JOINROOM_ACCOUNT_ERR,//用户不存在
			JOINROOM_PLAZA_ERR,//所选择的场不存在
			JOINGROOM_ALREADY_IN_ROOM,//已经在房间中
			JOINROOM_FIAL_SYSERR,//系统出错
			//etc...
		};
		public RESP_JOINROOM_FLAG flag;
		public int roomId;
		public int levelId;
		public int baseScore;
		public int owner;
		public string rule;
		public bool isRelink;
		public int plazaid;
		public int roomType;

		public static msgRespJoinRoom deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespJoinRoom>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 通知服务器，进入游戏界面完成，可以发送用户信息了
	/// </summary>
	public class msgNotifyEnterRoomFinish
	{
		public bool isRelink;
	};
	//通知服务器可以开始游戏
	public class msgNotifyStartGame
	{
		public GameType game;
		public bool isEnterRoomFinsh;
	};

	/// <summary>
	/// 创建房间
	/// </summary>
	public class msgReqCreateRoom
	{
		public GameType game;
		public CommonDefine.eCreateRoomType roomType;
		public int baseScore;
		public int minScore;
		public int maxScore;
		public string roomName;
		public string roomPassword;
		public string rule;//房间规则
	};

	[Serializable]
	public class msgRespCreateRoom
	{
		public int flag;//=0成功
		public int roomId;
		public string roomName;
		public string roomPassword;
		public string rule;//房间规则
		public static msgRespCreateRoom deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespCreateRoom>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 离开房间
	/// </summary>
	public class msgReqLeaveRoom
	{
		public GameType game;
	};

	[Serializable]
	public class msgRespLeaveRoom
	{
		public enum LEAVE_TYPE
		{
			LEAVE_NORMAL,
			LEAVE_KICK,
			LEAVE_ESCAPE,
			LEAVE_NOT_IN_ROOM,//不在房间中
			LEAVE_CANT_LEAVE,//不能离开
			LEAVE_DISSOLVE,//解散
		};
		public LEAVE_TYPE type;
		public int leaveResult;//0成功，other失败
		public string msg;
		public static msgRespLeaveRoom deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespLeaveRoom>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 心跳
	/// </summary>
	public class msgReqHeartbeat
	{
		public GameType game;
	};

	[Serializable]
	public class msgRespHeartbeat
	{
		public int active;
		public static msgRespHeartbeat deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespHeartbeat>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 托管
	/// </summary>
	public class msgReqTrust
	{
		public GameType game;
		public int seat;
	};

	[Serializable]
	public class msgRespTrust
	{
		public int seat;
		public static msgRespTrust deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespTrust>(msg.jsonObj.ToString());
		}
	};

	//// <summary>
	/// 用户离开房间，用于玩家未准备状态，其他用户离开
	/// 说明：收到此消息，如果不是自己，则需要移除保存的用户数据以及更新界面显示
	/// </summary>
	[Serializable]
	public class msgPlayerLeave{
		public enum PLAYER_LEAVE_ROOM_TYPE{
			LEAVE_ROOM_NORMAL,//正常退出
			LEAVE_ROOM_REMOVED,//等待游戏开始时间过长
			LEAVE_ROOM_EXIST_TIME_OUT,//超出最长房间占用时间
			LEAVE_ROOM_OWNER_OFFLINE,//房主离线（即房间里没有人了）
			LEAVE_ROOM_OWNER_DISSOLVE,//
			LEAVE_ROOM_GAMEEND,
		};
		public PLAYER_LEAVE_ROOM_TYPE type;
		public int seat;
		public int userID;
		public static msgPlayerLeave deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgPlayerLeave>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 用户动作－准备、坐下等
	/// </summary>
	public class msgPlayerAct
	{
		public enum ACT_TYPE
		{
			ACT_SITDOWN,//坐下，之后才能准备
			ACT_STANDUP,//站起
			ACT_READY,//准备
			ACT_SEEING,//旁观
		};
		public ACT_TYPE act;
		public int seat;
	};

	/// <summary>
	/// 玩家信息－即玩家进入房间
	/// </summary>
	[Serializable]
	public class msgPlayerInfo
	{
		public int userID;
		public string name;
		public string headUrl;
		public int sex;
		public int vip;
		public long gold;
		public int win;
		public int lose;
		public int draw;
		public int escape;
		public int talent;
		public long exp;//经验
		public int score;
		public int charm;
		public int seat;
		//etc...

		public static msgPlayerInfo deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgPlayerInfo>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 玩家状态
	/// </summary>
	[Serializable]
	public class msgPlayerState
	{
		public enum STATE_TYPE
		{
			STATE_TYPE_NONE,//无效状态
			STATE_TYPE_STANDUP,//站起
			STATE_TYPE_SITDOWN,//坐下
			STATE_TYPE_ROOMREADY,//准备中
			STATE_TYPE_PLAYING,//游戏中
			STATE_TYPE_OFFLINE,//离线
			STATE_TYPE_SEEING,//观战
		};
		public STATE_TYPE state;
		public int userID;
		public int seat;
		public static msgPlayerState deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgPlayerState>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 玩家聊天，在游戏内的聊天，这里发送和接受都用这个消息//发送userid可以填0
	/// </summary>
	[Serializable]
	public class msgPlayerTalkMsg
	{
		public msgSimpleResp.eFlag flag;
		public int seat;
		public string content;
		public static msgPlayerTalkMsg deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgPlayerTalkMsg>(msg.jsonObj.ToString());
		}
	};

	[Serializable]
	public class msgClock
	{
		public int seat;
		public int leftTime;
		public msgStep.GAME_STEP step;
		public static msgClock deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgClock>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 自建房间列表
	/// </summary>
	public class msgReqRoomList
	{
		public GameType game;
		public int areaID;
		public int begin;
		public int reqCnt;//请求多少条room信息
	};
	[Serializable]
	public class Room
	{
		public int roomId;
		public int isFull;
		public string roomName;
		public string roomDes;
		public int roomPersonCnt;
		public string roomPwd;
		public string rule;
	};
	[Serializable]
	public class msgRespRoomList
	{
		public List<Room> roomList;
		public static msgRespRoomList deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRespRoomList>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 房间状态变化
	/// </summary>
	[Serializable]
	public class msgRoomStateChange
	{
		public enum ROOMSTATE_CHANGE_TYPE
		{
			ROOMSATE_NONE,
			ROOMSTATE_ADD,//有增加房间，如果用户在房间列表显示界面，请插入此条到顶部
			ROOMSTATE_REMOVE,//有房间删除
			ROOMSTATE_UPDATE,//房间状态更新
		};
		public ROOMSTATE_CHANGE_TYPE type;
		public int roomId;
		public int personCnt;//删除无此字段，即内容空，勿访问
		//删除和更新无以下字段
		public string roomName;
		public string roomPassword;
		public string rule;//房间规则

		public static msgRoomStateChange deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgRoomStateChange>(msg.jsonObj.ToString());
		}
	};
	public enum LIANQI_DIRECTION_TYPE
	{
		LIANQI_DIRECTION_TYPE_NONE = -1,//不使用禁止方向
		LIANQI_DIRECTION_TYPE_0 = 0,
		LIANQI_DIRECTION_TYPE_1 = 1,
		LIANQI_DIRECTION_TYPE_2 = 2,
		LIANQI_DIRECTION_TYPE_3 = 3,
		LIANQI_DIRECTION_TYPE_4 = 4,
		LIANQI_DIRECTION_TYPE_5 =5
	};
	public enum GAME_OP_RESP_FLAG{
		SUCCESS,//成功
		SEAT_ERR,//座位号错误
		ILLEGAL,//非法落子
		INCORRECT_TURN,//非落子方
		NOT_IN_GAME,
		INCORRECT_STEP,
	};

	public enum REQ_RESP_TYPE{
		REQ,//向其它玩家发送
		REQ_RESP,//其它玩家的响应
		RESP,//自己收到的网络响应
	};

	//棋子基本数据结构
	[Serializable]
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
	[Serializable]
	public class Buff{
		public int healthChange;
		public int attackChange;
		public int absorbChange;
		public string type;
	};
	[Serializable]
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
	/// <summary>
	/// 联棋游戏开始 
	/// </summary>
	[Serializable]
	public class msgLianQiStart
	{
		public int flag;
		public int firstHandSeat;
		public static msgLianQiStart deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiStart>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 联棋棋盘
	/// </summary>
	[Serializable]
	public class msgLianQi
	{
		public int turn;//当前下棋的人的seat
		public List<Chess> checkerBoard;
		//棋盘数据相对复杂，需要进一步确认
		public static msgLianQi deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQi>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 联棋执子
	/// </summary>
	[Serializable]
	public class msgLianQiTurn
	{
		public int seat;
		public int round;// 当前回合数
		public bool isPassTurn;//是否是第一手，自动
		public bool isTimeOut;//是否超时切换手
		public List<LIANQI_DIRECTION_TYPE> lmt;
		public static msgLianQiTurn deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiTurn>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 服务器对于投降的玩家自动pass，客户端需要同时修改游戏逻辑的当前玩家
	/// </summary>
	[Serializable]
	public class msgLianQiAbandonPass{
		public int seat;
		public static msgLianQiAbandonPass deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiAbandonPass>(msg.jsonObj.ToString());
		}
	}

	/// <summary>
	/// 联棋过
	/// </summary>
	public class msgLianQiReqPass
	{
		public int seat;
	};

	[Serializable]
	public class msgLianQiRespPass
	{
		public GAME_OP_RESP_FLAG flag;
		public int turn;//当前手
		public static msgLianQiRespPass deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiRespPass>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 联棋投降
	/// </summary>
	public class msgLianQiReqAbandon
	{
		public int seat;
	};
	[Serializable]
	public class msgLianQiRespAbandon
	{
		public GAME_OP_RESP_FLAG flag;
		public int seat;
		public static msgLianQiRespAbandon deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiRespAbandon>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 联棋落子
	/// </summary>
	public class msgLianQiReqPlay
	{
		public int seat;
		public int x;
		public int y;
		public LIANQI_DIRECTION_TYPE direction;
		//ect...
	};
	[Serializable]
	public class msgLianQiRespPlay
	{
		public GAME_OP_RESP_FLAG flag;
		public int seat;
		public int x;
		public int y;
		public LIANQI_DIRECTION_TYPE direction;
		public List<Chess> checkerBoard;
		public static msgLianQiRespPlay deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiRespPlay>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 联棋移动
	/// </summary>
	public class msgLianQiReqMove
	{
		public int seat;
		public int x;
		public int y;
		public LIANQI_DIRECTION_TYPE direction;
	};
	[Serializable]
	public class msgLianQiRespMove
	{
		public GAME_OP_RESP_FLAG flag;
		public int seat;
		public int x;
		public int y;
		public LIANQI_DIRECTION_TYPE direction;
		public List<Chess> checkerBoard;
		public static msgLianQiRespMove deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiRespMove>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 联棋请求和局
	/// </summary>
	public class msgLianQiReqDraw
	{
		public int seat;
	};
	[Serializable]
	public class msgLianQiRespDraw
	{
		public REQ_RESP_TYPE type;
		public int flag;//0 同意和局，other 不同和局
		public int seat;// 如果不同意和局，该值有效
		public static msgLianQiRespDraw deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiRespDraw>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 联棋结算
	/// </summary>
	[Serializable]
	public class GameResult{
		public int seat;
		public int area;//领地
		public int kill;//消灭
		public int score;//得分
		public int multi;//倍率
		public bool hasAbandon;//是否投降了
	}

	[Serializable]
	public class msgLianQiResult
	{
		public enum FORMATION_TYPE
		{
			//最大阵型枚举，用于分享
			FORMATION_NONE,
			FORMATION_1,
		};
		public int poolGold;//实际的奖池积分
		public List<GameResult> result;
		public List<FORMATION_TYPE> type;//如果存在

		public List<Chess> checkerBoard;

		public static msgLianQiResult deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiResult>(msg.jsonObj.ToString());
		}
	};

	/// <summary>
	/// 各种flag，包括重连
	/// </summary>
	[Serializable]
	public class msgLianQiFlag
	{
		public enum FLAG_TYPE
		{
			RELINK_TYPE_BEGIN,
			RELINK_TYPE_END,
			//ect 动画
			//ect 成就
			//ect 大棋形
		};
		public FLAG_TYPE flag;

		public static msgLianQiFlag deserialize(Message msg)
		{
			return JsonUtility.FromJson<msgLianQiFlag>(msg.jsonObj.ToString());
		}
	};
	/// <summary>
	/// 用于结束某个游戏步骤，比如如果有开局动画，那么在收到游戏开始后，播放动画，动画完成后，发送该消息，则服务器会接下来发送下一个步骤
	/// 再比如，如果有掷骰子，则在骰子动画结束后，发送该消息，服务器再发送谁先手落子，否则服务器会等待超时，发送谁先手
	/// 这样是为了保证消息的顺序性，不需要客户端缓存状态，避免产生错乱显示
	/// </summary>
	public class msgStep{
		public enum GAME_STEP{
			GAME_STEP_NONE,//空状态
			GAME_STEP_START,//对局开始
			GAME_STEP_DICE,//掷骰子
			GAME_STEP_PLAY,//落子阶段
			GAME_STEP_MOVE_OR_PASS,
			GAME_STEP_MOVE,//移动阶段
			GAME_STEP_PASS,//回合结束阶段
			GAME_STEP_ABANDON,//投降阶段
			GAME_STEP_DRAW,//请和阶段
			GAME_STEP_END//结束
		};
		public GAME_STEP step;
		public int seat;
	}

	#endregion

}