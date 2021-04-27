using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private List<Achievement> achievements;

    // Getters
    public List<Achievement> AchievementsList { get => achievements; }

    // Singleton
    public static Achievements Instance;

    private void Start()
    {
        Instance = this;

        if (!Menu.LoadingGame)
        {
            foreach (Achievement achievement in achievements)
            {
                achievement.SetState(false);
            }
        }
    }
}