using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectiveToNPC : IEvent
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("NPC that will attack to the objective")] private NPCAttacks npc;
    [SerializeField] [Tooltip("Objective to set")] private GameObject objective;

    public override void Run()
    {
        npc.UpdateObjective(objective);
    }
}