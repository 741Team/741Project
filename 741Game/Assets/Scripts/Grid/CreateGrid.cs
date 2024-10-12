using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField]
    GameObject _gridTile;
    [SerializeField]
    private int _gridWidth;
    [SerializeField]
    private int _gridHeight;
    [SerializeField]
    private float _tileSize;

    private float _gridSpacing;

    public GameObject[][] _grid;
   
    // Start is called before the first frame update
    void Start()
    {
        _grid = new GameObject[_gridWidth][];
        for (int i = 0; i < _gridWidth; i++)
        {
            _grid[i] = new GameObject[_gridHeight];
        }
        BoxCollider tileCollider = _gridTile.GetComponent<BoxCollider>();
        _gridSpacing = (tileCollider.size.x * _tileSize);
        CreateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateTiles()
    {
        GameObject gridObject = new GameObject();
        gridObject.name = "GridStorage";
        float centreX = Mathf.Round(_gridWidth / 2);
        float centreZ = Mathf.Round(_gridHeight / 2);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            centreX = player.transform.position.x;
            centreZ = player.transform.position.z;

        }
        float gridStartX = centreX - ((_gridWidth/2) * _gridSpacing);
        float gridStartZ = centreZ + ((_gridHeight / 2) * _gridSpacing);
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                float x = gridStartX + (i * _gridSpacing);
                float z = gridStartZ - (j * _gridSpacing);
                GameObject newTile = Instantiate(_gridTile, new Vector3(x, 0.2f, z), _gridTile.transform.rotation);
                newTile.transform.localScale = newTile.transform.localScale * _tileSize;
                newTile.transform.parent = gridObject.transform;
                _grid[i][j] = newTile;
            }
        }
        gridObject.SetActive(false);
    }
}
