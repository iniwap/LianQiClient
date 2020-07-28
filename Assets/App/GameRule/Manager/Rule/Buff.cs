using System.Collections;
using System.Collections.Generic;

public class Buff {
    public int _HealthChange;

    public int _AttackChange;

    public int _AbsorbChange;

    // get

	public EffectType _Type;

	public Buff(){
		
	}

    public EffectType type { 
        get{return _Type;}
    }

    public bool isEqual(Buff other)
    {
        if(other != null &&
            _HealthChange == other._HealthChange &&
            _AttackChange == other._AttackChange &&
            _AbsorbChange == other._AbsorbChange &&
            _Type == other._Type)
        {
            return true;
        }

        return false;
    }

    public Buff getCopy()
    {
        Buff buff = new Buff(_Type);

        buff._AttackChange = _AttackChange;
        buff._HealthChange = _HealthChange;
        buff._AbsorbChange = _AbsorbChange;
        return buff;
    }

    public Buff(EffectType type)
    {
        this._Type = type;

        _AttackChange = 0;
        _HealthChange = 0;
        _AbsorbChange = 0;
    }

    public string getBuffInfo() {
        string str = "";
        switch (this._Type) {
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
        return getBuffInfo();
    }
}
