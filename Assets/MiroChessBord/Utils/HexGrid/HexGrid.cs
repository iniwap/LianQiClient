// Generated code -- http://www.redblobgames.com/grids/hexagons/

using System;
using System.Linq;
using System.Collections.Generic;


struct Point
{
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
    public readonly double x;
    public readonly double y;
}

public struct Hex
{
    public Hex(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }
    public string CoordString()
    {
        string txt = "(" + q + "," + r + "," + s + ")";
        return txt;
    }
    public readonly int q;
    public readonly int r;
    public readonly int s;

    static public Hex Add(Hex a, Hex b)
    {
        return new Hex(a.q + b.q, a.r + b.r, a.s + b.s);
    }


    static public Hex Subtract(Hex a, Hex b)
    {
        return new Hex(a.q - b.q, a.r - b.r, a.s - b.s);
    }


    static public Hex Scale(Hex a, int k)
    {
        return new Hex(a.q * k, a.r * k, a.s * k);
    }

    static public List<Hex> directions = new List<Hex>{new Hex(1, 0, -1), new Hex(1, -1, 0), new Hex(0, -1, 1), new Hex(-1, 0, 1), new Hex(-1, 1, 0), new Hex(0, 1, -1)};

    static public Hex Direction(int direction)
    {
        return Hex.directions[direction];
    }


    static public Hex Neighbor(Hex hex, int direction)
    {
        return Hex.Add(hex, Hex.Direction(direction));
    }

    static public List<Hex> diagonals = new List<Hex>{new Hex(2, -1, -1), new Hex(1, -2, 1), new Hex(-1, -1, 2), new Hex(-2, 1, 1), new Hex(-1, 2, -1), new Hex(1, 1, -2)};

    static public Hex DiagonalNeighbor(Hex hex, int direction)
    {
        return Hex.Add(hex, Hex.diagonals[direction]);
    }


    static public int Length(Hex hex)
    {
        return (int)((Math.Abs(hex.q) + Math.Abs(hex.r) + Math.Abs(hex.s)) / 2);
    }


    static public int Distance(Hex a, Hex b)
    {
        return Hex.Length(Hex.Subtract(a, b));
    }

		static public int CheckDirectionHex(Hex h)
		{
			for (int i = 0; i < 6; i++) {
			if (Hex.Distance( Hex.directions [i], h)==0) {
					return i;
				}
			}
			return -1;
		}

}

struct FractionalHex
{
    public FractionalHex(double q, double r, double s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }
    public readonly double q;
    public readonly double r;
    public readonly double s;

    static public Hex HexRound(FractionalHex h)
    {
        int q = (int)(Math.Round(h.q));
        int r = (int)(Math.Round(h.r));
        int s = (int)(Math.Round(h.s));
        double q_diff = Math.Abs(q - h.q);
        double r_diff = Math.Abs(r - h.r);
        double s_diff = Math.Abs(s - h.s);
        if (q_diff > r_diff && q_diff > s_diff)
        {
            q = -r - s;
        }
        else
            if (r_diff > s_diff)
            {
                r = -q - s;
            }
            else
            {
                s = -q - r;
            }
        return new Hex(q, r, s);
    }

    static public FractionalHex HexLerp(FractionalHex a, FractionalHex b, double t)
    {
        return new FractionalHex(a.q * (1 - t) + b.q * t, a.r * (1 - t) + b.r * t, a.s * (1 - t) + b.s * t);
    }

    static public List<Hex> HexLinedraw(Hex a, Hex b)
    {
        int N = Hex.Distance(a, b);
        FractionalHex a_nudge = new FractionalHex(a.q + 0.000001, a.r + 0.000001, a.s - 0.000002);
        FractionalHex b_nudge = new FractionalHex(b.q + 0.000001, b.r + 0.000001, b.s - 0.000002);
        List<Hex> results = new List<Hex>{};
        double step = 1.0 / Math.Max(N, 1);
        for (int i = 0; i <= N; i++)
        {
            results.Add(FractionalHex.HexRound(FractionalHex.HexLerp(a_nudge, b_nudge, step * i)));
        }
        return results;
    }

}

