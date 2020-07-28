using System.Collections;
using System.Collections.Generic;
using System;

//temp
using UnityEngine;

public class Judgement
{
    public static void updateSkillList(GameBoard boardRef)
    {
        if (boardRef == null)
        {
            return;
        }
        // 逐子更新
        foreach (GameChess chess in boardRef._GameChessList)
        {
            Vector3i chessPos = chess.getPosition();
            // pipeline
            // basic skill
            // 血量根据前后两格情况

            Judgement.wipeExceptSpecialSkill(chess);

            chess.addSkill(checkBasic(chessPos, boardRef));

            // Attck / Support
            chess.addSkill(checkSupportAndAttack(chessPos, boardRef));

            // special skill
            // Kiss / Thron / Ring
            chess.addSkill(checkKiss(chessPos, boardRef));
            chess.addSkill(checkThron(chessPos, boardRef));
            chess.addSkill(checkRing(chessPos, boardRef));

            // player skill
            // in the future
            // checkPlayerSkill(chessPos, boardRef);
        }
    }

    // li
    public static void updateBuffList(GameBoard boardRef)
    {
        if (boardRef == null) {
            return;
        }

        //清空buff列表
        foreach (GameChess gc in boardRef._GameChessList)
        {
            gc._BuffList = new List<Buff>();
        }

        //算出所有基础数值
        foreach (GameChess gc in boardRef._GameChessList)
        {
            //basic skill
            foreach (Skill sk in gc._SkillList)
            {
                if (sk.type == EffectType.basic)
                {
                    gc._BasicAttack = 1;
                    gc._BasicSupport = 1;
                    gc._Health = sk._HealthChange;
                    //gc._SkillList.Remove(sk);
                    Buff bf = new Buff(EffectType.basic);
                    bf._AttackChange = 1;
                    bf._HealthChange = gc._Health;
                    gc._BuffList.Add(bf);
                    break;
                }
            }

            //thron skill
            foreach (Skill sk in gc._SkillList)
            {
                if (sk.type == EffectType.thron)
                {
                    gc._BasicAttack = 3;
                    gc._BasicSupport = 1;
                    //gc._SkillList.Remove(sk);
                    Buff bf = new Buff(EffectType.thron);
                    bf._AttackChange = 2;
                    gc._BuffList.Add(bf);
                    break;
                }
            }

            //ring skill
            foreach (Skill sk in gc._SkillList)
            {
                if (sk.type == EffectType.ring)
                {
                    gc._Health += sk._HealthChange;
                    //gc._SkillList.Remove(sk);
                    Buff bf = new Buff(EffectType.ring);
                    bf._HealthChange = sk._HealthChange;
                    gc._BuffList.Add(bf);
                    break;
                }
            }

        }


        //算出所有对别人攻击减少
        foreach (GameChess gc in boardRef._GameChessList)
        {
            foreach (Skill sk in gc._SkillList)
            {
                if (sk.type == EffectType.kiss)
                {
                    gc._Absorb = 2;
                    //gc._SkillList.Remove(sk);
                    //添加自己吸收力增加的buff
                    Buff bf = new Buff(EffectType.kiss);
                    bf._AbsorbChange = 2;
                    gc._BuffList.Add(bf);
                    //为敌人增加减少攻击的buff

                    Vector3i chessPos = gc.getPosition();
                    int chessDirection = gc.getDirection();
                    //fChess是这个棋子背后的棋子
                    GameChess fChess = boardRef.getPointedGameChessRef(chessPos, (chessDirection + 3) % 6);

                    if (fChess != null && !isFriend(gc, fChess))
                    {
                        fChess._BasicAttack -= 2;
                        bf = new Buff(EffectType.kiss);
                        bf._AttackChange = -2;
                        fChess._BuffList.Add(bf);
                    }
                    break;
                }
            }
        }

        foreach (GameChess gc in boardRef._GameChessList)
        {
            //攻击力赋初值
            gc._Attack = gc._BasicAttack;
        }

        //算出所有对别人的支持
        foreach (GameChess gc in boardRef._GameChessList)
        {
            List<Skill> ns = new List<Skill>(gc._SkillList.ToArray());


            foreach (Skill sk in ns)
            {
                if (sk.type == EffectType.support)
                {
                    //找到指向的对象
                    Vector3i chessPos = gc.getPosition();
                    int chessDirection = gc.getDirection();

                    //fChess是这个棋子指向的棋子
                    GameChess fChess = boardRef.getPointedGameChessRef(chessPos, chessDirection);
                    if (fChess == null)
                    {
                        Debug.Log("error");
                        break;
                    }
                    Buff bf = new Buff(EffectType.support);

                    if (gc._BasicAttack > 0)
                    {
                        fChess._Attack += gc._BasicAttack;
                        bf._AttackChange = gc._BasicAttack;
                    }

                    if (gc._BasicSupport > 0)
                    {
                        fChess._Health += gc._BasicSupport;
                        bf._HealthChange += gc._BasicSupport;
                    }

                    fChess._BuffList.Add(bf);

                    //gc._SkillList.Remove(sk);
                    continue;
                }
            }
        }


        //算出所有对别人的攻击
        foreach (GameChess gc in boardRef._GameChessList)
        {
            List<Skill> ns = new List<Skill>(gc._SkillList.ToArray());

            foreach (Skill sk in ns)
            {
                if (sk.type == EffectType.attack)
                {
                    //找到指向的对象
                    Vector3i chessPos = gc.getPosition();
                    int chessDirection = gc.getDirection();

                    //fChess是这个棋子指向的棋子
                    GameChess fChess = boardRef.getPointedGameChessRef(chessPos, chessDirection);
                    if (fChess == null)
                    {
                        Debug.Log("error2");
                        break;
                    }
                    Buff bf = new Buff(EffectType.attack);

                    bf._HealthChange = -1 * gc._Attack;

                    fChess._BuffList.Add(bf);

                    gc._SkillList.Remove(sk);

                    continue;
                }
            }
        }
    }

