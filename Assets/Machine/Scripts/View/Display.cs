using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;

using Liuhe.Manager;
using Liuhe.Player;
using Liuhe.Model;
using Liuhe.Utils;

//游戏模式
public enum GameMode {
    LEGACY_MODE=0,
    TEACH_MODE=1,
    STORY_MODE=2,
    FREE_BATTLE=3,
    LOCAL_MODE=4,
    NONE=-1,
}

//显示类
public class Display : MonoBehaviour {
    //回合状态类型
    public enum DisplayState {
        HAS_NOT_PLACE_CHESS,
        HAS_PLACE_CHESS,
        PLACE_CHESS_WRONG,
        MOVE_CHESS,
        HAS_MOVED_CHESS,
        DO_NOTHING,
        ENDTURN,
    }
    [Header("基础数据")]
    [Space(10)]
    public int _PlayerNum;          //玩家人数
    public int _BoardLevel;         //棋盘阶数
    public float _Interval;         //网格之间的间隔
    public GridObj _GridPrefab;          //网格预设
    public ChessObj _ChessPrefab;        //棋子预设
    public GameObject _LuaPreSet;
    public GameObject _LoadLuaPreSet;

    public int _GameTypeInt;

    [Header("游戏过程数据")]
    [Space(10)]
    public Transform _GridRoot;             //网格根节点
    public Transform _ChessRoot;            //棋子根节点
    public Transform _UnderChess;           //最上层棋子根节点（实际上是死掉的棋子）
    public DisplayState _RoundState;  //当前回合状态
    public GameMode _Game_Mode;            //游戏模式
    private GameManager _Manager;           //游戏流程的Manager-->但需要Display唤醒
    public List<GridObj> _GridList;         //网格实例队列
    public List<ChessObj> _ChessList;       //棋子实例队列
    public ChessObj _TempChess;             //当前暂存棋子


    //public ChessObj _HasMovedChess = null;  //移动棋子的暂存
    public List<ChessObj> _HasMoveChessList;    //移动棋子的列表

    public bool _IsStarted = false;        //是否初始化结束

    public int _LastRound;

    public static Display _Main;

    #region 动画数据
    
    [Header("动画数据")]
    [Space(10)]

    public Sprite[] _PlayerSprites;         //玩家头像
    public Transform _PLayerOne;            //玩家一
    public Transform _PlayerTwo;            //玩家二
    [Space(10)]
    public string[] _Warning_Info;          //警告信息
    public Text _WarningText;              //警告信息位置
    [Space(10)]
    public Transform _ChessLogTransform;    //告示牌1
    public Transform _ChessLog2Transform;   //告示牌2
    public Transform _ChessLog3Transform;   //告示牌3
    public Transform _ChessLogStart;        //告示牌开始位置（外面）
    public Transform _ChessLogEnd;          //告示牌结束位置（里面）
    [Space(10)]
    public Transform _PlayerPosStartTransform;     //玩家头像起点位置
    public Transform _EnemyPosStartTransform;     //对手头像起点位置
    public Transform _PlayerPosEndTransform;        //玩家头像终点位置
    public Transform _EnemyPosEndTransform;         //对手头像终点位置
    [Space(10)]
    public Transform _PlayerSkill00;        //玩家技能0
    public Transform _PlayerSkill01;        //玩家技能1
    public Transform _EnemySkill00;         //对手技能0
    public Transform _EnemySkill01;         //对手技能1
    [Space(10)]
    public Transform _PlayerSkill00_Traget;     //玩家技能目标0
    public Transform _PlayerSkill01_Traget;     //玩家技能目标1
    public Transform _EnemySkill00_Traget;      //对手技能目标0
    public Transform _EnemySkill01_Traget;      //对手技能目标1
    [Space(10)]
    public Color _P0Tint;       //玩家0颜色
    public Color _P1Tint;       //玩家1颜色

    #endregion



    IEnumerator Start() {
        _Main = this;
        //有初始信息，则按照初始信息开始；否则开始本地游戏

        /*
        if (StageInfo._Main != null)
        {
            yield return StartGame(StageInfo._Main._Game_Mode_This_Time);
        }
        else {
            if (_GameTypeInt == 0)
            {
                yield return StartGame(GameMode.FREE_BATTLE);
            }
            else
            {
                yield return StartGame(GameMode.TEACH_MODE);
            }
            //_LoadLuaPreSet.SetActive(true);
            //_LuaPreSet.SetActive(true);
            //yield return StartGame(GameMode.LEGACY_MODE);
            //yield return StartGame(GameMode.LOCAL_MODE);
        }
        */
        yield return StartGame(GameMode.FREE_BATTLE);
    }

