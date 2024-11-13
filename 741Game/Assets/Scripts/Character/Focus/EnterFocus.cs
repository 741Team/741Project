using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnterFocus : MonoBehaviour
{
    public KeyCode _inputKey;
    FocusMode _focusMode;
    ExitFocus _exitFocus;
    PlayerController playerController;
    CharacterAnimations _characterAnimations;
    // Start is called before the first frame update
    void Start()
    {
        _focusMode = GetComponent<FocusMode>();
        _exitFocus = GetComponent<ExitFocus>();
        playerController = GetComponent<PlayerController>();
        _characterAnimations = GetComponent<CharacterAnimations>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_focusMode != null)
        {
            if (Input.GetKeyDown(_inputKey) && playerController.IsAllowedToMove())
            {
                _characterAnimations.EnterFocus();
                _focusMode.enabled = true;
                _exitFocus.enabled = true;
                playerController.Freeze();
                foreach(EnemyBase enemy in EnemyManager.singleton.GetEnemyList())
                {
                    enemy.Freeze();
                }
                ItemManager.singleton.adjustmentBar.SetActive(true);
                enabled = false;
            }
        }
    }
}
