using UnityEngine;

public class NPCQuests : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Quests that the NPC has, ordered by importance")] private Quest[] quests;
    [SerializeField] [Tooltip("Model of the exclamation mark")] private GameObject exclamationMark = null;

    public Dialogue GetQuestDialogue()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i].IsAvailable)
            {
                exclamationMark.SetActive(false);
                return quests[i].GetDialogue();
            }
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
        exclamationMark.SetActive(true);
    }
}