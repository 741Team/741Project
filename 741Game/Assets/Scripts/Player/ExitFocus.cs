using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFocus : MonoBehaviour
{
    public KeyCode _inputKey;
    FocusMode _focusMode;
    EnterFocus _enterFocus;
    // Start is called before the first frame update
    void Start()
    {
        _focusMode = GetComponent<FocusMode>();
        _enterFocus = GetComponent<EnterFocus>();
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_focusMode != null)
        {
            if (Input.GetKeyDown(_inputKey))
            {
                _focusMode.enabled = false;
                _enterFocus.enabled = true;
                enabled = false;
            }
        }
    }
}