    //生成玩家
    private bool generatePlayers(GameMode _Mode) {
        bool success;
        success = false;

        switch (_Mode)
        {
            case GameMode.LEGACY_MODE:
                {
                    success = true;

                    //残局模式生成两个玩家，一个本地，一个AI；类型一个主人公，一个残局
                    List<PLAYER_TYPE> pt;
                    pt = new List<PLAYER_TYPE>();
                    pt.Add(PLAYER_TYPE.LOCAL);
                    pt.Add(PLAYER_TYPE.AI);

                    List<PLAYER_CHARACTER> pc;
                    pc = new List<PLAYER_CHARACTER>();
                    pc.Add(PLAYER_CHARACTER.HERO);
                    pc.Add(PLAYER_CHARACTER.LEGACY);

                    generateChessBoardGameObjs(_BoardLevel);
                    _Manager.startGame(_BoardLevel, pt, pc);
                }
                break;
            case GameMode.TEACH_MODE:
                {
                    success = true;

                    //人机对战模式生成两个玩家，一个本地，一个AI；类型一个主人公，一个天枢
                    List<PLAYER_TYPE> pt;
                    pt = new List<PLAYER_TYPE>();

                    pt.Add(PLAYER_TYPE.LOCAL);
                    pt.Add(PLAYER_TYPE.AI);

                    List<PLAYER_CHARACTER> pc;
                    pc = new List<PLAYER_CHARACTER>();
                    pc.Add(PLAYER_CHARACTER.HERO);
                    pc.Add(PLAYER_CHARACTER.TIAN_SHU);

                    generateChessBoardGameObjs(_BoardLevel);
                    _Manager.startGame(_BoardLevel, pt, pc);
                }
                break;
            case GameMode.STORY_MODE:
                Debug.Log("此模式尚未开启");
                break;
            case GameMode.FREE_BATTLE:
                {
                    success = true;

                    //人机对战模式生成两个玩家，一个本地，一个AI；类型一个主人公，一个天枢
                    List<PLAYER_TYPE> pt;
                    pt = new List<PLAYER_TYPE>();
                    pt.Add(PLAYER_TYPE.LOCAL);
                    pt.Add(PLAYER_TYPE.AI);

                    List<PLAYER_CHARACTER> pc;
                    pc = new List<PLAYER_CHARACTER>();
                    pc.Add(PLAYER_CHARACTER.HERO);
                    pc.Add(PLAYER_CHARACTER.TIAN_SHU);

                    generateChessBoardGameObjs(_BoardLevel);
                    _Manager.startGame(_BoardLevel, pt, pc);
                }
                break;
            case GameMode.LOCAL_MODE:
                {
                    success = true;

                    //本地模式生成两个玩家，都是本地，一个主人公，一个天枢
                    List<PLAYER_TYPE> pt;
                    pt = new List<PLAYER_TYPE>();
                    pt.Add(PLAYER_TYPE.LOCAL);
                    pt.Add(PLAYER_TYPE.LOCAL);

                    List<PLAYER_CHARACTER> pc;
                    pc = new List<PLAYER_CHARACTER>();
                    pc.Add(PLAYER_CHARACTER.HERO);
                    pc.Add(PLAYER_CHARACTER.TIAN_SHU);

                    generateChessBoardGameObjs(_BoardLevel);
                    _Manager.startGame(_BoardLevel, pt, pc);
                }
                break;
            case GameMode.NONE:
                Debug.Log("错误选择模式");
                return generatePlayers(GameMode.LOCAL_MODE);
            default:
                break;
        }
        return success;
    }
    

