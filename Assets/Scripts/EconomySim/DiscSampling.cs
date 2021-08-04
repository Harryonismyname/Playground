using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiscSampling
{
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, GridMap2D<MapNode> worldMap, int numSamplesBeforeRejection = 30)
    {
        GridMap2D<MapNode> map = worldMap;
        float cellSize = map.cellSize;
        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize / 2);
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < numSamplesBeforeRejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = map.GetCellCenterWorld(spawnCenter * dir * Random.Range(radius, 2 * radius));
                if (IsValid(candidate, sampleRegionSize, radius, points, grid, map))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    GridTools2D<MapNode>.GetXY(map, candidate, map.orientation, out int x, out int y);
                    grid[x, y] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        return points;
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float radius, List<Vector2> points, int[,] grid, GridMap2D<MapNode> map)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            GridTools2D<MapNode>.GetXY(map, candidate, map.orientation, out int x, out int y);
            int cellX = x;
            int cellY = y;
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int Ix = searchStartX; Ix <= searchEndX; Ix++)
            {
                for (int Iy = searchStartY; Iy <= searchEndY; Iy++)
                {
                    int pointIndex = grid[Ix, Iy] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

}
