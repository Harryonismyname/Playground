using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridMap2D<TGridObject>
{
    public int Height { get; }
    public int Width { get; }
    public float cellSize;
    public Axis orientation;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
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


    public Vector3 GetCellCenterWorld(Vector3 position)
    {
        GridTools2D<TGridObject>.GetXY(this, position, orientation, out int x, out int y);
        Vector3 location = GetCellCenter(GetWorldPosition(x, y));
        return location;
    }
    public Vector3 GetCellCenterWorld(int x, int y)
    {
        Vector3 location = GetCellCenter(GetWorldPosition(x, y));
        return location;
    }


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

}
