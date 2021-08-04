using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridTools3D<TGridObject>
{
    public static void GetXYZ(GridMap3D<TGridObject> grid, Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x / grid.cellSize);
        y = Mathf.FloorToInt(worldPosition.y / grid.cellSize);
        z = Mathf.FloorToInt(worldPosition.z / grid.cellSize);
    }

    public static bool IsValidCell(GridMap3D<TGridObject> grid, int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && x <= grid.Width - 1 && y <= grid.Height - 1 && z >= 0 && z <= grid.Depth) return true;
        else return false;
    }
    public static bool IsValidCell(GridMap3D<TGridObject> grid, Vector3 location)
    {
        GetXYZ(grid, location, out int x, out int y, out int z);
        return IsValidCell(grid, x, y, z);
    }
    public static List<TGridObject> GetNeighborList(GridMap3D<TGridObject> grid, Vector3 nodePosition)
    {
        List<TGridObject> neighborList = new List<TGridObject>();
        GetXYZ(grid, nodePosition, out int x, out int y, out int z);
        TGridObject GetNode(int nodeX, int nodeY, int nodeZ)
        {
            if (IsValidCell(grid, nodeX, nodeY, nodeZ)) return grid.GetGridObject(nodeX, nodeY, nodeZ);
            else return default;
        }
        if (x - 1 >= 0)
        {
            // Left
            if (IsValidCell(grid, x - 1, y, z)) neighborList.Add(GetNode(x - 1, y, z));
            // Left Down
            if (y - 1 >= 0 && IsValidCell(grid, x - 1, y - 1, z)) neighborList.Add(GetNode(x - 1, y - 1, z));
            // Left Up
            if (y + 1 < grid.Height && IsValidCell(grid, x - 1, y + 1, z)) neighborList.Add(GetNode(x - 1, y + 1, z));
        }
        if (x + 1 < grid.Width)
        {
            // Right
            if (IsValidCell(grid, x + 1, y, z)) neighborList.Add(GetNode(x + 1, y, z));
            // Right Down
            if (y - 1 >= 0 && IsValidCell(grid, x + 1, y - 1, z)) neighborList.Add(GetNode(x + 1, y - 1, z));
            // Right Up
            if (y + 1 < grid.Height && IsValidCell(grid, x + 1, y + 1, z)) neighborList.Add(GetNode(x + 1, y + 1, z));
        }
        // Down
        if (y - 1 >= 0 && IsValidCell(grid, x, y - 1, z)) neighborList.Add(GetNode(x, y - 1, z));
        // Up
        if (y + 1 < grid.Height && IsValidCell(grid, x, y + 1, z)) neighborList.Add(GetNode(x, y + 1, z));
        return neighborList;
    }
    public static void DebugCell(GridMap3D<TGridObject> map, Vector3 location, Color color)
    {
        GetXYZ(map, location, out int x, out int y, out int z);
        Vector3 leftCorner = map.GetWorldPosition(x, y, z);
        Vector3 rightCorner = map.GetWorldPosition(x, y, z);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
    public static void DebugCell(GridMap3D<TGridObject> map, int x, int y, Color color)
    {
        Vector3 leftCorner = map.GetWorldPosition(x, y);
        Vector3 rightCorner = map.GetWorldPosition(x, y);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
}
