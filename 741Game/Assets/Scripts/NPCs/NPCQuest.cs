using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuest : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private DialogueTrigger startDialogue;
    [SerializeField] private DialogueTrigger midDialogue;
    [SerializeField] private DialogueTrigger finishDialogue;
    [SerializeField] private DialogueTrigger repeatDialogue;
    [SerializeField] private GameObject prompt;
    private Canvas canvas;
    private DialogueManager dialogue;

    [SerializeField] QuestTrigger quest;

    public bool talking;

    private bool playerInRange;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueManager>();
        player = ItemManager.singleton.Player;
        canvas = ItemManager.singleton.enemyCanvas;
        playerInRange = false;
        prompt.transform.SetParent(canvas.transform);
        prompt.SetActive(false);
        talking = false;
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
            talking = false;
        }
    }

    private void Update()
    {
        if (playerInRange & Input.GetKeyDown(KeyCode.E))
        {
            talking = true;
            if (quest.quest.questHandedIn == true)
            {
                repeatDialogue.TriggerDialogue();
            }
            else if (quest.quest.questCompleted == true)
            {
                finishDialogue.TriggerDialogue();
                quest.HandedIn();
            }
            else if (quest.quest.questStarted == true)
            {
                midDialogue.TriggerDialogue();
            }
            else
            {
                startDialogue.TriggerDialogue();
            }
        }

        if (talking & dialogue.questAccepted == true)
        {
            quest.Started();
        }
    }
}
