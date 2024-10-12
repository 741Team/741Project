using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    GameObject _occupant;
    SpriteRenderer spriteRenderer;

    public int _x;
    public int _y;
    public float _priority;
    public bool _isWalkable;
    public GameObject _parent;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
  
        
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

}
