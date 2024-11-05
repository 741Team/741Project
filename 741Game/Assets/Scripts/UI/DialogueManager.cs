using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Movement player;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Animator animator;

    private Queue<string> scentences;

    private void Start()
    {
        scentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        player.FreezePlayer();
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
        dialogueText.text = scentence;
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        player.UnfreezePlayer();
    }
}