/**************************************/
//FileName: ProtocolDefine.cs
//Author: wtx
//Data: 20/03/2017
//Describe: common define
/**************************************/

namespace CommonDefine
{	

	//-------------------协议和界面通用枚举定义--------------------
	public enum eRespResultType{
		SUCCESS,
		FAIL,
	};

	public enum eUpdateEmailType{
		READ,
		DEL,
		GET_AWARD,
	};
	public enum eSysBroadcastType{
		SYS,
		INFO,
	};
	public enum eFeedbackType{
		BUG,
		ACCOUNT,
		AWARD,
		RECHARGE,
		SUGGEST,
		DANMU,
	};

	public enum ePopupType{
		SETTING,
		EMAIL,
		RANK,
		FEEDBACK,
		TALENT,
	};

	//--------------------------------------------------
	public enum eLoginBtnType{
		BTN_YK,
		BTN_WX,
		BTN_QQ,
	};

	public enum  SEAT{
		SELF = 0,
		RIGHT = 1,
		TOP = 2,
		LEFT = 3,
		INVILID = 255,
	};

	public enum PLAYER_STATE
	{
		STATE_TYPE_NONE,//无效状态
		STATE_TYPE_STANDUP,//站起
		STATE_TYPE_SITDOWN,//坐下
		STATE_TYPE_ROOMREADY,//准备中
		STATE_TYPE_PLAYING, //游戏中
		STATE_TYPE_OFFLINE,//离线
		STATE_TYPE_SEEING,//观战
	};

	public enum eDialogBtnType{
		DIALOG_BTN_OK,
		DIALOG_BTN_CANCEL,
		DIALOG_BTN_CLOSE,
	};

	public enum ePlazaLevelType{
		PLAZA_LEVEL_LOW = 1,
		PLAZA_LEVEL_MIDDLE = 2,
		PLAZA_LEVEL_HIGH = 3,
	};
	public enum eRoomActionType{
		CAN_NONE,
		CAN_CREATE_ROOM,
		CAN_LEAVE_ROOM,
		CAN_DISSOLVE,
	};

	public enum eCreateRoomType{
		ROOM_CLASSIC_PLAZA = 0,
		ROOM_PLAZA = 1,
		ROOM_ROOM = 2,
		ROOM_TEAM = 3,
	};

	public enum eDialogEventType{
		SIMPLE,
		LOBBY_EMAIL_GET_AWARD_RESULT,
		GAME_ABANDON,
		NETWORK_ERROR,
		NETWORK_DISCONNECT,
	};

	public enum eRoomClassicType{
		MODE_2_4,//
		MODE_2_6,
		MODE_4_6,
	}
	public enum eOpenByType
	{
		OPEN_BY_GOLD,
		OPEN_BY_DIAMOND,
	};

	public enum eTalentType{
		TALENT_NONE,

		//base
		TALENT_A1,//资源占据：从自己所在格子获得1生命点
		TALENT_A2,//a2资源吸收：从前后未被敌人占据的格子各获得1生命点
		TALENT_A3,//a3基本攻击：面向敌人，以1点攻击力攻击前方敌人
		TALENT_B1,// b1加命：面对友军，为友军增加1生命点；
		TALENT_B2,//b2助攻：面对友军，且友军处于攻击状态，则自己的攻击力加到友军身上；备注：妨碍敌方的a2天赋可以减少敌方生命总量；背对背是天赋b5处于攻击状态= 面对敌人或面对空格

		//talent
		TALENT_B3,//b3环：己方n个棋子形成环，每个棋子增加n生命点
		TALENT_B4,// b4吸收：两个棋子相对，各具备3吸收力，能够对背后的敌人吸收最多3的攻击力；备注：经最近测试，改为3吸收力可以让吸收阵更加实用，而且也不会过于强大，不过可能导致以前设计的一些残局解法有变，一个有效的解决办法就是给以往的残局再设定天赋条件，比如每个残局都设定好了允许使用的天赋
		TALENT_B5,//b5双尖：两颗棋子背对背，各增加2攻击力；
		TALENT_C1,// c1吃子移动：己方活动时，若通过攻击干掉对方棋子，则获得1点移动能力，允许在本回合内向前移1格；
		//etc..
	}
	public enum TalentSlotState{
		TALENT_LOCK,
		TALENT_CAN_INSTALL,
		TALENT_INSTALLED,
	};
	public static class ResPath{
		//common
		public static readonly string LEVEL_ICON = "Lobby/Common/LevelIcon";
		public static readonly string HEAD = "Lobby/Common/Head/";

