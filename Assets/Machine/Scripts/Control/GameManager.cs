using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Liuhe.Model;
using Liuhe.Rules;
using Liuhe.Utils;
using Liuhe.Player;


namespace Liuhe.Manager
{
    public enum GAME_STATE
    {
        READY,
        RUNNING,
        PAUSE,
        STOP,

    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager _Main;

        public LiuheChessBoard _GameBoard;
        public List<LiuhePlayer> _Players;
        public int _BoardLevel;
        public GAME_STATE _State;

        public int _PlayerNow;
        public int _PlayerNum;

        public int _RoundNum;

        public int _AI_Type;

        //Timer set
        //Logger set


        private void Awake()
        {
            setupGameManager();
        }

        // Use this for initialization
        void Start()
        {
            if (_Main == null)
            {
                _Main = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }


        // Update is called once per frame
        void Update()
        {
            //Time update
            /*
             * 开始循环之后，首先查看是否已经结束游戏，
             * 
             * 然后查看是否暂停
             * 
             * 然后是是否正在进行游戏
             * 
             * 正在进行游戏的时候，计时器会走动
             * 
             * 更新计时器之后，查看此时时间是否超时，如果超时，则结束此玩家回合，并且到下一玩家
             * 
             * 
             */
        }

        public bool ifThisPlayerLocal() {
            if (_Players[_PlayerNow]._Type == PLAYER_TYPE.LOCAL)
            {
                return true;
            }
            else {
                return false;
            }
        }

        private void setupGameManager()
        {
            _GameBoard = null;
            _BoardLevel = 0;
            _PlayerNum = 0;
            _State = GAME_STATE.READY;
            _Players = new List<LiuhePlayer>();
            _AI_Type = 0;
        }

        public void startGame(int level, List<PLAYER_TYPE> ps, List<PLAYER_CHARACTER> pc)
        {
            if (_State != GAME_STATE.READY)
            {
                Debug.Log("当前状态下不能开始游戏！请检查后重新设置");
                return;
            }

            _BoardLevel = level;
            _GameBoard = new LiuheChessBoard(_BoardLevel);

            _PlayerNum = ps.Count;
            _Players = new List<LiuhePlayer>();
            for (int i = 0; i < _PlayerNum; i++)
            {
                if (ps[i] == PLAYER_TYPE.AI)
                {
                    _Players.Add(new LiuheAIPlayer(i,pc[i]));
                }
                else if (ps[i] == PLAYER_TYPE.LOCAL)
                {

                    _Players.Add(new LiuheLocalPlayer(i,pc[i]));
                }
                else if (ps[i] == PLAYER_TYPE.NETWORK)
                {

                    _Players.Add(new LiuheNetworkPlayer(i,pc[i]));
                }
                else
                {
                    Debug.Log("不存在这样的玩家类型！请检查后重新输入！");
                }
            }

            _State = GAME_STATE.RUNNING;
            _PlayerNow = 0;

            LiuheLogger.Log(_Players);
        }

        public LiuhePlayer getPlayerNow() {
            return _Players[_PlayerNow];
        }

        private void changeRound() {

        }

        public bool isThisIsPlayerNow(LiuhePlayer p) {
            if (p == _Players[_PlayerNow]) {
                return true;
            }

            return false;
        }


        public LiuheChessBoard tryToPlace(int xx, int yy, int dir)
        {
            return Rules.Rules.getTryResult(_GameBoard.getCopy(), xx, yy, dir, _Players[_PlayerNow]);
        }

        public bool placeChess(LiuhePlayer player, int xx, int yy, int dir) {
            if (!isThisIsPlayerNow(player))
            {
                Debug.Log("当前并非此玩家的回合，无法操作");
                return false;
            }

            LiuheChessBoard lcb = this._GameBoard.getCopy();

            if (!Rules.Rules.tryPlaceChessToGameBoard(lcb, xx, yy, dir, player)){
                Debug.Log("落子失败！");
                return false;
            }

            _GameBoard.addNewChess(xx, yy, dir, player._PlayerIndex);

            updateChessBoard();
            
            return true;
        }

        public void updateChessBoard() {
            Rules.Rules.GameBoardCalculateItself(_GameBoard);
        }

        private void updateChessBoard(LiuheChessBoard lcb)
        {
            Rules.Rules.GameBoardCalculateItself(lcb);
        }

        public LiuheChessBoard getWahsedOutChessBoardCopy() {
            LiuheChessBoard lcb = _GameBoard.getCopy();
            washOut(lcb);
            updateChessBoard(lcb);

            return lcb;
        }


        public void washOut() {
            Rules.Rules.washChessBoard(_GameBoard);
        }

        private void washOut(LiuheChessBoard lcb)
        {
            Rules.Rules.washChessBoard(lcb);
        }

        public LiuheChessBoard getTryMoveChessBoard(ChessObj co) {

            return Rules.Rules.getTryEndBoard(co, _GameBoard);
        }

        public LiuheChessBoard getTryMoveChessBoard(ChessObj co, LiuheChessBoard lcb)
        {

            return Rules.Rules.getTryEndBoard(co, lcb);
        }

        public LiuheChessBoard getTryMovesMultiChessBoard(List<ChessObj> coses, LiuheChessBoard lcb) {

            return Rules.Rules.getTryChessesEndBoard(coses, lcb);
        }

        public List<SpecialChessLink> tryEndAction() {

            LiuheChessBoard lcb = _GameBoard.getCopy();
            Rules.Rules.GameBoardCalculateItself(lcb);

            //没有棋子被吃掉的话
            if (lcb.deads.Count == 0)
            {
                return null;
            }
            else
            {
                return lcb.attacks;
            }
        }

        public void endTurn() {
            _GameBoard.endAction(_PlayerNow);

            _RoundNum = _GameBoard.roundNum;

            Rules.Rules.cleanAttacks(_GameBoard);

            _PlayerNow++;
            if (_PlayerNow >= _PlayerNum) {
                _PlayerNow = 0;
            }

            if (_Players[_PlayerNow]._Type == PLAYER_TYPE.AI) {
                Debug.Log(_PlayerNow);
                _Players[_PlayerNow].sayhello();
                StartCoroutine(_Players[_PlayerNow].callNewAction());
                Debug.Log("到了AI的回合");
            }
        }

        public bool moveChess(ChessObj co) {
            Debug.Log("移动了棋子，棋子是：");
            LiuheLogger.Log(co);

            bool result = Rules.Rules.moveChessInBoard(_GameBoard.findChessByIdnum(co._ChessIdentityNumber), _GameBoard);
            return result;
        }

        public bool moveChessAI(ChessObj co) {
            bool result = Rules.Rules.moveChessInBoardUnsafe(_GameBoard.findChessByIdnum(co._ChessIdentityNumber), _GameBoard);
            return result;
        }

        public bool getTryMoveBoard(ChessObj co) {
            LiuheChessBoard lcb = Rules.Rules.getTryEndBoard(co,_GameBoard);
            if (lcb != null)
            {
                int h = lcb.findChessByIdnum(co._ChessIdentityNumber).health;
                Debug.Log(h);
                if (h <= 0) {
                    return false;
                }
                return true;
            }
            else {
                return false;
            }
        }

        public int makeNewChessUnSafe(int xx, int yy, int dir, int ownner) {
            int ni = _GameBoard.makeNewChess(xx, yy, dir, ownner);
            _GameBoard.addForbiddenDir(dir, ownner);
            return ni;
        }


        public bool isThisGridEmpty(int xx,int yy) {
            GridValidState gvs = Rules.Rules.getGridValidState(_GameBoard, xx, yy);
            if (gvs == GridValidState.VOID) {
                return true;
            }

            return false;
        }

        public List<int> getUsableDirection() {
            return Rules.Rules.getUsableDirection(_GameBoard);
        }

        public List<LiuheChess> getChessList() {
            return _GameBoard.chesses;
        }

    }
}

