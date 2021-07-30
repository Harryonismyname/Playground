using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapVisual : MonoBehaviour
{
    [System.Serializable]
    public struct MapSpriteUV
    {
        public TileType state;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    private Dictionary<TileType, UVCoords> uvCoordsDictionary;

    [SerializeField] private MapSpriteUV[] mapSpriteUVArray;

    private GridMap<MapNode> grid;
    private WorldMap map;
    private Mesh mesh;
    private bool updateMesh;
    private bool subscribed = false;
    private void Map_OnMapUpdate(object sender, WorldMap.OnMapUpdateArgs e)
    {
        SetGrid(e.grid);
    }

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;
        uvCoordsDictionary = new Dictionary<TileType, UVCoords>();
        foreach (MapSpriteUV mapSpriteUV in mapSpriteUVArray)
        {
            uvCoordsDictionary[mapSpriteUV.state] = new UVCoords
            {
                uv00 = new Vector2(mapSpriteUV.uv00Pixels.x / textureWidth, mapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(mapSpriteUV.uv11Pixels.x / textureWidth, mapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
    }
    public void SetMap(WorldMap map)
    {
        this.map = map;

        if (!subscribed)
        {
            this.map.OnMapUpdate += Map_OnMapUpdate;
            subscribed = true;
        }
        SetGrid(map.GetGrid());
    }
    public void SetGrid(GridMap<MapNode> grid)
    {
        this.grid = grid;
        UpdateMeshVisual();
    }

    private void UpdateMeshVisual()
    {
        mesh.Clear();
        Tools.CreateEmptyMeshArrays(grid.Width * grid.Height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                for (int z = 0; z < grid.Depth; z++)
                {
                    int index = x * grid.Height + y;
                    Vector3 quadSize = new Vector3(1, 1) * grid.cellSize;
                    MapNode node = map.GetNode(x, y, z);
                    Vector2 gridUV00, gridUV11;
                    if (node.GetState() == TileType.None)
                    {
                        gridUV00 = Vector2.zero;
                        gridUV11 = Vector2.zero;
                        quadSize = Vector3.zero;
                    }
                    else
                    {
                        UVCoords uvCoords = uvCoordsDictionary[node.GetState()];
                        gridUV00 = uvCoords.uv00;
                        gridUV11 = uvCoords.uv11;
                    }
                    Tools.AddToMeshArrays(vertices, uvs, triangles, index, grid.GetCellCenterWorld(x, y), 0f, quadSize, gridUV00, gridUV11);
                }
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

    }
}