    IEnumerator StartGame(GameMode _Mode)
    {
        _IsStarted = false;
        _LastRound = 0;
        _Manager = GetComponent<GameManager>();
        _Game_Mode = _Mode;
        //初始化设定
        bool success = generatePlayers(_Mode);

        if (success)
        {
            //设置头像
            if (_Game_Mode == GameMode.LEGACY_MODE)
            {
                Image im1 = _PLayerOne.transform.Find("CharacterHead").Find("Image").GetComponent<Image>();
                Image im2 = _PlayerTwo.transform.Find("CharacterHead").Find("Image").GetComponent<Image>();
                im1.sprite = _PlayerSprites[0];
                im2.sprite = _PlayerSprites[2];
            }
            else if (_Game_Mode == GameMode.LOCAL_MODE)
            {
                Image im1 = _PLayerOne.transform.Find("CharacterHead").Find("Image").GetComponent<Image>();
                Image im2 = _PlayerTwo.transform.Find("CharacterHead").Find("Image").GetComponent<Image>();
                im1.sprite = _PlayerSprites[0];
                im2.sprite = _PlayerSprites[1];
            }
            
            //棋盘动画
            DOTween.To(() => _Interval, x => _Interval = x, 13f, 1.9f).SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(0.5f);

            //玩家游戏动画
            _PlayerPosStartTransform.DOMove(_PlayerPosEndTransform.position, 1f).SetEase(Ease.OutBack);
            _EnemyPosStartTransform.DOMove(_EnemyPosEndTransform.position, 1f).SetEase(Ease.OutBack);

            yield return new WaitForSeconds(0.8f);

            //技能动画
            _PlayerSkill00.DOLocalMove(_PlayerSkill00_Traget.position, 0.5f).SetEase(Ease.OutBack);
            _PlayerSkill01.DOLocalMove(_PlayerSkill01_Traget.position, 0.5f).SetEase(Ease.OutBack);

            //Legacy模式不需要技能显示
            if (_Game_Mode != GameMode.LEGACY_MODE)
            {
                _EnemySkill00.DOLocalMove(_EnemySkill00_Traget.position, 0.5f).SetEase(Ease.OutBack);
                _EnemySkill01.DOLocalMove(_EnemySkill01_Traget.position, 0.5f).SetEase(Ease.OutBack);
            }


            yield return new WaitForSeconds(0.7f);
            //updateBoardPos();
            //开始接受输入
            _IsStarted = true;
            //弹出说明框
            _ChessLogTransform.DOMove(_ChessLogStart.position, 0.5f);
            _ChessLog3Transform.DOMove(_ChessLogStart.position, 0.5f);
            //变色
            tintLog2();
            _ChessLog2Transform.DOMove(_ChessLogEnd.position, 0.5f);

            //回合状态改为还没有下棋
            _RoundState = DisplayState.HAS_NOT_PLACE_CHESS;
        }
    }

    public void readyToPlace() {
        _ChessLog2Transform.DOMove(_ChessLogStart.position, 0.5f);
    }

    // Update is called once per frame
    void Update () {
        //输出信息
        if (Input.GetButtonDown("Print")) {
            Debug.Log("输出信息：");
            LiuheChessBoard lb = _Manager._GameBoard;
            LiuheLogger.Log(lb);
        }
        if (Input.GetButtonDown("PrintDisplay"))
        {
            Debug.Log("输出信息：");
            LiuheLogger.Log(this);
        }

        if (_Manager._RoundNum != this._LastRound) {
            _LastRound = _Manager._RoundNum;
            _RoundState = DisplayState.HAS_NOT_PLACE_CHESS;
            Debug.Log("切换了新回合");
            

            //切换新的指示牌出来
            if (_Manager._Players[_Manager._PlayerNow]._Type == PLAYER_TYPE.LOCAL)
            {
                tintLog2();
                _ChessLog3Transform.DOMove(_ChessLogStart.position, 0.5f);
                _ChessLog2Transform.DOMove(_ChessLogEnd.position, 0.5f);
            }
            else
            {
                tintLog2();
                _ChessLog3Transform.DOMove(_ChessLogStart.position, 0.5f);
                _ChessLog2Transform.DOMove(_ChessLogEnd.position, 0.5f);
            }
        }
    }

    //点到了空白的地方
    public void clickEmypt() {
        if (!_Manager.ifThisPlayerLocal()) {
            return;
        }

        //如果有临时棋子
        if (_TempChess != null) {
            //删除临时棋子
            Destroy(_TempChess.gameObject);
            _TempChess = null;
            //如果是下过或者下错棋的模式，就把状态拨回
            if (_RoundState == DisplayState.DO_NOTHING || _RoundState == DisplayState.PLACE_CHESS_WRONG || _RoundState == DisplayState.HAS_NOT_PLACE_CHESS) {
                _RoundState = DisplayState.HAS_NOT_PLACE_CHESS;
                //弹出说明框
                _ChessLogTransform.DOMove(_ChessLogStart.position, 0.5f);
                _ChessLog3Transform.DOMove(_ChessLogStart.position, 0.5f);
                //变色
                tintLog2();
                _ChessLog2Transform.DOMove(_ChessLogEnd.position, 0.5f);
                //Debug.Log("切换状态");
            }
        }
        updateBoard();
    }

