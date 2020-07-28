using System;
using System.Collections;
using System.Collections.Generic;

public class VersionControl{
    
    private class DiffInfo {
        public Chess _Chess;
        public bool _Alive;

        public DiffInfo(Chess cs, bool live) {
            _Chess = cs;
            _Alive = live;
        }
    }
    
    private class Action {
        public List<DiffInfo> _Diffs;
        public int _PlayerID;
        public Action _PreAction;
        public Action _NextAction;

        public Action() {
            _Diffs = new List<DiffInfo>();
            _NextAction = null;
            _PreAction = null;
        }

        public Action(DiffInfo[] diffs, int id, Action pre)
        {
            _Diffs = new List<DiffInfo>(diffs);
            _PlayerID = id;
            _NextAction = null;
            _PreAction = pre;
        }
    }

    private Action _HeadAction;
    private Action _TailAction;

    private List<Chess> _ChessesMaster;

    private List<Chess> _ChessesWork;

    public VersionControl(){
        _HeadAction = new Action();
        _TailAction = _HeadAction;
        _ChessesMaster = new List<Chess>();
        _ChessesWork = new List<Chess>();
    }

    private void revertWorkToMaster() {
        _ChessesWork = new List<Chess>(_ChessesMaster.ToArray());
    }

    private void mergeWorkToMaster()
    {
        _ChessesMaster = new List<Chess>(_ChessesWork.ToArray());
    }

    public List<Chess> getMaster() {
        return new List<Chess>(_ChessesMaster.ToArray());
    }


    public bool push(Chess chess, bool isAlive) {
        //判断push操作是否成功
        bool success = false;

        //如果是新增棋子
        if (isAlive)
        {
            _ChessesWork.Add(chess.getCopy());
            success = true;
        }
        //如果是去掉棋子-->找不到这个棋子就算错误
        else {
            foreach(Chess cs in _ChessesWork){
                if (cs.isEqual(chess)) {
                    _ChessesWork.Remove(cs);
                    success = true;
                    break;
                }
            }
        }

        return success;
    }


    public bool commit(int id) {
        //检测唯一性
        if (!checkUnique(_ChessesWork))
        {
            Console.WriteLine("Error-提交版本控制器时存在重复位置的棋子");
            revertWorkToMaster();
            return false;
        }

        //获取区别列表
        DiffInfo[] diffs = diff(_ChessesWork,_ChessesMaster);
        //更新动作列表
        addNewAction(diffs,id);
        //更新主分支
        mergeWorkToMaster();
        return true;
    }

    //回滚到上一步
    public bool rollBack() {
        if (_TailAction == _HeadAction) {
            return false;
        }
        //获取上一步操作，并且从动作列表中去掉那一步操作
        Action act = _TailAction;
        _TailAction = _TailAction._PreAction;

        //为了安全，在work中操作

        revertWorkToMaster();

        foreach (DiffInfo ds in act._Diffs) {

            bool success;
            
            //如果是新下的一个棋，则把这个棋拿走
            if (ds._Alive)
            {
                success = false;
                foreach (Chess cs in _ChessesWork) {
                    if (cs.isEqual(ds._Chess)) {
                        success = true;
                        _ChessesWork.Remove(cs);
                        Console.WriteLine("去掉了一颗棋");
                        break;
                    }
                }
                if (!success) {
                    Console.WriteLine("Error-这条错误不应该出现，如果出现证明VersionControl的回滚功能有错");
                    revertWorkToMaster();
                    return false;
                }
            }
            //如果是去掉一个棋，则把这个棋加回来
            else {
                _ChessesWork.Add(ds._Chess);
                Console.WriteLine("补回了一颗棋");
            }

            Console.WriteLine("回退信息-->上一步" + ds._Chess._GridPos+ "  状态 "+ds._Alive);
        }



        if (!checkUnique(_ChessesWork))
        {
            Console.WriteLine("Error-这条错误不应该出现，如果出现证明VersionControl的回滚唯一性有问题");
            printAllChess(_ChessesWork);
            revertWorkToMaster();
            return false;
        }
        else {
            mergeWorkToMaster();
        }

        return true;
    }


    private void printAllChess(List<Chess> work) { 
        foreach(Chess cs in work){
            Console.WriteLine(cs._GridPos);
        }
    
    }


    //检测唯一性
    private bool checkUnique(List<Chess> work) {
        int count = work.Count;
        //工作目录下的棋子两两检测是否重复
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = i + 1; j < count; j++)
            {
                if (work[i].isEqual(work[j]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    //插入新的动作到动作列表的尾部
    private void addNewAction(DiffInfo[] diffs, int id) {
        Action newAction = new Action(diffs, id, _TailAction);
        _TailAction._NextAction = newAction;
        _TailAction = newAction;
    }


    //计算出区别列表
    private DiffInfo[] diff(List<Chess> current, List<Chess> stable) {
        List<DiffInfo> dfs = new List<DiffInfo>();
        foreach (Chess csc in current) {
            bool existOneInStable = false;
            foreach (Chess css in stable) {
                if (css.isEqual(csc)) {
                    existOneInStable = true;
                    break;
                }
            }
            //如果stable中不存在，则加到区别列表中；这是一个加操作
            if (!existOneInStable) {
                dfs.Add(new DiffInfo(csc.getCopy(),true));
            }
        }

        foreach (Chess css in stable) {
            bool existOneInCurrent = false;
            foreach (Chess csc in current) {
                if (csc.isEqual(css)) {
                    existOneInCurrent = true;
                    break;
                }
            }
            //如果stable中存在，而current不存在，则加到区别列表中；这是一个减操作
            if (!existOneInCurrent) {
                dfs.Add(new DiffInfo(css.getCopy(),false));
            }
        }


        return dfs.ToArray();
    }
    
}
