using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public PlayerController player;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [SerializeField] private GameObject regular;
    [SerializeField] private GameObject quest;

    public Animator animator;

    private Queue<string> scentences;

    public bool questAccepted;

    private void Start()
    {
        scentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        player.Freeze();
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        scentences.Clear();

        foreach (string scentence in dialogue.scentences)
        {
            scentences.Enqueue(scentence);
        }

        DisplayNextScentence();
    }

    public void DisplayNextScentence()
    {
        if (scentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string scentence = scentences.Dequeue();
        if (scentence == "Quest")
        {
            Quest();
            string scentence2 = scentences.Dequeue();
            dialogueText.text = scentence2;
        }
        else
        {
            Regular();
            dialogueText.text = scentence;
        }
    }

    public void AcceptQuest()
    {
        StartCoroutine(Accept());
    }

    public IEnumerator Accept()
    {
        DisplayNextScentence();
        questAccepted = true;
        yield return new WaitForSeconds(1f);
        questAccepted = false;
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        player.Unfreeze();
    }

    private void Regular()
    {
        regular.SetActive(true);
        quest.SetActive(false);
    }

    private void Quest()
    {
        regular.SetActive(false);
        quest.SetActive(true);
    }
}