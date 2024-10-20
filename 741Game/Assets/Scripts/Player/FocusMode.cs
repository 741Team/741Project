using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusMode : MonoBehaviour
{
    [SerializeField]
    Casting _casting;
    [SerializeField]
    GridManager _gridManager;
    [SerializeField]
    float _focusTime = 5;
    float _defaultFocusTime;
    // Start is called before the first frame update
    void Start()
    {
        _defaultFocusTime = _focusTime;
        _gridManager = FindAnyObjectByType<GridManager>();
        _casting = GetComponent<Casting>();
        enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        _focusTime -= Time.deltaTime;
        _gridManager.SetLineAlpha(_focusTime / _defaultFocusTime);
        if (_focusTime <= 0)
        {
            _casting.OutOfTIme();
            _focusTime = _defaultFocusTime;
        }
    }

    public void ResetFocus()
    {
        _focusTime = _defaultFocusTime;
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
            _casting.EnableInputs(true);
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
