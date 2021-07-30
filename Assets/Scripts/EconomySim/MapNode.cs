using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : GridNode<MapNode>
{
    private TileType state;
    private int productionValue;
    public int ProductionValue { get { return productionValue; } }
    private readonly int defaultProductionValue = 10;
    private Settlement settlement;

    private void UpdateNode()
    {
        switch (state)
        {
            case TileType.None:
                productionValue = 0;
                settlement = null;
                break;
            case TileType.Wood:
                SetDefaultProduct();
                break;
            case TileType.Wool:
                SetDefaultProduct();
                break;
            case TileType.Wheat:
                SetDefaultProduct();
                break;
            case TileType.Stone:
                SetDefaultProduct();
                break;
            case TileType.Settlement:
                settlement = new Settlement(this);
                productionValue = 0;
                break;
        }
    }

    private void SetDefaultProduct()
    {
        productionValue = defaultProductionValue;
        settlement = null;
    }
    public MapNode(GridMap<MapNode> grid, int x, int y, int z) : base(grid, x, y, z)
    {
        UpdateNode();
    }

    public Settlement GetSettlement()
    {
        return settlement;
    }

    public void SetProduct(TileType product)
    {
        if (state != product)
        {
            state = product;
            UpdateNode();
        }
    }
    public void SetNodeAsSettlement()
    {
        if (state != TileType.Settlement)
        {
            state = TileType.Settlement;
            UpdateNode();
        }
    }
    public TileType GetState()
    {
        if (Tools.NullCheck(state))
        {
            return state;
        }
        else return TileType.None;
    }
}
