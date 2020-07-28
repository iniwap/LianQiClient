using System;
using System.Collections;
using System.Collections.Generic;
using Eppy;

//temp
using UnityEngine;

// 提供棋盘数学运算
public class ChessBoard
{
    // 历史记录
    //版本控制系统
    protected VersionControl _VersionControl;

    protected List<Grid> _Grids;

    protected List<Chess> _Chess;

    // 网格阶数
    private int _GridLevel;

    // 判断自己是不是原件（自己生成还是拷贝生成）
    private bool _IsOrigin;



    //----------------------------------------构造部分------------------------------------------------------------


    // 空构造函数
    public ChessBoard()
    {
    }

    // 备用构造函数
    public ChessBoard(List<Grid> gds)
    {
        // do something
    }

    // 根据阶数初始化
    public ChessBoard(int gridLevel)
    {
        //初始化版本控制器
        _VersionControl = new VersionControl();
        //给网格阶数赋值
        _GridLevel = gridLevel;
        //初始化棋子棋盘数组
        _Grids = new List<Grid>();
        _Chess = new List<Chess>();

        //确定是原件
        _IsOrigin = true;

        // 根据阶数生成网格
        int n = _GridLevel;
        Grid[][] tempGrid;
        tempGrid = new Grid[2 * n - 1][];
        for (int i = 0; i < 2 * n - 1; i++)
        {
            //给每一排建立高度为2n-1的一排网格
            tempGrid[i] = new Grid[2 * n - 1];
            //给所有格点设空
            for (int j = 0; j < 2 * n - 1; j++)
            {
                tempGrid[i][j] = null;
            }
        }

        //赋值有意义格点
        for (int i = 0; i < 2 * n - 1; i++)
        {
            //如果在上升期（前n列）
            if (i < n)
            {
                for (int j = 0; j < n + i; j++)
                {
                    //每一个元素初始化
                    //i和j都是边角坐标系，需要转变为中心坐标系坐标
                    tempGrid[i][j] = new Grid(new Vector3i(i - n + 1, j - n + 1, 0));
                    _Grids.Add(tempGrid[i][j]);
                }
            }
            //如果在下降期（后n-1列）
            else
            {
                //从高度i-n+1开始 (即超过中点几格)
                for (int j = i - n + 1; j < 2 * n - 1; j++)
                {
                    tempGrid[i][j] = new Grid(new Vector3i(i - n + 1, j - n + 1, 0));
                    _Grids.Add(tempGrid[i][j]);
                }
            }
        }

        ////Grid已经都加到了List里面，把周围的网格都确定一下
        //foreach (Grid g in _Grids)
        //{
        //    int xx = g.x + n - 1;
        //    int yy = g.y + n - 1;
        //    g._NearGrid[0] = getGridSafe(xx - 1, yy, n, tempGrid);
        //    g._NearGrid[1] = getGridSafe(xx, yy + 1, n, tempGrid);
        //    g._NearGrid[2] = getGridSafe(xx + 1, yy + 1, n, tempGrid);
        //    g._NearGrid[3] = getGridSafe(xx + 1, yy, n, tempGrid);
        //    g._NearGrid[4] = getGridSafe(xx, yy - 1, n, tempGrid);
        //    g._NearGrid[5] = getGridSafe(xx - 1, yy - 1, n, tempGrid);
        //}
    }

    //判断是否越界（因为六边形的左上和右下两个角是被null填满的，所以只用考虑是不是数组越界就可以了）
    private Grid getGridSafe(int x, int y, int n, Grid[][] grids)
    {
        if (x < 0 || x >= 2 * n - 1 || y < 0 || y >= 2 * n - 1)
        {
            return null;
        }
        else
        {
            return grids[x][y];
        }
    }

    //----------------------------------------调用部分------------------------------------------------------------

    // 落子
    public Chess placeChess(Chess chess)
    {
        Grid gd = findGrid(chess._GridPos);
        if (gd == null) {
            Debug.Log("Error-寻找棋子放置的格子失败--"+chess._GridPos.ToString());
            return null;
        }

        //找到这个位置的格子实例
        Grid gdi = findGrid(gd.getPos());

        if (gdi == null)
        {
            Debug.Log("Error-寻找棋子放置的格子失败2");
            return null;
        }

        //为棋子找到正确的网格实例
        chess._GridPos = gdi.getPos();
        //让网格也知道棋子
        gdi._Chess = chess;
        //加到棋子队列里面去
        _Chess.Add(chess);
        //向版本控制器里面push
        _VersionControl.push(chess, true);

        return chess.getCopy();
    }

