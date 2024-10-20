using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Bolt : MonoBehaviour
{
    [SerializeField]
    List<Adjustment> _adjustments = new List<Adjustment>();
    [SerializeField]
    List<Vector3> _points = new List<Vector3>();
    [SerializeField]
    float _speed;
    int _currentPoint;
    GridManager _gridManager;
    Tile _currentTile;
    Vector2 _direction;
    bool _ended;
    bool _started;

    List<GameObject> _splitBolts;

    public void OnCreate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _gridManager = FindAnyObjectByType<GridManager>();
        _currentPoint = 0;
        _ended = false;
        _started = false;

        //CHANGE THIS TO MATCH PLAYER DIRECTION
        _direction = new Vector2(-1, 0);
        if (player != null)
        {
            _points.Add(player.transform.position);
        }
        if (_gridManager != null)
        {
            _currentTile = _gridManager.GetPlayerTile();
        }
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
            if (_currentPoint + 1 < _points.Count)
            {
                transform.position =  Vector3.MoveTowards(transform.position, _points[_currentPoint + 1], _speed * Time.deltaTime);
                if(Vector3.Distance(transform.position, _points[_currentPoint + 1]) < 0.1)
                {
                    _currentPoint += 1;
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

}
