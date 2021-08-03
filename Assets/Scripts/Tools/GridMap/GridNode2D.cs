using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode2D<TGridObject>
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public GridMap2D<TGridObject> Grid { get; }

    public GridNode2D(int x, int y, GridMap2D<TGridObject> grid)
    {
        X = x;
        Y = y;
        Grid = grid;
    }
    public Vector3 GetWorldPosition()
    {
        return Grid.GetCellCenterWorld(X, Y);
    }
}
