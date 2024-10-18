using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnterFocus : MonoBehaviour
{
    public KeyCode _inputKey;
    FocusMode _focusMode;
    ExitFocus _exitFocus;
    Movement _movement;
    // Start is called before the first frame update
    void Start()
    {
        _focusMode = GetComponent<FocusMode>();
        _exitFocus = GetComponent<ExitFocus>();
        _movement = GetComponent<Movement>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (_focusMode != null)
        {
            if (Input.GetKeyDown(_inputKey))
            {
                _focusMode.enabled = true;
                _exitFocus.enabled = true;
                _movement.enabled = false;
                enabled = false;
            }
        }
    }
}