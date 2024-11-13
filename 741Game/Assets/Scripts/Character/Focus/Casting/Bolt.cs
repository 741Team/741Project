using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;

public class Bolt : MonoBehaviour
{
    [SerializeField]
    List<Adjustment> _adjustments = new List<Adjustment>();
    [SerializeField]
    List<Tile> _points = new List<Tile>();
    [SerializeField]
    float _speed;
    int _currentPoint;
    GridManager _gridManager;
    Tile _currentTile;
    Vector2 _direction;
    bool _ended;
    bool _started;
    bool pointChecked;
    bool _inverse;      
    Terrain _terrain;
    List<Adjustment> remainingAdjustments;
    List<Tile> _splitPoints = new List<Tile>();
    List<GameObject> _splitBolts = new List<GameObject>();

    public void OnCreate()
    {
        _inverse = false;
        pointChecked = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _gridManager = FindAnyObjectByType<GridManager>();
        _currentPoint = 0;
        _ended = false;
        _started = false;

        if (_gridManager != null)
        {
            _currentTile = _gridManager.GetPlayerTile();
            _points.Add(_gridManager.GetPlayerTile());
            _terrain = _gridManager.GetTerrain();
        }
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    public void SetAdjustments(List<Adjustment> adjustments)
    {
        _adjustments = adjustments;
    }

    public void SetCurrentTile(Tile tile)
    {
        _currentTile = tile;
        _points.Clear();
        _points.Add(tile);
    }

    public void SetInverse(bool inverse)
    {
        _inverse = inverse;
    }


    public void ApplyAdjustments()
    {
        remainingAdjustments = new List<Adjustment>();
        remainingAdjustments.AddRange(_adjustments);
        foreach (Adjustment a in _adjustments)
        {

            a.AdjustSpell(ref _currentTile, ref _direction, ref _points, _gridManager, this);


            remainingAdjustments.Remove(a);
        }
    }

    public void FireBolt()
    {
        Vector3 pos = new Vector3(_points[0].transform.position.x, _terrain.SampleHeight(_points[0].transform.position) + 0.5f, _points[0].transform.position.z);
        transform.position = pos;
        GetComponentInChildren<TrailRenderer>().enabled = true;
        _started = true;
    }

    private void Update()
    {
        if (_started)
        {
            if (!pointChecked)
            {
                CheckPointForEnemy(_currentPoint);
                pointChecked = true;

            }
            if (_currentPoint + 1 < _points.Count)
            {
                float pointHeight = _terrain.SampleHeight(_points[_currentPoint + 1].transform.position) + 0.5f;
                Vector3 pointPosition = new Vector3(_points[_currentPoint + 1].transform.position.x, pointHeight, _points[_currentPoint + 1].transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, pointPosition, _speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, pointPosition) < 0.1)
                {
                    if(_splitPoints.Contains(_points[_currentPoint + 1]))
                    {
                        int index = _splitPoints.IndexOf(_points[_currentPoint + 1]);
                        _splitBolts[index].GetComponent<Bolt>().FireBolt();
                    }
                    _currentPoint += 1;
                    pointChecked = false;
                }
            }
            else
            {
                _ended = true;
                _started = false;
            }
        }

    }

    public bool IsBoltEnded()
    {
        return _ended;
    }

    public List<Adjustment> GetRemainingAdjustments()
    {
        return remainingAdjustments;
    }

    public bool GetInverse()
    {
        return _inverse;
    }

    private void CheckPointForEnemy(int point)
    {
        Tile tile = _points[point];
        GameObject occupant = tile.GetOccupant();
        if (occupant != null)
        {
            if (occupant.tag == "Enemy")
            {
                EnemyBase enemy = occupant.GetComponent<EnemyBase>();
                enemy.BoltHit(50);
            }
        }
    }

    public void AddSplit(Tile tile, GameObject bolt)
    {
        _splitPoints.Add(tile);
        _splitBolts.Add(bolt);
    }

}
