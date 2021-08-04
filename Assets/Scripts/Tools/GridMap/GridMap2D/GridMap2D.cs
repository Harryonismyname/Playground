using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 2-Dimensional Grid capable of storing custom 2D nodes
/// </summary>
public class GridMap2D<TGridObject>
{
    /// <summary>Y extreme</summary>
    public int Height { get; }
    /// <summary>X extreme</summary>
    public int Width { get; }
    /// <summary>Controls grid density</summary>
    public float cellSize;
    /// <summary>Determines which plane the grid is oriented on</summary>
    public Axis orientation;
    /// <summary>Where grid[0,0] is located in 3D space</summary>
    private Vector3 originPosition;
    /// <summary>Where the grid data is stored</summary>
    private TGridObject[,] gridArray;
    /// <summary>
    /// Constructor for creating a 2-dimensional grid
    /// </summary>
    /// <param name="height">Y extreme</param>
    /// <param name="width">X extreme</param>
    /// <param name="cellSize">Determines cell size</param>
    /// <param name="orientation">Dermines which plane the grid is oriented on</param>
    /// <param name="originPosition">Determines where the grid originates</param>
    /// <param name="createGridObject">Lambda function that creates 2D Grid Nodes</param>
    public GridMap2D(int height, int width, float cellSize, Axis orientation, Vector3 originPosition, Func<GridMap2D<TGridObject>, int, int, TGridObject> createGridObject)
    {
        Height = height;
        Width = width;
        this.cellSize = cellSize;
        this.orientation = orientation;
        this.originPosition = originPosition;
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }
    // Setters
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (GridTools2D<TGridObject>.IsValidCell(this, x, y))
        {
            gridArray[x, y] = value;
        }
    }
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GridTools2D<TGridObject>.GetXY(this, worldPosition, orientation, out int x, out int y);
        SetGridObject(x, y, value);
    }
    // Getters
    public TGridObject GetGridObject(int x, int y)
    {
        return GridTools2D<TGridObject>.IsValidCell(this, x, y) ? gridArray[x, y] : default;
    }
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        GridTools2D<TGridObject>.GetXY(this, worldPosition, orientation, out int x, out int y);
        return GetGridObject(x, y);
    }
    /// <summary>Converts XY coordinates to Vector3 location</summary>
    public Vector3 GetWorldPosition(int x, int y)
    {
        switch (orientation)
        {
            case Axis.X:
                {
                    return new Vector3(0, y, x) * cellSize;
                }
            case Axis.Y:
                {
                    return new Vector3(x, 0, y) * cellSize;
                }
            case Axis.Z:
                {
                    return new Vector3(x, y) * cellSize;
                }
        }
        return originPosition;
    }
    /// <summary>Translates GridMap2D.GetWorldPosition() results to return the center of the cell instead of the origin point</summary>
    private Vector3 GetCellCenter(Vector3 location)
    {
        if (GridTools2D<TGridObject>.IsValidCell(this, location))
        {
            switch (orientation)
            {
                case Axis.X:
                    {
                        location.z += (cellSize / 2);
                        location.y += (cellSize / 2);
                        return location;
                    }
                case Axis.Y:
                    {
                        location.x += (cellSize / 2);
                        location.z += (cellSize / 2);
                        return location;
                    }
                case Axis.Z:
                    {
                        location.x += (cellSize / 2);
                        location.y += (cellSize / 2);
                        return location;
                    }
            }
            return originPosition;
        }
        else return originPosition;
    }
    /// <summary>Recieves Vector3 and returns the Vector3 center of the inspected cell</summary>
    public Vector3 GetCellCenterWorld(Vector3 position)
    {
        GridTools2D<TGridObject>.GetXY(this, position, orientation, out int x, out int y);
        Vector3 location = GetCellCenter(GetWorldPosition(x, y));
        return location;
    }
    /// <summary>Recieves XY coordinates and returns the Vector3 center of the inspected cell</summary>
    public Vector3 GetCellCenterWorld(int x, int y)
    {
        Vector3 location = GetCellCenter(GetWorldPosition(x, y));
        return location;
    }
}
