using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Adjustment : MonoBehaviour
{
    public KeyCode _inputKey;
    [SerializeField]
    protected Sprite _sprite;
    [SerializeField]
    float focusCost;

    public abstract void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Tile> points, GridManager gridManager, Bolt bolt);

    public Sprite GetSprite()
    {
        return _sprite;
    }

    public float GetFocusCost()
    {
        return focusCost;
    }
}
