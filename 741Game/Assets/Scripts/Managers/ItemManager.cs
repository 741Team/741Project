using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager singleton;

    public PlayerController Player;
    public Canvas enemyCanvas;
    public Camera mainCamera;
    public Bar focusBar;
    public Bar healthBar;
    public GameObject adjustmentBar;
    public Image castReady;
    public Image castNotReady;

    protected void Awake()
    {
        singleton = this;
        if (adjustmentBar != null)
        {
            adjustmentBar.SetActive(false);
        }
        if(castReady != null)
        {
            castReady.enabled = false;
        }
    }
}
