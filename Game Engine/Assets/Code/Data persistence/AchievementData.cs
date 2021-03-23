using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AchievementData
{
    [SerializeField] private string name;
    [SerializeField] private bool completed;

    public AchievementData(string name, bool completed)
    {
        this.name = name;
        this.completed = completed;
    }

    public bool Load()
    {
        try
        {
            Achievement achievement = Achievements.Instance.AchievementsList.Find(x => x.name == name);
            if (completed) achievement.SetState(true);
        }
        catch (UnityException e)
        {
            // Add to text file?
            Debug.Log("Achievement (" + name + ") data error: " + e);

            return false;
        }

        return true;
    }
}