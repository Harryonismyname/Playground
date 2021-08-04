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
    public static List<TGridObject> GetNeighborList(GridMap2D<TGridObject> grid, Vector3 nodePosition)
    {
        List<TGridObject> neighborList = new List<TGridObject>();
        GetXY(grid, nodePosition, grid.orientation, out int x, out int y);
        TGridObject GetNode(int nodeX, int nodeY)
        {
            if (IsValidCell(grid, nodeX, nodeY)) return grid.GetGridObject(nodeX, nodeY);
            else return default;
        }
        if (x - 1 >= 0)
        {
            // Left
            if (IsValidCell(grid, x - 1, y)) neighborList.Add(GetNode(x - 1, y));
            // Left Down
            if (y - 1 >= 0 && IsValidCell(grid, x - 1, y - 1)) neighborList.Add(GetNode(x - 1, y - 1));
            // Left Up
            if (y + 1 < grid.Height && IsValidCell(grid, x - 1, y + 1)) neighborList.Add(GetNode(x - 1, y + 1));
        }
        if (x + 1 < grid.Width)
        {
            // Right
            if (IsValidCell(grid, x + 1, y)) neighborList.Add(GetNode(x + 1, y));
            // Right Down
            if (y - 1 >= 0 && IsValidCell(grid, x + 1, y - 1)) neighborList.Add(GetNode(x + 1, y - 1));
            // Right Up
            if (y + 1 < grid.Height && IsValidCell(grid, x + 1, y + 1)) neighborList.Add(GetNode(x + 1, y + 1));
        }
        // Down
        if (y - 1 >= 0 && IsValidCell(grid, x, y - 1)) neighborList.Add(GetNode(x, y - 1));
        // Up
        if (y + 1 < grid.Height && IsValidCell(grid, x, y + 1)) neighborList.Add(GetNode(x, y + 1));
        return neighborList;
    }
    public static void DebugCell(GridMap2D<TGridObject> map, Vector3 location, Color color)
    {
        GetXY(map, location, map.orientation, out int x, out int y);
        Vector3 leftCorner = map.GetWorldPosition(x, y);
        Vector3 rightCorner = map.GetWorldPosition(x, y);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
    public static void DebugCell(GridMap2D<TGridObject> map, int x, int y, Color color)
    {
        Vector3 leftCorner = map.GetWorldPosition(x, y);
        Vector3 rightCorner = map.GetWorldPosition(x, y);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
}
