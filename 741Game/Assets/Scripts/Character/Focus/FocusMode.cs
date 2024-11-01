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
    private Vector2 _gridDirection;

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
        PlayerDirectionToGridDirection();
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
            _casting.SetDirection(_gridDirection);
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


    void PlayerDirectionToGridDirection()
    {
        int[] possibleDirections = { 0, 90, 180, 270 };
        int closestDirection = 0;
        int angleDifference = 180;
        float playerAngle = transform.eulerAngles.y;
        foreach (int direction in possibleDirections)
        {
            int newDifference = Mathf.Abs(direction - (int)playerAngle);
            if (newDifference < angleDifference)
            {
                angleDifference = newDifference;
                closestDirection = direction;
            }
        }
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, closestDirection, transform.eulerAngles.z);
        transform.eulerAngles = newRotation;
        Vector2 gridDirection = Vector2.zero;
        switch (closestDirection)
        {
            case 0:
                gridDirection = Vector2.down;
                break;
            case 90:
                gridDirection = Vector2.right;
                break;
            case 180:
                gridDirection = Vector2.up;
                break;
            case 270:
                gridDirection = Vector2.left;
                break;
        }
        _gridDirection = gridDirection;
    }


}