    // 取子
    public bool removeChess(Vector3i pos)
    {
        Chess equalposChess = findChess(pos);
        // 判断这个位子的棋子是否存在
        if (equalposChess == null)
        {
            Debug.Log("Error-寻找棋子失败");
            return false;
        }

        //从队列里面减去这个棋子
        _Chess.Remove(equalposChess);
        //向版本控制器里面push
        _VersionControl.push(equalposChess, false);

        return true;
    }

    //当前玩家结束行动
    //（可以理解为这一次的行动终止了-->无论走了几步或者根本没走都算一次行动）
    public void endAction(int playerId)
    {
        if (!_VersionControl.commit(playerId))
        {
            Debug.Log("Error-提交失败");
        }
    }

    //悔一步棋-->存在潜在的风险，暂时不开放
    private void rollBack()
    {
        if (_VersionControl.rollBack())
        {
            _Chess = _VersionControl.getMaster();
        }
        else
        {
            Debug.Log("Error-悔棋失败");
        }
    }

    //----------------------------------------获得副本------------------------------------------------------------


    public int  getGridLevel(){
        return this._GridLevel;
    }

    // 取指定位置格子副本
    public Grid getGridCopy(Vector3i position)
    {
        Grid gd = findGrid(position);
        if (gd == null) {
            return null;
        }

        return gd.getCopy();
    }

    // 取指定位置棋子副本
    public Chess getChessCopy(Vector3i position)
    {
        Chess cs = findChess(position);
        if (cs == null) {
            //Debug.Log("查找失败"+position.ToString());
            return null;
        }

        //Debug.Log("查找成功" + position.ToString());

        return cs.getCopy();
    }

    public Chess getChessInDirection(Vector3i position, int direction)
    {
        Vector3i pos = new Vector3i(position.x, position.y, position.z);

        switch (direction)
        {
            case 0:
                pos.x -= 1;
                break;

            case 1:
                pos.y += 1;
                break;

            case 2:
                pos.x += 1;
                pos.y += 1;
                break;

            case 3:
                pos.x += 1;
                break;

            case 4:
                pos.y -= 1;
                break;

            case 5:
                pos.x -= 1;
                pos.y -= 1;
                break;

            default:
                Debug.Log("错误方向活位置");
                break;
        }

        return getChessCopy(pos);
    }

    // 获得整个棋子队列的副本(里面每个棋子的_GridPos属性可以用于去寻找对应格子)
    public List<Chess> getChessListCopy() {
        List<Chess> ncs = new List<Chess>();
        foreach (Chess c in _Chess) {
            ncs.Add(c.getCopy());
        }

        return ncs;
    }

    // 获得整个棋盘的副本
    public ChessBoard getChessBoardCopy() {
        ChessBoard ncb = new ChessBoard();
        ncb._VersionControl = new VersionControl();
        ncb._IsOrigin = false;
        ncb._GridLevel = this._GridLevel;
        ncb._Chess = new List<Chess>();
        ncb._Grids = new List<Grid>();

        //根据_Grids中的内容建立新的列表并复制；而同时如果棋盘格中有棋子，也把棋子复制，并建立连接关系
        foreach (Grid gd in this._Grids) {
            Grid nd = gd.getCopy();
            ncb._Grids.Add(nd);
            if (gd._Chess != null) { 
                Chess c = gd._Chess.getCopy();
                ncb._Chess.Add(c);
                ncb.linkChessAndGrid(c,nd);
            }
        }

        //// 对网格的邻接网格进行赋值
        //foreach (Grid g in ncb._Grids) {
        //    //找到原列表对应的真实实例
        //    Grid rng = this.findGrid(g.getPos());
        //    // 对于六个方向，每一个方向的对应连接都对应构建
        //    for (int i = 0; i < 6; i++) {
        //        //获得i方向的棋子og（ncb中的实例）
        //        Grid og = ncb.findGrid(rng._NearGrid[i].getPos());
        //        if (og == null) {
        //            Console.WriteLine("Error-找不到对应实例");
        //        }
        //        g._NearGrid[i] = og;
        //    }
        //}

        return ncb;
    }


    //----------------------------------------获得实例----------------------------------------------------------

    //找到网格实例
    private Grid findGrid(Vector3i pos) {
        foreach (Grid gd in _Grids) {
            if (gd.isEqualPosition(pos)) {
                return gd;
            }
        }

        return null;
    }

    //找到棋子实例
    private Chess findChess(Vector3i pos)
    {
        foreach (Chess cs in _Chess)
        {
            if (cs.isEqualPosition(pos))
            {
                return cs;
            }
        }
        return null;
    }




    //----------------------------------------工具函数------------------------------------------------------------


    // 建立链接
    private void linkChessAndGrid(Chess chess, Grid grid)
    {
        chess._GridPos = grid.getPos();
        grid._Chess = chess;
    }


}
