using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>Custom Node for storing data inside GridMap3D</summary>
public class GridNode3D<TGridObject>
{
    public GridMap3D<TGridObject> Grid { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }
    public GridNode3D(GridMap3D<TGridObject> grid, int x, int y, int z)
    {
        Grid = grid;
        X = x;
        Y = y;
        Z = z;
    }
    public Vector3 GetWorldPosition()
    {
        return Grid.GetCellCenterWorld(X, Y, Z);
    }
}
