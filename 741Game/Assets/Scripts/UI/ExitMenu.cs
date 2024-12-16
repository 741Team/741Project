using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    [SerializeField] private GameObject exitMenu;
    // Start is called before the first frame update
    void Start()
    {
        if(exitMenu != null)
        {
            exitMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitMenu != null)
            {
                exitMenu.SetActive(!exitMenu.activeSelf);
            }
        }
    }

    public void CloseMenu()
    {
        if (exitMenu != null)
        {
            exitMenu.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
