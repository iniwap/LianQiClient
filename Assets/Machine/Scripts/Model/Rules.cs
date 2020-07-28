using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liuhe.Model;
using Liuhe.Utils;
using Liuhe.Player;

using UnityEngine.Assertions;


namespace Liuhe.Rules {
    public static class Rules
    {

        //确定这个地方能否下子
        public static GridValidState getGridValidState(LiuheChessBoard board, int xx, int yy)
        {
            int boardSize = board.boardSize;
            //完全越界的情况
            if ((xx < 0 && xx < -boardSize) ||
                (xx > 0 && xx > boardSize) ||
                (yy < 0 && yy < -boardSize) ||
                (yy > 0 && yy > boardSize))
            {
                return GridValidState.OUTRANGE;
            }
            //两个尖角越界的情况
            if (xx > 0)
            {
                if (yy < xx - boardSize)
                {
                    return GridValidState.INVALID;
                }
            }
            else if (xx < 0)
            {
                if (yy > boardSize + xx)
                {
                    return GridValidState.INVALID;
                }
            }

            if (board.findChessByPosition(xx, yy) != null)
            {
                return GridValidState.USING;
            }

            return GridValidState.VOID;
        }

        public static LiuheChessBoard washChessBoard(LiuheChessBoard lcb) {
            foreach (SpecialChessLink item in lcb.deads)
            {
                if (item._Type == SpecialLinkType.DEAD) {
                    LiuheChess lc = lcb.findChessByIdnum(item._From_Idm);
                    lcb.chesses.Remove(lc);
                }
            }
            GameBoardCalculateItself(lcb);
            

            return lcb;
        }

        public static LiuheChessBoard getTryResult(LiuheChessBoard board, int xx, int yy, int dir, LiuhePlayer lp) {
            GridValidState gvs = getGridValidState(board, xx, yy);
            if (gvs != GridValidState.VOID)
            {
                LiuheLogger.Log(gvs);
                return null;
            }
            int idm = board.addNewChess(xx, yy, dir, lp._PlayerIndex);

            if (idm < 0)
            {
                //Debug.Log("此位置存在错误！");
                return null;
            }

            GameBoardCalculateItself(board);

            return board;
        }

        public static bool tryPlaceChessToGameBoard(LiuheChessBoard board, int xx, int yy, int dir, LiuhePlayer lp) {
            GridValidState gvs = getGridValidState(board, xx, yy);
            if (gvs != GridValidState.VOID)
            {
                LiuheLogger.Log(gvs);
                return false;
            }

            int ss = board.places.Count;
            if (ss > 2) {
                Debug.Log("严重问题！！ 过去存储超过限额！");
            }

            if (ss == 1)
            {
                PlaceMemory pm = board.places[0];
                if (pm.placeOwnner == lp._PlayerIndex) {
                    Debug.Log("该玩家这回合已经下过了！");
                    return false;
                }
                if (pm.chessDir == dir) {
                    Debug.Log("这个方向是禁手方向！");
                    return false;
                }
            }
            else if (ss == 2) {
                PlaceMemory pm0 = board.places[0];
                PlaceMemory pm1 = board.places[1];
                if (pm1.placeOwnner == lp._PlayerIndex) {
                    Debug.Log("该玩家这回合已经下过了！");
                    return false;
                }
                if (pm0.chessDir == dir || pm1.chessDir == dir) {
                    Debug.Log("这个方向是禁手方向！");
                    return false;
                }
            }


            int idm = board.addNewChess(xx, yy, dir, lp._PlayerIndex);
            
            if(idm<0){
                Debug.Log("此位置存在错误！");
                return false;
            }

            GameBoardCalculateItself(board);

            if (board.findChessByIdnum(idm).health<=0) {
                Debug.Log("下在此处生命值会不够！");
                return false;
            }

            return true;
        }


        public class RingInfo {
            public LiuheChess chess;
            public int level;

            public RingInfo(LiuheChess lc, int ll) {
                chess = lc;
                level = ll;
            }
        }

        private static bool hasChessThisInList(List<RingInfo> rfs, LiuheChess lc) {
            foreach (RingInfo r in rfs) {
                if (r.chess == lc) {
                    return true;
                }
            }

            return false;
        }

