using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Adjustment : MonoBehaviour
{
    public KeyCode _inputKey;

    public abstract void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Tile> points, GridManager gridManager, Bolt bolt);

}
