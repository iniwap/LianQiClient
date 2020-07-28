using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Liuhe.Model;

using UnityEngine.Assertions;

public class ChessObj : MonoBehaviour {

    public int _Health;
    public int _Attack;
    public int _Absorb;

    public Text _UI_Health;
    public Text _UI_Attack;
    public Text _UI_Absorb;

    public Transform _DeadMark;

    public int _Direction;

    public int x;
    public int y;

    private int oldx;
    private int oldy;

    public int _Ownner;

    public int _ChessIdentityNumber;

    public Transform _UIDirectionArrow;


    public Display _Father;

    public GridObj _GridOccupy;

    public GridObj _GridOccupyOld;

    public Sprite[] _DirsSprite;

    public Image _DisRenderer;

    public Collider2D _Co2d;

    public Transform _ForwardTarget;

    public ChessObj _Temp_ForwardChess;

    public bool _CanMove;

    public bool _HasMove;

    public Vector3 _TenpTrans;

    public Vector3 _OldTrans;

    public Animator _Ani;

    public bool _IsAlive;

    public int _TargetInt;

    public ChessObj set(int xx, int yy, int dir, GridObj go, int ownner)
    {
        _Ownner = ownner;
        _Direction = dir;
        _ChessIdentityNumber = -1;
        _GridOccupy = go;

        x = go.x;
        y = go.y;

        Debug.Log(ownner);
        
        _DisRenderer.sprite = _DirsSprite[ownner];

        _Health = 0;
        _Absorb = 0;
        _Attack = 0;

        this.transform.name = "(" + go.x + ", " + go.y +  ")--棋子";

        _CanMove = false;

        _IsAlive = true;
        _Temp_ForwardChess = null;

        return this;
    }

    public void recalculateRotate()
    {
        if (_Direction >= 0 && _Direction < 6)
        {
            _UIDirectionArrow.localRotation = Quaternion.Euler(new Vector3(0, 0, -60 * _Direction));
        }
        else
        {
            _UIDirectionArrow.gameObject.SetActive(false);
        }
    }

    public void kill() {
        _Ani.SetTrigger("kill");
        _UI_Health.text = "";
        _UI_Attack.text = "";
        _UI_Absorb.text = "";
        _Co2d.enabled = false;
        _IsAlive = false;
    }

    //不要移动
    public void setNotMove() {
        _ForwardTarget.DOScale(new Vector3(0, 0, 1), 0.25f);
        _Temp_ForwardChess = null;
        _CanMove = false;
    }

    //可以移动
    public void setForward(Vector3 np, ChessObj tar, int ti) {
        _OldTrans = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _TenpTrans = new Vector3(np.x,np.y,np.z);
        _ForwardTarget.DOScale(new Vector3(1, 1, 1), 0.25f);
        _Temp_ForwardChess = tar;
        _CanMove = true;
        _HasMove = false;
        _GridOccupyOld = _GridOccupy;
        oldx = x;
        oldy = y;
        _TargetInt = ti;
    }

    public void aiMove(Vector3 np, ChessObj tar) {
        transform.DOMove(np, 0.35f);
        _GridOccupy = tar._GridOccupy;
        x = tar.x;
        y = tar.y;
        this.transform.name = "(" + x + ", " + y + ")--棋子";
    }

    public void OnMouseDown()
    {
        //Debug.Log("点击了棋子");
        if (_CanMove) {
            _Father.clickMoveChess(this);
        }
    }

    //移动到新地方
    public void moveToTemp() {
        transform.DOMove(_TenpTrans,0.35f);
        _GridOccupy = _Temp_ForwardChess._GridOccupy;
        x = _Temp_ForwardChess.x;
        y = _Temp_ForwardChess.y;
        this.transform.name = "(" + x + ", " + y + ")--棋子";
        _HasMove = true;
    }

    //移动到老地方
    public void moveToOld() {
        transform.DOMove(_OldTrans, 0.35f);
        _GridOccupy = _GridOccupyOld;
        x = oldx;
        y = oldy;
        this.transform.name = "(" + x + ", " + y + ")--棋子";
        _HasMove = false;
    }

    public void recalculatorTransform()
    {
        //float inter = _Father._Interval;
        float inter = 13;
        Vector3 v2 = new Vector3(inter * (x - y / 2f), inter * (y * Mathf.Sqrt(3) / 2f), 0);
        this.transform.position = v2;
    }

    public bool isEqualPostionDirection(int xx, int yy, int dir, int ownner)
    {
        if (this.x == xx && this.y == yy  && dir == this._Direction && ownner == this._Ownner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void rotate(int dir)
    {
        _Direction = dir;
        recalculateRotate();
    }

    public void setVoid()
    {
        _UI_Health.text = "";
        _UI_Attack.text = "";
        _UI_Absorb.text = "";
        _Health = -1;
        _Attack = -1;
        _Absorb = -1;
        _DeadMark.DOScale(new Vector3(0, 0, 1), 0.25f);
    }

    public void setDigitals(LiuheChess lc) {
        if (_Health != lc.health) {
            _UI_Health.transform.localScale = new Vector3(0,0,1);
            _UI_Health.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            _UI_Health.gameObject.SetActive(true);
            _UI_Health.text = "" + lc.health;
            _Health = lc.health;
        }

        if (_Health <= 0)
        {
            _DeadMark.DOScale(new Vector3(1, 1, 1), 0.25f);
            transform.SetParent(_Father._UnderChess);

        }
        else
        {
            _DeadMark.DOScale(new Vector3(0, 0, 1), 0.25f);
            transform.SetParent(_Father._ChessRoot);
        }

        if (_Attack != lc.attack)
        {
            _UI_Attack.transform.localScale = new Vector3(0, 0, 1);
            _UI_Attack.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            _UI_Attack.gameObject.SetActive(true);
            _UI_Attack.text = "" + lc.attack;
            _Attack = lc.attack;
        }

        if (_Absorb != lc.absorb) {
            if (lc.absorb == 0)
            {
                _UI_Absorb.transform.DOScale(new Vector3(0,0,1),0.5f);
                _UI_Absorb.gameObject.SetActive(true);
                _Absorb = 0;
                _UI_Absorb.text = "" + _Absorb;
            }
            else
            {
                _UI_Absorb.transform.localScale = new Vector3(0,0,1);
                _UI_Absorb.transform.DOScale(new Vector3(1,1,1),0.5f);
                _UI_Absorb.gameObject.SetActive(true);
                _Absorb = lc.absorb;
                _UI_Absorb.text = "" + _Absorb;
            }
        }
    }

    public void setHaveCollider() {

        _Co2d.enabled = true;
    }

    public void setReal() {

    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
