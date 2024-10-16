using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusMode : MonoBehaviour
{
    [SerializeField]
    Casting _casting;
    [SerializeField]
    GridManager _gridManager;
    // Start is called before the first frame update
    void Start()
    {
        _gridManager = FindAnyObjectByType<GridManager>();
        _casting = GetComponent<Casting>();
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (_gridManager != null)
        {
            _gridManager.EnableGrid();
        }
        if (_casting != null)
        {
            _casting.enabled = true;
        }
    }
    private void OnDisable()
    {
        if (_gridManager != null)
        {
            _gridManager.DisableGrid();
        }
        if (_casting != null)
        {
            _casting.enabled = false;
        }
    }
}
