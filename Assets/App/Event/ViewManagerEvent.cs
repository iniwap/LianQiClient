/**************************************/
//FileName: ViewManagerEvent.cs
//Author: wtx
//Data: 13/04/2017
//Describe: 界面管理事件定义
/**************************************/
using System;
using System.Collections;
using System.Collections.Generic;

public class ViewManagerEvent : LianQiEventManager{

	public enum SCENE_TYPE{
		MACHINE_SCENE,
		APP_SCENE,
	}

	public enum VIEW_TYPE{
		NONE_VIEW,
		START_VIEW,
		ACCOUNT_VIEW,
		LOBBY_VIEW,
		GAME_VIEW,
	}
	public enum EVENT{
		SHOW_VIEW,
		SHOW_DIALOG,
		SHOW_TIP,
		SHOW_LOADING_ANI,//加载小动画
		SHOW_SCENE,//切换场景
	};

	private static ViewManagerEvent _instance = null;

	public struct sShowView{
		public VIEW_TYPE fromView;
		public VIEW_TYPE toView;
	};
	public struct sShowScene{
		public SCENE_TYPE fromScene;
		public SCENE_TYPE toScene;
	};

	//原则上可以提取到父类里去
	//显示弹窗
	public struct s_ShowDialog{
		public CommonDefine.eDialogEventType type;
		public string tip;
		public bool hasOk;
		public bool hasCancel;
		public bool hasClose;
		public Action<CommonDefine.eDialogBtnType,CommonDefine.eDialogEventType> callBack;
	};

	public static ViewManagerEvent EM()
	{
		if(_instance == null)
		{
			_instance = new ViewManagerEvent();
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
