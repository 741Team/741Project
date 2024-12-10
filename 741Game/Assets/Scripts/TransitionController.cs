using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    public static TransitionController Instance;
    private LevelTeleporter teleporter;
    private PlayerController player;
    [SerializeField] private Vector3 position;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NewScene()
    {
        if (position != new Vector3(0f,0f,0f))
        {
            player = FindObjectOfType<PlayerController>();
            player.transform.position = position;
            Debug.Log("Hai");
        }
        teleporter = FindObjectOfType<LevelTeleporter>();
        position = teleporter.position;
    }
}