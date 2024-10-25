using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager singleton;

    public Movement Player;
    public Canvas enemyCanvas;
    public Camera mainCamera;
    public Bar focusBar;

    protected void Awake()
    {
        singleton = this;
    }
}