struct OffsetCoord
{
    public OffsetCoord(int col, int row)
    {
        this.col = col;
        this.row = row;
    }
    public readonly int col;
    public readonly int row;
    static public int EVEN = 1;
    static public int ODD = -1;

    static public OffsetCoord QoffsetFromCube(int offset, Hex h)
    {
        int col = h.q;
        int row = h.r + (int)((h.q + offset * (h.q & 1)) / 2);
        return new OffsetCoord(col, row);
    }


    static public Hex QoffsetToCube(int offset, OffsetCoord h)
    {
        int q = h.col;
        int r = h.row - (int)((h.col + offset * (h.col & 1)) / 2);
        int s = -q - r;
        return new Hex(q, r, s);
    }


    static public OffsetCoord RoffsetFromCube(int offset, Hex h)
    {
        int col = h.q + (int)((h.r + offset * (h.r & 1)) / 2);
        int row = h.r;
        return new OffsetCoord(col, row);
    }


    static public Hex RoffsetToCube(int offset, OffsetCoord h)
    {
        int q = h.col - (int)((h.row + offset * (h.row & 1)) / 2);
        int r = h.row;
        int s = -q - r;
        return new Hex(q, r, s);
    }

}

struct Orientation
{
    public Orientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle)
    {
        this.f0 = f0;
        this.f1 = f1;
        this.f2 = f2;
        this.f3 = f3;
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
        this.b3 = b3;
        this.start_angle = start_angle;
    }
    public readonly double f0;
    public readonly double f1;
    public readonly double f2;
    public readonly double f3;
    public readonly double b0;
    public readonly double b1;
    public readonly double b2;
    public readonly double b3;
    public readonly double start_angle;
}



struct Layout
{
    public Layout(Orientation orientation, Point size, Point origin)
    {
        this.orientation = orientation;
        this.size = size;
        this.origin = origin;
    }
    public readonly Orientation orientation;
    public readonly Point size;
    public readonly Point origin;
    static public Orientation pointy = new Orientation(
        Math.Sqrt(3.0), 
        Math.Sqrt(3.0) / 2.0, 
        0.0, 
        3.0 / 2.0, 
        Math.Sqrt(3.0) / 3.0, 
        -1.0 / 3.0, 
        0.0, 
        2.0 / 3.0, 
        0.5);
    static public Orientation flat = new Orientation(
        3.0 / 2.0, 
        0.0, 
        Math.Sqrt(3.0) / 2.0, 
        Math.Sqrt(3.0), 
        2.0 / 3.0, 
        0.0, 
        -1.0 / 3.0, 
        Math.Sqrt(3.0) / 3.0, 
        0.0);

    static public Point HexToPixel(Layout layout, Hex h)
    {
        Orientation M = layout.orientation;
        Point size = layout.size;
        Point origin = layout.origin;
        double x = (M.f0 * h.q + M.f1 * h.r) * size.x;
        double y = (M.f2 * h.q + M.f3 * h.r) * size.y;
        return new Point(x + origin.x, y + origin.y);
    }

    static public FractionalHex PixelToHex(Layout layout, Point p)
    {
        Orientation M = layout.orientation;
        Point size = layout.size;
        Point origin = layout.origin;
        Point pt = new Point((p.x - origin.x) / size.x, (p.y - origin.y) / size.y);
        double q = M.b0 * pt.x + M.b1 * pt.y;
        double r = M.b2 * pt.x + M.b3 * pt.y;
        return new FractionalHex(q, r, -q - r);
    }


    static public Point HexCornerOffset(Layout layout, int corner)
    {
        Orientation M = layout.orientation;
        Point size = layout.size;
        double angle = 2.0 * Math.PI * (corner + M.start_angle) / 6;
        return new Point(size.x * Math.Cos(angle), size.y * Math.Sin(angle));
    }


