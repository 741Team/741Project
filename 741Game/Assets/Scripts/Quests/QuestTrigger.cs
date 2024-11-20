using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    public Quest quest;

    public void Started()
    {
        quest.questStarted = true;
    }

    public void Completed()
    {
        quest.questCompleted = true;
    }

    public void HandedIn()
    {
        quest.questHandedIn = true;
    }
}
