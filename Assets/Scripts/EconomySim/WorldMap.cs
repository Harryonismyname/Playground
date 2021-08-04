using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap
{
    public bool debugging = false;
    public GridMap2D<MapNode> Grid { get; private set; }
    private float settlementRadius = 1.5f;
    private int settlementRejectionSamples = 5;

    public System.EventHandler<OnMapUpdateArgs> OnMapUpdate;

    public class OnMapUpdateArgs : System.EventArgs
    {
        public GridMap2D<MapNode> Grid;
    }

    public WorldMap(int width, int height, int depth)
    {
        Grid = new GridMap2D<MapNode>(height, width, 1f, Axis.Z, Vector3.zero, (GridMap2D<MapNode> g, int x, int y) => new MapNode(g, x, y));
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                SetProduct(x, y, Tools.RandomEnum<TileType>());
                if (debugging)
                {
                    if (GetNode(x, y).GetState() == TileType.Wood)
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.green);
                    }
                    else if (GetNode(x, y).GetState() == TileType.Stone)
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.grey);
                    }
                    else if (GetNode(x, y).GetState() == TileType.Wheat)
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.yellow);
                    }
                    else if (GetNode(x, y).GetState() == TileType.Wool)
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.white);
                    }
                }
            }
        }
        List<Vector2> settlementSpawnPoints = DiscSampling.GeneratePoints(settlementRadius, new Vector2(GetWidth(), GetHeight()), Grid, settlementRejectionSamples);
        foreach (Vector2 point in settlementSpawnPoints)
        {
            PlaceSettlement(point);
        }
    }

    public int GetHeight()
    {
        return Grid.Height;
    }

    public int GetWidth()
    {
        return Grid.Width;
    }
    public void SetProduct(int x, int y, TileType product)
    {
        GetNode(x, y).SetProduct(product);
        if (debugging)
        {
            switch (GetNode(x, y).GetState())
            {
                case TileType.Wood:
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.green);
                        break;
                    }
                case TileType.Stone:
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.grey);
                        break;
                    }
                case TileType.Wheat:
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.yellow);
                        break;
                    }
                case TileType.Wool:
                    {
                        GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.white);
                        break;
                    }
            }
        }
        OnMapUpdate?.Invoke(this, new OnMapUpdateArgs { Grid = Grid });
    }
    public void SetProduct(Vector3 position, TileType product)
    {
        GridTools2D<MapNode>.GetXY(Grid, position, Grid.orientation, out int x, out int y);
        SetProduct(x, y, product);
    }

    public void PlaceSettlement(Vector3 location)
    {
        if (GridTools2D<MapNode>.IsValidCell(Grid, location) && Tools.NullCheck(GetNode(location)))
        {
            GetNode(location).SetNodeAsSettlement();
            if (debugging) GridTools2D<MapNode>.DebugCell(Grid, location, Color.black);
        }
    }
    public void PlaceSettlement(int x, int y)
    {
        if (GridTools2D<MapNode>.IsValidCell(Grid, x, y) && Tools.NullCheck(GetNode(x, y)))
        {
            GetNode(x, y).SetNodeAsSettlement();
            if (debugging) GridTools2D<MapNode>.DebugCell(Grid, x, y, Color.black);
        }
    }
    public MapNode GetNode(int x, int y)
    {
        if (GridTools2D<MapNode>.IsValidCell(Grid, x, y))
        {
            return Grid.GetGridObject(x, y);
        }
        else return null;
    }
    public MapNode GetNode(Vector3 location)
    {
        if (GridTools2D<MapNode>.IsValidCell(Grid, location))
        {
            GridTools2D<MapNode>.GetXY(Grid, location, Grid.orientation, out int x, out int y);
            return Grid.GetGridObject(x, y);
        }
        else return null;
    }
}
