using System.Collections;
using System.Collections.Generic;

public class Grid
{
    //棋子引用
    public Chess _Chess;

    // 周围网格引用
    //public Grid[] _NearGrid;

    // 坐标
    public int x;

    public int y;

    public int z;

    // 构造函数
    public Grid(Vector3i position)
    {
        x = position.x;

        y = position.y;

        z = position.z;

        _Chess = null;

        //_NearGrid = new Grid[6];
        
        // 可以初始化NearGrid
    }


    //判断位置是否相同
    public bool isEqualPosition(Vector3i pp) {
        if (pp.z < 0) {
            return false;
        }

        // 如果xy两轴的数据正确
        if (pp.x == this.x && pp.y == this.y)
        {
            return true;
        }
        else {
            return false;
        }
    }

    //得到Vector3i类型的坐标
    public Vector3i getPos() {
        return new Vector3i(x,y,z);
    }

    //返回拷贝
    public Grid getCopy()
    {
        // 返回的结果中，棋子的指针是null，_NearGrids是一个已初始化但其中成员没有赋值的数组
        return new Grid(this.getPos());
    }
}

