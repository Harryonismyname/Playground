using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap
{
    private GridMap<MapNode> grid;
    private float settlementRadius = 1.5f;
    private int settlementRejectionSamples = 5;

    public System.EventHandler<OnMapUpdateArgs> OnMapUpdate;

    public class OnMapUpdateArgs : System.EventArgs
    {
        public GridMap<MapNode> grid;
    }

    public WorldMap(int width, int height, int depth)
    {
        grid = new GridMap<MapNode>(height, width, 1f, Vector3.zero, (GridMap<MapNode> g, int x, int y, int z) => new MapNode(g, x, y, z), depth);
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int z = 0; z < grid.Depth; z++)
                {
                    TileType newNode = Tools.RandomEnum<TileType>();
                    SetProduct(x, y, z, newNode);
                    if (grid.debugging)
                    {
                        if (GetNode(x, y, z).GetState() == TileType.Wood)
                        {
                            GridTools<MapNode>.DebugCell(grid, x, y, Color.green);
                        }
                        else if (GetNode(x, y, z).GetState() == TileType.Stone)
                        {
                            GridTools<MapNode>.DebugCell(grid, x, y, Color.grey);
                        }
                        else if (GetNode(x, y, z).GetState() == TileType.Wheat)
                        {
                            GridTools<MapNode>.DebugCell(grid, x, y, Color.yellow);
                        }
                        else if (GetNode(x, y, z).GetState() == TileType.Wool)
                        {
                            GridTools<MapNode>.DebugCell(grid, x, y, Color.white);
                        }
                    }
                }
            }
        }
        List<Vector2> settlementSpawnPoints = DiscSampling.GeneratePoints(settlementRadius, new Vector2(grid.Width, grid.Height), grid, settlementRejectionSamples);
        foreach (Vector2 point in settlementSpawnPoints)
        {
            PlaceSettlement(point);
        }
    }

    public int GetHeight()
    {
        return grid.Height;
    }

    public int GetWidth()
    {
        return grid.Width;
    }

    public GridMap<MapNode> GetGrid()
    {
        return grid;
    }
    public void SetProduct(int x, int y, int z, TileType product)
    {
        grid.GetGridObject(x, y, z).SetProduct(product);
        if (grid.debugging)
        {
            if (GetNode(x, y, z).GetState() == TileType.Wood)
            {
                GridTools<MapNode>.DebugCell(grid, x, y, Color.green);
            }
            else if (GetNode(x, y, z).GetState() == TileType.Stone)
            {
                GridTools<MapNode>.DebugCell(grid, x, y, Color.grey);
            }
            else if (GetNode(x, y, z).GetState() == TileType.Wheat)
            {
                GridTools<MapNode>.DebugCell(grid, x, y, Color.yellow);
            }
            else if (GetNode(x, y, z).GetState() == TileType.Wool)
            {
                GridTools<MapNode>.DebugCell(grid, x, y, Color.white);
            }
        }

        OnMapUpdate?.Invoke(this, new OnMapUpdateArgs { grid = grid });
    }
    public void SetProduct(Vector3 position, TileType product)
    {
        GridTools<MapNode>.GetXYZ(grid, position, out int x, out int y, out int z);
        SetProduct(x, y, z, product);
    }

    public void PlaceSettlement(Vector3 location)
    {
        if (GridTools<MapNode>.IsValidCell(grid, location) && grid.GetGridObject(location) != null)
        {
            GridTools<MapNode>.GetXYZ(grid, location, out int x, out int y, out int z);
            grid.GetGridObject(x, y).SetNodeAsSettlement();
            if (grid.debugging)
            {
                if (grid.GetGridObject(x, y) != null)
                {
                    GridTools<MapNode>.DebugCell(grid, location, Color.black);
                }
            }
        }
    }
    public void PlaceSettlement(int x, int y, int z)
    {
        if (GridTools<MapNode>.IsValidCell(grid, x, y, z) && grid.GetGridObject(x, y) != null)
        {
            grid.GetGridObject(x, y).SetNodeAsSettlement();
            if (grid.debugging)
            {
                if (Tools.NullCheck(grid.GetGridObject(x, y)))
                {
                    GridTools<MapNode>.DebugCell(grid, x, y, Color.black);
                }
            }
        }
    }
    public MapNode GetNode(int x, int y, int z = 0)
    {
        if (GridTools<MapNode>.IsValidCell(grid, x, y, z))
        {
            return grid.GetGridObject(x, y, z);
        }
        else return null;
    }
    public MapNode GetNode(Vector3 location)
    {
        if (GridTools<MapNode>.IsValidCell(grid, location))
        {
            GridTools<MapNode>.GetXYZ(grid, location, out int x, out int y, out int z);
            return grid.GetGridObject(x, y, z);
        }
        else return null;
    }
}
