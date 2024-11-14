using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [SerializeField] private QuestTrigger quest;

    private PlayerController player;
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
        if (collider.tag == "Player" & quest.quest.questStarted == true)
        {
            playerInRange = true;
            prompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player" & quest.quest.questStarted == true)
        {
            playerInRange = false;
            prompt.SetActive(false);
        }
    }

    private void ObjectObtained()
    {
        quest.Completed();
    }

    private void Update()
    {
        if (playerInRange & Input.GetKeyDown(KeyCode.E))
        {
            ObjectObtained();
            Destroy(prompt.gameObject);
            Destroy(this.gameObject);
        }
    }
}
