using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesKilledCounter : IQuestCondition
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Number of enemies that needs to be killed")] private int numberOfEnemies = 0;
    [SerializeField] [Tooltip("Type of enemy")] private EnemyInfo enemyInfo;

    public override bool MeetsCondition => enemyInfo.AmountKilled >= numberOfEnemies;
}