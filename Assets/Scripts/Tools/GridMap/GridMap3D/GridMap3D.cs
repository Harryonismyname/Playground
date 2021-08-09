using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 3-Dimensional Grid capable of storing custom 3D nodes
/// </summary>
public class GridMap3D<TGridObject>
{
    ///<summary>Y extreme</summary>
    public int Height { get; }
    ///<summary>X extreme</summary>
    public int Width { get; }
    ///<summary>Z extreme</summary>
    public int Depth { get; }
    ///<summary>Controls grid density</summary>
    public float cellSize;
    ///<summary>where Grid[0,0,0] is located in 3D space</summary>
    private Vector3 originPosition;
    ///<summary>Where grid data is stored</summary>
    private TGridObject[,,] gridArray;
    public bool debugging = false;

    public GridMap3D(int height, int width, float cellSize, Vector3 originPosition, Func<GridMap3D<TGridObject>, int, int, int, TGridObject> createGridObject, int depth = 1)
    {
        Height = height;
        Width = width;
        Depth = depth;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height, depth];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    gridArray[x, y, z] = createGridObject(this, x, y, z);
                }
            }
        }
        if (debugging)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }
    // Setters
    public void SetGridObject(int x, int y, TGridObject value, int z = 0)
    {
        if (GridTools3D<TGridObject>.IsValidCell(this, x, y, z))
        {
            gridArray[x, y, z] = value;
        }
    }
    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GridTools3D<TGridObject>.GetXYZ(this, worldPosition, out int x, out int y, out int z);
        SetGridObject(x, y, value, z);
    }
    // Getters
    public TGridObject GetGridObject(int x, int y, int z = 0)
    {
        if (GridTools3D<TGridObject>.IsValidCell(this, x, y, z))
        {
            return gridArray[x, y, z];
        }
        else
        {
            return default;
        }
    }
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        GridTools3D<TGridObject>.GetXYZ(this, worldPosition, out int x, out int y, out int z);
        return GetGridObject(x, y, z);
    }
    public Vector3 GetCellCenterWorld(Vector3 position)
    {
        GridTools3D<TGridObject>.GetXYZ(this, position, out int x, out int y, out int z);
        Vector3 location = GetCellCenter(GetWorldPosition(x, y));
        return location;
    }
    public Vector3 GetCellCenterWorld(int x, int y, int z = 0)
    {
        Vector3 location = GetCellCenter(GetWorldPosition(x, y, z));
        return location;
    }
    private Vector3 GetCellCenter(Vector3 location)
    {
        location.x += (cellSize / 2);
        location.y += (cellSize / 2);
        location.z += (cellSize / 2);
        return location;
    }
    public Vector3 GetWorldPosition(int x, int y, int z = 0)
    {
        return new Vector3(x, y, z) * cellSize;
    }
}
