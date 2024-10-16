using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLeft : Adjustment
{
    // Start is called before the first frame update
    public override void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Vector3> points, GridManager gridManager)
    {
        float newY = direction.x * -1;
        direction = new Vector2(direction.y, newY);
    }
}
