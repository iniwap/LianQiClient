using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObj : MonoBehaviour {
    public int x;
    public int y;

    public Display _Father;
    public ChessObj _Chess;
    private Collider2D _Co2d;

    public bool _CanChange;

    public GridObj set(int xx, int yy)
    {
        x = xx;
        y = yy;

        this.transform.name = "grid -- (" + x + ", " + y + ")";

        _CanChange = false;
        return this;
    }


    public bool isThisCollider(Collider2D co)
    {
        if (co == this._Co2d)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    


    // 初始化
    void Start()
    {
        _Co2d = GetComponent<Collider2D>();
    }

    // 每帧更新
    void Update()
    {
        float inter = _Father._Interval;

        Vector3 v2 = new Vector3(inter * (x - y / 2f), inter * (y * Mathf.Sqrt(3) / 2f), 0);
        this.transform.position = v2;
    }

    public void clear() {
        _Chess = null;
        _CanChange = false;
    }

    public void setChess(ChessObj co) {
        _Chess = co;
        _CanChange = false;
    }


    public void OnMouseDown()
    {
       // Debug.Log("点击了网格");
        //有棋子存在情况下
        if (_Chess != null) {
            //Debug.Log("waht");
            //如果这个棋子可以转动（也就是这个回合下的棋子的话）
            if (!_CanChange)
            {
                Debug.Log("不可移动");
                return;
            }
            else {
                //Debug.Log("点击到了本回合生成的棋子上");
            }
        }

        //没有棋子存在的情况下，生成棋子并赋值
        ChessObj co = _Father.addChess(this);
        if (co != null)
        {
            _Chess = co;
            _CanChange = true;
        }
        else {
            //Debug.Log("没有成功生成");
        }
    }

    public void OnMouseDrag()
    {
        if (_Chess != null && _CanChange)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float deg;

            deg = Mathf.Atan2(pos.x - transform.position.x, pos.y - transform.position.y);

            _Father.changeDgree(deg);
        }
    }

    public void OnMouseUp()
    {
    }

    public void setLock() {
        _CanChange = false;
        _Chess.setHaveCollider();
    }
}