		//大厅
		public static readonly string RANK_NUM_ICON = "Lobby/Lobby/06_start_popup_ranking/ranking_icon_";
		public static readonly string EMAIL_NOT_GET_AWARD_BTN = "Lobby/Email/GetRwdBtn";
		public static readonly string EMAIL_HAS_GET_AWARD_BTN = "Lobby/Email/HasGotRwdBtn";
		public static readonly string CANT_CHANGE_MODE_BTN = "Lobby/UpBtn1";
		public static readonly string CAN_CHANGE_MODE_BTN = "Lobby/UpBtn2";

		//天赋配置
		public static readonly string TALENT_ICON_BTN = "Lobby/Lobby/talent/icon_talent_";
		public static readonly string TALENT_LOCK_BTN = "Lobby/Lobby/talent/btn_talent_lock";
		public static readonly string TALENT_CANINSTALL_BTN = "Lobby/Lobby/talent/btn_open_talent";

		public static readonly string TALENT_HIGHTLIGHT = "Lobby/Lobby/talent/point_hightlight";
		public static readonly string TALENT_DE_HIGHTLIGHT = "Lobby/Lobby/talent/point_normal";

		//房间和场
		public static readonly string ROOM_TOPBAR_BG = "Lobby/Plaza/plaza03_room/plaza03_room";//队伍面板背景图

		public static readonly string PLAZA_TAG = "Lobby/Plaza/01/plaza_icon_special_";//场左上角角标图
		public static readonly string PLAZA_ICON = "Lobby/PlazaRoom/PlazaModelIcon";//场的标示图
		public static readonly string PLAZA_HALF_STAR = "Lobby/Plaza/plaza00/plaza_icon_halfstar";
		public static readonly string PLAZA_STAR = "Lobby/Plaza/plaza00/plaza_icon_star";
		public static readonly string PLAZA_MODEL_N_BTN = "Lobby/Plaza/plaza00/plaza_radio_bg";
		public static readonly string PLAZA_MODEL_S_BTN = "Lobby/Plaza/plaza00/plaza_panel_radio_selected";
		public static readonly string PLAZA_ROOM_LEVEL = "Lobby/Plaza/plaza01_roomsetting/plaza_icon_integral_";
		public static readonly string VIP_ICON = "Lobby/Plaza/03/icon_vip_";
		public static readonly string ROOM_HEAD_BG = "Lobby/Plaza/plaza03_room/room_head_defult_0";
		public static readonly string ROOM_READY_HEAD_BG = "Lobby/Plaza/plaza03_room/room_head_preparing_0";
		public static readonly string ROOM_HEAD = "Lobby/Plaza/plaza03_room/img_defulthead";

		public static readonly string LQ_DIRECTION_INFOBG = "Game/GameLianQi/DirInfoBg";
		public static readonly string LQ_DIRECTION_DIRECT = "Game/GameLianQi/Dir";
		public static readonly string LQ_DIRECTION_DIRECT_WITHE = "Game/GameLianQi/NotBanDir";
		public static readonly string LQ_DIRECTION_DIR_BG_1 = "Game/GameLianQi/DirBanBg1";
		public static readonly string LQ_DIRECTION_DIR_BG_2 = "Game/GameLianQi/DirBanBg2";// 最深
		public static readonly string LQ_PASS_BTN = "Game/GameLianQi/Pass";

		public static readonly string PLAYER_HEAD = "Game/GameLianQi/PlayerInfoHead";
		public static readonly string PLAYER_BG = "Game/GameLianQi/PlayerInfoBg";

		public static readonly string CHESS = "Game/GameLianQi/QiZi";
		public static readonly string CHESS_BG = "Game/GameLianQi/ChessBg";

		//结算
		public static readonly string GAME_RESULT_TITLE = "Game/GameResult/title";
		public static readonly string GAME_RESULT_TITLE_WIN = "Game/GameResult/titlewin";
		public static readonly string GAME_RESULT_TITLE_LOSE = "Game/GameResult/titlelose";
		public static readonly string GAME_RESULT_TITLE_DRAW = "Game/GameResult/titledraw";
	};

	public static class CONST {
		public static readonly int MAX_LMT_ROUND = 500;
		public static readonly int MIN_LMT_ROUND = 10;
		public static readonly int MAX_LMT_TURNTIME = 60;
		public static readonly int MIN_LMT_TURNTIME = 5;

		public static readonly int MAX_TALENT_CFG_NUM = 4;
		public static readonly string TALENT_SLOT_STATE = "TALENT_SLOT_STATE";
	};
}