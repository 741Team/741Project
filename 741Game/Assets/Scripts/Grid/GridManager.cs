using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameObject _grid;

    public void SetGrid(GameObject grid)
    {
        _grid = grid;
    }

    public void EnableGrid()
    {
        if (_grid != null)
        {
            _grid.SetActive(true);
        }
    }

    public void DisableGrid()
    {
        if (_grid != null)
        {
            _grid.SetActive(false);
        }
    }
}