    // GameChess forwadChess = boardRef.getPointedGameChessCopy(position, chess.getDirection());

    // GameChess backwardChess = boardRef.getPointedGameChessCopy(position, (chess.getDirection() + 3) % 6);

    // check skill
    // li--ok
    private static Skill checkKiss(Vector3i position, GameBoard boardRef)
    {
        GameChess chess = boardRef.getGameChessRef(position);
        Vector3i chessPos = chess.getPosition();
        int chessDirection = chess.getDirection();
        Skill skill = null;
        GameChess fChess = boardRef.getPointedGameChessRef(position, chessDirection);

        if (isFriend(chess, fChess))
        {
            Vector3i fChessPos = fChess.getPosition();
            int fChessDirection = fChess.getDirection();
            // 当前Chess指向对象所指向的位置
            Vector3i fPointedPos = boardRef.getPointedPosition(fChessPos, fChessDirection);
            if (fPointedPos == chessPos)
            {
                Vector3i pos = boardRef.getPointedPosition(chessPos, (chessDirection + 3) % 6);
                skill = new Skill(EffectType.kiss, pos,pos);
                skill._AttackChange = -2;
            }
        }
        return skill;
    }

    // li--ok
    private static Skill checkRing(Vector3i position, GameBoard boardRef)
    {
        GameChess chess = boardRef.getGameChessRef(position);
        if (chess == null)
            return null;
        Vector3i chessPos = chess.getPosition();
        int chessDirection = chess.getDirection();

        Skill skill = new Skill(EffectType.ring, chessPos, chessPos);

        GameChess fChess = boardRef.getPointedGameChessRef(chessPos, chessDirection);
        int result=-1;
        if (fChess != null && isFriend(fChess, chess)) {
            //对于所有五种方向进行判断（除了自己指向的方向）
            for (int i = 1; i < 6; i++)
            {
                //获得现在这个方向的棋子
                GameChess fg = boardRef.getPointedGameChessRef(chessPos, (chessDirection + i) % 6);
                if (fg != null)
                {
                    //如果非空则找这个棋子指向的对象
                    GameChess fgfg = boardRef.getPointedGameChessRef(fg.getPosition(), fg.getDirection());
                    //如果这个棋子指向自己
                    if (fgfg != null && fgfg.getPosition() == chess.getPosition() && isFriend(chess, fg))
                    {
                        //对这个方向进行搜索
                        result = getNextLength(chess, fg, boardRef);
                        //递归回来的结果如果是可用的
                        if (result > 0)
                        {
                            break;
                        }
                        //否则继续找其他方向
                    }
                }
            }
        }

        if (result > 0)
        {
            skill._HealthChange += result;
            return skill;
        }
        else {

            return null;
        }

    }

    //递归函数
    private static int getNextLength(GameChess origin, GameChess thisC, GameBoard boardRef) {
        //当递归回到了头的时候-->返回长度
        if (thisC.getPosition() == origin.getPosition())
        {
            return  1;
        }
        else {
            //对于所有五种方向进行判断（除了自己指向的方向）
            for (int i = 1; i < 6; i++) {
                Vector3i pos = thisC.getPosition();
                int chessDirection = thisC.getDirection();
                //获得现在这个方向的棋子
                GameChess fChess = boardRef.getPointedGameChessRef(pos, (chessDirection + i) % 6);
                if (fChess != null) { 
                    //如果非空则找这个棋子指向的对象
                    GameChess fcfc = boardRef.getPointedGameChessRef(fChess.getPosition(),fChess.getDirection());
                    //如果这个棋子指向自己
                    if (fcfc != null && fcfc.getPosition() == thisC.getPosition() && isFriend(fChess, thisC)) {
                        //对这个方向进行搜索
                        int result;
                        result = getNextLength(origin,fChess,boardRef);
                        //递归回来的结果如果是可用的
                        if (result > 0) {
                            return result+1;
                        }
                        //否则继续找其他方向
                    }
                }
            }
            //如果这里所有方向都不行，则返回不可用的-1
            return -1;
        }
    }



