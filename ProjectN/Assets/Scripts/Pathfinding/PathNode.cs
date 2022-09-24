using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int x;
    public int y;

    //Walking Cost from the Start Node
    public int gCost;
    //Heuristic Cost to reach End Node
    public int hCost;
    //Final Cost (f = g + h)
    public int fCost;

    public PathNode cameFromNode;

    public PathNode(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public static PathNode GetNode(int x, int y)
    {
        PathNode pathNode = new PathNode(x, y);
        return pathNode;
    }
}
