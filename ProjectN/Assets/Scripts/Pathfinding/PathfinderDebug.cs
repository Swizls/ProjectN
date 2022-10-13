using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfinderDebug : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private float _updateSpeed = 0.001f;
    [SerializeField] private bool _isDebugging = false;

    List<PathNode> openList;
    List<PathNode> closedList;
    List<GameObject> debugObjects;

    public IEnumerator FindPath(Vector3Int startPos, Vector3Int endPos, Tilemap tileMap)
    {
        if(_isDebugging)
        { 
            PathNode startNode = new PathNode(startPos.x, startPos.y);
            PathNode endNode = new PathNode(endPos.x, endPos.y);
            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();
            debugObjects = new List<GameObject>();

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0 && openList.Count < 3000)
            {
                PathNode currentNode = GetLowestFCostNode();
                if (currentNode.x == endNode.x && currentNode.y == endNode.y)
                {
                    Debug.Log("Target is reached!");
                    yield break;
                }

                int j = 0;
                foreach (PathNode neighbourNode in GetNeighboursList(currentNode, tileMap))
                {
                    bool nodeInList = false;
                    foreach(PathNode node in closedList)
                    {
                        if (neighbourNode.x == node.x && neighbourNode.y == node.y) nodeInList = true;
                    }
                    if (nodeInList) continue;

                    //DEBUG
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.position = new Vector3(neighbourNode.x + 0.5f, neighbourNode.y + 0.5f, -1);
                    quad.name = "Quad " + j;
                    j++;
                    debugObjects.Add(quad);
                    //DEBUG

                    int tentativGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                    }
                    nodeInList = false;
                    foreach (PathNode node in openList)
                    {
                        if (neighbourNode.x == node.x && neighbourNode.y == node.y)
                        {
                            nodeInList = true;
                        }
                    }
                    if(!nodeInList) openList.Add(neighbourNode);
                    yield return new WaitForSeconds(_updateSpeed);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (GameObject obj in debugObjects)
                {
                    Destroy(obj);
                }
                debugObjects.Clear();
            }
            Debug.Log("There is no path!");
            yield break;
        }
    }

    //private List<Vector3> CalculatePath(PathNode endNode, Tilemap tileMap)
    //{
    //    List<Vector3> path = new List<Vector3>();
    //    PathNode currentNode = endNode;
    //    while (currentNode.cameFromNode != null)
    //    {
    //        path.Add(tileMap.GetCellCenterWorld(new Vector3Int(currentNode.x, currentNode.y, 0)));
    //        currentNode = currentNode.cameFromNode;
    //    }
    //    path.Reverse();
    //    return path;
    //}

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
            neighbourList.Add(PathNode.GetNode(currentNode.x - 1, currentNode.y + 1));
        //Left down
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x - 1, currentNode.y - 1, 0));
        if (tile.isPassable)
            neighbourList.Add(PathNode.GetNode(currentNode.x - 1, currentNode.y - 1));
        //Right
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x + 1, currentNode.y, 0));
        if (tile.isPassable)
            neighbourList.Add(PathNode.GetNode(currentNode.x + 1, currentNode.y));
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
            neighbourList.Add(PathNode.GetNode(currentNode.x, currentNode.y + 1));
        //Down
        tile = tileMap.GetTile<BaseTile>(new Vector3Int(currentNode.x, currentNode.y - 1, 0));
        if (tile.isPassable)
            neighbourList.Add(PathNode.GetNode(currentNode.x, currentNode.y - 1));

        foreach (PathNode neighbourNode in neighbourList)
        {
            neighbourNode.gCost = int.MaxValue;
        }

        return neighbourList;
    }
    private PathNode GetLowestFCostNode()
    {
        PathNode lowestFCostNode = openList[0];
        foreach (PathNode node in openList)
            if (lowestFCostNode.fCost > node.fCost) lowestFCostNode = node;
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