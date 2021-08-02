using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private GridMap3D<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    public Pathfinder(int width, int height, Vector3 origin)
    {
        grid = new GridMap3D<PathNode>(height, width, 1f, Vector3.zero, (GridMap3D<PathNode> g, int x, int y, int z) => new PathNode(g, x, y));
        openList = new List<PathNode>();
        openList.Add(grid.GetGridObject(origin));
    }

    public List<Vector3> Pathfind(Vector3 origin, Vector3 target)
    {
        target.z = 0;
        List<Vector3> worldPath = new List<Vector3>();
        GridTools<PathNode>.GetXYZ(grid, origin, out int Ox, out int Oy, out int Oz);
        GridTools<PathNode>.GetXYZ(grid, target, out int Tx, out int Ty, out int Tz);
        List<PathNode> path = FindPath(Ox, Oy, Tx, Ty);
        if (path != null)
        {
            Vector3 prevNode = origin;
            foreach (PathNode node in path)
            {
                Vector3 temp = grid.GetCellCenterWorld(new Vector3(node.x, node.y));
                Debug.DrawLine(prevNode, temp, Color.green, 3f);
                worldPath.Add(grid.GetCellCenterWorld(new Vector3(node.x, node.y)));
                prevNode = temp;

            }
            return worldPath;
        }
        else
        {
            return new List<Vector3>();
        }
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        if (NullNodeCheck(startX, startY))
        {

            if (NullNodeCheck(endX, endY))
            {
                PathNode startNode = grid.GetGridObject(startX, startY);
                PathNode endNode = grid.GetGridObject(endX, endY);
                openList = new List<PathNode> { startNode };
                startNode.gCost = 0;
                startNode.hCost = CalculateDistance(startNode, endNode);
                startNode.CalculateFCost(); closedList = new List<PathNode>();
                for (int x = 0; x < grid.Width; x++)
                {
                    for (int y = 0; y < grid.Height; y++)
                    {
                        PathNode pathNode = grid.GetGridObject(x, y);
                        pathNode.gCost = int.MaxValue;
                        pathNode.CalculateFCost();
                        pathNode.cameFromNode = null;
                    }
                }


                while (openList.Count > 0)
                {
                    PathNode currentNode = GetLowestFCostNode(openList);
                    if (currentNode == endNode)
                    {
                        return CalculatePath(endNode);
                    }
                    openList.Remove(currentNode);
                    closedList.Add(currentNode);
                    foreach (PathNode neighborNode in GetNeighborList(currentNode))
                    {
                        if (closedList.Contains(neighborNode)) continue;
                        if (!neighborNode.isWalkable)
                        {
                            closedList.Add(neighborNode);
                            continue;
                        }
                        int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighborNode);
                        if (tentativeGCost < neighborNode.gCost)
                        {
                            neighborNode.cameFromNode = currentNode;
                            neighborNode.gCost = tentativeGCost;
                            neighborNode.hCost = CalculateDistance(neighborNode, endNode);
                            neighborNode.CalculateFCost();

                            if (!openList.Contains(neighborNode))
                            {
                                openList.Add(neighborNode);
                            }
                        }
                    }
                }
            }

        }



        return null;
    }

    private List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighborList = new List<PathNode>();
        if (currentNode.x - 1 >= 0)
        {
            // Left
            if (NullNodeCheck(currentNode.x - 1, currentNode.y))
            {
                neighborList.Add(GetNode(currentNode.x - 1, currentNode.y));
            }

            /*
                        if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                        // Left Down
                        if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
                        // Left Up
            */
        }
        if (currentNode.x + 1 < grid.Width)
        {
            // Right
            if (NullNodeCheck(currentNode.x + 1, currentNode.y))
            {
                neighborList.Add(GetNode(currentNode.x + 1, currentNode.y));
            }

            /*
                        if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                        // Right Down
                        if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
                        // Right Up
            */
        }
        // Down

        if (currentNode.y - 1 >= 0)
        {
            if (NullNodeCheck(currentNode.x, currentNode.y - 1))
            {
                neighborList.Add(GetNode(currentNode.x, currentNode.y - 1));
            }

        }
        // Up
        if (currentNode.y + 1 < grid.Height)
        {
            if (NullNodeCheck(currentNode.x, currentNode.y + 1))
            {
                neighborList.Add(GetNode(currentNode.x, currentNode.y + 1));
            }
        }


        return neighborList;
    }
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    // Getters
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public PathNode GetNode(int x, int y)
    {
        PathNode node = grid.GetGridObject(x, y);
        if (node != null)
        {
            return node;
        }
        else return null;
    }
    public bool NullNodeCheck(int x, int y)
    {
        if (GetNode(x, y) != null)
        {
            return true;
        }
        else return false;
    }
    public GridMap3D<PathNode> GetGrid()
    {
        return grid;
    }
}
