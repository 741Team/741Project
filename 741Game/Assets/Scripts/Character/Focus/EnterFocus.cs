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
            if (Input.GetKeyDown(_inputKey) && playerController.IsAllowedToMove() && playerController.CanEnterFocus())
            {
                _characterAnimations.EnterFocus();
                _focusMode.enabled = true;
                _exitFocus.enabled = true;
                playerController.Freeze();
                playerController.SwitchToFocusCam();
                foreach(EnemyBase enemy in EnemyManager.singleton.GetEnemyList())
                {
                    enemy.Freeze();
                }
                if (ItemManager.singleton.adjustmentBar != null)
                {
                    ItemManager.singleton.adjustmentBar.SetActive(true);
                }
                if(ItemManager.singleton.castReady != null)
                {
                    ItemManager.singleton.castReady.enabled = false;
                }
                if (ItemManager.singleton.castNotReady != null)
                {
                    ItemManager.singleton.castNotReady.enabled = false;
                }

                enabled = false;
            }
        }
    }
}
