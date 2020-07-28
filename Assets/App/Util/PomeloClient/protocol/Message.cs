using System;
using SimpleJson;

namespace Pomelo.DotNetClient
{
    public class Message
    {
        public MessageType type;
        public string route;
        public uint id;
        public JsonObject jsonObj;
        public string rawString;

        public Message(MessageType type, string route, JsonObject data)
        {
            this.type = type;
            this.route = route;
            this.jsonObj = data;
        }

        public Message(MessageType type, uint id)
        {
            this.type = type;
            this.id = id;
            this.route = "";
            this.jsonObj = null;
        }

        public Message(MessageType type, uint id, string route, JsonObject data, string rawString)
        {
            this.type = type;
            this.id = id;
            this.route = route;
            this.jsonObj = data;
            this.rawString = rawString;
        }
    }
}