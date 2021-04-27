using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemiesInfoData : SaveElement
{
    [Header("Debug")]
    [SerializeField] private List<EnemyInfoData> enemiesInfoData;

    public override SaveElement Save()
    {
        Name = "EnemiesInfo";
        enemiesInfoData = new List<EnemyInfoData>();
        foreach (EnemyInfo enemyInfo in EnemiesInfo.Instance.EnemiesInfoList)
        {
            enemiesInfoData.Add(new EnemyInfoData(enemyInfo.name, enemyInfo.AmountKilled));
        }
        return this;
    }

    public override bool Load()
    {
        foreach (EnemyInfoData enemyInfoData in enemiesInfoData)
        {
            enemyInfoData.Load();
        }
        return true;
    }
}