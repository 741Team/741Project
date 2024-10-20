using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameObject _gridStorage;
    private GameObject[][] _grid;
    [SerializeField]
    private Tile _playerTile;
    AnimateGridLines _animateGridLines;



    private void Start()
    {
       _animateGridLines = GetComponent<AnimateGridLines>();
    }

    public void SetGrid(GameObject gridStorage, GameObject[][] grid)
    {
        _gridStorage = gridStorage;
        _grid = grid;

        AnimateGridLines animateGridLines = GetComponent<AnimateGridLines>();
        if (animateGridLines != null)
        {
           animateGridLines.CreateLineRenderers();
        }
    }

    public GameObject[][] GetGrid()
    {
        return _grid;
    }

    public void EnableGrid()
    {
        if (_gridStorage != null)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            _gridStorage.SetActive(true);
            if (playerPos != null) { 
                Vector3 gridPos = new Vector3(playerPos.x, 0, playerPos.z);
                _gridStorage.transform.position = gridPos;
            }
            _animateGridLines.EnableLines();
        }
    }

    public void DisableGrid()
    {
        if (_gridStorage != null)
        {
            _gridStorage.SetActive(false);
            _animateGridLines.DisableLines();
        }
    }

    public void SetLineAlpha(float alpha)
    {
        Color color = _animateGridLines.GetColor();
        color.a = alpha;
        _animateGridLines.AdjustLineColours(color);
    }

    public GameObject GetTile(int x, int y)
    {
        return _grid[x][y];
    }

    public void SetPlayerTile(Tile playerTile)
    {
        _playerTile = playerTile;
    }

    public Tile GetPlayerTile() { 
        return _playerTile;
    }

}
