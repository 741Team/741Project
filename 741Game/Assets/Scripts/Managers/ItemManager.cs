using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager singleton;

    public PlayerController Player;
    public Canvas enemyCanvas;
    public Camera mainCamera;
    public Bar focusBar;
    public Bar healthBar;

    protected void Awake()
    {
        singleton = this;
    }
}