        public static void GameBoardCalculateItself(LiuheChessBoard lcb) {
            List<LiuheChess> lcs = lcb.chesses;


            //清空buff列表
            foreach (LiuheChess l in lcs)
            {
                l.buffers = new List<BufferEffect>();
            }

            List<RingInfo> rfs;
            rfs = new List<RingInfo>();

            foreach (LiuheChess l in lcs)
            {
                if (hasChessThisInList(rfs,l))
                {
                    continue;
                }
                else
                {
                    checkRing(lcb, l.identityNumber, rfs);
                }
            }

            foreach(RingInfo r in rfs)
            {
                r.chess.buffers.Add(new BufferEffect()
                    .set(r.chess.identityNumber, 
                    r.chess.identityNumber, 
                    BufferType.RING, r.level, 0, 0));
            }


            //重新获得buff列表（Basic，Thron，Kiss，Ring）
            foreach (LiuheChess l in lcs)
            {

                bool hasFront = false;
                bool hasBack = false;
                LiuheChess fc = null;
                LiuheChess bc = null;

                //获取前方棋子
                int[] pos = lcb.getPointPos(l.x, l.y, l.dir);

                int bh;

                bh = 3;

                GridValidState gvs = Rules.getGridValidState(lcb, pos[0], pos[1]);

                if (gvs == GridValidState.USING)
                {
                    hasFront = true;
                    fc = lcb.findChessByPosition(pos[0], pos[1]);
                    if ( fc!= null)
                    {
                        if (fc.ownner != l.ownner)
                        {
                            bh--;
                            //Debug.Log("here");
                        }
                    }
                    else
                    {
                        Debug.Log("指向的格子上应该有棋子却找不到！！");
                        return;
                    }
                }
                else {
                    hasFront = false;
                }


                //获取后方棋子
                int[] pos2 = lcb.getPointPos(l.x, l.y, (l.dir + 3) % 6);
                
                gvs = Rules.getGridValidState(lcb, pos2[0], pos2[1]);

                if (gvs == GridValidState.USING)
                {
                    hasBack = true;
                    bc = lcb.findChessByPosition(pos2[0], pos2[1]);
                    if (bc != null)
                    {
                        if (bc.ownner != l.ownner)
                        {
                            bh--;
                            //Debug.Log("here");
                        }
                    }
                    else
                    {
                        Debug.Log("指向的格子上应该有棋子却找不到！！");
                        return;
                    }
                }
                else
                {
                    hasBack = false;
                }

                //增加基础buff
                l.buffers.Add(new BufferEffect().set(l.identityNumber, l.identityNumber, BufferType.BASIC, bh, 1, 0));
                

                //如果后背的是己方，并且方向和自己正好相反（背对背），增加双三的buff
                if (hasBack && bc.ownner == l.ownner && (l.dir+3)%6 == bc.dir) {
                    l.buffers.Add(new BufferEffect().set(bc.identityNumber, l.identityNumber, BufferType.THRON, 0, 3, 0));
                }
                
                //如果前面是友方，而且方向正好相对（面对面），加上吸收力增值
                if (hasFront && fc.ownner== l.ownner && (l.dir + 3) % 6 == fc.dir)
                {
                    l.buffers.Add(new BufferEffect().set(fc.identityNumber, l.identityNumber, BufferType.ABSORBER, 0, 0, 2));
                    //如果有吸收，而且后方是敌人，就加一个吸收的debuff
                    if (hasBack && bc.ownner != l.ownner)
                    {
                        bc.buffers.Add(new BufferEffect().set(l.identityNumber,bc.identityNumber,BufferType.ABSORBEE,0,-2,0));
                    }
                }
                

                if (hasFront) {
                    if (fc.ownner == l.ownner) {
                        fc.buffers.Add(new BufferEffect().set(l.identityNumber, fc.identityNumber, BufferType.SUPPORT, 0, 0, 0));
                    }
                    else
                    {
                        fc.buffers.Add(new BufferEffect().set(l.identityNumber, fc.identityNumber, BufferType.ATTACK, 0, 0, 0));
                    }
                }


                BufferEffect be = l.getSuchBuff(BufferType.BASIC);

                if ( be != null)
                {
                    l.health = be.healthChange;
                    l.absorb = 0;
                    l.attack = be.attackChange;
                }
                else {
                    Debug.Log("严重错误，没有基础buff");
                }

                be = l.getSuchBuff(BufferType.THRON);
                if (be != null)
                {
                    l.attack = be.attackChange;
                }

                be = l.getSuchBuff(BufferType.ABSORBER);
                if (be != null)
                {
                    l.absorb = be.absorbChange;
                }

                be = l.getSuchBuff(BufferType.RING);
                if (be != null) {
                    l.health += be.healthChange;
                }

                //此处不能更新被吸收的buff,因为这个时候可能没加上
            }

            //减少基本攻击力，最小到0
            foreach (LiuheChess l in lcs)
            {
                BufferEffect be = l.getSuchBuff(BufferType.ABSORBEE);
                if (be != null) {
                    l.attack += be.attackChange;
                    if (l.attack < 0) {
                        l.attack = 0;
                    }
                }
            }
            

            //更新互助列表
            foreach (LiuheChess l in lcs)
            {
                List<BufferEffect> bes = l.getSuchBuffAll(BufferType.SUPPORT);
                foreach (BufferEffect bfe in bes)
                {
                    LiuheChess nc = lcb.findChessByIdnum(bfe.perfromChess);
                    if (nc == null) {
                        Debug.Log("重大错误！！ 找不到本能找到的棋子！");
                        return;
                    }
                    bfe.attackChange = nc.attack;
                    bfe.absorbChange = 0;
                    bfe.healthChange = 1;
                }
            }

            //更新攻击列表，直接减少生命
            foreach (LiuheChess l in lcs)
            {
                List<BufferEffect> bes = l.getSuchBuffAll(BufferType.SUPPORT);
                foreach (BufferEffect bfe in bes)
                {
                    l.attack += bfe.attackChange;
                    l.health += bfe.healthChange;
                }
            }

            foreach (LiuheChess l in lcs)
            {
                List<BufferEffect> bes = l.getSuchBuffAll(BufferType.ATTACK);
                foreach (BufferEffect bfe in bes)
                {
                    LiuheChess nc = lcb.findChessByIdnum(bfe.perfromChess);
                    if (nc == null)
                    {
                        Debug.Log("重大错误！！ 找不到本能找到的棋子！");
                        return;
                    }
                    l.health -= nc.attack;
                }
            }

            lcb.attacks = new List<SpecialChessLink>();
            lcb.skills = new List<SpecialChessLink>();
            lcb.deads = new List<SpecialChessLink>();

            List<SpecialChessLink> tscl = new List<SpecialChessLink>();

            foreach (LiuheChess lc in lcb.chesses) {
                if (lc.health <= 0) {
                    lcb.deads.Add(new SpecialChessLink(SpecialLinkType.DEAD, lc.identityNumber, lc.identityNumber));
                    List<BufferEffect> bes = lc.buffers;
                    foreach (BufferEffect be in bes) {
                        if (be.bfType == BufferType.ATTACK) {
                            tscl.Add(new SpecialChessLink(SpecialLinkType.ATTACK, be.perfromChess, be.acceptChess));
                        }
                    }
                }
            }

            foreach (SpecialChessLink scl in tscl) {
                if (!lcb.deads.Contains(scl)) {
                    lcb.attacks.Add(scl);
                }
            }

        }


