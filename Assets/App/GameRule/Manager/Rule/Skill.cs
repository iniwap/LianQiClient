using System.Collections;
using System.Collections.Generic;

public class Skill {
    public int _HealthChange;

    public int _AttackChange;

    public int _AbsorbChange;
    
    //作用对象
	public Vector3i _ApplyPosition;

    //作用来源
	public Vector3i _BasePosition;
    

	public EffectType _Type;

    public EffectType type
    {
        get { return _Type; }    
    }

	public Skill(){
		
	}
    public bool isEqual(Skill other)
    {

        if (other != null &&
            _HealthChange == other._HealthChange &&
            _AttackChange == other._AttackChange &&
            _AbsorbChange == other._AbsorbChange &&
            _ApplyPosition == other._ApplyPosition &&
            _BasePosition == other._BasePosition &&
            _Type == other._Type)
        {
            return true;
        }

        return false;
    }

    public bool isEqualMoveSkill(Skill other) {
        if (other != null &&
            _BasePosition == other._BasePosition &&
            _Type == other._Type)
        {
            return true;
        }

        return false;
    }


    public Skill getCopy()
    {
        Skill skill = new Skill(_Type, _ApplyPosition,_BasePosition);

        skill._HealthChange = _HealthChange;
        skill._AttackChange = _AttackChange;
        skill._AbsorbChange = _AbsorbChange;

        return skill;
    }
    
    public Skill(EffectType type, Vector3i applyPosition, Vector3i basePosition)
    {
        this._Type = type;
        this._ApplyPosition = applyPosition;
        this._BasePosition = basePosition;

        _HealthChange = 0;
        _AttackChange = 0;
        _AbsorbChange = 0;
    }

    public string getSkillInfo()
    {
        string str = "";
        switch (this._Type)
        {
            case EffectType.basic: str += "basic"; break;
            case EffectType.attack: str += "attack"; break;
            case EffectType.kiss: str += "kiss"; break;
            case EffectType.ring: str += "ring"; break;
            case EffectType.support: str += "support"; break;
            case EffectType.thron: str += "thron"; break;
            case EffectType.move: str += "move"; break;
        }
        str += " " + this._HealthChange;

        str += " " + this._AttackChange;

        str += " " + this._AbsorbChange;

        return str;
    }

    public override string ToString()
    {
        return getSkillInfo();
    }

    public Vector3i getEffectPosition() {
        return this._ApplyPosition;
    }
}
