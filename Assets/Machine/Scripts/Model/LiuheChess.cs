using System.Collections;
using System.Collections.Generic;
using UnityEngine; //-->约定好只在Debug时使用Log，例如随机数等请用纯c#实现

using Liuhe.Utils;

namespace Liuhe.Model {
    public enum GridValidState
    {
        VOID,
        USING,
        INVALID,
        OUTRANGE,
    }

    public enum BufferType {
        BASIC,
        SUPPORT,
        ATTACK,
        THRON,
        RING,
        ABSORBER,
        ABSORBEE,

    }

    public class BufferEffect {
        public int perfromChess;
        public int acceptChess;
        public  BufferType bfType;
        public int healthChange;
        public int attackChange;
        public int absorbChange;

        public BufferEffect() {

        }

        public BufferEffect set(int inc, int outc, BufferType bt, int hc, int atc, int abc) {
            perfromChess = inc;
            acceptChess = outc;
            bfType = bt;
            healthChange = hc;
            attackChange = atc;
            absorbChange = abc;
            return this;
        }


        public BufferEffect getCopy() {
            BufferEffect bf = new BufferEffect().set(this.perfromChess,this.acceptChess,
                this.bfType,this.healthChange,this.attackChange,this.absorbChange);
            return bf;
        }
    }


    public class LiuheChess
    {
        public int identityNumber;

        public int x;
        public int y;
        public int dir;
        public int ownner;

        public int health;
        public int attack;
        public int absorb;

        public List<BufferEffect> buffers;

        public LiuheChess(int xx, int yy, int dd, int oo, int idnum) {
            x = xx;
            y = yy;
            dir = dd;
            ownner = oo;
            health = 0;
            attack = 0;
            absorb = 0;
            buffers = new List<BufferEffect>();
            identityNumber = idnum;
        }
        

        public bool isPosEqual(int xx, int yy) {
            if (x == xx && y == yy) {
                return true;
            }

            return false;
        }

        public bool isPosEqual(LiuheChess that)
        {
            if (this.x == that.x && this.y == that.y)
            {
                return true;
            }

            return false;
        }

        public bool isIDEqual(int nid) {
            return nid == identityNumber;
        }

        public bool isIDEqual(LiuheChess that)
        {
            return that.identityNumber == this.identityNumber;
        }

        public LiuheChess getCopy() {
            LiuheChess lc = new LiuheChess(this.x, this.y, this.dir, this.ownner, this.identityNumber);
            lc.health = this.health;
            lc.absorb = this.absorb;
            lc.attack = this.attack;
            foreach (BufferEffect be in this.buffers) {
                lc.buffers.Add(be.getCopy());
            }

            return lc;
        }


        public BufferEffect getSuchBuff(BufferType bt)
        {
            foreach (BufferEffect b in this.buffers) {
                if (b.bfType == bt) {
                    return b;
                }
            }

            return null;
        }

        public List<BufferEffect> getSuchBuffAll(BufferType bt)
        {
            List<BufferEffect> bes = new List<BufferEffect>();
            foreach (BufferEffect b in this.buffers)
            {
                if (b.bfType == bt)
                {
                    bes.Add(b);
                }
            }

            return bes;
        }

        public bool hasSuchBuff(BufferType bt) {
            foreach (BufferEffect b in this.buffers)
            {
                if (b.bfType == bt)
                {
                    return true;
                }
            }

            return false;
        }
    }


    public enum ChessBoardState {
        HEALTH,
        POSITION_MIXED,
        ID_MIXED,
    }

    public class ChessBoardStateInfo {
        public ChessBoardState state;
        public string info;

        public ChessBoardStateInfo(ChessBoardState s, string i) {
            state = s;
            info = i;
        }
    }

    public enum SpecialLinkType {
        DEAD,
        ATTACK,
        SKILL,
    }

    public class SpecialChessLink
    {
        public int _From_Idm;
        public int _To_Idm;
        public SpecialLinkType _Type;

        public SpecialChessLink(SpecialLinkType slt, int f, int t) {
            _Type = slt;
            _From_Idm = f;
            _To_Idm = t;
        }

        public SpecialChessLink getCopy() {
            return new SpecialChessLink(this._Type,this._From_Idm,this._To_Idm);
        }
    }

    public class PlaceMemory {
        public int placeOwnner;
        public int chessID;
        public int chessDir;

        public PlaceMemory(int ownner, int idm, int dir) {
            placeOwnner = ownner;
            chessID = idm;
            chessDir = dir;
        }

        public PlaceMemory getCopy() {
            return new PlaceMemory(this.placeOwnner,this.chessID,this.chessDir);
        }

    }

    public class LiuheChessBoard
    {
        public int boardSize;   //棋盘尺寸
        public List<LiuheChess> chesses;

        public List<SpecialChessLink> deads;

        public List<SpecialChessLink> attacks;

        public List<SpecialChessLink> skills;

        public List<PlaceMemory> places;

        private int IdentityNumberNow;

        public int roundNum;

        public LiuheChessBoard(int level) {
            boardSize = level;
            chesses = new List<LiuheChess>();
            places = new List<PlaceMemory>();
            deads = new List<SpecialChessLink>();
            attacks = new List<SpecialChessLink>();
            skills = new List<SpecialChessLink>();
            IdentityNumberNow = 0;
            roundNum = 0;
        }

        //根据位置找棋子
        public LiuheChess findChessByPosition(int xx, int yy) {
            foreach (LiuheChess lc in chesses) {
                if (lc.isPosEqual(xx, yy)) {
                    return lc;
                }
            }

            return null;
        }

