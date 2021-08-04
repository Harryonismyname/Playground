using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>Custom Node for use in GridMap2D</summary>
public class GridNode2D<TGridObject>
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public GridMap2D<TGridObject> Grid { get; }

    public GridNode2D(GridMap2D<TGridObject> grid, int x, int y)
    {
        Grid = grid;
        X = x;
        Y = y;
    }
    public Vector3 GetWorldPosition()
    {
        return Grid.GetCellCenterWorld(X, Y);
    }
}
