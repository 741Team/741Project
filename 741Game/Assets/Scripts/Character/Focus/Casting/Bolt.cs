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
    public List<Tile> _points = new List<Tile>();
    [SerializeField]
    float _speed;
    int _currentPoint;
    GridManager _gridManager;
    public Tile _currentTile;
    public Vector2 _direction;
    bool _ended;
    bool _started;
    bool pointChecked;
    bool _inverse;      
    Terrain _terrain;
    List<Adjustment> remainingAdjustments;
    List<Tile> _splitPoints = new List<Tile>();
    List<GameObject> _splitBolts = new List<GameObject>();


    public void ApplySprite(Adjustment adjustment)
    {
        Sprite sprite = adjustment.GetSprite();
        Transform spriteSpot = _currentTile.GetSpriteSpot();
        float height = _terrain.SampleHeight(spriteSpot.position);
        spriteSpot.position = new Vector3(spriteSpot.position.x, height + 2.5f, spriteSpot.position.z);
        spriteSpot.GetComponent<SpriteRenderer>().sprite = sprite;
        float rotation = 0;
        if (_direction == Vector2.up)
        {
            rotation = 0;
        }
        else if (_direction == Vector2.right)
        {
            rotation = 90;
        }
        else if (_direction == Vector2.down)
        {
            rotation = 180;
        }
        else if (_direction == Vector2.left)
        {
            rotation = 270;
        }

        if (_inverse)
        {
            rotation += 180;
        }

        spriteSpot.rotation = Quaternion.Euler(0, rotation, 0);
    }

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

    public void AddAdjustment(Adjustment adjustment)
    {
        _adjustments.Add(adjustment);
    }

    public void ApplySingleAdjustment(Adjustment adjustment)
    {
        if (_splitBolts.Count > 0)
        {
            foreach (GameObject bolt in _splitBolts)
            {
                bolt.GetComponent<Bolt>().ApplySingleAdjustment(adjustment);
            }
        }
        _adjustments.Add(adjustment);
        ApplySprite(adjustment);
        adjustment.AdjustSpell(ref _currentTile, ref _direction, ref _points, _gridManager, this);
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

            ApplySprite(a);
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
        foreach(GameObject bolt in _splitBolts)
        {
            if (bolt != null)
            {
                bolt.GetComponent<Bolt>().FireBolt();
            }
        }
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
                        if(_splitBolts[index] != null)
                        {
                            _splitBolts[index].GetComponent<Bolt>().FireBolt();
                        }
                    }
                    _currentPoint += 1;
                    pointChecked = false;
                }
            }
            else
            {
                _ended = true;
                List<GameObject> deadBolts = new List<GameObject>();
                foreach (GameObject bolt in _splitBolts)
                {
                    if (bolt != null)
                    {
                        if (!bolt.GetComponent<Bolt>().IsBoltEnded())
                        {
                            _ended = false;
                        }
                    }
                    else
                    {
                        deadBolts.Add(bolt);
                    }
                }
                foreach (GameObject bolt in deadBolts)
                {
                    _splitBolts.Remove(bolt);
                }
                if (!_ended)
                {
                    return;
                }
                _started = false;
                Invoke("DestroyBolt", 0.1f);
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
                if(enemy != null)
                {
                    enemy.BoltHit(50);
                }
            }
        }
    }

    public void AddSplit(Tile tile, GameObject bolt)
    {
        _splitPoints.Add(tile);
        _splitBolts.Add(bolt);
    }

    public void DestroyBolt()
    {
        Destroy(gameObject);
    }
}
