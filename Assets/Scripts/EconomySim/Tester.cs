using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>Defunct Class</summary>
public class Tester : MonoBehaviour
{
    public int height;
    public int width;
    public WorldMap map;
    [SerializeField] MapVisual mapVisual;
    private TileType tile;

    public void Start()
    {
        GenerateMap();

    }

    private void Update()
    {
        Vector3 position = Tools.MouseToWorldPosition();
        if (Tools.NullCheck(map.GetNode(position)))
        {
            if (Input.GetMouseButtonDown(0))
            {
                map.SetProduct(position, tile);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(map.GetNode(position).GetState());
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdatePlacer(TileType.Stone);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            UpdatePlacer(TileType.Wheat);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdatePlacer(TileType.Wood);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdatePlacer(TileType.Wool);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            UpdatePlacer(TileType.Settlement);
        }
    }

    private void UpdatePlacer(TileType tile)
    {
        this.tile = tile;
    }

    public void GenerateMap()
    {
        WorldMap worldMap = new WorldMap(width, height, 0);
        map = worldMap;
        mapVisual.SetMap(map);
    }
}
