using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "NPCs/Quest", order = 0)]
public class Quest : ScriptableObject
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("If the quest can be started")] private bool available = false;
    [SerializeField] [Tooltip("Dialogue that shows at the first time the main character talks with the quest giver")] private Dialogue onStart;
    [SerializeField] [Tooltip("Dialogue that shows when main character talks with the quest completed")] private Dialogue onCompleted;
    [SerializeField] [Tooltip("Dialogue that shows if the quest is started but not completed")] private Dialogue hint;
    [SerializeField] [Tooltip("Conditions to complete the quest")] private IQuestCondition[] conditions;

    [Header("Debug")]
    [SerializeField] private bool started = false;
    [SerializeField] private bool finished = false;

    public bool IsAvailable { get => available && !finished; }

    public Dialogue GetDialogue()
    {
        if (MeetsAllConditions())
        {
            finished = true;
            return onCompleted;
        }

        if (started) return hint;

        // Add to UI?
        started = true;
        return onStart;
    }

    public bool MeetsAllConditions()
    {
        foreach (IQuestCondition condition in conditions)
        {
            if (!condition.MeetsCondition) return false;
        }
        return true;
    }
}