    public void clearDis() {
        GameManager._Main._GameBoard.places = new List<PlaceMemory>();
        //Debug.Log("clearded");
    }

    private ChessObj findChessByMoveTarget(int ti) {
        foreach (ChessObj co in _HasMoveChessList) {
            if (co._TargetInt == ti) {
                return co;
            }
        }

        return null;
    }

    //点击移动棋子，输入是要移动的棋子
    public void clickMoveChess(ChessObj co) {
        if (!_Manager.ifThisPlayerLocal())
        {
            return;
        }

        if (co._HasMove)
        {
            co.moveToOld();
            _HasMoveChessList.Remove(co);
        }
        else
        {
            ChessObj oldc = findChessByMoveTarget(co._TargetInt);
            if (oldc != null)
            {
                _HasMoveChessList.Remove(oldc);
                oldc.moveToOld();
            }

            co.moveToTemp();
            _HasMoveChessList.Add(co);
        }

        LiuheChessBoard lcb = _Manager._GameBoard.getCopy();
        lcb = _Manager.getTryMovesMultiChessBoard(_HasMoveChessList, lcb);

        updateBoard(lcb.chesses);

        #region old


        /*

        _HasMovedChess = null;
        //Debug.Log("iinin");
        foreach (ChessObj c in _ChessList) {
            if (c == co)
            {
                if (c._HasMove)
                {
                    c.moveToOld();
                }
                else
                {
                    _HasMovedChess = co;
                    c.moveToTemp();
                }
            }
            else if (c._CanMove && c._HasMove) {
                c.moveToOld();
            }
        }
        if (_HasMovedChess != null)
        {
            //更新移动后的数值
            LiuheChessBoard lcb2 = _Manager.getTryMoveChessBoard(_HasMovedChess);
            updateBoard(lcb2.chesses);
        }
        else {
            updateBoard();
        }

    */

        #endregion
    }

