/**************************************/
//FileName: ProtocolManager.cs
//Author: wtx
//Data: 21/03/2017
//Describe: network and protocol manager
/**************************************/

using UnityEngine;
using System;
using System.Collections;
using Pomelo.DotNetClient;
using SimpleJson;
using System.IO;
using ProtocolDefine;

public class ProtocolManager 
{
	private static ProtocolManager _instance = null;
	private static JsonObject ROUTE_MAP;
	private string _ip = "116.62.57.248";
	private int _port = 3014;

	private bool _isSelectServerMode = false;//  是否为选择服务器登陆模式
	private Action<bool,Message> _connectCallback = null;

	private Action<Message> _errorCallback = null;
	private Action<Message> _disconnectCallback = null;

	//是否连接成功标志
	private bool _connectSuccess = false;
	//是否可以登陆了，选择服务器模式，第一次连接不能登陆，需要用户选择服务器
	private bool _canLogin = false;
	private Message _serverList = null;

    private ProtocolManager()
	{
		TextAsset routeConfig = (TextAsset)Resources.Load("Config/RouteConfig");
		ROUTE_MAP = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(routeConfig.text);
	}

    public static ProtocolManager getInstance()
    {
        if(_instance == null)
        {
            _instance = new ProtocolManager();
        }
        return _instance;
    }


	//
	private static PomeloClient _connection = null;

	private PomeloClient createConnect(){
		PomeloClient connection = new PomeloClient();

		connection.on("onTick", msg => 
		{
			Debug.unityLogger.Log("onTick:" + msg.rawString);
		});
		
		//listen on network state changed event
		connection.on(PomeloClient.DisconnectEvent, msg =>
		{
			//网络错误，诸如已经掉线等等
			Debug.unityLogger.Log("Network error, reason: " + msg.jsonObj["reason"]);
			if(_disconnectCallback != null){
				_disconnectCallback(msg);
			}
		});

		connection.on (PomeloClient.ErrorEvent, msg => 
		{
			//没网
			Debug.unityLogger.Log ("Error, reason: " + msg.jsonObj ["reason"]);
			if(_errorCallback != null){
				_errorCallback(msg);
			}
		});

		return connection;
	}


	// Update is called once per frame
	public void Update () 
	{
		//dispatch msg
		if (_connection != null) {
			_connection.Update ();

			//处理网络连接成功
			if(_connectCallback != null && _connectSuccess){
				_connectCallback (_canLogin,_serverList);

				_serverList = null;
				_connectSuccess = false;
				_canLogin = false;
			}
		}
	}

	public void Disconnect()
	{
		_connection.Disconnect( PackageType.PKG_NONE);
		_isSelectServerMode = false;
		_connectCallback = null;
		_errorCallback = null;
		_disconnectCallback = null;
		_connectSuccess = false;
		_canLogin = false;
		_serverList = null;
	}
	private void Start()
	{
		_connection = createConnect();
		_connection.InitClient(_ip,_port , msgObj =>
			{
				_connection.connect(null, data =>
				{
					//process handshake call back data
					JsonObject msg = new JsonObject();
					sendMsg(LobbyProtocol.P_LOBBY_REQ_CONNECT, msg, onRespConnet);
				});
			});
	}
	/// <summary>
	/// 如果是选择服务器登陆方式需要调用这个
	/// </summary>
	/// <param name="selectServerModel">If set to <c>true</c> select server model.</param>
	public void Start(bool isSelectServerMode,Action<Message>errcb,Action<Message>discb,Action<bool,Message>connectcb)
	{
		_isSelectServerMode = isSelectServerMode;
		_connectCallback = connectcb;
		_errorCallback = errcb;
		_disconnectCallback = discb;

		Start();
	}
	public void Restart()
	{
		//
		//Start ();
		SelectServer(_ip,_port);
	}

	/// <summary>
	///  选择服务器登陆方式
	/// </summary>
	/// <param name="host">Host.</param>
	/// <param name="ip">Ip.</param>
	public void SelectServer(string host,int port)
	{
		_connection = createConnect ();
		_ip = host;
		_port = port;
		_connection.InitClient (_ip, _port, msgObj => {
			_connection.connect (null, (data) => {
				_connectSuccess = true;
				_canLogin = true;
			});
		});
	}
	private void onRespConnet(Message msg)
	{
		JsonObject result = msg.jsonObj;

		System.Object code = null;
		if (result.TryGetValue("code", out code))
		{
			if (Convert.ToInt32(code) == 500)
			{
				return;
			}
			else
			{
				_connection.Disconnect(PackageType.PKG_NONE);
				_connection = null;

				if (_isSelectServerMode) 
				{
					_serverList = msg;
					_connectSuccess = true;
					_canLogin = false;
				} 
				else 
				{
					System.Object host, port;
					if (result.TryGetValue ("host", out host) &&
						result.TryGetValue ("port", out port)) {
						SelectServer(host.ToString(),Convert.ToInt32 (port));
					}
				}
			}
		}
	}

