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
            if (eventOnCompleted != null) eventOnCompleted.Run();
        }
    }

    public virtual bool MeetsConditions()
    {
        return completeOnTalk;
    }
}