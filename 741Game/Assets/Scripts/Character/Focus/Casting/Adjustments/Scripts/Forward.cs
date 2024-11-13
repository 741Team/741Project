using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forward : Adjustment
{
    public override void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Tile> points, GridManager gridManager, Bolt bolt)
    {
        int x = tile._x + (int)direction.x;
        int y = tile._y + (int)direction.y;

        if (gridManager != null)
        {
             tile = gridManager.GetTile(x, y).GetComponent<Tile>();
            points.Add(tile);
        }
    }
}
