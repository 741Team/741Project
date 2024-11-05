using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    public Animator animator;

    private Queue<string> scentences;

    private PlayerController player;

    private void Start()
    {
        player = ItemManager.singleton.Player;
        scentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        player.Freeze();
        nameText.text = dialogue.name;
        animator.SetBool("IsOpen", true);
        scentences.Clear();

        foreach(string scentence in dialogue.scentences)
        {
            scentences.Enqueue(scentence);
        }

        DisplayNextScentence();
    }

    public void DisplayNextScentence()
    {
        if(scentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string scentence = scentences.Dequeue();
        dialogueText.text = scentence;
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        player.Unfreeze();
    }

}