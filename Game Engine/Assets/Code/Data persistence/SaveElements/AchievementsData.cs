using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AchievementsData : SaveElement
{
    [Header("Debug")]
    [SerializeField] private List<AchievementData> achievements;

    public override SaveElement Save()
    {
        Name = "Achievements";
        achievements = new List<AchievementData>();
        foreach (Achievement achievement in Achievements.Instance.AchievementsList)
        {
            achievements.Add(new AchievementData(achievement.name, achievement.Completed));
        }
        return this;
    }

    public override bool Load()
    {
        foreach (AchievementData achievement in achievements)
        {
            achievement.Load();
        }
        return true;
    }
}