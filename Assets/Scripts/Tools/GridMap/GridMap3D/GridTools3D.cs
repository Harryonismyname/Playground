using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>List of tools for performing operations on 3D GridMaps</summary>
public static class GridTools3D<TGridObject>
{
    ///<summary>Recieves grid and Vector3 position in the world, returns XYZ coordinates within parent grid</summary>
    public static void GetXYZ(GridMap3D<TGridObject> grid, Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x / grid.cellSize);
        y = Mathf.FloorToInt(worldPosition.y / grid.cellSize);
        z = Mathf.FloorToInt(worldPosition.z / grid.cellSize);
    }
    ///<summary>Recieves parent grid and XYZ coordinates returns true if coordinates are within the extremes of the parent grid</summary>
    public static bool IsValidCell(GridMap3D<TGridObject> grid, int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && x <= grid.Width - 1 && y <= grid.Height - 1 && z >= 0 && z <= grid.Depth) return true;
        else return false;
    }
    ///<summary>Recieves parent grid and Vector3 location returns true if Vector3 is within the extremes of the parent grid</summary>
    public static bool IsValidCell(GridMap3D<TGridObject> grid, Vector3 location)
    {
        GetXYZ(grid, location, out int x, out int y, out int z);
        return IsValidCell(grid, x, y, z);
    }
    ///<summary>Recieves perent grid and Vector3 location of GridNode and returns list of nodes adjacent to the passed node</summary>
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
    ///<summary>Draws Debug lines of passed grid at cell location with the passed color</summary>
    public static void DebugCell(GridMap3D<TGridObject> map, Vector3 location, Color color)
    {
        GetXYZ(map, location, out int x, out int y, out int z);
        Vector3 leftCorner = map.GetWorldPosition(x, y, z);
        Vector3 rightCorner = map.GetWorldPosition(x, y, z);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
    ///<summary>Draws Debug lines of passed grid at cell location with the passed color</summary>
    public static void DebugCell(GridMap3D<TGridObject> map, int x, int y, Color color)
    {
        Vector3 leftCorner = map.GetWorldPosition(x, y);
        Vector3 rightCorner = map.GetWorldPosition(x, y);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
}
