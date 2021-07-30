using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAIPathfinder : MonoBehaviour
{

    private List<Vector3> path;
    private Vector3 location;
    private Vector3 targetPosition;
    private int i;
    public bool pathEnd;
    private Pathfinder pathfinder;
    private GridMap<PathNode> grid;
    private Tilemap map;
    private readonly float MOVESPEED = 1.5f;
    private Transform playerLocation;
    private void Start()
    {
        map = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        pathfinder = new Pathfinder(20, 11, Vector3.zero);
        grid = pathfinder.GetGrid();
        foreach (var position in map.cellBounds.allPositionsWithin)
        {
            if (map.HasTile(position))
            {
                Vector3 cellPos = map.GetCellCenterWorld(position);
                GridTools<PathNode>.GetXYZ(grid, cellPos, out int x, out int y, out int z);
                if (pathfinder.NullNodeCheck(x, y))
                {
                    pathfinder.GetNode(x, y).isWalkable = false;
                }

                Vector3 cellPosition = new Vector3(cellPos.x - 0.5f, cellPos.y - 0.5f);

                Vector3 diagonalLine = new Vector3(cellPos.x + 0.5f, cellPos.y + 0.5f);
                Debug.DrawLine(cellPosition, diagonalLine, Color.red, 100f);
            }
        }
        i = 0;
        path = null;
        targetPosition = transform.position;
        pathEnd = true;
    }
    private void FixedUpdate()
    {
        playerLocation = GameObject.Find("Player").GetComponent<Transform>();
        location = transform.position;
        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        if (path != null)
        {

            if (i < path.Count - 1)
            {
                targetPosition = path[i];

            }
            if (!Tools.PositionCheck(transform.position, targetPosition))
            {
                Tools.MoveObjectToLocation(gameObject, targetPosition, MOVESPEED);
            }
            else if (Tools.PositionCheck(transform.position, targetPosition) && targetPosition != path[path.Count - 1])
            {
                if (i < path.Count - 1)
                {
                    i++;
                    targetPosition = path[i];
                }
            }
            else if (Vector3.Distance(location, path[path.Count - 1]) < 3f)
            {
                pathEnd = true;
            }
        }
    }
    private void SetPath(List<Vector3> newPath)
    {
        path = newPath;
        i = 0;
        pathEnd = false;
        targetPosition = path[i];
    }
    public void GeneratePathTo(Vector3 position)
    {
        Vector3 mapBounds = grid.GetCellCenterWorld(new Vector3(grid.Width, grid.Height));
        if (position.y < mapBounds.y && position.x < mapBounds.x && position.y > 0f && position.x > 0f)
        {
            List<Vector3> temp = pathfinder.Pathfind(location, position);
            if (temp.Count > 0)
            {
                SetPath(temp);
            }
        }
    }

    public void FollowPlayer()
    {
        StartCoroutine(PathToPlayer());
    }
    public void LostPlayer()
    {
        StopCoroutine(PathToPlayer());
    }

    IEnumerator PathToPlayer()
    {
        while (true)
        {
            GeneratePathTo(playerLocation.position + Tools.GetRandomDirection());
            if (pathfinder.GetGrid().GetCellCenterWorld(location) == location)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
