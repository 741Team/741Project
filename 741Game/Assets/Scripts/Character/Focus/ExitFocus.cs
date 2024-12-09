using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFocus : MonoBehaviour
{
    public KeyCode _inputKey;
    FocusMode _focusMode;
    EnterFocus _enterFocus;
    PlayerController _movement;
    CharacterAnimations _characterAnimations;

    // Start is called before the first frame update
    void Start()
    {
        _characterAnimations = GetComponent<CharacterAnimations>();
        _focusMode = GetComponent<FocusMode>();
        _enterFocus = GetComponent<EnterFocus>();
        _movement = GetComponent<PlayerController>();
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
        _movement.Unfreeze();
        _movement.SwitchToMainCam();
        foreach (EnemyBase enemy in EnemyManager.singleton.GetEnemyList())
        {
            enemy.SetOccupant(false);
            enemy.Unfreeze();
        }
        if (ItemManager.singleton.adjustmentBar != null)
        {
            ItemManager.singleton.adjustmentBar.SetActive(false);
        }
        if (ItemManager.singleton.castReady != null)
        {
            ItemManager.singleton.castReady.enabled = false;
        }
        if (ItemManager.singleton.castNotReady != null)
        {
            ItemManager.singleton.castNotReady.enabled = true;
        }
        enabled = false;
    }
}
