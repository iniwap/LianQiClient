/**************************************/
//FileName: Lobby.cs
//Author: wtx
//Data: 04/13/2017
//Describe:  大厅数据
/**************************************/
using System;
using System.Collections.Generic;
using System.IO;

// 重新定义是为了更好的与界面交互，不希望直接用网络消息定义来操作
namespace Lobby{

	public static class Lobby{
		//静态配置数据
		public static List<LobbyEvent.Plaza> plazaList;
		public static List<LobbyEvent.Store> storeList;
		public static List<LobbyEvent.Prop>  propList;
		public static List<LobbyEvent.Package> packageList;
		public static List<LobbyEvent.PrivateMsg> privateMsgList;
		public static List<LobbyEvent.Friend> friendList;
		public static List<LobbyEvent.Rank> rankList;

		public static LobbyEvent.SignInData signInData;
		public static LobbyEvent.LuckDrawData luckDrawData;

		//动态数据
		public static void initData(){
			plazaList = new List<LobbyEvent.Plaza>();
			storeList= new List<LobbyEvent.Store>();
			propList = new List<LobbyEvent.Prop>();
			packageList = new List<LobbyEvent.Package>();
			privateMsgList = new List<LobbyEvent.PrivateMsg>();
			friendList = new List<LobbyEvent.Friend>();
			rankList = new List<LobbyEvent.Rank>();
			signInData.signInList = new List<LobbyEvent.SignIn> ();
			luckDrawData.luckDrawList = new List<LobbyEvent.LuckDraw> ();
		}
	}
}