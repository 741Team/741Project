using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRight : Adjustment
{
    public override void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Vector3> points, GridManager gridManager)
    {
        float newX = direction.y * -1;
        direction = new Vector2(newX, direction.x);
    }
}
