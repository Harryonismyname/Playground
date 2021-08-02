using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement
{
    private int woodOnHand;
    private int woolOnHand;
    private int wheatOnHand;
    private int stoneOnHand;
    private readonly MapNode parent;
    private readonly GridMap3D<MapNode> parentGrid;
    private bool improved;
    private int productionMultiplier;
    private int population;

    public Settlement(MapNode parent)
    {
        wheatOnHand = 0;
        woodOnHand = 0;
        woolOnHand = 0;
        stoneOnHand = 0;
        this.parent = parent;
        parentGrid = parent.Grid;
        improved = false;
        productionMultiplier = 1;
        population = 10;
    }

    public Dictionary<TileType, int> YieldRescources()
    {
        Dictionary<TileType, int> products = new Dictionary<TileType, int>
        {
            { TileType.Wood, woodOnHand },
            { TileType.Wool, woolOnHand },
            { TileType.Wheat, wheatOnHand },
            { TileType.Stone, stoneOnHand }
        };
        return products;
    }

    private void ConsumeRescource(TileType rescource, int amount)
    {
        switch (rescource)
        {
            case TileType.Stone:
                stoneOnHand -= amount;
                break;
            case TileType.Wool:
                woolOnHand -= amount;
                break;
            case TileType.Wood:
                woodOnHand -= amount;
                break;
            case TileType.Wheat:
                wheatOnHand -= amount;
                break;
        }
    }

    public void GatherRescources()
    {
        List<MapNode> neighborList = GridTools<MapNode>.GetNeighborList(parentGrid, parent.GetWorldPosition());
        foreach (MapNode node in neighborList)
        {
            switch (node.GetState())
            {
                case TileType.Stone:
                    stoneOnHand += Mathf.Abs(node.ProductionValue * productionMultiplier);
                    break;
                case TileType.Wheat:
                    wheatOnHand += Mathf.Abs(node.ProductionValue * productionMultiplier);
                    break;
                case TileType.Wood:
                    woodOnHand += Mathf.Abs(node.ProductionValue * productionMultiplier);
                    break;
                case TileType.Wool:
                    woolOnHand += Mathf.Abs(node.ProductionValue * productionMultiplier);
                    break;
            }
        }
    }

    public void PopulationConsumption()
    {
        if (woodOnHand != 0)
        {
            ConsumeRescource(TileType.Wood, population);
        }
        if (woolOnHand != 0)
        {
            ConsumeRescource(TileType.Wool, population);
        }
        if (wheatOnHand != 0)
        {
            ConsumeRescource(TileType.Wheat, population);
        }
        if (stoneOnHand != 0)
        {
            ConsumeRescource(TileType.Stone, population);
        }
    }

    public void ImproveSettlement()
    {
        if (!improved)
        {
            ConsumeRescource(TileType.Wood, 100);
            ConsumeRescource(TileType.Stone, 400);
            productionMultiplier = 2;
            improved = true;
        }
    }
}
