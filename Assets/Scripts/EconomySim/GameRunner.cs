using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    private int globalWood;
    private int globalWool;
    private int globalWheat;
    private int globalStone;
    private List<Settlement> settlementList;
    public int height;
    public int width;
    public WorldMap map;
    [SerializeField] MapVisual mapVisual;
    private int settlementIndex;
    // Start is called before the first frame update
    void Start()
    {
        globalWood = 0;
        globalWool = 0;
        globalWheat = 0;
        globalStone = 0;
        settlementIndex = 0;
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Turn();
        }
        Vector3 position = Tools.MouseToWorldPosition();
        if (Tools.NullCheck(map.GetNode(position)))
        {
            if (Input.GetMouseButtonDown(0))
            {
                map.SetProduct(position, TileType.Settlement);
                settlementList.Add(map.GetNode(position).GetSettlement());
            }
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(map.GetNode(position).GetState());
            }
        }
    }

    private void Turn()
    {
        Settlement settlement = settlementList[settlementIndex];
        settlement.GatherRescources();
        Dictionary<TileType, int> settlementYield = settlement.YieldRescources();
        int stoneOnhand = settlementYield[TileType.Stone];
        int woodOnhand = settlementYield[TileType.Wood];
        int woolOnhand = settlementYield[TileType.Wool];
        int wheatOnhand = settlementYield[TileType.Wheat];
        if (woodOnhand >= 100 && stoneOnhand >= 400)
        {
            Debug.Log("Settlement_" + settlementIndex + " Has Improved!");
            settlement.ImproveSettlement();
        }
        settlement.PopulationConsumption();
        UpdateGlobals();
        Debug.Log("Settlement_" + settlementIndex + " Has: " + woodOnhand + " Wood " + woolOnhand + " Wool " + wheatOnhand + " Wheat " + stoneOnhand + " Stone ");
        Debug.Log("Global Onhands: " + " Wood: " + globalWood + " | Wool: " + globalWool + " | Wheat: " + globalWheat + " | Stone: " + globalStone);
        if (settlementIndex < settlementList.Count - 1)
        {
            settlementIndex++;
        }
        else settlementIndex = 0;
    }

    private void GenerateMap()
    {
        WorldMap worldMap = new WorldMap(width, height, 1);
        settlementList = new List<Settlement>();
        map = worldMap;
        mapVisual.SetMap(map);
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int y = 0; y < map.GetHeight(); y++)
            {
                MapNode node = map.GetNode(x, y);
                if (node.GetState().Equals(TileType.Settlement))
                {
                    settlementList.Add(node.GetSettlement());
                }
            }
        }

    }

    private void UpdateGlobals()
    {
        globalWood = 0;
        globalWool = 0;
        globalWheat = 0;
        globalStone = 0;
        foreach (Settlement settlement in settlementList)
        {
            Dictionary<TileType, int> rescources = settlement.YieldRescources();
            globalWood += rescources[TileType.Wood];
            globalWool += rescources[TileType.Wool];
            globalWheat += rescources[TileType.Wheat];
            globalStone += rescources[TileType.Stone];
        }
    }
}
