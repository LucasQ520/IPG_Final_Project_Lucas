using UnityEngine;

public static class TetrisData
{
    public static readonly Vector2Int[][] Shapes =
    {
        new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(-2,0)
        },

        new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,0)
        },

        new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(1,1)
        },

        new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(1,1)
        },

        new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
            new Vector2Int(-1,1)
        },

        new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(1,1)
        },

        new Vector2Int[]
        {
            new Vector2Int(0,0),
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(-1,1)
        }
    };
}
