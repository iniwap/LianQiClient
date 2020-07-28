using SimpleJson;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Pomelo.DotNetClient
{
    /// <summary>
    /// network state enum
    /// </summary>
    public enum NetWorkState
    {
        [Description("Connecting server")]
        CONNECTING,

        [Description("Server connected")]
        CONNECTED,

        [Description("Disconnected with server")]
        DISCONNECTED,
    }

    public class PomeloClient
    {
        /// 网络断开事件
        static public string DisconnectEvent = "Disconnect";
        /// 其它错误/异常。例如在DISCONNECTED下调用request，则会抛出该事件。注意：这些错误都不是网络错误
        /// 网络错误会通过DisconnectEvent抛出
        static public string ErrorEvent = "Error";

        static protected uint SYS_MSG_CONNECTED = 1;

        private Queue<Message> __receiveMsgQueue;

        // Current network state
        public NetWorkState netWorkState { get; protected set; }   

        private EventManager _eventManager;
        private Socket _socket;
        private Protocol _protocol;

        public PomeloClient()
        {
            netWorkState = NetWorkState.DISCONNECTED;
            _eventManager = new EventManager();

            __receiveMsgQueue = new Queue<Message>();
        }

        public UnityEngine.WaitUntil InitClient(string host, int port)
        {
            bool bDone = false;
            Action<Message> callback = ret => { bDone = true; };
            InitClient(host, port, callback);

            return new UnityEngine.WaitUntil(() => bDone);
        }

        /// 初始化连接
        public void InitClient(string host, int port, Action<Message> callback)
        {
            _assert(netWorkState == NetWorkState.DISCONNECTED);

            UnityEngine.Debug.Log("Connect to " + host + " with port " + port);

            netWorkState = NetWorkState.CONNECTING;

            IPAddress ipAddress = null;

            try
            {
                if (!IPAddress.TryParse(host, out ipAddress))
                {
                    IPAddress[] addresses = Dns.GetHostEntry(host).AddressList;
                    foreach (var item in addresses)
                    {
                        if (item.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _onDisconnect(e.Message);
                return;
            }

            if (ipAddress == null) throw new Exception("Cannot parse host : " + host);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ie = new IPEndPoint(ipAddress, port);

            _eventManager.RemoveCallback(SYS_MSG_CONNECTED);
			if (_eventManager.AddCallback (SYS_MSG_CONNECTED, callback)) 
			{
				_socket.BeginConnect (ie, _onConnectCallback, _socket);
			}
        }

        public void connect()
        {
            connect(null, null);
        }

        public void connect(JsonObject user)
        {
            connect(user, null);
        }

        public void connect(Action<JsonObject> handshakeCallback)
        {
            connect(null, handshakeCallback);
        }

        public bool connect(JsonObject user, Action<JsonObject> handshakeCallback)
        {
            try
            {
                _protocol.start(user, handshakeCallback);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public void request(uint pid,string route, JsonObject msg, Action<Message> action)
        {
            if(netWorkState != NetWorkState.CONNECTED)
            {
                _onError("Network is down, cannot send request now!");
                return;
            }

            UnityEngine.Debug.Log(">>> Send: " + route + " data: " + msg.ToString());
			if (action == null) 
			{
				notify (route,msg);
			}
			else
			{
				if (_eventManager.AddCallback (pid, action)) 
				{
					_protocol.send (route, pid, msg);
				}
			}
        }

        public void notify(string route, JsonObject msg)
        {
            if (netWorkState != NetWorkState.CONNECTED)
            {
                _onError("Network is down, cannot send request now!");
                return;
            }

			UnityEngine.Debug.Log(">>> Send: " + route + " data: " + msg.ToString());


            _protocol.send(route, msg);
        }

        public void on(string eventName, Action<Message> action)
        {
            _eventManager.AddOnEvent(eventName, action);
        }

        public void removeEventListeners(string eventName)
        {
            _eventManager.RemoveOnEvent(eventName);
        }

        public void removeAllEventListeners()
        {
            _eventManager.ClearEventMap();
        }

        internal void processMessage(Message msg)
        {
            __receiveMsgQueue.Enqueue(msg);
        }

        public void Update()
        {
            while(__receiveMsgQueue.Count != 0)
            {
                var msg = __receiveMsgQueue.Dequeue();

                if (msg.type == MessageType.MSG_RESPONSE)
                {
					UnityEngine.Debug.Log("<<< Receive: " + msg.route + " data: " + msg.jsonObj.ToString());

                    UnityEngine.Debug.Assert(_eventManager._GetCallbackCount() != 0);

                    _eventManager.InvokeCallBack(msg.id, msg);
                    _eventManager.RemoveCallback(msg.id);
                }
                else if (msg.type == MessageType.MSG_PUSH)
                {
					UnityEngine.Debug.Log("<<< Receive event: " + msg.route + " data: " + msg.jsonObj.ToString());
                    _eventManager.InvokeOnEvent(msg.route, msg);
                }
                else if(msg.type == MessageType.MSG_SYS)
                {
                    if (msg.id != 0)
                    {
                        _eventManager.InvokeCallBack(msg.id, msg);
                        _eventManager.RemoveCallback(msg.id);
                    }
                    else
                    {
                        _eventManager.InvokeOnEvent(msg.route, msg);
                    }
                }
            }
        }
    
		public void Disconnect(PackageType type)
        {
			if (netWorkState == NetWorkState.DISCONNECTED)
				return;

			/// Force update to make sure all received messages been dispatched.
			Update ();

			// free managed resources
			if (_protocol != null)
				_protocol.close ();

			try {
				_socket.Shutdown (SocketShutdown.Both);
				_socket.Close ();
				_socket = null;
			} catch (Exception) {
				//todo : 有待确定这里是否会出现异常，这里是参考之前官方github上pull request。emptyMsg
			}

			netWorkState = NetWorkState.DISCONNECTED;

			_eventManager.ClearCallBackMap ();
			_eventManager.ClearCallBackMap ();

			//网络关闭会触发这个断开
			if (PackageType.PKG_NONE != type) {
				if (PackageType.PKG_KICK == type) {
					_onError ("kick by server");
				}else if(PackageType.PKG_HEARTBEAT == type){
					_onError ("network is down");
				}
			}
        }

        protected void _assert(bool bOperation, string msg = "")
        {
            if (!bOperation)
            {
                throw new Exception(msg);
            }
        }

        internal void _onError(string reason)
        {
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("reason", reason);
            Message msg = new Message(MessageType.MSG_SYS, ErrorEvent, jsonObj);
            __receiveMsgQueue.Enqueue(msg);
        }

        internal void _onDisconnect(string reason)
        {
            netWorkState = NetWorkState.DISCONNECTED;

            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("reason", reason);
            Message msg = new Message(MessageType.MSG_SYS, DisconnectEvent, jsonObj);
            __receiveMsgQueue.Enqueue(msg);

            _socket.Close();
            _socket = null;
        }

        protected void _onConnectCallback(IAsyncResult result)
        {
            try
            {
                netWorkState = NetWorkState.CONNECTED;

                _socket.EndConnect(result);
                _protocol = new Protocol(this, _socket);

                Message msg = new Message(MessageType.MSG_SYS, SYS_MSG_CONNECTED);
                __receiveMsgQueue.Enqueue(msg);
            }
            catch (SocketException e)
            {
                _onDisconnect(e.Message);
            }
        }
    }
}