using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : GridNode2D<MapNode>
{
    private TileType state;
    public int ProductionValue { get; private set; }
    private readonly int defaultProductionValue = 10;
    private Settlement settlement;

    private void UpdateNode()
    {
        switch (state)
        {
            case TileType.None:
                ProductionValue = 0;
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
                ProductionValue = 0;
                break;
        }
    }

    private void SetDefaultProduct()
    {
        ProductionValue = defaultProductionValue;
        settlement = null;
    }
    public MapNode(GridMap2D<MapNode> grid, int x, int y) : base(grid, x, y)
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
