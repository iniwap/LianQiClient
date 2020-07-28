using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Liuhe.Manager;
using Liuhe.Model;

namespace Liuhe.Player
{
    [System.Serializable]
    public enum PLAYER_TYPE
    {
        LOCAL,
        AI,
        NETWORK,
    }

    public enum PLAYER_CHARACTER {
        TIAN_SHU,
        HERO,
        LEGACY,
    }

    
    public abstract class LiuhePlayer
    {
        public PLAYER_TYPE _Type;
        public PLAYER_CHARACTER _PC;
        public int _PlayerIndex;

        public LiuhePlayer(int i, PLAYER_TYPE t, PLAYER_CHARACTER p)
        {
            _PlayerIndex = i;
            _Type = t;
            _PC = p;
        }
        public abstract string sayhello();

        public abstract IEnumerator callNewAction();
        
    }
    
    public class LiuheAIPlayer : LiuhePlayer {

        public LiuheAIPlayer(int i, PLAYER_CHARACTER pc) : base(i, PLAYER_TYPE.AI, pc)
        {

        }

        public override string sayhello()
        {
            return "I'm ai";
        }

        public override IEnumerator callNewAction()
        {
            //Debug.Log("哈哈，我的回合");
            if (_PC == PLAYER_CHARACTER.LEGACY)
            {

                yield return new WaitForSeconds(1f);

                Display._Main.readyToPlace();

                yield return new WaitForSeconds(0.5f);

                //叫我我就结束回合
                GameManager._Main.endTurn();

                //Debug.Log("我已经让结束了啊");
            }
            else if (_PC == PLAYER_CHARACTER.TIAN_SHU)
            {
                if (Display._Main._Game_Mode == GameMode.TEACH_MODE)
                {
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);

                    yield return AI_Manager._Main.getResult();

                    Display._Main.readyToPlace();

                    yield return new WaitForSeconds(0.5f);


                    Display._Main.deleteAllDeadChess();

                    //叫我我就结束回合
                    GameManager._Main.endTurn();
                    GameManager._Main.updateChessBoard();
                    Display._Main.updateBoard();
                }
            }
        }
    }
    public class LiuheLocalPlayer : LiuhePlayer
    {
        public LiuheLocalPlayer(int i, PLAYER_CHARACTER pc) : base(i, PLAYER_TYPE.LOCAL, pc)
        {

        }

        public override string sayhello()
        {
            return "I'm local";
        }

        public override IEnumerator callNewAction()
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
    public class LiuheNetworkPlayer : LiuhePlayer
    {
        public LiuheNetworkPlayer(int i, PLAYER_CHARACTER pc) : base(i, PLAYER_TYPE.NETWORK, pc)
        {

        }

        public override string sayhello()
        {
            return "I'm newwork";
        }
        

        public override IEnumerator callNewAction()
        {
            yield return new WaitForSeconds(0.1f);
        }
    }


}