    static public List<Point> PolygonCorners(Layout layout, Hex h)
    {
        List<Point> corners = new List<Point>{};
        Point center = Layout.HexToPixel(layout, h);
        for (int i = 0; i < 6; i++)
        {
            Point offset = Layout.HexCornerOffset(layout, i);
            corners.Add(new Point(center.x + offset.x, center.y + offset.y));
        }
        return corners;
    }

}



// Tests


struct Tests
{

    static public void EqualHex(String name, Hex a, Hex b)
    {
        if (!(a.q == b.q && a.s == b.s && a.r == b.r))
        {
            Tests.Complain(name);
        }
    }


    static public void EqualOffsetcoord(String name, OffsetCoord a, OffsetCoord b)
    {
        if (!(a.col == b.col && a.row == b.row))
        {
            Tests.Complain(name);
        }
    }


    static public void EqualInt(String name, int a, int b)
    {
        if (!(a == b))
        {
            Tests.Complain(name);
        }
    }


    static public void EqualHexArray(String name, List<Hex> a, List<Hex> b)
    {
        Tests.EqualInt(name, a.Count, b.Count);
        for (int i = 0; i < a.Count; i++)
        {
            Tests.EqualHex(name, a[i], b[i]);
        }
    }


    static public void TestHexArithmetic()
    {
        Tests.EqualHex("hex_add", new Hex(4, -10, 6), Hex.Add(new Hex(1, -3, 2), new Hex(3, -7, 4)));
        Tests.EqualHex("hex_subtract", new Hex(-2, 4, -2), Hex.Subtract(new Hex(1, -3, 2), new Hex(3, -7, 4)));
    }


    static public void TestHexDirection()
    {
        Tests.EqualHex("hex_direction", new Hex(0, -1, 1), Hex.Direction(2));
    }


    static public void TestHexNeighbor()
    {
        Tests.EqualHex("hex_neighbor", new Hex(1, -3, 2), Hex.Neighbor(new Hex(1, -2, 1), 2));
    }


    static public void TestHexDiagonal()
    {
        Tests.EqualHex("hex_diagonal", new Hex(-1, -1, 2), Hex.DiagonalNeighbor(new Hex(1, -2, 1), 3));
    }


    static public void TestHexDistance()
    {
        Tests.EqualInt("hex_distance", 7, Hex.Distance(new Hex(3, -7, 4), new Hex(0, 0, 0)));
    }


    static public void TestHexRound()
    {
        FractionalHex a = new FractionalHex(0, 0, 0);
        FractionalHex b = new FractionalHex(1, -1, 0);
        FractionalHex c = new FractionalHex(0, -1, 1);
        Tests.EqualHex("hex_round 1", new Hex(5, -10, 5), FractionalHex.HexRound(FractionalHex.HexLerp(new FractionalHex(0, 0, 0), new FractionalHex(10, -20, 10), 0.5)));
        Tests.EqualHex("hex_round 2", FractionalHex.HexRound(a), FractionalHex.HexRound(FractionalHex.HexLerp(a, b, 0.499)));
        Tests.EqualHex("hex_round 3", FractionalHex.HexRound(b), FractionalHex.HexRound(FractionalHex.HexLerp(a, b, 0.501)));
        Tests.EqualHex("hex_round 4", FractionalHex.HexRound(a), FractionalHex.HexRound(new FractionalHex(a.q * 0.4 + b.q * 0.3 + c.q * 0.3, a.r * 0.4 + b.r * 0.3 + c.r * 0.3, a.s * 0.4 + b.s * 0.3 + c.s * 0.3)));
        Tests.EqualHex("hex_round 5", FractionalHex.HexRound(c), FractionalHex.HexRound(new FractionalHex(a.q * 0.3 + b.q * 0.3 + c.q * 0.4, a.r * 0.3 + b.r * 0.3 + c.r * 0.4, a.s * 0.3 + b.s * 0.3 + c.s * 0.4)));
    }