    //重新链接网格
    private void relinkGrid() {
        //网格清扫
        foreach (GridObj go in _GridList) {
            go.clear();
        }

        //棋子刷新
        foreach (ChessObj co in _ChessList) {
            co._GridOccupy.setChess(co);
            co.setNotMove();
        }
    }
	public void endGame() {
		SceneManager.LoadScene("StartScene");
	}
    public void endTurn() {
        if (!_Manager.ifThisPlayerLocal())
        {
            return;
        }

        switch (_RoundState)
        {
            case DisplayState.HAS_NOT_PLACE_CHESS:
                //有棋子
                if (_TempChess != null)
                {
                    LiuheChessBoard lcb = _Manager.tryToPlace(_TempChess.x, _TempChess.y, _TempChess._Direction);
                    //落子成功
                    if (lcb != null)
                    {
                        //如果生命值小于等于零
                        if (_TempChess._Health <= 0)
                        {
                            _ChessLogTransform.GetComponent<Animator>().SetTrigger("atten");
                            Destroy(_TempChess.gameObject);
                            _TempChess = null;
                            _RoundState = DisplayState.PLACE_CHESS_WRONG;
                        }
                        //如果生命值大于零
                        else {
                            //如果可以下棋，则下棋一个
                            _ChessList.Add(_TempChess);
                            _Manager.placeChess(_Manager._Players[_Manager._PlayerNow], _TempChess.x, _TempChess.y, _TempChess._Direction);
                            _TempChess._GridOccupy.setLock();
                            _TempChess = null;
                            //状态转为下过了棋子，再一次调用这个函数
                            _RoundState = DisplayState.HAS_PLACE_CHESS;
                            endTurn();
                        }
                    }
                    //落子失败
                    else
                    {
                        _ChessLogTransform.GetComponent<Animator>().SetTrigger("atten");
                        Destroy(_TempChess.gameObject);
                        _TempChess = null;
                        _RoundState = DisplayState.PLACE_CHESS_WRONG;
                    }
                }
                //没有棋子
                else
                {
                    //状态转为什么也不干
                    _RoundState = DisplayState.DO_NOTHING;
                    //告知将结束回合
                    _ChessLogTransform.DOMove(_ChessLogStart.position, 0.5f);
                    _ChessLog2Transform.DOMove(_ChessLogStart.position, 0.5f);
                    _ChessLog3Transform.DOMove(_ChessLogEnd.position, 0.5f);
                }

                break;
            case DisplayState.HAS_PLACE_CHESS:
                //判断下子之后需不需要特殊处理
                if (_Manager._GameBoard.deads.Count > 0)
                {
                    //如果有棋子死亡，则转到移动棋子模式，再一次调用这个函数
                    _RoundState = DisplayState.MOVE_CHESS;
                    endTurn();
                }
                else
                {
                    //如果没有棋子死亡，则自动进入下一回合
                    //状态转为结束回合，再一次调用这个函数
                    _RoundState = DisplayState.ENDTURN;
                    endTurn();
                }
                break;
            case DisplayState.PLACE_CHESS_WRONG:
                if (_TempChess != null)
                {
                    _RoundState = DisplayState.HAS_NOT_PLACE_CHESS;
                    endTurn();
                    break;
                }

                //状态转为什么也不干
                _RoundState = DisplayState.DO_NOTHING;
                //告知将结束回合
                _ChessLogTransform.DOMove(_ChessLogStart.position, 0.5f);
                _ChessLog2Transform.DOMove(_ChessLogStart.position, 0.5f);
                _ChessLog3Transform.DOMove(_ChessLogEnd.position, 0.5f);
                break;
            case DisplayState.MOVE_CHESS:   //找出可以移动的棋子
                //移动棋子代码不在这里，这里是设置移动棋子状态的地方

                //杀掉不需要的棋子
                foreach (ChessObj co in _ChessList)
                {
                    if (co._Health <= 0)
                    {
                        co.kill();
                    }
                }

                //把所有的棋子都设为不可移动
                foreach (ChessObj item in _ChessList)
                {
                    item.setNotMove();
                }

                //获得尝试结束的结果
                List<SpecialChessLink> scl = _Manager.tryEndAction();
                _HasMoveChessList = new List<ChessObj>();

                //当返回为空、或者没有可当前攻击的棋子时，结束回合；反之，进行移动棋子
                if (scl != null)
                {
                    List<ChessObj> cos = new List<ChessObj>();
                    //查看所有能攻击的棋子
                    foreach (SpecialChessLink item in scl)
                    {
                        ChessObj c = findChessObj(item._From_Idm);
                        //当可以攻击的棋子存在并且是当前回合的玩家时
                        if (c != null && c._Ownner == _Manager._PlayerNow)
                        {
                            ChessObj atc = findChessObj(item._To_Idm);
                            if (atc == null)
                            {
                                Debug.Log(item._To_Idm);
                                LiuheLogger.Log(_Manager._GameBoard.findChessByIdnum(c._ChessIdentityNumber));
                                Debug.Log("严重错误！");
                                LiuheLogger.Log(scl);
                            }
                            //设置这个棋子可以移动
                            c.setForward(atc.transform.position, atc, item._To_Idm);
                            cos.Add(c);
                        }
                    }

                    if (cos.Count > 0)
                    {
                        //当有足够的可攻击棋子存在时，去除所有的死亡棋子
                        _Manager.washOut();
                        //状态转换为等待棋子移动
                        _RoundState = DisplayState.HAS_MOVED_CHESS;
                        clickEmypt();
                    }
                    else {
                        //当没有足够可攻击棋子存在时，也就是结束回合
                        _Manager.washOut();
                        _RoundState = DisplayState.ENDTURN;
                        endTurn();
                    }
                    
                }
                //当返回为空时,也就是没有合理棋子指着这里
                else {
                    //结束回合
                    _Manager.washOut();
                    _RoundState = DisplayState.ENDTURN;
                    endTurn();
                }
                break;

            case DisplayState.HAS_MOVED_CHESS:  //移动的棋子移动之后（或者没有移动之后）
                {
                    //这里都是已经决定好移动结果后进来的地方
                    //也就是移动的代码在别处，这里只提交结果
                    if (_HasMoveChessList!=null && _HasMoveChessList.Count>0)
                    {
                        //判断是否真的可以
                        bool allright = true;
                        LiuheChessBoard lcb = _Manager._GameBoard.getCopy();
                        lcb = _Manager.getTryMovesMultiChessBoard(_HasMoveChessList, lcb);
                        List<ChessObj> dcoes = new List<ChessObj>();
                        foreach (ChessObj co in _HasMoveChessList) {
                            if (lcb.findChessByIdnum(co._ChessIdentityNumber).health<=0) {
                                co.moveToOld();
                                dcoes.Add(co);
                                allright = false;
                                //Debug.Log("应该回去");
                            }
                        }

                        if (allright)
                        {
                            //删除所有死掉的棋子
                            List<ChessObj> dd = new List<ChessObj>();
                            foreach (ChessObj co in _ChessList)
                            {
                                if (!co._IsAlive)
                                {
                                    dd.Add(co);
                                }
                            }

                            foreach (ChessObj co in dd)
                            {
                                _ChessList.Remove(co);
                                Destroy(co.gameObject);
                            }

                            _Manager.washOut();


                            //正式移动
                            foreach (ChessObj co in _HasMoveChessList) {
                                _Manager.moveChess(co);
                            }
                            _RoundState = DisplayState.MOVE_CHESS;
                            updateBoard();
                            _HasMoveChessList = new List<ChessObj>();
                            endTurn();
                        }
                        else
                        {
                            foreach (ChessObj c in dcoes) {
                                _HasMoveChessList.Remove(c);
                            }
                        }



                        #region old
                        /*
                        if (_Manager.getTryMoveBoard(_HasMovedChess))
                        {
                            //删除所有死掉的棋子
                            List<ChessObj> dd = new List<ChessObj>();
                            foreach (ChessObj co in _ChessList)
                            {
                                if (!co._IsAlive)
                                {
                                    dd.Add(co);
                                }
                            }

                            foreach (ChessObj co in dd)
                            {
                                _ChessList.Remove(co);
                                Destroy(co.gameObject);
                            }

                            _Manager.washOut();


                            //正式移动
                            _Manager.moveChess(_HasMovedChess);
                            _RoundState = DisplayState.MOVE_CHESS;
                            updateBoard();
                            _HasMovedChess = null;
                            endTurn();
                        }
                        else
                        {
                            _HasMovedChess.moveToOld();
                            _HasMovedChess = null;
                            updateBoard();
                            Debug.Log("应该回去");
                        }
                        */
                        #endregion
                    }
                    else
                    {
                        //删除所有死掉的棋子
                        List<ChessObj> dd = new List<ChessObj>();
                        foreach (ChessObj co in _ChessList)
                        {
                            if (!co._IsAlive)
                            {
                                dd.Add(co);
                            }
                        }

                        foreach (ChessObj co in dd)
                        {
                            _ChessList.Remove(co);
                            Destroy(co.gameObject);
                        }

                        _RoundState = DisplayState.MOVE_CHESS;
                        clickEmypt();
                        endTurn();
                    }
                }
                break;

            case DisplayState.DO_NOTHING:
                if (_TempChess != null) {
                    _RoundState = DisplayState.HAS_NOT_PLACE_CHESS;
                    endTurn();
                    break;
                }

                //状态转为结束游戏，然后再一次调用
                _RoundState = DisplayState.ENDTURN;
                endTurn();
                break;
            case DisplayState.ENDTURN:
                //真正结束回合
                _Manager.endTurn();
                //切换新的指示牌出来
                if (_Manager._Players[_Manager._PlayerNow]._Type == PLAYER_TYPE.LOCAL)
                {
                    tintLog2();
                    _ChessLog3Transform.DOMove(_ChessLogStart.position, 0.5f);
                    _ChessLog2Transform.DOMove(_ChessLogEnd.position, 0.5f);
                }
                //删除所有死掉的棋子
                List<ChessObj> dco = new List<ChessObj>();
                foreach (ChessObj co in _ChessList) {
                    if (!co._IsAlive) {
                        dco.Add(co);
                    }
                }

                foreach (ChessObj co in dco) {
                    _ChessList.Remove(co);
                    Destroy(co.gameObject);
                }
                relinkGrid();
                _RoundState = DisplayState.HAS_NOT_PLACE_CHESS;
                break;
            default:
                break;
        }
    }

