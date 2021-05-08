using UnityEngine;

public class NPCAchievementGiver : IEvent
{
    [Header("Attributes")]
    [SerializeField] private Achievement achievement;
    [SerializeField] private IEvent eventOnCompleted;
    [SerializeField] private bool completeOnTalk = false;

    public override void Run()
    {
        if (achievement.Completed) return;

        if (MeetsConditions())
        {
            achievement.UnlockAchievement();
            Debug.Log("Y");
            if (eventOnCompleted != null)
            {
                Debug.Log("Y??");
                eventOnCompleted.Run();
            }
        }
    }

    public virtual bool MeetsConditions()
    {
        return completeOnTalk;
    }
}