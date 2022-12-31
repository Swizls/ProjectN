using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public List<Vector3> FindPath(Vector3 startPosFloat, Vector3 endPosFloat, Tilemap tileMap)
    {
        Vector3Int startPos = tileMap.WorldToCell(startPosFloat);
        Vector3Int endPos = tileMap.WorldToCell(endPosFloat);
        RuleBaseTile targetTile = tileMap.GetTile<RuleBaseTile>(endPos);
        if (CanPass(targetTile, new PathNode(endPos.x, endPos.y)))
        {
            PathNode startNode = new PathNode(startPos.x, startPos.y);
            PathNode endNode = new PathNode(endPos.x, endPos.y);
            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while(openList.Count > 0 && closedList.Count < 10000)
            {
                PathNode currentNode = GetLowestFCostNode();

                if (currentNode.x == endNode.x && currentNode.y == endNode.y)
                {
                    return CalculatePath(currentNode, tileMap);
                }

                foreach (PathNode neighbourNode in GetNeighboursList(currentNode, tileMap))
                {
                    bool isNeighbourNodeInList = false;
                    foreach (PathNode node in closedList)
                    {
                        if (neighbourNode.x == node.x && neighbourNode.y == node.y)
                        {
                            isNeighbourNodeInList = true;
                            break;
                        }
                    }
                    if (isNeighbourNodeInList)
                    {
                        continue;
                    }

                    int tentativGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if(tentativGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                    }
                    isNeighbourNodeInList = false;
                    foreach (PathNode node in openList)
                    {
                        if (neighbourNode.x == node.x && neighbourNode.y == node.y)
                        {
                            isNeighbourNodeInList = true;
                        }
                    }
                    if (!isNeighbourNodeInList)
                    {
                        openList.Add(neighbourNode);
                    }
                }
                openList.Remove(currentNode);
                closedList.Add(currentNode);
            }
        }
        return null;
    }

    private List<Vector3> CalculatePath(PathNode endNode, Tilemap tileMap)
    {
        List<Vector3> path = new List<Vector3>();
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(tileMap.GetCellCenterWorld(new Vector3Int(currentNode.x, currentNode.y, 0)));
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighboursList(PathNode currentNode, Tilemap tileMap)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        RuleBaseTile tile;

        PathNode nextNode = currentNode;

        bool isRightBlocked = false;
        bool isLeftBlocked = false;
        bool isUpBlocked = false;
        bool isDownBlocked = false;

        //Left
        nextNode = new PathNode(currentNode.x - 1, currentNode.y);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
        if (CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }
        else
        {
            isLeftBlocked = true;
        }
        //Right
        nextNode = new PathNode(currentNode.x + 1, currentNode.y);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
        if (CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }
        else
        {
            isRightBlocked = true;
        }
        //Up
        nextNode = new PathNode(currentNode.x, currentNode.y + 1);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
         if (CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }
        else
        {
            isUpBlocked = true;
        }
        //Down
        nextNode = new PathNode(currentNode.x, currentNode.y - 1);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
        if (CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }
        else
        {
            isDownBlocked = true;
        }
        //Left up
        nextNode = new PathNode(currentNode.x - 1, currentNode.y + 1);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
        if ((!isUpBlocked && !isLeftBlocked) && CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }
        //Left down
        nextNode = new PathNode(currentNode.x - 1, currentNode.y - 1);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
        if ((!isDownBlocked && !isLeftBlocked) && CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }
        //Right up
        nextNode = new PathNode(currentNode.x + 1, currentNode.y + 1);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
        if ((!isUpBlocked && !isRightBlocked) && CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }
        //Right down
        nextNode = new PathNode(currentNode.x + 1, currentNode.y - 1);
        tile = tileMap.GetTile<RuleBaseTile>(new Vector3Int(nextNode.x, nextNode.y, 0));
        if ((!isDownBlocked && !isRightBlocked) && CanPass(tile, nextNode))
        {
            neighbourList.Add(PathNode.GetNode(nextNode.x, nextNode.y));
        }

        foreach (PathNode neighbourNode in neighbourList)
        {
            neighbourNode.gCost = int.MaxValue;
        }

        return neighbourList;
    }

    private bool CanPass(RuleBaseTile tile, PathNode node)
    {
        Collider2D hit = Physics2D.OverlapPoint(new Vector2(node.x, node.y));

        if (hit != null)
            if (hit.TryGetComponent(out IObject obj))
                if (!obj.IsPassable)
                    return false;

        return tile != null && tile.isPassable;
    }

    private PathNode GetLowestFCostNode()
    {
        PathNode lowestFCostNode = openList[0];
        foreach(PathNode node in openList)
        {
            if(lowestFCostNode.fCost > node.fCost) lowestFCostNode = node;
        }
        return lowestFCostNode;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
}