        private static void checkRing(LiuheChessBoard boardRef, int idm, List<RingInfo> rfs)
        {
            LiuheChess l = boardRef.findChessByIdnum(idm);

            if (l == null)
            {
                Debug.Log("严重错误！ 找不到的开始值！");
                return;
            }

            LiuheChess fc = boardRef.findChessInDir(l.x, l.y, l.dir);
            
            //l指向的不为空，fc是己方
            if (fc != null && fc.ownner == l.ownner)
            {
                getNextLength(fc, l, boardRef, 0, rfs);
            }
            return;
        }

        //递归函数
        private static int getNextLength(LiuheChess origin, LiuheChess thisC, LiuheChessBoard boardRef, int depth, List<RingInfo> rfs)
        {
            //当递归回到了头的时候-->返回长度
            if (thisC == origin)
            {
                rfs.Add(new RingInfo(thisC, depth + 1));
                return depth+1;
            }

            if (depth > 10) {
                Debug.Log("环过长");
                return -1;
            }


            //对于所有五种方向进行判断（除了自己指向的方向）
            for (int i = 0; i < 6; i++)
            {
                if (thisC.dir == i)
                {
                    continue;
                }

                //获得现在这个方向的棋子
                LiuheChess nc = boardRef.findChessInDir(thisC.x, thisC.y, i);
                if (nc != null && nc.ownner == thisC.ownner)
                {
                    //如果非空则找这个棋子指向的对象
                    LiuheChess nc2 = boardRef.findChessInDir(nc.x, nc.y, nc.dir);

                    //如果这个棋子指向自己
                    if (nc2 == thisC)
                    {
                        //对这个方向进行搜索
                        int result;
                        result = getNextLength(origin, nc, boardRef, depth + 1, rfs);
                        //递归回来的结果如果是可用的
                        if (result > 0)
                        {
                            rfs.Add(new RingInfo(thisC, result));
                            return result;
                        }
                        //否则继续找其他方向
                    }
                }
            }
            //如果这里所有方向都不行，则返回不可用的-1
            return -1;
        }
        

