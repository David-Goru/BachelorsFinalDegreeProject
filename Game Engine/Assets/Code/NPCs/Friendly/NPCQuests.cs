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

    public void ActivateQuest(int questNumber)
    {
        if (questNumber >= quests.Length)
        {
            Debug.Log("Wrong quest id");
            return;
        }

        quests[questNumber].Activate();
    }
}