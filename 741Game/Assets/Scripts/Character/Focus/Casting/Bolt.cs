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

    List<GameObject> _splitBolts;

    public void OnCreate()
    {
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

    public void  ApplyAdjustments()
    {
        List<Adjustment> remainingAdjustments = new List<Adjustment>();
        remainingAdjustments.AddRange(_adjustments);
        foreach (Adjustment a in _adjustments)
        {
            a.AdjustSpell(ref _currentTile, ref _direction, ref _points, _gridManager);
            remainingAdjustments.Remove(a);
        }
    }

    public void FireBolt()
    {
        _started = true;
    }

    private void Update()
    {
        if (_started)
        {
            if(!pointChecked)
            {
                CheckPointForEnemy(_currentPoint);
                pointChecked = true;

            }
            if (_currentPoint + 1 < _points.Count)
            {
                Vector3 pointPosition = new Vector3(_points[_currentPoint + 1].transform.position.x, transform.position.y, _points[_currentPoint + 1].transform.position.z);
                transform.position =  Vector3.MoveTowards(transform.position, pointPosition, _speed * Time.deltaTime);
                if(Vector3.Distance(transform.position, pointPosition) < 0.1)
                {
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

    private void CheckPointForEnemy(int point) {
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

}
