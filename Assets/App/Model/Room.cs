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

namespace Room{
	public struct Player{

		public int seat;
		public bool isOwner;
		public CommonDefine.PLAYER_STATE state;
		public int userID;
		public string name;
		public string head;
		public int vip;
		public int sex;//0女，1男
		public int win;
		public int lose;
		public int draw;
		public int escapse;
		public int charm;//魅力值
		public long gold;//金币,主要消耗货币类型
		public int score;

		public List<CommonDefine.eTalentType> talentList;
	};

	public struct RoomData{
		public int roomId;
		public int isFull;
		public string roomName;
		public string roomDes;
		public int roomPersonCnt;
		public string roomPwd;
		public string rule;
	};

	public static class Room{
		public static int roomID;
		public static string rule;
		public static CommonUtil.RoomRule roomRule;
		public static int selfSeat;
		public static bool isRelink;
		public static int roomLevel;
		public static int owner;
		public static int baseScore;

		//
		public static int tagId;
		public static string plazaName;
		public static int plazaid;
		public static CommonDefine.eCreateRoomType roomType;
		public static bool hasAbandon;// 自己是否已经投降了

		public static List<Player> playerList = new List<Player>();//存储对局玩家	
		public static List<RoomData> roomList = new List<RoomData>();//此条不随具体对局变化，是静态数据，只有再次收到才清空

		public static void reset(){
			roomType = CommonDefine.eCreateRoomType.ROOM_CLASSIC_PLAZA;
			plazaid = 0;
			plazaName = "";
			tagId = -1;
			roomLevel = 0;
			owner = 0;
			roomID = 0;
			rule = "";
			//roomRule.playerNum = 0;
			playerList.Clear ();
			selfSeat = 255;
			isRelink = false;
			baseScore = 0;
			hasAbandon = false;

			Account.inRoomId = 0;//清除
		}
		public static void setPlazaData(string name,int tag){
			tagId = tag;
			plazaName = name;
		}
		public static void setRoomData(int roomid,int levelId,int plazaID,int roomtype,int roomOwner,string ruleStr,int bs){
			roomID = roomid;
			rule = ruleStr;
			roomLevel = levelId;
			owner = roomOwner;

			roomRule = CommonUtil.RoomRule.deserialize(ruleStr);
			roomType = (CommonDefine.eCreateRoomType)roomtype;
			plazaid = plazaID;
			//其他参数
			baseScore = bs;

		}

		public static string getPlayerNameBySeat(int seat){
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList [i].seat == seat) {
					return playerList [i].name;
				}
			}
			return "";
		}
		public static void addPlayer(Player player){
			//存在则更新，不存在则添加
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].userID == player.userID) {
					Player p = playerList[i];
					p.charm = player.charm;
					p.draw = player.draw;
					p.escapse = player.escapse;
					p.gold = player.gold;
					p.head = player.head;
					p.lose = player.lose;
					p.name = player.name;
					p.score = player.score;
					p.seat = player.seat;
					p.sex = player.sex;
					p.state = player.state;
					p.win = player.win;
					playerList[i] = p;
					return;
				}
			}

			//设置自己的座位
			if (player.userID == Account.getSelfData ().userID) {
				selfSeat = player.seat;
			}

			playerList.Add(player);

		}
		public static bool removePlayerBySeat(int seat){
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].seat == seat) {
					playerList.RemoveAt(i);
					return true;
				}
			}
			return false;
		}
		public static bool removePlayerByUserID(int userID){
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].userID == userID) {
					playerList.RemoveAt(i);
					return true;
				}
			}
			return false;
		}


		public static Player? getPlayerBySeat(int seat){
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].seat == seat) {
					return playerList[i];
				}
			}
			return null;
		}

		public static Player? getPlayerByUserID(int userID){
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].userID == userID) {
					return playerList[i];
				}
			}
			return null;
		}

		public static bool updatePlayerStateByUserID(int userID,CommonDefine.PLAYER_STATE state){
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].userID == userID) {
					Player p = playerList[i];
					p.state = state;
					playerList[i] = p;
					return true;
				}
			}
			return false;
		}

		public static bool getIfAllPlayerReady(){
			int readyNum = 0;
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].state == CommonDefine.PLAYER_STATE.STATE_TYPE_ROOMREADY) {
					readyNum++;
				}
			}

			if (readyNum == roomRule.playerNum) {
				return true;
			} else {
				return false;				
			}
		}

		public static bool updatePlayerStateBySeat(int seat,CommonDefine.PLAYER_STATE state){
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].seat == seat) {
					Player p = playerList[i];
					p.state = state;
					playerList[i] = p;
					return true;
				}
			}
			return false;
		}

		public static void setHasAbandon(){
			hasAbandon = true;
		}

		public static bool getHasAbandon(){

			return hasAbandon;
		}

		public static int getLocalBySeat(int seat){
			int[] PLAYER_2 = { 0,2};//0,1
			int[] PLAYER_3 = { 0,1,3};//0,1,2
			int[] PLAYER_4 = { 0,1,2,3};//0,1,2,3

			if(seat < 0 || seat > roomRule.playerNum - 1){
				return (int)CommonDefine.SEAT.SELF;
			}

			int index = (seat - selfSeat + (int)CommonDefine.SEAT.SELF + roomRule.playerNum) % roomRule.playerNum;
			if (roomRule.playerNum == 2) {
				return PLAYER_2[index];
			} else if (roomRule.playerNum == 3) {
				return PLAYER_3[index];
			} else if (roomRule.playerNum == 4) {
				return PLAYER_4[index];
			}

			//BUG
			return (int)CommonDefine.SEAT.SELF;
		}

		public static int getSeatByLocal(int local){
			int[] PLAYER_2 = { 0,2};
			int[] PLAYER_3 = { 0,1,3};
			int[] PLAYER_4 = { 0,1,2,3};

			if (local < 0 || local > roomRule.playerNum) {
				return selfSeat;
			}
			int localIndex = 0;
			int [] pseat = PLAYER_2;

			if (roomRule.playerNum == 2) {
				pseat = PLAYER_2;
			}else if(roomRule.playerNum == 3){
				pseat = PLAYER_3;
			}else if(roomRule.playerNum == 4){
				pseat = PLAYER_4;
			}
			for (int i = 0; i < pseat.Length; i++) {
				if (local == pseat [i]) {
					localIndex = i;
					break;
				}
			}
			return (selfSeat + localIndex ) % roomRule.playerNum;
		}
		public static bool updateRoomListByRoomID(int roomid,int pcnt){
			for (int i = 0; i < roomList.Count; i++) {
				if (roomid == roomList [i].roomId) {
					RoomData rd = roomList[i];
					rd.roomPersonCnt = pcnt;
					roomList[i] = rd;
					return true;
				}
			}

			return false;
		}
		public static bool removeRoomListByRoomID(int roomid){
			for (int i = 0; i < roomList.Count; i++) {
				if (roomid == roomList [i].roomId) {
					roomList.RemoveAt (i);
					return true;
				}
			}
			return false;
		}
		public static void addRoomListByRoomID(int roomid,RoomData data){
			for (int i = 0; i < roomList.Count; i++) {
				if (roomid == roomList [i].roomId) {
					//更新
					RoomData rd = roomList[i];
					rd.isFull = data.isFull;
					rd.roomDes = data.roomDes;
					rd.roomId = data.roomId;
					rd.roomName = data.roomName;
					rd.roomPersonCnt = data.roomPersonCnt;
					rd.roomPwd = data.roomPwd;
					rd.rule = data.rule;
					roomList[i] = rd;
					return;
				}
			}
			roomList.Add (data);
		}
	}
}