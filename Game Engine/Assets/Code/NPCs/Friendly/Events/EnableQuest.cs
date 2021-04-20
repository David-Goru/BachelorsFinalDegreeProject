using UnityEngine;

public class EnableQuest : IEvent
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("NPCQuests component of the NPC that has the quest")] private NPCQuests npcQuests;
    [SerializeField] [Tooltip("Number of the quest in the quests array of the NPC")] private int questId = 0;

    public override void Run()
    {
        npcQuests.ActivateQuest(questId);
    }
}