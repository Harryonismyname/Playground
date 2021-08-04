using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Collection of tools for working with GridMap2D
/// </summary>
public static class GridTools2D<TGridObject>
{
    /// <summary>
    /// Recieves Vector3 position and returns XY coordinates of the 2DGrid cell it intersects
    /// </summary>
    /// <param name="grid">The parent grid of the cell in question</param>
    /// <param name="worldPosition">The position being inspected</param>
    /// <param name="axis">The axis on which the grid is oriented. (Common Implementation: passing parent grid.orientation)</param>
    /// <param name="x">The returned X position of the inspected cell</param>
    /// <param name="y">The returned Y position of the inspected cell</param>
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
    /// <summary>
    /// Recieves parent grid and XY coordinates and assesses if the coordinates are contained within the parent grid.
    /// </summary>
    /// <param name="grid">The parent grid that the coordinates will be compared to</param>
    /// <param name="x">The X coordinate being assessed</param>
    /// <param name="y">The Y coordinate being assessed</param>
    /// <returns>bool</returns>
    public static bool IsValidCell(GridMap2D<TGridObject> grid, int x, int y)
    {
        if (x >= 0 && y >= 0 && x <= grid.Width - 1 && y <= grid.Height - 1) return true;
        else return false;
    }
    /// <summary>
    /// Recieves parent grid and Vector3 location and assesses if the location is contained within the parent grid.
    /// </summary>
    /// <param name="grid">The parent grid that the coordinates will be compared to</param>
    /// <param name="location">The Vector3 position being assessed</param>
    /// <returns>bool</returns>
    public static bool IsValidCell(GridMap2D<TGridObject> grid, Vector3 location)
    {
        GetXY(grid, location, grid.orientation, out int x, out int y);
        return IsValidCell(grid, x, y);
    }
    /// <summary>
    /// Recieves parent grid and Vector3 location finds the position on the grid of the Vector3 and returning a list of adjacent nodes.
    /// </summary>
    /// <param name="grid">The parent grid that the coordinates will be compared to</param>
    /// <param name="location">The Vector3 position being assessed</param>
    /// <returns>List[TGridObject]</returns>
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
    /// <summary>
    /// Recieves parent grid, cell location via Vector3 and desired color and draws a colored line to show cell position in editor
    /// </summary>
    /// <params name="map">The parent grid</params>
    /// <params name="location">Vector3 location of grid cell being displayed</params>
    /// <params name="color">The desired color of the debug line</params>
    public static void DebugCell(GridMap2D<TGridObject> map, Vector3 location, Color color)
    {
        GetXY(map, location, map.orientation, out int x, out int y);
        Vector3 leftCorner = map.GetWorldPosition(x, y);
        Vector3 rightCorner = map.GetWorldPosition(x, y);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
    /// <summary>
    /// Recieves parent grid, cell location via XY coordinates and desired color and draws a colored line to show cell position in editor
    /// </summary>
    /// <params name="map">The parent grid</params>
    /// <params name="x">X position of grid cell being displayed</params>
    /// <params name="y">Y position of grid cell being displayed</params>
    /// <params name="color">The desired color of the debug line</params>
    public static void DebugCell(GridMap2D<TGridObject> map, int x, int y, Color color)
    {
        Vector3 leftCorner = map.GetWorldPosition(x, y);
        Vector3 rightCorner = map.GetWorldPosition(x, y);
        rightCorner.x += map.cellSize;
        rightCorner.y += map.cellSize;
        Debug.DrawLine(leftCorner, rightCorner, color, 100f);
    }
}
