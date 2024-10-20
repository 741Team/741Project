using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRight : Adjustment
{
    public override void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Vector3> points, GridManager gridManager)
    {
        float newX = direction.y * -1;
        direction = new Vector2(newX, direction.x);

        int x = tile._x + (int)direction.x;
        int y = tile._y + (int)direction.y;

        if (gridManager != null)
        {
            GameObject newTile = gridManager.GetTile(x, y);
            if (newTile != null)
            {
                Vector3 newTilePos = newTile.transform.position;
                Vector3 newPoint = new Vector3(newTilePos.x, points[0].y, newTilePos.z);
                tile = newTile.GetComponent<Tile>();
                points.Add(newPoint);
            }
        }
    }
}
