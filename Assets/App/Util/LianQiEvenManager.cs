/**************************************/
//FileName: LianQiEventManager.cs
//Author: wtx
//Data: 13/04/2017
//Describe: 所有事件定义以及处理封装，请使用此来完成ui和c之间的交互，禁ui直接访问controller
/**************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class LianQiEventManager
{
	private Dictionary<int, Action<object>> _eventMap;

	public LianQiEventManager()
	{
		_eventMap = new Dictionary<int, Action<object>>();
	}

	public void ClearEvent()
	{
		_eventMap.Clear();
	}
	public void AddEvent(int eventID, Action<object> callback)
	{
		if (_eventMap.ContainsKey(eventID)) return;
		_eventMap.Add(eventID, callback);
	}
	public void RemoveEvent(int eventID)
	{
		if (!_eventMap.ContainsKey(eventID)) return;
		_eventMap.Remove(eventID);
	}
	public void InvokeEvent(int eventID, object data)
	{
		if (!_eventMap.ContainsKey(eventID)) return;
		_eventMap[eventID].Invoke(data);
	}
}
