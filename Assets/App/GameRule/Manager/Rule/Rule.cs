using System.Collections;
using System.Collections.Generic;

public class Rule
{
    // 当前回合可用方向(禁手)
    public static List<int> getUsableDirections(GameBoard board)
    {
        List<int> li = new List<int>();
        List<int> pastdir = board.getPastDir();
        //如果之前有下过棋(没有棋子就没有禁手了)
        //把最后一个方向加入列表
        if (pastdir.Count > 0)
        {
            li.Add(pastdir[pastdir.Count - 1]);
            pastdir.RemoveAt(pastdir.Count - 1);
        }
        //重复刚才的操作
        if (pastdir.Count > 0)
        {
            li.Add(pastdir[pastdir.Count - 1]);
            pastdir.RemoveAt(pastdir.Count - 1);
        }
        //li里面是不许下的方向了，现在建立一个6方向的列表d
        List<int> d = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            d.Add(i);
        }
        //去掉li里面的方向
        foreach (int lll in li)
        {
            d.Remove(lll);
        }


        return d;
    }


    // 给定棋盘更新一次
    public static void updateBoard(GameBoard board)
    {
        //更新技能列表
        Judgement.updateSkillList(board);
        //更新buff列表
        Judgement.updateBuffList(board);
        //calculator计算buff结果
        foreach(GameChess gc in board._GameChessList ){
            Calculator.applyBuff(gc);
        }
    }


    public static void clearHealthLowChess(GameBoard board) {
        List<GameChess> gsc = board.getGameChessListCopy();
        foreach (GameChess gc in gsc) {
            if (gc._Health <= 0) {
                Judgement.addMoveSkillFrom(gc, board);
                board.removeChess(gc.getPosition());
            }
        }

    }

    // 格子合法性判断
    public static bool isGridValid(GameBoard board, Vector3i position)
    {
        //判断这一回合有没有下过棋子
        if (board.getThisTurnChessNumber() > 0)
        {
            return false;
        }
        //检测这个位置上有没有棋子
        else if (board.getGameChessRef(position) == null)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public static void addNewSkill(EffectType type, Vector3i pos, GameBoard board) {
        if (type == EffectType.move) {
            Judgement.addMoveSkillFrom(board.getGameChessRef(pos),board);
        }
    }

    public static void removeNewSkill(EffectType type, Vector3i pos, GameBoard board) {
        if (type == EffectType.move)
        {
            Judgement.moveSkillRemove(pos, board);
        }
    }

    public static void clearAllMove(GameBoard board) {
        foreach (GameChess gc in board.getGameChessListCopy()) {
            GameChess realChess = board.getGameChessRef(gc.getPosition());
            Judgement.wipeMoveSkill(realChess);
        }
    }

}
