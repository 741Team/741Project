using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusMode : MonoBehaviour
{
    [SerializeField]
    Casting _casting;
    [SerializeField]
    GridManager _gridManager;
    PlayerController _playerController;
    private float maxFocus = 0;
    private float decreaseRate = 1;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _gridManager = FindAnyObjectByType<GridManager>();
        _casting = GetComponent<Casting>();
        enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        float _focusTime = _playerController.GetFocus();
        _gridManager.SetLineAlpha(_focusTime /maxFocus);
        _playerController.ChangeFocus(-Time.deltaTime * decreaseRate);
        if (_focusTime <= 0)
        {
            _casting.OutOfTIme();
        }
    }

    private void OnEnable()
    {
        if (_playerController != null)
        {
            maxFocus = _playerController.GetMaxFocus();
            decreaseRate = _playerController.GetFocusDecreaseRate();
        }
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