	//需要手动调用添加服务器push给客户端的消息，不同的界面请添加不同的消息回调
	public void addPushMsgEventListener(LobbyProtocol pID,Action<Message>action)
	{
		System.Object route;

		if (ROUTE_MAP.TryGetValue ("P"+(int)pID, out route))
		{
			_connection.on (route.ToString (),action);
		}
	}
	//需要手动调用添加服务器push给客户端的消息，不同的界面请添加不同的消息回调
	public void addPushMsgEventListener(GameProtocol pID,Action<Message>action)
	{
		System.Object route;
		if (ROUTE_MAP.TryGetValue ("P"+(int)pID, out route))
		{
			_connection.on (route.ToString (),action);
		}
	}
	//需要手动调用添加服务器push给客户端的消息，不同的界面请添加不同的消息回调
	public void addPushMsgEventListener(GameLianQiProtocol pID,Action<Message>action)
	{
		System.Object route;

		if (ROUTE_MAP.TryGetValue ("P"+(int)pID, out route))
		{
			_connection.on (route.ToString (),action);
		}
	}

	public void removePushMsgEventListener(LobbyProtocol pID)
	{
		System.Object route;

		if (ROUTE_MAP.TryGetValue ("P"+(int)pID, out route))
		{
			_connection.removeEventListeners (route.ToString ());
		}
	}
	public void removePushMsgEventListener(GameProtocol pID)
	{
		System.Object route;
		if (ROUTE_MAP.TryGetValue ("P"+(int)pID, out route))
		{
			_connection.removeEventListeners (route.ToString ());
		}
	}
	public void removePushMsgEventListener(GameLianQiProtocol pID)
	{
		System.Object route;

		if (ROUTE_MAP.TryGetValue ("P"+(int)pID, out route))
		{
			_connection.removeEventListeners (route.ToString ());
		}
	}
	#region 为了防止漏填等字段等相关bug，这里不提供直接发送json-msg的的接口，请使用对应的消息接口进行发送

	/////////////////////////////////////LOBBY/////////////////////////////////////