        public static LiuheChessBoard getTryEndBoard(ChessObj co, LiuheChessBoard lcb) {

            LiuheChess lc = lcb.findChessByIdnum(co._ChessIdentityNumber);
            int[] pos = lcb.getPointPos(lc.x, lc.y, lc.dir);
            GridValidState gvs = getGridValidState(lcb, pos[0], pos[1]);

            if (gvs != GridValidState.VOID)
            {
                return null;
            }
            else
            {
                LiuheChessBoard lcb2 = lcb.getCopy();
                LiuheChess lc2 = lcb2.findChessByIdnum(lc.identityNumber);
                if (lc2 == null)
                {
                    Debug.Log("严重错误");
                    return null;
                }
                lc2.x = pos[0];
                lc2.y = pos[1];

                GameBoardCalculateItself(lcb2);
                return lcb2;
            }
        }

        public static LiuheChessBoard getTryChessesEndBoard(List<ChessObj> coses, LiuheChessBoard lcb)
        {
            LiuheChessBoard lcb2 = lcb.getCopy();
            foreach (ChessObj co in coses) {
                //获取新棋子
                LiuheChess lc = lcb.findChessByIdnum(co._ChessIdentityNumber);
                int[] pos = lcb.getPointPos(lc.x, lc.y, lc.dir);
                GridValidState gvs = getGridValidState(lcb, pos[0], pos[1]);

                if (gvs != GridValidState.VOID)
                {
                    return null;
                }
                //棋子存在而且位置可用
                else
                {
                    LiuheChess lc2 = lcb2.findChessByIdnum(lc.identityNumber);
                    //移动棋子
                    if (lc2 == null)
                    {
                        //不可移动的时候严重错误
                        Debug.Log("严重错误");
                        return null;
                    }
                    lc2.x = pos[0];
                    lc2.y = pos[1];
                }

            }

            GameBoardCalculateItself(lcb2);
            return lcb2;
        }

        public static bool moveChessInBoard(LiuheChess lc, LiuheChessBoard lcb) {
            int[] pos = lcb.getPointPos(lc.x, lc.y, lc.dir);
            GridValidState gvs = getGridValidState(lcb, pos[0], pos[1]);

            if (gvs != GridValidState.VOID)
            {
                return false;
            }
            else {
                LiuheChessBoard lcb2 = lcb.getCopy();
                LiuheChess lc2 = lcb2.findChessByIdnum(lc.identityNumber);
                if (lc2 == null)
                {
                    Debug.Log("严重错误");
                    return false;
                }
                lc2.x = pos[0];
                lc2.y = pos[1];

                GameBoardCalculateItself(lcb2);
                if (lc2.health <= 0) {
                    return false;
                }
                
                lc.x = pos[0];
                lc.y = pos[1];
                GameBoardCalculateItself(lcb);
            }

            return true;
        }

        public static bool moveChessInBoardUnsafe(LiuheChess lc, LiuheChessBoard lcb)
        {
            int[] pos = lcb.getPointPos(lc.x, lc.y, lc.dir);

            lc.x = pos[0];
            lc.y = pos[1];
            GameBoardCalculateItself(lcb);
            return true;
        }

        public static void cleanAttacks(LiuheChessBoard lcb) {
            washChessBoard(lcb);

            lcb.skills = new List<SpecialChessLink>();
            lcb.deads = new List<SpecialChessLink>();
            lcb.attacks = new List<SpecialChessLink>();

            foreach (LiuheChess lc in lcb.chesses) {
                lc.buffers = new List<BufferEffect>();
            }
        }

        public static List<int> getUsableDirection(LiuheChessBoard lcb) {
            List<int> ud = new List<int>();

            for (int i = 0; i < 6; i++)
            {
                ud.Add(i);
            }

            foreach (var item in lcb.places)
            {
                if (item.placeOwnner >= 0) {
                    ud.Remove(item.chessDir);
                }
            }

            return ud;
        }
    }
}
