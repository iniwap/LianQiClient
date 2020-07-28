using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Liuhe.Manager;
using Liuhe.Model;
using Liuhe.Rules;

using UnityEngine.Assertions;

public class AI_Manager : MonoBehaviour {
    public static AI_Manager _Main;
    private IEnumerator _Process = null;
    private bool _HasEnded;
    private PlaceMethod _PM;

	// Use this for initialization
	void Start () {
        if (_Main != null)
        {
            Destroy(this.gameObject);
        }
        else {
            _Main = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void doPlace() {
        Display dp = Display._Main;

        if (_PM == null)
        {
            Debug.Log("没有准备好！！");
        }
        foreach (PlaceAction pa in _PM._Actions) {
            switch (pa.type)
            {
                case PLACE_TYPE.NONE:
                    //Debug.Log("什么也不做");
                    break;
                case PLACE_TYPE.ADD:
                    //Debug.Log("新增棋子！");
                    dp.makeChess(pa.x,pa.y,pa.direction,pa.ownner);
                    GameManager._Main.updateChessBoard();
                    dp.updateBoard();

                    List<ChessObj> dd = new List<ChessObj>();
                    foreach (ChessObj co in dp._ChessList)
                    {
                        if (co._Health <=0)
                        {
                            dd.Add(co);
                        }
                    }

                    foreach (ChessObj co in dd)
                    {
                        co.kill();
                    }


                    break;
                case PLACE_TYPE.MOVE:
                    //Debug.Log("什么也不做-->现在还没有写移动");
                    break;
                default:
                    break;
            }
        }
    }

    public IEnumerator getResult() {
        _PM = null;
        _HasEnded = false;
        //Debug.Log(Time.time);
        //Debug.Log("I'm thinking...");
        //选择评估算法
        _Process = getValueMultiLayerPurge(2,0.9f);

        StartCoroutine(_Process);

        for (int i = 0; i < 15; i++) {
            if (_HasEnded) {
                Debug.Log("结果已经算出  --  "+(0.5+i*0.5f)+"s以内");
                break;
            }
            else{
                Debug.Log("经过时间+"+i*0.5f+"s");
                yield return new WaitForSeconds(0.5f);
            }
        }


        StopCoroutine(_Process);

        if (_HasEnded)
        {
            Debug.Log(_PM);
            doPlace();
            Debug.Log("实现了优质算法");
        }
        else {
            Debug.Log("只够最简算法");
        }
    }



    private IEnumerator printResulr()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);
        }
        _PM = new PlaceMethod().setNone();
        _HasEnded = true;
        Debug.Log("finfished");
    }

    private IEnumerator randomChoose() {
        PlaceMethod pm = PlaceMethodGenerator.getRandomPlaceMethod();
        _PM = pm;
        //Debug.Log(pm);
        _HasEnded = true;
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator getMinValueOnce() {
        _PM = TreeSelector.getMinPlaceMethod_OL();

        _HasEnded = true;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator getValueMultiLayer(int layer)
    { 
        _PM = TreeSelector.getBestMethodMultiLayer(layer);

        _HasEnded = true;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator getValueMultiLayerPurge(int layer, float purgeRate)
    {
        _PM = TreeSelector.getBestMethodMultiLayerPurge(layer, purgeRate);

        _HasEnded = true;

        yield return new WaitForEndOfFrame();
    }

}



public enum PLACE_TYPE
{
    NONE,
    ADD,
    MOVE,
}

public class PlaceAction
{
    public PLACE_TYPE type;
    public int x;
    public int y;
    public int direction;
    public int ownner;

    public PlaceAction(PLACE_TYPE pt, int xx, int yy, int dir, int oo) {
        this.type = pt;
        this.x = xx;
        this.y = yy;
        this.direction = dir;
        this.ownner = oo;
    }

    public PlaceAction()
    {
        this.type =  PLACE_TYPE.NONE;
        this.x = -1;
        this.y = -1;
        this.direction = -1;
        this.ownner = -1;
    }

    public LiuheChessBoard isValid(LiuheChessBoard lcb)
    {
        int idm = lcb.addNewChessUnSafe(this.x, this.y, this.direction, this.ownner);
        Rules.GameBoardCalculateItself(lcb);

        LiuheChess lc = lcb.findChessByIdnum(idm);

        Assert.IsNotNull(lc, "找到了空棋子，ai下子有问题");

        if (lc.health <= 0)
        {
            return null;
        }
        else
        {
            return lcb;
        }
    }
}

public class PlaceMethod {
    public List<PlaceAction> _Actions;

    public PlaceMethod() {
        _Actions = new List<PlaceAction>();
    }

    public PlaceMethod addAction(PLACE_TYPE pt, int xx, int yy, int dir, int ownner) {
        _Actions.Add(new PlaceAction(pt, xx, yy, dir, ownner));
        return this;
    }

    public PlaceMethod setNone() {
        _Actions = new List<PlaceAction>();
        _Actions.Add(new PlaceAction());
        return this;
    }

    public bool isValid(LiuheChessBoard lcb) {
        if (_Actions.Count == 1 && _Actions[0].type == PLACE_TYPE.NONE) {
            return true;
        }

        LiuheChessBoard lcb2 = lcb.getCopy();
        foreach (PlaceAction pi in _Actions)
        {
            lcb2 = pi.isValid(lcb2);
            if (lcb2 == null) {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        string st = "";

        foreach (PlaceAction pa in _Actions) {
            switch (pa.type) {
                case PLACE_TYPE.NONE: st += "什么也不做\n"; break;
                case PLACE_TYPE.MOVE: st += "移动棋子 (" + pa.x + ", " + pa.y + ", " + pa.direction + ")\n"; break;
                case PLACE_TYPE.ADD: st += "增加棋子 (" + pa.x + ", " + pa.y + ", " + pa.direction + ") "+pa.ownner+"\n"; break;
                default:break;
            }
        }

        return st;
    }
}


public class PlaceMethodGenerator {
    public static LiuheChessBoard getPlacedChessBoard(PlaceMethod _PM, LiuheChessBoard lcb) {
        if (_PM == null)
        {
            Debug.Log("没有准备好！！");
        }
        foreach (PlaceAction pa in _PM._Actions)
        {
            switch (pa.type)
            {
                case PLACE_TYPE.NONE:
                    //Debug.Log("什么也不做");
                    break;
                case PLACE_TYPE.ADD:
                    //Debug.Log("新增棋子！");
                    lcb.addNewChessUnSafe(pa.x, pa.y, pa.direction, pa.ownner);
                    break;
                case PLACE_TYPE.MOVE:
                    //Debug.Log("什么也不做-->现在还没有写移动");
                    break;
                default:
                    break;
            }
        }

        return lcb;
    }


    public static PlaceMethod getRandomPlaceMethod() {
        List<PlaceMethod> pms = new List<PlaceMethod>();

        //预制不下棋到选择里面去
        pms.Add(new PlaceMethod().setNone());

        int ownner,gridLevel;
        ownner = GameManager._Main._PlayerNow;
        gridLevel = GameManager._Main._BoardLevel;

        LiuheChessBoard lcb = GameManager._Main._GameBoard.getCopy();
        List<int> udrs = Rules.getUsableDirection(lcb);

        for (int i = 0; i < 2 * gridLevel - 1; i++)
        {
            //上半截
            if (i < gridLevel)
            {
                for (int j = 0; j < i + gridLevel; j++)
                {
                    int xx, yy;
                    xx = i - gridLevel + 1;
                    yy = j - gridLevel + 1;

                    GridValidState gvs = Rules.getGridValidState(lcb, xx, yy);
                    //如果此网格可以用
                    if (gvs == GridValidState.VOID)
                    {
                        for (int k = 0; k < udrs.Count; k++)
                        {
                            pms.Add(new PlaceMethod().addAction(PLACE_TYPE.ADD, xx, yy, udrs[k], ownner));
                        }
                    }
                }
            }
            //下半截
            else
            {

                for (int j = i - gridLevel + 1; j < 2 * gridLevel - 1; j++)
                {

                    int xx, yy;
                    xx = i - gridLevel + 1;
                    yy = j - gridLevel + 1;
                    GridValidState gvs = Rules.getGridValidState(lcb, xx, yy);
                    //如果此网格可以用
                    if (gvs == GridValidState.VOID)
                    {
                        for (int k = 0; k < udrs.Count; k++)
                        {
                            pms.Add(new PlaceMethod().addAction(PLACE_TYPE.ADD, xx, yy, udrs[k], ownner));
                        }
                    }
                }
            }
        }


        while (pms.Count > 0) {
            PlaceMethod pm;
            int rand;
            rand = (int)Random.Range(0, pms.Count);
            pm = pms[rand];
            if (pm.isValid(lcb))
            {
                return pm;
            }
            else {
                pms.Remove(pm);
            }
        }

        Debug.Log("不可能啊！");
        return null;
    }

    public static List<PlaceMethod> getAllValidPlaceMethod(LiuheChessBoard lcbIn) {
        List<PlaceMethod> pms = new List<PlaceMethod>();

        //预制不下棋到选择里面去
        pms.Add(new PlaceMethod().setNone());

        int ownner, gridLevel;
        ownner = GameManager._Main._PlayerNow;
        gridLevel = GameManager._Main._BoardLevel;

        LiuheChessBoard lcb = lcbIn;
        List<int> udrs = Rules.getUsableDirection(lcb);

        for (int i = 0; i < 2 * gridLevel - 1; i++)
        {
            //上半截
            if (i < gridLevel)
            {
                for (int j = 0; j < i + gridLevel; j++)
                {
                    int xx, yy;
                    xx = i - gridLevel + 1;
                    yy = j - gridLevel + 1;

                    GridValidState gvs = Rules.getGridValidState(lcb, xx, yy);
                    //如果此网格可以用
                    if (gvs == GridValidState.VOID)
                    {
                        for (int k = 0; k < udrs.Count; k++)
                        {
                            pms.Add(new PlaceMethod().addAction(PLACE_TYPE.ADD, xx, yy, udrs[k], ownner));
                        }
                    }
                }
            }
            //下半截
            else
            {

                for (int j = i - gridLevel + 1; j < 2 * gridLevel - 1; j++)
                {

                    int xx, yy;
                    xx = i - gridLevel + 1;
                    yy = j - gridLevel + 1;
                    GridValidState gvs = Rules.getGridValidState(lcb, xx, yy);
                    //如果此网格可以用
                    if (gvs == GridValidState.VOID)
                    {
                        for (int k = 0; k < udrs.Count; k++)
                        {
                            pms.Add(new PlaceMethod().addAction(PLACE_TYPE.ADD, xx, yy, udrs[k], ownner));
                        }
                    }
                }
            }
        }


        List<PlaceMethod> pms2 = new List<PlaceMethod>();

        foreach (PlaceMethod pm in pms)
        {
            if (pm.isValid(lcb)) {
                pms2.Add(pm);
            }
        }

        return pms2;
    }

    public static List<PlaceMethod> getPurchedMethods(LiuheChessBoard lcb, float purchRate, bool isAI) {
        List<PlaceMethod> pms = getAllValidPlaceMethod(lcb);
        int purchNum = (int)(pms.Count * purchRate);
        if (purchNum == pms.Count) {
            purchNum--;
        }

        List<int> results = new List<int>();
        foreach (PlaceMethod p in pms)
        {
            LiuheChessBoard lcb2 = PlaceMethodGenerator.getPlacedChessBoard(p, lcb.getCopy());
            results.Add(GameBoardEvaluator.evaluateGameBoard_MK0(lcb2));
        }

        if (isAI)
        {
            //是ai，从小到大
            for (int i = 0; i < pms.Count; i++)
            {
                for (int j = i; j < pms.Count; j++)
                {
                    if (results[i] > results[j]) {
                        int temp = results[i];
                        results[i] = results[j];
                        results[j] = temp;
                        PlaceMethod pm = pms[i];
                        pms[i] = pms[j];
                        pms[j] = pm;
                    }
                }
            }

        }
        else
        {
            //是人，从大到小
            for (int i = 0; i < pms.Count; i++)
            {
                for (int j = i; j < pms.Count; j++)
                {
                    if (results[i] < results[j])
                    {
                        int temp = results[i];
                        results[i] = results[j];
                        results[j] = temp;
                        PlaceMethod pm = pms[i];
                        pms[i] = pms[j];
                        pms[j] = pm;
                    }
                }
            }
        }

        for (int i = 0; i < purchNum; i++)
        {
            pms.RemoveAt(pms.Count - 1);
        }

        return pms;
    }
}

public class GameBoardEvaluator {
    
    public static int evaluateGameBoard_MK0(LiuheChessBoard lcb)
    {
        int result = 0;
        Rules.GameBoardCalculateItself(lcb);
        List<LiuheChess> chesses = lcb.chesses;
        foreach (LiuheChess lc in chesses)
        {
            int thisone, zhengfu;
            thisone = 3;
            zhengfu = 1;
            //对于0玩家，分数是增加的，1玩家分数是减少的
            zhengfu = lc.ownner == 0 ? 1 : -1;

            thisone += lc.hasSuchBuff(BufferType.THRON) ? 3 : 0;
            thisone += lc.hasSuchBuff(BufferType.RING) ? 3 : 0;
            thisone += lc.hasSuchBuff(BufferType.SUPPORT) ? 1 : 0;
            thisone += lc.hasSuchBuff(BufferType.ATTACK) ? -1 : 0;

            if (lc.health <= 0)
            {
                thisone -= 6;
            }
            result += thisone * zhengfu;
        }

        return result;
    }

}



public class TreeSelector {
    public static PlaceMethod getMinPlaceMethod_OL() {
        List<PlaceMethod> pms = PlaceMethodGenerator.getAllValidPlaceMethod(GameManager._Main._GameBoard.getCopy());
        LiuheChessBoard lcb2 = GameManager._Main._GameBoard.getCopy();

        LiuheChessBoard lcbTemp = PlaceMethodGenerator.getPlacedChessBoard(pms[0], lcb2.getCopy());

        int minNum = 0;
        int minValue = GameBoardEvaluator.evaluateGameBoard_MK0(lcbTemp);
        List<PlaceMethod> _NPM = new List<PlaceMethod>();
        _NPM.Add(pms[0]);
        for (int i = 1; i < pms.Count; i++)
        {
            int temp;
            lcbTemp = PlaceMethodGenerator.getPlacedChessBoard(pms[i], lcb2.getCopy());
            temp = GameBoardEvaluator.evaluateGameBoard_MK0(lcbTemp);

            //Debug.Log("评分为"+temp);

            if (temp < minValue)
            {
                minNum = i;
                minValue = temp;
                _NPM = new List<PlaceMethod>();
                _NPM.Add(pms[i]);
            }
            else if (temp == minValue) {
                _NPM.Add(pms[i]);
            }
        }

        int iend = (int)Random.Range(0, _NPM.Count);

        return _NPM[iend];
    }


    public static PlaceMethod getBestMethodMultiLayer(int layer) {
        LiuheChessBoard lcb = GameManager._Main._GameBoard;
        List<PlaceMethod> pms = PlaceMethodGenerator.getAllValidPlaceMethod(lcb);
        List<int> results = new List<int>();
        foreach (PlaceMethod p in pms)
        {
            results.Add(getBestResult(lcb.getCopy(), p, layer - 1, false));
        }
        int min = 0;
        int minvalue = results[0];
        for (int i = 1; i < results.Count; i++)
        {
            if (results[i] < minvalue)
            {
                minvalue = results[i];
                min = i;
            }
        }
        return pms[min];
    }


    public static int getBestResult(LiuheChessBoard lcb, PlaceMethod pm, int layer, bool isAI) {
        if (layer <= 0)
        {
            LiuheChessBoard lcbTemp = PlaceMethodGenerator.getPlacedChessBoard(pm, lcb);
            return GameBoardEvaluator.evaluateGameBoard_MK0(lcbTemp);
        }
        else {
            LiuheChessBoard lcbTemp = PlaceMethodGenerator.getPlacedChessBoard(pm, lcb);
            List<PlaceMethod> pms = PlaceMethodGenerator.getAllValidPlaceMethod(lcbTemp);
            List<int> results = new List<int>();
            foreach (PlaceMethod p in pms)
            {
                results.Add(getBestResult(lcbTemp.getCopy(), p, layer - 1, !isAI));
            }

            if (isAI)
            {
                int minvalue = results[0];
                for (int i = 1; i < results.Count; i++)
                {
                    if (results[i] < minvalue) {
                        minvalue = results[i];
                    }
                }
                return minvalue;
            }
            else
            {
                int maxvalue = results[0];
                for (int i = 1; i < results.Count; i++)
                {
                    if (results[i] > maxvalue)
                    {
                        maxvalue = results[i];
                    }
                }
                return maxvalue;
            }
        }
    }


    #region 剪枝的

    public static PlaceMethod getBestMethodMultiLayerPurge(int layer, float purge)
    {
        LiuheChessBoard lcb = GameManager._Main._GameBoard;
        List<PlaceMethod> pms = PlaceMethodGenerator.getPurchedMethods(lcb, purge, true);
        List<int> results = new List<int>();
        foreach (PlaceMethod p in pms)
        {
            results.Add(getBestResultPurge(lcb.getCopy(), p, layer - 1, false,purge));
        }
        int min = 0;
        int minvalue = results[0];
        for (int i = 1; i < results.Count; i++)
        {
            if (results[i] < minvalue)
            {
                minvalue = results[i];
                min = i;
            }
        }
        return pms[min];
    }

    public static int getBestResultPurge(LiuheChessBoard lcb, PlaceMethod pm, int layer, bool isAI, float purgeRate)
    {
        if (layer <= 0)
        {
            LiuheChessBoard lcbTemp = PlaceMethodGenerator.getPlacedChessBoard(pm, lcb);
            return GameBoardEvaluator.evaluateGameBoard_MK0(lcbTemp);
        }
        else
        {
            LiuheChessBoard lcbTemp = PlaceMethodGenerator.getPlacedChessBoard(pm, lcb);

            List<PlaceMethod> pms = PlaceMethodGenerator.getPurchedMethods(lcb, purgeRate, isAI);
            List<int> results = new List<int>();
            foreach (PlaceMethod p in pms)
            {
                results.Add(getBestResult(lcbTemp.getCopy(), p, layer - 1, !isAI));
            }

            if (isAI)
            {
                int minvalue = results[0];
                for (int i = 1; i < results.Count; i++)
                {
                    if (results[i] < minvalue)
                    {
                        minvalue = results[i];
                    }
                }
                return minvalue;
            }
            else
            {
                int maxvalue = results[0];
                for (int i = 1; i < results.Count; i++)
                {
                    if (results[i] > maxvalue)
                    {
                        maxvalue = results[i];
                    }
                }
                return maxvalue;
            }
        }
    }

    #endregion

}