	/// <summary>
	/// 请求登陆
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgLogin msg,Action<Message>action)
	{
		//填写相应字段
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("userID", msg.userID);
		jsonMsg.Add ("area", msg.area);
		jsonMsg.Add ("loginType",(int)msg.loginType);
		jsonMsg.Add ("openID", msg.openID);
		jsonMsg.Add ("password", msg.password);
		jsonMsg.Add ("deviceID",msg.deviceID);
		jsonMsg.Add ("nickName", msg.nickName);
		jsonMsg.Add ("osVersion",msg.osVersion);
		jsonMsg.Add ("ipAddr", msg.ipAddr);
		jsonMsg.Add ("channelID",msg.channelID);
		jsonMsg.Add ("appVersion", msg.appVersion);
		jsonMsg.Add ("netWorkType",msg.netWorkType);

		jsonMsg.Add ("token", msg.token);
		jsonMsg.Add ("head",msg.head);
		jsonMsg.Add ("sex", msg.sex);
		jsonMsg.Add ("expireTime",msg.expireTime);

		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求场配置列表
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqPlazaList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求道具配置列表
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqPropList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求背包信息
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqPackageList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求系统公告
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqSysMsgList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("channelId", (int)msg.channelId);
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求个人邮件信息
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqPrivateMsgList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("cnt",msg.cnt);
		jsonMsg.Add ("begin",msg.begin);
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求排行榜
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqRankList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game",(int) msg.game);
		jsonMsg.Add ("type",(int)msg.type);
		jsonMsg.Add ("scope",(int)msg.scope);
		jsonMsg.Add ("area",msg.areaID);
		jsonMsg.Add ("rankNum", msg.rankNum);
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求商品列表
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>

	public void sendMsg(LobbyProtocol pID, msgReqStoreList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("channelID",(int)msg.channelID );
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求好友列表
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqFriendList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		sendMsg(pID,jsonMsg,action);
	}

	/// <summary>
	/// 请求签到和抽奖数据
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqSignInLuckDrawData msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("areaID",msg.areaID );
		jsonMsg.Add ("deviceID",msg.deviceID );
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求签到
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqSignIn msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("areaID",msg.areaID );
		jsonMsg.Add ("deviceID",msg.deviceID );
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求抽奖
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqLuckDraw msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("areaID",msg.areaID );
		jsonMsg.Add ("deviceID",msg.deviceID );
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 请求更新邮件信息
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqUpdateEmail msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("type", (int)msg.type);
		jsonMsg.Add ("awardEmailId",msg.awardEmailId );
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// Sends the message.
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqFeedback msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("type", (int)msg.type);
		jsonMsg.Add ("content",msg.content );
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// Sends the message.
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(LobbyProtocol pID, msgReqOpenTalentslot msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("openBy",(int)msg.openBy );
		sendMsg(pID,jsonMsg,action);
	}
	/////////////////////////////LOBBY END////////////////////////////////////

	//////////////////////////////ROOM////////////////////////////////////////
	public void sendMsg(GameProtocol pID, msgReqJoinRoom msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("roomId",msg.roomId);
		jsonMsg.Add ("plazaID",msg.plazaID);
		jsonMsg.Add ("playerNum",msg.playerNum);
		jsonMsg.Add ("gridLevel",msg.gridLevel);
		jsonMsg.Add ("pwd",msg.pwd);
		sendMsg(pID,jsonMsg,action);
	}
	/// <summary>
	/// 创建房间
	/// </summary>
	/// <param name="pID">P I.</param>
	/// <param name="msg">Message.</param>
	/// <param name="action">Action.</param>
	public void sendMsg(GameProtocol pID, msgReqCreateRoom msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("roomType",(int)msg.roomType );
		jsonMsg.Add ("baseScore",msg.baseScore );
		jsonMsg.Add ("minScore",msg.minScore );
		jsonMsg.Add ("maxScore",msg.maxScore );
		jsonMsg.Add ("roomName",msg.roomName );
		jsonMsg.Add ("roomPassword",msg.roomPassword );
		jsonMsg.Add ("rule",msg.rule );
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgReqLeaveRoom msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgReqHeartbeat msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgReqTrust msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("seat",msg.seat );
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgPlayerAct msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("act", (int)msg.act);
		jsonMsg.Add ("seat",msg.seat);
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgPlayerTalkMsg msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("seat",msg.seat );
		jsonMsg.Add ("content",msg.content );
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgTalentList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("seat",msg.seat );
		jsonMsg.Add ("talentList",msg.talentList );
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgReqRoomList msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", (int)msg.game);
		jsonMsg.Add ("areaID",msg.areaID );
		jsonMsg.Add ("begin",msg.begin );
		jsonMsg.Add ("reqCnt",msg.reqCnt );
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameProtocol pID, msgNotifyEnterRoomFinish msg)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("isRelink", msg.isRelink);
		sendMsg(pID,jsonMsg,null);
	}
	//////////////////////////////ROOM END////////////////////////////////////
	//////////////////////////////GAME///////////////////////////////////////

	public void sendMsg(GameProtocol pID, msgNotifyStartGame msg)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("game", msg.game);
		jsonMsg.Add ("isEnterRoomFinsh",msg.isEnterRoomFinsh);
		sendMsg(pID,jsonMsg,null);
	}
	public void sendMsg(GameLianQiProtocol pID, msgLianQiReqPass msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("seat", msg.seat);
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameLianQiProtocol pID, msgLianQiReqPlay msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("seat", msg.seat);
		jsonMsg.Add ("x", msg.x);
		jsonMsg.Add ("y", msg.y);
		jsonMsg.Add ("direction", (int)msg.direction);
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameLianQiProtocol pID, msgLianQiReqMove msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("seat", msg.seat);
		jsonMsg.Add ("x", msg.x);
		jsonMsg.Add ("y", msg.y);
		jsonMsg.Add ("direction", (int)msg.direction);
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameLianQiProtocol pID, msgLianQiReqDraw msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("seat", msg.seat);
		sendMsg(pID,jsonMsg,action);
	}
	public void sendMsg(GameLianQiProtocol pID, msgLianQiReqAbandon msg,Action<Message>action)
	{
		JsonObject jsonMsg = new JsonObject ();
		jsonMsg.Add ("seat", msg.seat);
		sendMsg(pID,jsonMsg,action);
	}
	///////////////////////////////GAME END//////////////////////////////////

	#endregion

	#region 私有接口，发送消息，外部禁止访问
	private void sendMsg(LobbyProtocol pID, JsonObject msg,Action<Message>action)
	{
		sendMsg((uint)pID, msg,action);
	}
	private void sendMsg(GameProtocol pID, JsonObject msg,Action<Message>action)
	{
		sendMsg((uint)pID, msg,action);
	}
	private void sendMsg(GameLianQiProtocol pID, JsonObject msg,Action<Message>action)
	{
		sendMsg((uint)pID, msg,action);
	}

	private void sendMsg(uint pID, JsonObject msg,Action<Message>action)
	{
		if (_connection != null) 
		{
			System.Object route;

			if (ROUTE_MAP.TryGetValue ("P"+(int)pID, out route))
			{
				if (action == null) {
					_connection.notify(route.ToString(),msg);
				} else {
					_connection.request (pID, route.ToString (), msg, action);
				}
			}
		}
	}
	#endregion
}
