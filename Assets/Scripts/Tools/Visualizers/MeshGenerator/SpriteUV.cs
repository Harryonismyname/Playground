using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpriteUV<TEnum>
{
    public TEnum state;
    public Vector2Int uv00Pixels;
    public Vector2Int uv11Pixels;
}
