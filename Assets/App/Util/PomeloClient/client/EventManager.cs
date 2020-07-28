using System;
using System.Collections.Generic;
using System.Text;
using SimpleJson;

namespace Pomelo.DotNetClient
{
    public class EventManager : IDisposable
    {
        protected Dictionary<uint, Action<Message>> _callBackMap;
        protected Dictionary<string, List<Action<Message>>> _eventMap;

        public EventManager()
        {
            _callBackMap = new Dictionary<uint, Action<Message>>();
            _eventMap = new Dictionary<string, List<Action<Message>>>();
        }

        public void ClearCallBackMap()
        {
            _callBackMap.Clear();
        }

        public void ClearEventMap()
        {
            _eventMap.Clear();
        }

        public int _GetCallbackCount()
        {
            return _callBackMap.Count;
        }

        //Adds callback to callBackMap by id.
		public bool AddCallback(uint id, Action<Message> callback)
        {
            UnityEngine.Debug.Assert(callback != null);
            UnityEngine.Debug.Assert(id > 0);

			//已经存在
			if (_callBackMap.ContainsKey(id)) return false;

            _callBackMap.Add(id, callback);
			return true;
        }

        public void RemoveCallback(uint id)
        {
			if (!_callBackMap.ContainsKey(id)) return;
            _callBackMap.Remove(id);
        }

        /// <summary>
        /// Invoke the callback when the server return messge.
        /// </summary>
        /// <param name='pomeloMessage'>
        /// Pomelo message.
        /// </param>
        public void InvokeCallBack(uint id, Message data)
        {
            if (!_callBackMap.ContainsKey(id)) return;
            _callBackMap[id].Invoke(data);
        }

        public void RemoveOnEvent(string eventName)
        {
			if (!_eventMap.ContainsKey(eventName)) return;
            _eventMap.Remove(eventName);
        }

        //Adds the event to eventMap by name.
        public void AddOnEvent(string eventName, Action<Message> callback)
        {
            List<Action<Message>> list = null;
            if (_eventMap.TryGetValue(eventName, out list))
            {
                list.Add(callback);
            }
            else
            {
                list = new List<Action<Message>>();
                list.Add(callback);
                _eventMap.Add(eventName, list);
            }
        }

        /// <summary>
        /// If the event exists,invoke the event when server return messge.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ///
        public void InvokeOnEvent(string route, Message msg)
        {
            if (!_eventMap.ContainsKey(route)) return;

            List<Action<Message>> list = _eventMap[route];
            foreach (Action<Message> action in list) action.Invoke(msg);
        }

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected void Dispose(bool disposing)
        {
            _callBackMap.Clear();
            _eventMap.Clear();
        }
    }
}