using System.Collections;
using System.Collections.Generic;

public class Calculator
{
    public static void applyBuff(GameChess gameChessRef) {
        List<Buff> bufflist = gameChessRef._BuffList;

        List<Buff> temp;
        //基础值刷一遍
        temp = getAllBuffBtType(EffectType.basic, bufflist);
        foreach (Buff bf in temp) {
            //support这两个实际上是计算buff时候用的，现在没有实际意义了，姑且更新一下
            gameChessRef._BasicSupport = 1;
            gameChessRef._Support = 1;
            //由于生命力不影响支援效果，所以就没有基础值的设定（但是考虑到显示，也许以后会出现；解决方法参照attack）
            gameChessRef._BasicAttack = bf._AttackChange;
            gameChessRef._Health = bf._HealthChange;
            gameChessRef._Absorb = 0;
            gameChessRef._Attack = 0;
        }
        //thron加攻击
        temp = getAllBuffBtType(EffectType.thron, bufflist);
        foreach (Buff bf in temp) {
            gameChessRef._BasicAttack += bf._AttackChange;
        }
        //算出自己被减少后的攻击，或者加强吸收力
        temp = getAllBuffBtType(EffectType.kiss, bufflist);
        foreach (Buff bf in temp)
        {
            gameChessRef._BasicAttack += bf._AttackChange;
            gameChessRef._Absorb += bf._AbsorbChange;
        }
        //算出环加成
        temp = getAllBuffBtType(EffectType.ring, bufflist);
        foreach (Buff bf in temp)
        {
            gameChessRef._Health += bf._HealthChange;
        }
        //把基础攻击力加到攻击力上
        gameChessRef._Attack += gameChessRef._BasicAttack;

        //算出支援加成的攻击力和生命值
        temp = getAllBuffBtType(EffectType.support, bufflist);
        foreach (Buff bf in temp)
        {
            gameChessRef._Health += bf._HealthChange;
            gameChessRef._Attack += bf._AttackChange;
        }

        //算出生命被减少之后的结果
        temp = getAllBuffBtType(EffectType.attack, bufflist);
        foreach (Buff bf in temp)
        {
            if(bf._HealthChange<0)
            {
                gameChessRef._Health += bf._HealthChange;
            }
        }
    }

    private static List<Buff> getAllBuffBtType(EffectType ty, List<Buff> bflist) {
        List<Buff> bfs;
        bfs = new List<Buff>();
        foreach(Buff bf in bflist){
            if (bf.type == ty) {
                bfs.Add(bf);
            }
        }


        return bfs;
    }
}
