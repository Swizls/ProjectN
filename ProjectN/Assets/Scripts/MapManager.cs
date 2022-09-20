using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;
    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }
    public bool isWalkable(Vector2 worldPos)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(worldPos);
        TileBase clickedTile = tileMap.GetTile(cellPosition);

        if (clickedTile == null) return false;
        return dataFromTiles[clickedTile].isWalkable;
    }
}
