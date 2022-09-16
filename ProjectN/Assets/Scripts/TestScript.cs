using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScript : MonoBehaviour
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
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3Int cellPosition = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            TileBase clickedTile = tileMap.GetTile(cellPosition);

            bool isWalkable = dataFromTiles[clickedTile].isWalkable;
            Debug.Log("This tile is walkable: " + isWalkable);
        }
    }
}