    public ChessObj findChessObj(int idm) {
        foreach (ChessObj co in _ChessList)
        {
            if (co._ChessIdentityNumber == idm) {
                return co;
            }
        }

        return null;
    }

    public GridObj findGridObj(int xx, int yy)
    {
        foreach (GridObj go in _GridList)
        {
            if (go.x == xx && go.y == yy)
            {
                return go;
            }
        }

        return null;
    }

    private void tintLog2() {
        Image im = _ChessLog2Transform.GetComponent<Image>();
        if (_Manager._PlayerNow == 0)
        {
            im.color = _P0Tint;
        }
        else {
            im.color = _P1Tint;
        }
        if (_Manager._Players[_Manager._PlayerNow]._Type == PLAYER_TYPE.AI)
        {
            Text t = _ChessLog2Transform.Find("Text").GetComponent<Text>();
            t.text = "AI正在思考中……";
        }
        else {
            Text t = _ChessLog2Transform.Find("Text").GetComponent<Text>();
            t.text = "现在是您的回合，请下一颗棋子";
        }
    }

    public ChessObj addChess(GridObj go)
    {
        if (!_Manager.ifThisPlayerLocal())
        {
            return null;
        }

        if (!_IsStarted || (_RoundState != DisplayState.HAS_NOT_PLACE_CHESS && 
                          _RoundState != DisplayState.HAS_PLACE_CHESS && 
                          _RoundState != DisplayState.PLACE_CHESS_WRONG &&
                          _RoundState != DisplayState.DO_NOTHING))
        {
            //Debug.Log("回合状态不对");
            return null;
        }

        if (_Manager.isThisGridEmpty(go.x, go.y))
        {
            ChessObj co = generateChess(go.x, go.y, go, _Manager._PlayerNow);
            if (_TempChess != null)
            {
                Destroy(_TempChess.gameObject);
            }
            _TempChess = co;
            changeDgree(0);
            return co;
        }

        Debug.Log("底层认为格子不空");
        return null;
    }

