/**************************************/
//FileName: Game.cs
//Author: wtx
//Data: 04/13/2017
//Describe:  游戏数据，用于存储游戏相关数据，数据访问接口只提供给GameController使用
/**************************************/

using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

namespace Game{

	public static class Game{
		public static int firstHandSeat = 255;
		public static int currentTurn = 255;

		public static void reset(){
			currentTurn = 255;
			firstHandSeat = 255;
		}
	}

}