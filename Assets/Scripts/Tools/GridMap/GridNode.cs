using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode<TGridObject>
{
    public GridMap3D<TGridObject> Grid { get; private set; }
    public int x { get; private set; }
    public int y { get; private set; }
    public int z { get; private set; }
    public GridNode(GridMap3D<TGridObject> grid, int x, int y, int z)
    {
        Grid = grid;
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3 GetWorldPosition()
    {
        return Grid.GetCellCenterWorld(x, y, z);
    }
}
