using System.Collections;
using System.Collections.Generic;

//temp

using UnityEngine;

public class GameBoard
{
    //private List<GameChess> _GameChessList;
    public List<GameChess> _GameChessList;
    //过去的玩家选择的方向-->可能存在一些问题-->主要是悔棋方面
    private List<int> _PastChessDirection;

    private ChessBoard _ChessBoard;
    //这回合下了几个棋子
    private int _ThisTurnNewChessNumber;
    // 现在的玩家序号
    private int _PlayerNow;
    // 玩家数量
    private int _PlayerNumber;


    public List<int> getPastDir()
    {
        return new List<int>(_PastChessDirection.ToArray());
    }


    //增加新的下子方向
    public void addNewDirection(int dir)
    {
        if (_PastChessDirection.Count >= 6)
        {
            _PastChessDirection.RemoveAt(0);
        }
        _PastChessDirection.Add(dir);
    }


    public int getThisTurnChessNumber() {
        return _ThisTurnNewChessNumber;
    }

    public int getPlayerNow() {
        return _PlayerNow;
    }
    
    // 改变当前的回合所有者，属于改变棋盘的状态
    public void changeTurn() {
        _PlayerNow++;

        _PlayerNow %= _PlayerNumber;
        
        _ThisTurnNewChessNumber = 0;
    }
    
    public void endAction() {
        _ChessBoard.endAction(_PlayerNow);
    }

   
    public GameChess getGameChessRef(Vector3i position)
    {
        for (int i = 0; i < _GameChessList.Count; i++)
        {
			GameChess c = _GameChessList[i];
			Vector3i p = c.getPosition ();
            if (p == position)
            {
                //Debug.Log("成功"+position.ToString());
                return _GameChessList[i];
            }
        }

        //Debug.Log("失败" + position.ToString());
        return null;
    }

    // 成功放置棋子后调用，属于改变棋盘的状态
    public bool placeChess(Vector3i position, int direction) {
        // add game chess
        Grid grid = new Grid(position);

        Chess chess = new Chess(_PlayerNow, direction,grid.getPos());

        GameChess gameChess = new GameChess(chess);

        // 游戏棋盘落子
        _GameChessList.Add(gameChess);

        _ThisTurnNewChessNumber++;

        //更新方向列表
        addNewDirection(direction);

        // 物理棋盘落子
        if (_ChessBoard.placeChess(chess) == null)
        {
            return false;
        }
        else {
            return true;
        }
    }

    // 取子
    public bool removeChess(Vector3i position)
    {
        // remove game chess
        for (int i = 0; i < _GameChessList.Count; i++)
        {
            if (_GameChessList[i].getPosition() == position)
            {
                Grid grid = new Grid(_GameChessList[i].getPosition());

                Chess chess = new Chess( _GameChessList[i].getPlayerID(), _GameChessList[i].getDirection(),grid.getPos());

                // 物理棋盘移除
                if (_ChessBoard.removeChess(chess._GridPos))
                    // 游戏棋盘移除
                    _GameChessList.RemoveAt(i);

                return true;
            }
        }

        return false;
    }

    public bool moveChess(Vector3i position) {
        GameChess gc = getGameChessRef(position);
        //如果有这个棋子并且有移动技能

        if (gc != null && gc.hasSuchKindSkill(EffectType.move)){
            //移动并且告诉Judgement去掉其他移动技能
            //先把这个棋子从原来GameChessList那里抹杀掉
            _GameChessList.Remove(gc);
            //然后从ChessBoard层面也抹杀掉
            _ChessBoard.removeChess(gc.getPosition());
            //然后加入新棋子
            Skill sk = gc.findSkill(EffectType.move);
            Chess nc = _ChessBoard.placeChess(new Chess(gc.getPlayerID(), gc.getDirection(), sk.getEffectPosition()));
            _GameChessList.Add(new GameChess(nc));

            Vector3i pos = getPointedPosition(position, gc.getDirection());
            Rule.removeNewSkill(EffectType.move, pos, this);
            return true;
        }
        return false;
    }

