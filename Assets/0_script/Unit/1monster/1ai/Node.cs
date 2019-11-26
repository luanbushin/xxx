
using UnityEngine;

public class Node
{
    public int mapindex;
    public Vector3 _worldPos;
    public int _gridX, _gridY;

    public int gCost;
    public int hCost;

    public int fCost
    {
        get { return gCost + hCost; }
    }
    public Node parent;
    public Node(int _mapindex, Vector3 _worldPos, int _gridX, int _gridY)
    {
        this.mapindex = _mapindex;
        this._worldPos = _worldPos;
        this._gridX = _gridX;
        this._gridY = _gridY;
    }
}