    // li--ok
    private static Skill checkThron(Vector3i position, GameBoard boardRef)
    {
        GameChess chess = boardRef.getGameChessRef(position);
		if (chess == null)
			return null;
		
        Vector3i chessPos = chess.getPosition();
        int chessDirection = chess.getDirection();
        Skill skill = null;
        //fChess是这个棋子背后的棋子
        GameChess fChess = boardRef.getPointedGameChessRef(position, (chessDirection + 3) % 6);

        if (isFriend(chess, fChess))
        {
            int dir01 = chessDirection;
            int dir02 = fChess.getDirection();


            if ((dir01 + 6 - dir02) % 6 == 3)
            {
                Vector3i pos = chessPos;
                skill = new Skill(EffectType.thron, pos, pos);
                skill._AttackChange = 2;
            }
        }
        return skill;
    }

    // guan--ok
    private static Skill checkBasic(Vector3i position, GameBoard boardRef)
    {
        GameChess chess = boardRef.getGameChessRef(position);
		if (chess == null)
			return null;

        Vector3i chessPos = chess.getPosition();
        int chessDirection = chess.getDirection();

        // 根据前后格 来判定血量加成(作用于自身)
        Skill skill = new Skill(EffectType.basic, chessPos, chessPos);

        GameChess forwardChess = boardRef.getPointedGameChessRef(position, chessDirection);

        GameChess backwardChess = boardRef.getPointedGameChessRef(position, (chessDirection + 3) % 6);

        //自身所占格子的生命值加成
        skill._HealthChange = 1;

        // 仅友方或空格加成血量
        if (forwardChess==null || isFriend(forwardChess, chess)){
            skill._HealthChange += 1;
        }
        // 仅友方或空格加成血量
        if (backwardChess == null || isFriend(backwardChess, chess))
        {
            skill._HealthChange += 1;
        }
        return skill;
    }

    // li--ok
    private static Skill checkSupportAndAttack(Vector3i position, GameBoard boardRef)
    {
        GameChess chess = boardRef.getGameChessRef(position);
        int chessDirection = chess.getDirection();
        GameChess fChess = boardRef.getPointedGameChessRef(position, chessDirection);

        Skill skill = null;

        if (fChess == null)
        {
            return null;
        }
        else
        {
            if (isFriend(fChess, chess))
            {
                skill = new Skill(EffectType.support, fChess.getPosition(), position);
            }
            else
            {
                skill = new Skill(EffectType.attack, fChess.getPosition(), position);
            }
        }
        return skill;

    }

    public static void addMoveSkillFrom(GameChess chess, GameBoard board)
    {
        Vector3i pos = chess.getPosition();
        int oner = chess.getPlayerID();
        //在这个棋子的周围六个方向上
        for (int i = 0; i < 6; i++) {
            GameChess fchess;
            fchess = board.getPointedGameChessRef(pos, i);
            //所有存在且可以移动的（即有棋子，是现在回合方，不是友军）
            if (fchess != null && fchess.getPlayerID() != oner && fchess.getPlayerID() == board.getPlayerNow()) {
                int direction = fchess.getDirection();
                GameChess dchess = board.getPointedGameChessRef(fchess.getPosition(),direction);
                //如果这个棋子指向的正是我们原来的棋子的话
                if (dchess!=null && dchess.getPosition() == pos)
                {
                    //增加一个移动技能
                    fchess._SkillList.Add(new Skill(EffectType.move, pos, fchess.getPosition()));
                }
            }
        }
    }

    public static void moveSkillRemove(Vector3i pos, GameBoard board) {
        Skill sk = new Skill(EffectType.move, pos, pos);
        foreach (GameChess gc in board.getGameChessListCopy()) {
            GameChess realchess;
            realchess = board.getGameChessRef(gc.getPosition());
            if (realchess.hasSuchSkill(sk)) {
                realchess.wipeSkill(sk);
            }
        }
    }

    private static void wipeExceptSpecialSkill(GameChess chess) {
        List<Skill> sk = new List<Skill>();
        foreach (Skill s in chess._SkillList) {
            if (s.type == EffectType.move) {
                sk.Add(s);
            }
        }

        chess._SkillList = sk;
    }


    public static void wipeMoveSkill(GameChess chess) {
        List<Skill> sk = new List<Skill>();
        foreach (Skill s in chess._SkillList)
        {
            if (s.type == EffectType.move)
            {
                sk.Add(s);
            }
        }

        foreach (Skill s in sk) {
            chess._SkillList.Remove(s);
        }
    }


    // 敌友判定
    private static bool isEnemy(GameChess a, GameChess b)
    {
        if (a != null &&
            b != null &&
            a.getPlayerID() != b.getPlayerID())
            return true;

        return false;
    }

    private static bool isFriend(GameChess a, GameChess b)
    {
        if (a != null &&
            b != null &&
            a.getPlayerID() == b.getPlayerID())
            return true;

        return false;
    }
    
}
