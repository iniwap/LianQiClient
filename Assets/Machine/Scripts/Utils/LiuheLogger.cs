using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Liuhe.Player;
using Liuhe.Manager;
using Liuhe.Rules;
using Liuhe.Model;

namespace Liuhe.Utils
{
    public class LiuheLogger
    {
        public static void Log(List<LiuhePlayer> _Ps)
        {
            Debug.Log("一共含有" + _Ps.Count + "名玩家，其中：");
            foreach (LiuhePlayer lp in _Ps)
            {
                Log(lp);
            }
        }

        public static void Log(LiuhePlayer lp)
        {
            string str = "" + lp._PlayerIndex + "号玩家 是 ";
            switch (lp._Type)
            {
                case PLAYER_TYPE.LOCAL:
                    str += "本地玩家";
                    break;
                case PLAYER_TYPE.AI:
                    str += "AI玩家";
                    break;
                case PLAYER_TYPE.NETWORK:
                    str += "网络玩家";
                    break;
                default:
                    break;
            }
            Debug.Log(str);
        }

        public static void Log(LiuheChessBoard chessboard)
        {
            Debug.Log("现在棋盘上有"+chessboard.chesses.Count+"颗棋子");
            foreach (LiuheChess item in chessboard.chesses)
            {
                Log(item);
            }
            Debug.Log("棋盘上的特殊状态为：");
            foreach (SpecialChessLink item in chessboard.deads)
            {
                Log(item);
            }
            foreach (SpecialChessLink item in chessboard.attacks)
            {
                Log(item);
            }
        }

        public static void Log(SpecialChessLink scl) {
            switch (scl._Type)
            {
                case SpecialLinkType.DEAD:
                    Debug.Log("编号为"+scl._From_Idm+"的棋子已经死亡");
                    break;
                case SpecialLinkType.ATTACK:
                    Debug.Log("编号为" + scl._From_Idm + "的棋子对编号为"+scl._To_Idm+"的棋子有攻击力");
                    break;
                default:
                    break;
            }
        }


        public static void Log(List<SpecialChessLink> scls)
        {
            foreach (SpecialChessLink scl in scls)
            {
                Log(scl);
            }
        }



        public static void Log(LiuheChess chess) {
            string str = "";
            str += "ID:" + chess.identityNumber + "  Pos: (" + chess.x + ", " + chess.y + ", " + chess.dir + ")  Ownner: " + chess.ownner + "\n"
                + "Attack: " + chess.attack + "  Health: " + chess.health + "  Absorb: " + chess.absorb + "\n";
            foreach (BufferEffect be in chess.buffers) {
                str += be.bfType;
                str += "  " + be.healthChange + "  " + be.attackChange + "  " + be.absorbChange + "  from-" + be.perfromChess + "  to-" + be.acceptChess + "\n";
            }

            Debug.Log(str);
        }

        public static void Log(GridValidState gvs) {
            switch (gvs)
            {
                case GridValidState.VOID:
                    Debug.Log("当前意向网格的状态为空");
                    break;
                case GridValidState.USING:
                    Debug.Log("当前意向网格的状态为正在使用中");
                    break;
                case GridValidState.INVALID:
                    Debug.Log("当前意向网格的状态为非法越界");
                    break;
                case GridValidState.OUTRANGE:
                    Debug.Log("当前意向网格的状态为尖角越界");
                    break;
                default:
                    Debug.Log("当前意向网格的状态超脱了限制范围");
                    break;
            }
        }

        public static void Log(ChessBoardStateInfo cbsi) {
            switch (cbsi.state)
            {
                case ChessBoardState.HEALTH:
                    Debug.Log("当前意向棋盘状态为：健康--"+cbsi.info);
                    break;
                case ChessBoardState.POSITION_MIXED:
                    Debug.Log("当前意向棋盘状态为：位置混用--" + cbsi.info);
                    break;
                case ChessBoardState.ID_MIXED:
                    Debug.Log("当前意向棋盘状态为：棋子ID混用--" + cbsi.info);
                    break;
                default:
                    Debug.Log("Error Default");
                    break;
            }
        }


        public static void Log(List<int> ud) {
            Debug.Log("可以使用的方向有：");
            foreach (var item in ud)
            {
                Debug.Log(item);
            }
        }


        public static void Log(Display dp) {
            Debug.Log("可视棋盘中具有棋子对象：");
            foreach (ChessObj co in dp._ChessList) {
                Log(co);
                if (co._GridOccupy != null)
                {
                    Debug.Log(co._GridOccupy);
                }
                else {
                    Debug.Log("不占据任何网格！！");
                }
            }

        }


        public static void Log(ChessObj co) {
            string str = "棋子" + co.transform.name + ":\n";
            str += co.x +", "+ co.y + ", "+co._Direction+"\n";
            str += "Can move:  "+ co._CanMove+"\n";
            str += "Has Move: " + co._HasMove + "\n";
            str += "Is Alive: " + co._IsAlive + "\n";
            str += "ID" + co._ChessIdentityNumber + "\n";

            Debug.Log(str);
        }

        public static void Log(GridObj go) {
            string str = "棋格" + go.transform.name + ":\n";
            if (go._CanChange) {
                str += "可以下棋\n";
            }
            if (go._Chess != null)
            {
                str += "里面有棋子\n";
            }

            Debug.Log(str);
        }

        public static void Log(Display db, LiuheChessBoard lcb) {
            foreach (ChessObj co in db._ChessList) {
                foreach (LiuheChess lc in lcb.chesses) {
                }
            }
        }


    }



    
}
