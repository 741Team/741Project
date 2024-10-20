using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFocus : MonoBehaviour
{
    public KeyCode _inputKey;
    FocusMode _focusMode;
    EnterFocus _enterFocus;
    Movement _movement;
    CharacterAnimations _characterAnimations;

    // Start is called before the first frame update
    void Start()
    {
        _characterAnimations = GetComponent<CharacterAnimations>();
        _focusMode = GetComponent<FocusMode>();
        _enterFocus = GetComponent<EnterFocus>();
        _movement = GetComponent<Movement>();
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_focusMode != null)
        {
            if (Input.GetKeyDown(_inputKey))
            {

                FocusOver();
            }
        }
    }

    public void FocusOver()
    {
        _characterAnimations.ExitFocus();
        _focusMode.enabled = false;
        _enterFocus.enabled = true;
        _movement.enabled = true;
        enabled = false;
    }
}
