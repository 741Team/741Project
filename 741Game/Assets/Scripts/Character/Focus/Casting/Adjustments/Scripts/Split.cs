using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : Adjustment
{
    public override void AdjustSpell(ref Tile tile, ref Vector2 direction, ref List<Tile> points, GridManager gridManager, Bolt bolt)
    {
        StoreBoltPrefab storeBoltPrefab = GetComponent<StoreBoltPrefab>();
        GameObject prefab = storeBoltPrefab.GetPrefab();
        GameObject forward = storeBoltPrefab.GetForward();
        GameObject right = storeBoltPrefab.GetRight();
        Tile newTile = gridManager.GetTile(tile._x, tile._y).GetComponent<Tile>();

        right.GetComponent<Adjustment>().AdjustSpell(ref tile, ref direction, ref points, gridManager, bolt);

        GameObject newBolt = Instantiate(prefab, bolt.transform.position, Quaternion.identity);
        newBolt.GetComponent<Bolt>().OnCreate();
        newBolt.GetComponent<Bolt>().SetDirection(direction * -1);
        newBolt.GetComponent<Bolt>().SetCurrentTile(newTile);
        List<Adjustment> adjustments = new List<Adjustment>();
        adjustments.Add(forward.GetComponent<Adjustment>());
        adjustments.AddRange(bolt.GetRemainingAdjustments());
        adjustments.Remove(this);
        newBolt.GetComponent<Bolt>().SetAdjustments(adjustments);
        newBolt.GetComponent<Bolt>().SetInverse(!bolt.GetInverse());
        newBolt.GetComponent<Bolt>().ApplyAdjustments();

        bolt.AddSplit(newTile, newBolt);
    }
}


