/**************************************/
//FileName: Room.cs
//Author: wtx
//Data: 04/13/2017
//Describe:  房间数据，保存房间相关的信息
/**************************************/
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJson;

namespace CommonUtil{
	public static class Util{
	 
		private static JsonObject APP_CONFIG = null;
		private static JsonObject TALENT_CONFIG = null;

		public static JsonObject getAppConfig(){
			if (APP_CONFIG == null) {
				TextAsset appConfig = (TextAsset)Resources.Load ("Config/AppConfig");

				APP_CONFIG = (JsonObject)SimpleJson.SimpleJson.DeserializeObject (appConfig.text);
			}

			return APP_CONFIG;
		}


		public static JsonObject getTalentConfig(){
			if (TALENT_CONFIG == null) {
				TextAsset cfg = (TextAsset)Resources.Load ("Config/TalentConfig");

				TALENT_CONFIG = (JsonObject)SimpleJson.SimpleJson.DeserializeObject (cfg.text);
			}

			return TALENT_CONFIG;
		}


		public static int getAppVer(){
			System.Object v;
			if (getAppConfig ().TryGetValue ("AppVersion", out v)) {
				return (int)(long)v;
			} else {
				return 0;
			}
		}
		public static int getChanelId(){
			System.Object v;
			if (getAppConfig ().TryGetValue ("ChannelId", out v)) {
				return (int)(long)v;
			} else {
				return 0;
			}
		}

		///////////获取网络头像 ////
		public static string IMAGE_CACHE_PATH  
		{  
			get  
			{  
				//pc,ios //android :jar:file//  
				return Application.persistentDataPath + "/ImageCache/";  

			}  
		} 
		public static Sprite getSpriteByLocal(int local,string path){
			return Resources.Load(path + Room.Room.getSeatByLocal(local),typeof(Sprite)) as Sprite;
		}

		public static Sprite getSprite(string path){
			return Resources.Load(path,typeof(Sprite)) as Sprite;
		}
		public static int getSeatByLocal(int local){
			return Room.Room.getSeatByLocal (local);
		}
		public static int getLocalBySeat(int seat){
			return Room.Room.getLocalBySeat (seat);
		}

		public static string [] split(string str,char separator){
			return str.Split (separator);
		}
		public static int getVipLevel(int vip){
			if (vip == 0) {
				return 0;
			}
			if (vip > 0 && vip <= 10) {
				return 1;
			}
			if (vip > 10 && vip <= 99) {
				return 2;
			}
			if (vip > 99) {
				return 3;
			}

			return 0;
		}
		public static bool getIsSelf(int userID){
			return Account.getSelfData().userID == userID;
		}
		public static void playTalkSnd(int index,AudioSource audio){

			if (!getSetting("EffectSnd"))
				return;

			//AudioSource audio = GetComponent<AudioSource>();
			string[] talk = {
				"先下手",
				"承让承让",
				"大哥手下留情啊",
				"乌龟睡咪咪",
				"wahahahaha",
				"你苏定了",
				"不要走",
			};

			AudioClip ac = Resources.Load("Sound/Talk/"+talk[index]) as AudioClip;
			audio.clip = ac;
			audio.Play();
		}

		public static Single getScreenScale(){
			return Screen.width/1920.0f;
		}
		public static string getModeStr(CommonDefine.eCreateRoomType type,int playerNum,int gridLevel){
			string mode = "";
			if (type != CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA) {
				if (playerNum == 2) {
					mode = mode + "双人";
				} else if (playerNum == 3) {
					mode = mode + "三人";
				} else if (playerNum == 4) {
					mode = mode + "四人";
				}
				if (gridLevel == 4) {
					mode = mode + "四阶";
				} else if (gridLevel == 6) {
					mode = mode + "六阶";
				} else if (gridLevel == 8) {
					mode = mode + "八阶";
				}
			} else {

				if (playerNum == 2 && gridLevel == 4) {
					mode = "楚汉逐鹿";
				} else if (playerNum == 2 && gridLevel == 6) {
					mode = "一决雌雄";
				} else if (playerNum == 4 && gridLevel == 6) {
					mode = "四国混战";
				} else if (playerNum == 3 && gridLevel == 6) {
					mode = "三足鼎立";
				}
			}

			return mode;
		}
		public static void showDialog(string title,string content){
			ViewManagerEvent.s_ShowDialog d;
			d.callBack = null;
			d.hasCancel = false;
			d.hasClose = true;
			d.hasOk = false;
			d.tip = title;
			d.tip = content;	
			d.type = CommonDefine.eDialogEventType.SIMPLE;

			ViewManagerEvent.EM ().InvokeEvent (ViewManagerEvent.EVENT.SHOW_DIALOG,(object)d);
		
		}

		//获取本地保存的数据
		public static void setSetting(string key,int value){
			PlayerPrefs.SetInt (key,value);
		}
		public static bool getSetting(string key){
			return PlayerPrefs.GetInt(key,1) == 0?false:true;
		}

		public static void setPlayerPrefs(string key,string value){
			PlayerPrefs.SetString (key,value);
		}

		public static string getPlayerPrefs(string key,string dft){
			return PlayerPrefs.GetString (key, dft);
		}

		public static void setPlayerPrefs(string key,int value){
			PlayerPrefs.SetInt (key,value);
		}

		public static int getPlayerPrefs(string key,int dft){
			return PlayerPrefs.GetInt(key, dft);
		}

		public static void setPlayerPrefs(string key,float value){
			PlayerPrefs.SetFloat (key,value);
		}

		public static float getPlayerPrefs(string key,float dft){
			return PlayerPrefs.GetFloat (key, dft);
		}

		public static void delPlayerPrefs(string key){
			PlayerPrefs.DeleteKey(key);
		}

		public static void delAllPlayerPrefs(){
			PlayerPrefs.DeleteAll();
		}

		//q,r,s ->y,z,x->
		public static Vector3i Hex2Point(Hex hex){
			return new Vector3i (-hex.s,-hex.r,0);
		}
		public static Hex Point2Hex(Vector3i point){
			return new Hex (point.x + point.y,-point.y,-point.x);
		}

	}

	[Serializable]
	public class RoomRule
	{
		//to do
		public int playerNum;
		public int gridLevel;
		public string rule;
		public int gameTime;
		public int lmtRound;
		public int lmtTurnTime;
		//"{\"playerNum\":2,\"gridLevel\":4,\"rule\":\"default\",\"gameTime\":\"360\"}";

		public static RoomRule deserialize(string ruleStr)
		{
			return JsonUtility.FromJson<RoomRule>(ruleStr);
		}
		//ect ...
	}
	[Serializable]
	public class EmailContent
	{
		public enum AWARD_TYPE
		{
			NONE,
			GOLD,
			PROP,
			//etc...
		};
		public string content;

		public bool hasAward;
		public AWARD_TYPE type;
		public int propId;//如果有
		public int awardCnt;
		public string awardIcon;//可以从道具列表中查到相关图标

		public bool hasGottenAward;

		public static EmailContent deserialize(string ct)
		{
			return JsonUtility.FromJson<EmailContent>(ct);
		}
		public static string serialize(EmailContent ec){
			return JsonUtility.ToJson(ec);
		}
	}

	[Serializable]
	public class SysBroadCast
	{
		public CommonDefine.eSysBroadcastType type;
		public string content;
		public int playCnt;

		public static SysBroadCast deserialize(string ct)
		{
			return JsonUtility.FromJson<SysBroadCast>(ct);
		}
		public static string serialize(SysBroadCast ec){
			return JsonUtility.ToJson(ec);
		}
	}
}