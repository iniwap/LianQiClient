using System.Collections;
using System.Collections.Generic;

public struct Vector3i{
    public int x;
    public int y;
    public int z;

    public Vector3i(int xx, int yy, int zz) {
        x = xx;
        y = yy;
        z = zz;
    }

    public static bool operator ==(Vector3i lv, Vector3i rv) {
        return (lv.x == rv.x &&
            lv.y == rv.y &&
            lv.z == rv.z);
    }

    public static bool operator !=(Vector3i lv, Vector3i rv)
    {
        return (lv.x != rv.x ||
            lv.y != rv.y ||
            lv.z != rv.z);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public  bool Equals(Vector3i other) {
        if (other.z >= 0) {
            if (this.x == other.x && this.y == other.y) {
                return true;
            }
        }

        return false;
    }

    public override int GetHashCode()
    {
        return x^y^z;
    }

    public override string ToString()
    {
        return "" + x + ", " + y + ", " + z;
    }
}