    // 获取指定方向上格子位置
    public Vector3i getPointedPosition(Vector3i _position, int direction)
    {
        Chess gc = _ChessBoard.getChessInDirection(_position, direction);
        if(gc!=null){
            //Debug.Log("查找成功"+_position.ToString());
            return gc._GridPos; 
        }
        //Debug.Log("查找失败" + _position.ToString());
        return new Vector3i(0,0,-1);
    }

    // 获取指定方向上棋子内容
    //public GameChess getPointedGameChessCopy(Vector3i _position, int direction)
    //{
    //    GameChess chess = null;
    //    Chess tempChess = _ChessBoard.getChessInDirection(_position, direction);

    //    if (tempChess != null)
    //    {
    //        chess = new GameChess(tempChess);
    //    }

    //    return chess;
    //}

    public GameChess getPointedGameChessRef(Vector3i _position, int direction)
    {
        Vector3i p = getPointedPosition(_position, direction);
        
        if (p.z >= 0)
        {
            //Debug.Log("查找成功"+p.ToString());
            GameChess gc = getGameChessRef(p);
            if (gc != null)
            {
                //Debug.Log("成功"+p.ToString());
            }
            else {
                //Debug.Log("失败" + p.ToString());
            }
            return gc;
        }
        else
        {
            //Debug.Log("查找失败" + _position.ToString());
            return null;
        }
    }

    //----------------------------------------获得副本------------------------------------------------------------

    public GameBoard getCopy()
    {
		GameBoard gb = new GameBoard(_PlayerNow, _ChessBoard.getGridLevel());
        gb._ChessBoard = this._ChessBoard.getChessBoardCopy();
        //gb._PastChessDirection = new List<int>();
        //gb._GameChessList = new List<GameChess>();
        foreach (GameChess gc in this._GameChessList)
        {
            gb._GameChessList.Add(gc.getCopy());
        }
        gb._PlayerNow = this._PlayerNow;
        gb._PlayerNumber = this._PlayerNumber;

        gb._ThisTurnNewChessNumber = this._ThisTurnNewChessNumber;

        gb._PastChessDirection = new List<int>();
        foreach (int i in this._PastChessDirection) {
            gb._PastChessDirection.Add(i);
        }

        return gb;
    }

    // 获得棋子列表拷贝
    public List<GameChess> getGameChessListCopy() {
        List<GameChess> tempGameChessList = new List<GameChess>();
        foreach (GameChess gc in this._GameChessList) {
            tempGameChessList.Add(gc.getCopy());
        }

        return tempGameChessList;
    }


    // 获取指定位置棋子游戏属性
    public GameChess getGameChessCopy(Vector3i position)
    {
        for (int i = 0; i < _GameChessList.Count; i++)
        {
            if (_GameChessList[i].getPosition() == position)
                return _GameChessList[i].getCopy();
        }

        return null;
    }



    public GameBoard(int playerNum, int gridLevel){
        _ChessBoard = new ChessBoard(gridLevel);

        _GameChessList = new List<GameChess>();

        _PastChessDirection = new List<int>();

        _PlayerNow = 0;

        _PlayerNumber = playerNum;

		_ThisTurnNewChessNumber = 0;
    }

    /*public GameBoard(){
		_ChessBoard = new ChessBoard(gridLevel);

		_GameChessList = new List<GameChess>();

		_PastChessDirection = new List<int>();

        _PlayerNow = 0;

        // 默认双人
        _PlayerNumber = 2;
    }*/

    //当前玩家结束行动
    private void endAction(int playerId)
    {
        _ChessBoard.endAction(playerId);
    }

    public Chess findtest(Vector3i pos) {
        Chess c = _ChessBoard.getChessCopy(pos);
        return c;
    }

    public string getRealchess() { 
        string s="";
        List<Chess>c = _ChessBoard.getChessListCopy();
        foreach(Chess cc in c){
            s += cc._GridPos.ToString() + "  ";
        }

        return s;
    }
}