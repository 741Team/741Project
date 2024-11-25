using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTeleporter : MonoBehaviour
{
    [SerializeField] private string level;

    private bool teleporting;

    private void Start()
    {
        teleporting = false;
    }

    private void OnTriggerEnter (Collider collider)
    {
        if (collider.tag == "Player" & teleporting == false)
        {
            StartCoroutine(Teleport());
            teleporting = true;
        }
    }

    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level);
    }
}