        private static int getPointX(int x, int dir)
        {
            switch (dir)
            {
                case 0: return x - 1;
                case 1: return x;
                case 2: return x + 1;
                case 3: return x + 1;
                case 4: return x;
                case 5: return x - 1;
                default:
                    break;
            }
            Debug.Log("重大错误！！ 错误方向！");
            return -1;
        }

        private static int getPointY(int y, int dir)
        {
            switch (dir)
            {
                case 0: return y;
                case 1: return y + 1;
                case 2: return y + 1;
                case 3: return y;
                case 4: return y - 1;
                case 5: return y - 1;
                default:
                    break;
            }
            Debug.Log("重大错误！！ 错误方向！");
            return -1;
        }

        public int[] getPointPos(int xx, int yy, int dir) {
            int[] re = new int[2];
            re[0] = getPointX(xx, dir);
            re[1] = getPointY(yy, dir);
            return re;
        }

        public LiuheChess findChessInDir(int xx, int yy, int dir)
        {
            int fx, fy;
            fx = getPointX(xx, dir);
            fy = getPointY(yy, dir);

            foreach (LiuheChess lc in chesses)
            {
                if (lc.isPosEqual(fx, fy))
                {
                    return lc;
                }
            }

            return null;
        }

        //根据ID找棋子
        public LiuheChess findChessByIdnum(int idn) {
            foreach (LiuheChess lc in chesses)
            {
                if (lc.isIDEqual(idn))
                {
                    return lc;
                }
            }

            return null;
        }

        public ChessBoardStateInfo checkChessBoardState()
        {
            for (int i = 0; i < chesses.Count; i++)
            {
                for (int j = i+1; j < chesses.Count; j++)
                {
                    if (chesses[i].isIDEqual(chesses[j]))
                    {
                        return new ChessBoardStateInfo(ChessBoardState.ID_MIXED,
                            "ID Mixed with chess-" + i + "  and  chess-" + j + " , id: " + chesses[i].identityNumber);
                    }
                    if (chesses[i].isPosEqual(chesses[j]))
                    {
                        return new ChessBoardStateInfo(ChessBoardState.POSITION_MIXED,
                            "Position Mixed with chess-" + i + "  and  chess-" + j + " , pos: (" + chesses[i].x + " , " + chesses[i].y + " )");
                    }

                }
            }
            return new ChessBoardStateInfo(ChessBoardState.HEALTH, "ChessBoard Health");
        }

        public int makeNewChess(int xx, int yy, int dir, int ownner) {
            LiuheChess nlc = new LiuheChess(xx, yy, dir, ownner, IdentityNumberNow);
            chesses.Add(nlc);

            IdentityNumberNow++;
            return IdentityNumberNow - 1;
        }

        public void addForbiddenDir(int dir, int ownner) {
            List<int> dirs = Rules.Rules.getUsableDirection(this);
            

            places.Add(new PlaceMemory(ownner, IdentityNumberNow, dir));
            if (places.Count > 2)
            {
                places.RemoveAt(0);
            }
        }

        public int addNewChessUnSafe(int xx, int yy, int dir, int ownner) {
            LiuheChess nlc = new LiuheChess(xx, yy, dir, ownner, IdentityNumberNow);
            chesses.Add(nlc);
            places.Add(new PlaceMemory(ownner, IdentityNumberNow, dir));
            if (places.Count > 2)
            {
                places.RemoveAt(0);
            }
            IdentityNumberNow++;
            return IdentityNumberNow - 1;
        }

        public int addNewChess(int xx, int yy, int dir,int ownner) {
            LiuheChess nlc = new LiuheChess(xx, yy, dir, ownner, IdentityNumberNow);

            chesses.Add(nlc);

            ChessBoardStateInfo cbsi = checkChessBoardState();

            if (cbsi.state != ChessBoardState.HEALTH) {
                LiuheLogger.Log(cbsi);
                chesses.Remove(nlc);
                return -1;
            }

            List<int> dirs = Rules.Rules.getUsableDirection(this);

            if (!dirs.Contains(dir)) {
                chesses.Remove(nlc);
                return -2;
            }

            places.Add(new PlaceMemory(ownner,IdentityNumberNow,dir));
            if (places.Count > 2) {
                places.RemoveAt(0);
            }

            IdentityNumberNow++;
            return IdentityNumberNow-1;
        }

        public void endAction(int np) {
            int ss = places.Count;
            if (ss == 0) {
                places.Add(new PlaceMemory(-1, -1, -1));
            }
            else if (ss == 1 && np != places[0].placeOwnner) {
                places.Add(new PlaceMemory(-1, -1, -1));
            }
            else if (ss == 2 && np != places[1].placeOwnner) {
                places.Add(new PlaceMemory(-1, -1, -1));
                places.RemoveAt(0);
            }
            roundNum++;
        }

        public LiuheChessBoard getCopy() {
            LiuheChessBoard lcb = new LiuheChessBoard(this.boardSize);
            lcb.IdentityNumberNow = this.IdentityNumberNow;
            foreach (LiuheChess lc in this.chesses) {
                lcb.chesses.Add(lc.getCopy());
            }
            foreach (PlaceMemory pm in this.places) {
                lcb.places.Add(pm.getCopy());
            }

            foreach (SpecialChessLink sc in this.deads) {
                lcb.deads.Add(sc.getCopy());
            }
            foreach (SpecialChessLink sc in this.attacks)
            {
                lcb.attacks.Add(sc.getCopy());
            }
            foreach (SpecialChessLink sc in this.skills)
            {
                lcb.skills.Add(sc.getCopy());
            }

            lcb.roundNum = this.roundNum;

            return lcb;
        }

        
    }
}

