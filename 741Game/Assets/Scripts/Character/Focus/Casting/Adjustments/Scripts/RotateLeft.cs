using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLeft : Adjustment
{
    // Start is called before the first frame update
    public override void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Tile> points, GridManager gridManager, Bolt bolt)
    {
        int x;
        int y;

        if (!bolt.GetInverse())
        {
            float newY = direction.x * -1;
            direction = new Vector2(direction.y, newY);

             x = tile._x + (int)direction.x;
             y = tile._y + (int)direction.y;
        }
        else
        {
            float newX = direction.y * -1;
            direction = new Vector2(newX, direction.x);

             x = tile._x + (int)direction.x;
             y = tile._y + (int)direction.y;
        }



        if (gridManager != null)
        {
            tile = gridManager.GetTile(x, y).GetComponent<Tile>();
            points.Add(tile);
        }
    }
}
