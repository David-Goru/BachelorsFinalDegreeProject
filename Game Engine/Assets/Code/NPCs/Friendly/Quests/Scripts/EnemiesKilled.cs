using UnityEngine;

[System.Serializable]
public class EnemiesKilled : IQuestCondition
{
    [Header("Attributes")]
    [SerializeField] private EnemySpawn spawner;

    public override bool MeetsCondition { get => !spawner.EnemiesOnSpawn; }
}