    public void changeDgree(float dg)
    {
        if (!_Manager.ifThisPlayerLocal())
        {
            return;
        }

        if (!_IsStarted)
        {
            return;
        }

        if (_TempChess == null)
        {
            return;
        }
        _ChessLog2Transform.DOMove(_ChessLogStart.position, 0.5f);
        _ChessLog3Transform.DOMove(_ChessLogStart.position, 0.5f);


        int i = (int)(Mathf.Rad2Deg * dg);
        i = (i + 480) % 360;
        i = i / 60;

        if (i == _TempChess._Direction) {
            return;
        }

        _TempChess.rotate(i);

        List<int> oks = _Manager.getUsableDirection();

        //如果这个方向不禁手
        if (oks.Contains(i)) {
            LiuheChessBoard lcb = _Manager.tryToPlace(_TempChess.x, _TempChess.y, _TempChess._Direction);


            if (lcb == null)
            {
                Debug.Log("严重错误，不能增加棋子的格子增加棋子了！");
                return;
            }

            //更新棋子显示
            updateBoard(lcb.chesses);

            LiuheChess lc = lcb.findChessByPosition(_TempChess.x, _TempChess.y);
            if (lc == null) {
                Debug.Log("严重错误，不能增加棋子的格子增加棋子了！");
                return;
            }

            if (lc.health <= 0)
            {
                _WarningText.text = _Warning_Info[0];
                _ChessLogTransform.DOMove(_ChessLogEnd.position, 0.5f);
            }
            else
            {
                _ChessLogTransform.DOMove(_ChessLogStart.position, 0.5f);
            }
            //Debug.Log("可以下哦");
        }
        //如果这个方向禁手
        else {
            _WarningText.text = _Warning_Info[1];
            _ChessLogTransform.DOMove(_ChessLogEnd.position, 0.5f);
            _TempChess.setVoid();
            updateBoard();
            //Debug.Log("不可以下哦");
        }
    }

    public void updateBoard() {
        updateBoard(this._Manager._GameBoard.chesses);
    }

    public void updateBoardPos()
    {
        foreach (ChessObj co in _ChessList) {
            co.recalculatorTransform();
        }
    }

    public void updateBoard(List<LiuheChess> chesses)
    {

        if (_TempChess != null)
        {
            foreach (LiuheChess lc in chesses)
            {
                if (lc.isPosEqual(_TempChess.x, _TempChess.y)) {
                    _TempChess._ChessIdentityNumber = lc.identityNumber;
                    _TempChess.setDigitals(lc);
                    break;
                }
            }
        }

        foreach (ChessObj co in _ChessList) {
            foreach (LiuheChess lc in chesses) {
                if (co._ChessIdentityNumber == lc.identityNumber) {
                    co.setDigitals(lc);
                    //Debug.Log("hello");
                }
            }
        }
        

        //扩增棋子（比如AI等，会不经过addChess这样的方法去调用底层数据）
        //因此会出现这样被动新增棋子的局面，现在不写
        //do something
        //

        //do something
    }


