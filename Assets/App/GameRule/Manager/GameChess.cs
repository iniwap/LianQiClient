using System.Collections;
using System.Collections.Generic;

public class GameChess
{
    public int _Health;

    public int _Attack;

    public int _Support;

    public int _Absorb;

    public int _BasicAttack;

    public int _BasicSupport;

    public List<Skill> _SkillList;

    public List<Buff> _BuffList;

    // add/remove list
    public bool addSkill(Skill skill)
    {
        if(skill != null)
        {
            _SkillList.Add(skill);
            return true;
        }
        return false;
    }

    public bool addBuff(Buff buff)
    {
        if (buff != null)
        {
            _BuffList.Add(buff);
            return true;
        }
        return false;
    }

    //public void removeSkill()
    //{

    //}

    //public void removeBuff()
    //{

    //}

    public int getPlayerID()
    {
        return _Chess._PlayerID;
    }

    public Vector3i getPosition()
    {
        return _Chess._GridPos;
    }

    public int getDirection()
    {
        return _Chess._Direction;
    }

    // 等价性判断
    public bool isEqual(GameChess gameChess)
    {
        if (gameChess != null)
        {
            if (gameChess._Support == _Support &&
                gameChess._Health == _Health &&
                gameChess._Attack == _Attack &&
                gameChess._Absorb == _Absorb &&
                gameChess._BasicAttack == _BasicAttack &&
                gameChess._BasicSupport == _BasicSupport &&
                gameChess._Chess.isEqual(_Chess))
            {
                // 理应不存在技能列表与Buff一致的不同GameChess
                for (int i = 0; i < _SkillList.Count; i++)
                {
                    if (_SkillList[i].isEqual(gameChess._SkillList[i]))
                        return false;
                }

                for (int i = 0; i < _BuffList.Count; i++)
                {
                    if (_BuffList[i].isEqual(gameChess._BuffList[i]))
                        return false;
                }

                return true;
            }

            return false;
        }

        return false;
    }

    private Chess _Chess;

    public GameChess(Chess chess)
    {
        // init
        _Chess = chess;

        _Health = 0;
        _Attack = 0;
        _Support = 0;
        _Absorb = 0;
        _BasicAttack = 0;
        _BasicSupport = 0;
        _SkillList = new List<Skill>();
        _BuffList = new List<Buff>();
    }

    public GameChess getCopy()
    {
        Chess tempChess = new Chess(_Chess._PlayerID, _Chess._Direction, _Chess._GridPos);

        GameChess tempGameChess = new GameChess(tempChess);

        tempGameChess._Health = _Health;
        tempGameChess._Attack = _Attack;
        tempGameChess._Support = _Support;
        tempGameChess._Absorb = _Absorb;
        tempGameChess._BasicAttack = _BasicAttack;
        tempGameChess._BasicSupport = _BasicSupport;

        foreach (Skill skill in _SkillList)
        {
            tempGameChess._SkillList.Add(skill.getCopy());
        }

        foreach (Buff buff in _BuffList)
        {
            tempGameChess._BuffList.Add(buff.getCopy());
        }
        
        return tempGameChess;
    }

    public bool hasSuchSkill(Skill sk) {
        foreach (Skill s in _SkillList) {
            if (s.isEqualMoveSkill(sk)) {
                return true;
            }
        }
        return false;
    }

    public bool hasSuchKindSkill(EffectType ty)
    {
        foreach (Skill s in _SkillList)
        {
            if (s.type == ty)
            {
                return true;
            }
        }
        return false;
    }

    public bool wipeSkill(Skill sk) {
        foreach (Skill s in _SkillList)
        {
            if (s.isEqual(sk))
            {
                _SkillList.Remove(sk);
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        string str;
        str = "pos: " + getPosition() + "  ";
        foreach (Skill sk in _SkillList) {
            str += sk.ToString();
            str += '\n';
        }
        foreach (Buff bf in _BuffList)
        {
            str += bf.ToString();
            str += '\n';
        }
        return str;
    }

    public Skill findSkill(EffectType ec) {
        foreach (Skill sk in _SkillList) {
            if (sk.type == ec) {
                return sk;
            }
        }

        return null;
    }
}
