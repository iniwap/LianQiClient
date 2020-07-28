using System.Collections;
using System.Collections.Generic;
using Eppy;

//temp

using UnityEngine;

public class Manager {
    public enum GameState {
        UNINITIALIZED,  // 未初始化
        READY,          // 已初始化 等待开始
        STARTED,        // 运行中
        ENDING,         // 已结束
    }

    //此处的网格阶数实际代表的是初始化网格的方式，鉴于后面可能出现非规整网格，此处参数应该会变成一个对象或者结构体
    public Manager(int numOfPlayer, int levelOfGrid) {
        _State = GameState.UNINITIALIZED;

        init(numOfPlayer, levelOfGrid);
    }

    // 可重入以重新开始游戏
    public void init(int numOfPlayer, int levelOfGrid) {
        _Board = new GameBoard(numOfPlayer, levelOfGrid);

        _State = GameState.READY;
    }

    //从此处开始游戏
    public void start() {
        if (_State == GameState.READY) {
            _TempBoard = _Board.getCopy();

            _State = GameState.STARTED;
        }
    }

    // 执子相关
    // 格子合法性判断
    public bool isGridValid(Vector3i position) {
        // 交由下层的Rule判断
        return Rule.isGridValid(_TempBoard, position);
    }


    // 尝试在指定位置下棋
    public GameBoard tryToPlace(Vector3i position, int direction) {
        if (isGridAndDirectionValid(position, direction)) {
            GameBoard tempBoard = _Board.getCopy();

            tempBoard.placeChess(position, direction);
            //更新下棋之后的数值
            Rule.updateBoard(tempBoard);

            return tempBoard;
        }
        else
            return null;
    }

    public int getThisTurnChessNum() {
        return this._Board.getThisTurnChessNumber();
    }


    public int getChessThisTurn() {
        return _Board.getThisTurnChessNumber();
    }

    // 正式落子
    public bool placeChess(Vector3i position, int direction) {
        if (isGridAndDirectionValid(position, direction)) {
            _TempBoard = tryToPlace(position,direction);
            GameChess gc = _TempBoard.getGameChessCopy(position);
            if (gc._Health <= 0){
                return false;
            }

            _Board.placeChess(position, direction);
           
            //更新下棋之后的数值
            Rule.updateBoard(_Board);
            Rule.clearHealthLowChess(_Board);

            _TempBoard = _Board.getCopy();

            return true;
        }

        return false;
    }


    // 尝试移动棋子
    public GameBoard tryChessMove(Vector3i position, int direction)
    {
        // 当前格指向方向的信息判断
        GameChess gc = _Board.getGameChessRef(position);
        //如果这个棋子上有向前走的技能，则可以走
        if (gc.hasSuchKindSkill(EffectType.move))
        {
            _TempBoard = _Board.getCopy();
            if (_TempBoard.moveChess(position))
            {
                Rule.updateBoard(_TempBoard);
                return _TempBoard;
            }
            else {
                Debug.Log("严重错误");
            }
        }
        //如果不能移动，返回一个null
        return null;
    }



    // 移动棋子
    public GameBoard chessMove(Vector3i position, int direction) {

        if (tryChessMove(position, direction) == null)
        {
            Debug.Log("重大错误");
            return null;
        }

        else {
            if (!_Board.moveChess(position)) {
                Debug.Log("有问题出现");
            }
            Rule.updateBoard(_Board);

            return _Board.getCopy();
        }
    }

    // 获得所有棋子的副本
    public List<GameChess> getGameChessList() {
        return _Board.getGameChessListCopy();
    }

    // 获取指定位置游戏棋子
    public GameChess getGameChess(Vector3i position) {
        return _Board.getGameChessCopy(position);
    }

    // 当前回合可用方向
    public List<int> getUsableDirectionsCopy() {
        return Rule.getUsableDirections(_Board);
    }

    public GameBoard getTempBoard() {
        return _Board.getCopy();
    }

    // 游戏状态相关
    // 结束当前玩家回合
    public void endTurn() {
        _Board.endAction();

        _Board.changeTurn();

        Rule.clearHealthLowChess(_Board);

        Rule.clearAllMove(_Board);

        _TempBoard = _Board.getCopy();
    }

    public void updateButNotEndTurn() {
        _Board.endAction();

        Rule.clearHealthLowChess(_Board);

        _TempBoard = _Board.getCopy();
    }

    // 弃权
    public void abandon() {

    }

    public GameState getGameState() {
        GameState tempState = new GameState();

        tempState = _State;

        return tempState;
    }

    // 返回当前正在玩的玩家编号
    public int getPlayerNow() {
        return _Board.getPlayerNow();
    }

    // 方便内部实现
    private bool isGridAndDirectionValid(Vector3i position, int direction) {
        if (isGridValid(position) && isDirectionValid(direction))
            return true;

        return false;
    }

    private bool isDirectionValid(int direction) {
        List<int> dirList = getUsableDirectionsCopy();

        if (dirList.Contains(direction))
            return true;

        return false;
    }

    private GameState _State;

    // 游戏棋盘
    private GameBoard _Board;

    private GameBoard _TempBoard;

    public bool fintest(Vector3i pos) {
        if (_Board.findtest(pos) == null) {
            return false;
        }
        return true;
    }

    public string getstr() {
        return this._Board.getRealchess();
    }
}
