using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuests : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Quests that the NPC has, ordered by importance")] private Quest[] quests;

    public Dialogue GetQuestDialogue()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i].IsAvailable) return quests[i].GetDialogue();
        }
        return null;
    }
}