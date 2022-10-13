using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    List<PathNode> openList;
    List<PathNode> closedList;

    public List<Vector3> FindPath(Vector3Int startPos, Vector3Int endPos, Tilemap tileMap)
    {
        PathNode startNode = new PathNode(startPos.x, startPos.y);
        PathNode endNode = new PathNode(endPos.x, endPos.y);
        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0 && openList.Count < 3000)
        {
            PathNode currentNode = GetLowestFCostNode();

            if (currentNode.x == endNode.x && currentNode.y == endNode.y)
            {
                return CalculatePath(currentNode, tileMap);
            }

            foreach (PathNode neighbourNode in GetNeighboursList(currentNode, tileMap))
            {
                bool isNeighbourNodeInList = false;
                foreach(PathNode node in closedList)
                {
                    if (neighbourNode.x == node.x && neighbourNode.y == node.y) isNeighbourNodeInList = true;
                    break;
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
        BaseTile tile;

        //Left
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x - 1, currentNode.y, 0));
        if (tile.isPassable)
        {
            neighbourList.Add(PathNode.GetNode(currentNode.x - 1, currentNode.y));
        }
        //Left up
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x - 1, currentNode.y + 1, 0));
        if (tile.isPassable)
        {
            neighbourList.Add(PathNode.GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        //Left down
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x - 1, currentNode.y - 1, 0));
        if (tile.isPassable)
        {
            neighbourList.Add(PathNode.GetNode(currentNode.x - 1, currentNode.y - 1));
        }
        //Right
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x + 1, currentNode.y, 0));
        if (tile.isPassable)
        {
            neighbourList.Add(PathNode.GetNode(currentNode.x + 1, currentNode.y));
        }
        //Right up
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x + 1, currentNode.y + 1, 0));
        if (tile.isPassable)
            neighbourList.Add(PathNode.GetNode(currentNode.x + 1, currentNode.y + 1));
        //Right down
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x + 1, currentNode.y - 1, 0));
        if (tile.isPassable)
            neighbourList.Add(PathNode.GetNode(currentNode.x + 1, currentNode.y - 1));
        //Up
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x, currentNode.y + 1, 0));
        if (tile.isPassable)
        {
            neighbourList.Add(PathNode.GetNode(currentNode.x, currentNode.y + 1));
        }
        //Down
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x, currentNode.y - 1, 0));
        if (tile.isPassable)
        {
            neighbourList.Add(PathNode.GetNode(currentNode.x, currentNode.y - 1));
        }

        foreach(PathNode neighbourNode in neighbourList)
        {
            neighbourNode.gCost = int.MaxValue;
        }

        return neighbourList;
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
