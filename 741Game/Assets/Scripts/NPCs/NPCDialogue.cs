using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    private Movement player;
    [SerializeField] private DialogueTrigger dialogue;
    [SerializeField] private GameObject prompt;
    private Canvas canvas;

    private bool playerInRange;

    private void Start()
    {
        player = ItemManager.singleton.Player;
        canvas = ItemManager.singleton.enemyCanvas;
        playerInRange = false;
        prompt.transform.SetParent(canvas.transform);
        prompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            playerInRange = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            playerInRange = false;
            prompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange & Input.GetKeyDown(KeyCode.E))
        {
            dialogue.TriggerDialogue();
        }
    }
}