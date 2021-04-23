using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDialogue : IEvent
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("NPC that will enable the dialogue")] private NPCDialogues npc;
    [SerializeField] [Tooltip("Dialogue ID to enable")] private int dialogueId = 0;

    public override void Run()
    {
        npc.EnableDialogue(dialogueId);
    }
}