    private void generateChessBoardGameObjs(int level)
    {
        _GridList = new List<GridObj>();
        _ChessList = new List<ChessObj>();

        for (int i = 0; i < 2 * level - 1; i++)
        {
            if (i < level)
            {
                for (int j = 0; j < level + i; j++)
                {
                    GridObj gb = generateSingleGird(i - level + 1, j - level + 1);
                    _GridList.Add(gb);
                }
            }
            else
            {
                for (int j = i - level + 1; j < 2 * level - 1; j++)
                {
                    GridObj gb = generateSingleGird(i - level + 1, j - level + 1);
                    _GridList.Add(gb);
                }
            }
        }
    }


    public void makeChess(int xx, int yy, int dir, int oo)
    {
        GridObj go = findGridObj(xx, yy);
        ChessObj co = generateChess(go.x, go.y, go, oo);
        int idm = _Manager.makeNewChessUnSafe(xx, yy, dir, oo);
        co._ChessIdentityNumber = idm;
        _ChessList.Add(co);
        go._Chess = co;
        co.rotate(dir);

        Debug.Log("生成了一颗棋子");

        return;
    }

    public void updateMakeEnds()
    {
        _Manager.updateChessBoard();
        updateBoard();
    }


    private GridObj generateSingleGird(int xx, int yy)
    {
        GridObj go = Instantiate(_GridPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        go.set(xx,yy);
        go._Father = this;
        go.transform.SetParent(_GridRoot);
        return go;
    }

    private ChessObj generateChess(int xx, int yy, GridObj go, int ownner)
    {
        ChessObj co = Instantiate(_ChessPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        co.set(xx,yy,0,go,ownner);
        co._Father = this;
        co.recalculatorTransform();
        co.transform.SetParent(_ChessRoot);
        //Debug.Log(pos.ToString());
        return co;
    }

    public void deleteAllDeadChess() {
        List<ChessObj> dd = new List<ChessObj>();
        foreach (ChessObj co in _ChessList)
        {
            if (co._Health <= 0)
            {
                dd.Add(co);
            }
        }

        foreach (ChessObj co in dd)
        {
            _ChessList.Remove(co);
            Destroy(co.gameObject);
        }
    }

    public void placeChessUnsafeAI(int xx, int yy, int dir, int ownner) {
        GridObj go = findGridObj(xx, yy);
        ChessObj co = generateChess(go.x, go.y, go, ownner);
        int idm = _Manager.makeNewChessUnSafe(xx, yy, dir, ownner);
        co._ChessIdentityNumber = idm;
        _ChessList.Add(co);
        go._Chess = co;
        co.rotate(dir);
        GameManager._Main.updateChessBoard();
        updateBoard();
        Debug.Log("生成了一颗棋子");
    }

    public IEnumerator endTurnAI() {
        GameManager._Main.endTurn();
        Display._Main.readyToPlace();
        yield return new WaitForSeconds(0.5f);
    }

    public void clearDisDir() {
        _Manager._GameBoard.places = new List<PlaceMemory>();
    }

    public IEnumerator killAChessAndMove(int xx, int yy) {
        _Manager.updateChessBoard();
        updateBoard();

        int idm = GameManager._Main._GameBoard.findChessByPosition(xx, yy).identityNumber;
        ChessObj cc = findChessObj(idm);
        ChessObj ck = null;
        foreach (ChessObj co in _ChessList)
        {
            if (co._Health <= 0) {
                co.kill();
                ck = co;
                break;
            }
        }

		if (ck != null) {

			yield return new WaitForSeconds (0.5f);

			cc.aiMove (ck.GetComponent<Transform> ().position, ck);
			GameManager._Main.washOut ();
			if (_Manager.moveChessAI (cc)) {
				//Debug.Log("fine");
			} else {
				Debug.Log ("老兄你又写错了");
			}
			_ChessList.Remove (ck);
			Destroy (ck.gameObject);
			yield return new WaitForSeconds (0.5f);

			relinkGrid ();
		}
    }
}
