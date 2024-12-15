using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    GameObject _occupant;
    SpriteRenderer spriteRenderer;
    GridManager _gridManager;
    [SerializeField]
    Transform _spriteSpot;

    public int _x;
    public int _y;
    public float _priority;
    public bool _isWalkable;
    public GameObject _parent;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _gridManager = FindAnyObjectByType<GridManager>();
    }

    public void ColourChange(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }

    }

    public void SetOccupant(GameObject occupant)
    {
        _occupant = occupant;
        _isWalkable = false;

    }   

    public void RemoveOccupant()
    {
        _occupant = null;
        _isWalkable = true;
    }

    public GameObject GetOccupant()
    {
        return _occupant;
    }

    public Transform GetSpriteSpot()
    {
        return _spriteSpot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SetOccupant(other.gameObject);
            _gridManager.SetPlayerTile(this);
        }
        else if(other.gameObject.tag == "Enemy")
        {
            if(other.gameObject.GetComponent<EnemyBase>() == null)
            {
                return;
            }
            if(other.gameObject.GetComponent<EnemyBase>().GetOccupant())
            {
                return;
            }
            other.gameObject.GetComponent<EnemyBase>().SetOccupant(true);
            SetOccupant(other.gameObject);
        }
    }

    private void OnDisable()
    {
        _spriteSpot.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void SetXAndY(int x, int y)
    {
        _x = x;
        _y = y;
    }
}
