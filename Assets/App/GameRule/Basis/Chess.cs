using System.Collections;
using System.Collections.Generic;

public class Chess
{
    // 执子玩家编号
    public int _PlayerID;

    //棋子方向
    public int _Direction;

    // 引用的棋盘各格子的坐标
    public Vector3i _GridPos;

    // 构造函数
    public Chess(int playerID, int direction, Vector3i gridPos)
    {
        _PlayerID = playerID;
        _Direction = direction;
        _GridPos = gridPos;
    }


    //返回拷贝
    public Chess getCopy() {
        // 此处的拷贝结果中，_Grid是null
        return new Chess(_PlayerID, _Direction, _GridPos);
            //.updatePosWithGrid(this._Grid);
    }

    //判断是否等价-->重载函数
    public bool isEqual(Chess other) {
        if (other._GridPos == this._GridPos &&
            other._PlayerID == this._PlayerID &&
            other._Direction == this._Direction) {
                return true;
        }

        return false;
    }

    //判断是否等价-->重载函数
    public bool isEqual(Vector3i pos, int playerNum, int dircetion)
    {
        if (this.isEqualPosition(pos) &&
            playerNum == this._PlayerID &&
            dircetion == this._Direction)
        {
            return true;
        }

        return false;
    }

    //判断是否位置相同
    public bool isEqualPosition(Vector3i pos) {
        return this._GridPos.Equals(pos);
    }
}