    static public void TestHexLinedraw()
    {
        Tests.EqualHexArray("hex_linedraw", new List<Hex>{new Hex(0, 0, 0), new Hex(0, -1, 1), new Hex(0, -2, 2), new Hex(1, -3, 2), new Hex(1, -4, 3), new Hex(1, -5, 4)}, FractionalHex.HexLinedraw(new Hex(0, 0, 0), new Hex(1, -5, 4)));
    }


    static public void TestLayout()
    {
        Hex h = new Hex(3, 4, -7);
        Layout flat = new Layout(Layout.flat, new Point(10, 15), new Point(35, 71));
        Tests.EqualHex("layout", h, FractionalHex.HexRound(Layout.PixelToHex(flat, Layout.HexToPixel(flat, h))));
        Layout pointy = new Layout(Layout.pointy, new Point(10, 15), new Point(35, 71));
        Tests.EqualHex("layout", h, FractionalHex.HexRound(Layout.PixelToHex(pointy, Layout.HexToPixel(pointy, h))));
    }


    static public void TestConversionRoundtrip()
    {
        Hex a = new Hex(3, 4, -7);
        OffsetCoord b = new OffsetCoord(1, -3);
        Tests.EqualHex("conversion_roundtrip even-q", a, OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, a)));
        Tests.EqualOffsetcoord("conversion_roundtrip even-q", b, OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, b)));
        Tests.EqualHex("conversion_roundtrip odd-q", a, OffsetCoord.QoffsetToCube(OffsetCoord.ODD, OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, a)));
        Tests.EqualOffsetcoord("conversion_roundtrip odd-q", b, OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, OffsetCoord.QoffsetToCube(OffsetCoord.ODD, b)));
        Tests.EqualHex("conversion_roundtrip even-r", a, OffsetCoord.RoffsetToCube(OffsetCoord.EVEN, OffsetCoord.RoffsetFromCube(OffsetCoord.EVEN, a)));
        Tests.EqualOffsetcoord("conversion_roundtrip even-r", b, OffsetCoord.RoffsetFromCube(OffsetCoord.EVEN, OffsetCoord.RoffsetToCube(OffsetCoord.EVEN, b)));
        Tests.EqualHex("conversion_roundtrip odd-r", a, OffsetCoord.RoffsetToCube(OffsetCoord.ODD, OffsetCoord.RoffsetFromCube(OffsetCoord.ODD, a)));
        Tests.EqualOffsetcoord("conversion_roundtrip odd-r", b, OffsetCoord.RoffsetFromCube(OffsetCoord.ODD, OffsetCoord.RoffsetToCube(OffsetCoord.ODD, b)));
    }


    static public void TestOffsetFromCube()
    {
        Tests.EqualOffsetcoord("offset_from_cube even-q", new OffsetCoord(1, 3), OffsetCoord.QoffsetFromCube(OffsetCoord.EVEN, new Hex(1, 2, -3)));
        Tests.EqualOffsetcoord("offset_from_cube odd-q", new OffsetCoord(1, 2), OffsetCoord.QoffsetFromCube(OffsetCoord.ODD, new Hex(1, 2, -3)));
    }


    static public void TestOffsetToCube()
    {
        Tests.EqualHex("offset_to_cube even-", new Hex(1, 2, -3), OffsetCoord.QoffsetToCube(OffsetCoord.EVEN, new OffsetCoord(1, 3)));
        Tests.EqualHex("offset_to_cube odd-q", new Hex(1, 2, -3), OffsetCoord.QoffsetToCube(OffsetCoord.ODD, new OffsetCoord(1, 2)));
    }


    static public void TestAll()
    {
        Tests.TestHexArithmetic();
        Tests.TestHexDirection();
        Tests.TestHexNeighbor();
        Tests.TestHexDiagonal();
        Tests.TestHexDistance();
        Tests.TestHexRound();
        Tests.TestHexLinedraw();
        Tests.TestLayout();
        Tests.TestConversionRoundtrip();
        Tests.TestOffsetFromCube();
        Tests.TestOffsetToCube();
    }


    static public void Main()
    {
        Tests.TestAll();
    }


    static public void Complain(String name)
    {
        Console.WriteLine("FAIL " + name);
    }

}

