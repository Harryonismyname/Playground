using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridTools2D<TGridObject>
{
    public static void GetXY(GridMap2D<TGridObject> grid, Vector3 worldPosition, Axis axis, out int x, out int y)
    {
        switch (axis)
        {
            case Axis.X:
                {
                    x = Mathf.FloorToInt(worldPosition.z / grid.cellSize);
                    y = Mathf.FloorToInt(worldPosition.y / grid.cellSize);
                    break;
                }
            case Axis.Y:
                {
                    x = Mathf.FloorToInt(worldPosition.x / grid.cellSize);
                    y = Mathf.FloorToInt(worldPosition.z / grid.cellSize);
                    break;
                }
            case Axis.Z:
                {
                    x = Mathf.FloorToInt(worldPosition.x / grid.cellSize);
                    y = Mathf.FloorToInt(worldPosition.y / grid.cellSize);
                    break;
                }
        }
        x = 0;
        y = 0;
    }
    public static bool IsValidCell(GridMap2D<TGridObject> grid, int x, int y)
    {
        if (x >= 0 && y >= 0 && x <= grid.Width - 1 && y <= grid.Height - 1) return true;
        else return false;
    }
    public static bool IsValidCell(GridMap2D<TGridObject> grid, Vector3 location)
    {
        GetXY(grid, location, grid.orientation, out int x, out int y);
        return IsValidCell(grid, x, y);
    }
}
