using System.Collections.Generic;
using UnityEngine;

public class EnemiesInfo : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private List<EnemyInfo> enemiesInfo;

    // Getters
    public List<EnemyInfo> EnemiesInfoList { get => enemiesInfo; }

    // Singleton
    public static EnemiesInfo Instance;

    private void Start()
    {
        Instance = this;

        if (!Menu.LoadingGame)
        {
            foreach (EnemyInfo enemyInfo in enemiesInfo)
            {
                enemyInfo.AmountKilled = 0;
            }
        }
    }
}