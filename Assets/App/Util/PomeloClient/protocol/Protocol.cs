using System;
using SimpleJson;
using System.Text;

namespace Pomelo.DotNetClient
{
    public class Protocol
    {
        private MessageProtocol _messageProtocol;
        private ProtocolState _state;
        private Transporter _transporter;
        private HandShakeService _handshake;
        private HeartBeatService _heartBeatService = null;
		private PomeloClient _pomeloClient;

		public PomeloClient getPomeloClient()
        {
            return this._pomeloClient;
        }

		public Protocol(PomeloClient pc, System.Net.Sockets.Socket socket)
        {
            this._pomeloClient = pc;
            this._transporter = new Transporter(socket, this.processMessage);
            this._transporter.onDisconnect = onDisconnect;

            this._handshake = new HandShakeService(this);
            this._state = ProtocolState.start;
        }

        internal void start(JsonObject user, Action<JsonObject> callback)
        {
            this._transporter.start();
            this._handshake.request(user, callback);

            this._state = ProtocolState.handshaking;
        }

        //Send notify, do not need id
        internal void send(string route, JsonObject msg)
        {
            send(route, 0, msg);
        }

        //Send request, user request id 
        internal void send(string route, uint id, JsonObject msg)
        {
            if (this._state != ProtocolState.working) return;

            byte[] body = _messageProtocol.encode(route, id, msg);

            send(PackageType.PKG_DATA, body);
        }

        internal void send(PackageType type)
        {
            if (this._state == ProtocolState.closed) return;
            _transporter.send(PackageProtocol.encode(type));
        }

        //Send system message, these message do not use messageProtocol
        internal void send(PackageType type, JsonObject msg)
        {
            //This method only used to send system package
            if (type == PackageType.PKG_DATA) return;

            byte[] body = Encoding.UTF8.GetBytes(msg.ToString());

            send(type, body);
        }

        //Send message use the transporter
        internal void send(PackageType type, byte[] body)
        {
            if (this._state == ProtocolState.closed) return;

            byte[] pkg = PackageProtocol.encode(type, body);

            _transporter.send(pkg);
        }

        //Invoke by Transporter, process the message
        internal void processMessage(byte[] bytes)
        {
            Package pkg = PackageProtocol.decode(bytes);

            //Ignore all the message except handshading at handshake stage
            if (pkg.type == PackageType.PKG_HANDSHAKE && this._state == ProtocolState.handshaking)
            {

                //Ignore all the message except handshading
                JsonObject data = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(Encoding.UTF8.GetString(pkg.body));

                processHandshakeData(data);

                this._state = ProtocolState.working;

            }
            else if (pkg.type == PackageType.PKG_HEARTBEAT && this._state == ProtocolState.working)
            {
                this._heartBeatService.resetTimeout();
            }
            else if (pkg.type == PackageType.PKG_DATA && this._state == ProtocolState.working)
            {
                this._heartBeatService.resetTimeout();
                _pomeloClient.processMessage(_messageProtocol.decode(pkg.body));
            }
            else if (pkg.type == PackageType.PKG_KICK)
            {
				this.getPomeloClient().Disconnect(PackageType.PKG_KICK);
                this.close();
            }
        }

        private void processHandshakeData(JsonObject msg)
        {
            //Handshake error
            if (!msg.ContainsKey("code") || !msg.ContainsKey("sys") || Convert.ToInt32(msg["code"]) != 200)
            {
                throw new Exception("Handshake error! Please check your handshake config.");
            }

            //Set compress data
            JsonObject sys = (JsonObject)msg["sys"];

            JsonObject dict = new JsonObject();
            if (sys.ContainsKey("dict")) dict = (JsonObject)sys["dict"];

            JsonObject protos = new JsonObject();
            JsonObject serverProtos = new JsonObject();
            JsonObject clientProtos = new JsonObject();

            if (sys.ContainsKey("protos"))
            {
                protos = (JsonObject)sys["protos"];
                serverProtos = (JsonObject)protos["server"];
                clientProtos = (JsonObject)protos["client"];
            }

            _messageProtocol = new MessageProtocol(dict, serverProtos, clientProtos);

            //Init heartbeat service
            int interval = 0;
            if (sys.ContainsKey("heartbeat")) interval = Convert.ToInt32(sys["heartbeat"]);
            _heartBeatService = new HeartBeatService(interval, this);

            if (interval > 0)
            {
                _heartBeatService.start();
            }

            //send ack and change protocol state
            _handshake.ack();
            this._state = ProtocolState.working;

            //Invoke handshake callback
            JsonObject user = new JsonObject();
            if (msg.ContainsKey("user")) user = (JsonObject)msg["user"];
            _handshake.invokeCallback(user);
        }

        //The socket disconnect
        private void onDisconnect(string reason)
        {
            this._pomeloClient._onDisconnect(reason);
        }

        internal void close()
        {
            _transporter.close();

            if (_heartBeatService != null) _heartBeatService.stop();

            this._state = ProtocolState.closed;
        }
    }
}