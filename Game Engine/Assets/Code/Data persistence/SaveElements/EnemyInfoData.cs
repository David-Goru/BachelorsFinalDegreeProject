using UnityEngine;

[System.Serializable]
public class EnemyInfoData
{
    [SerializeField] private string name;
    [SerializeField] private int amountKilled;

    public EnemyInfoData(string name, int amountKilled)
    {
        this.name = name;
        this.amountKilled = amountKilled;
    }

    public bool Load()
    {
        try
        {
            EnemyInfo enemyInfo = EnemiesInfo.Instance.EnemiesInfoList.Find(x => x.name == name);
            enemyInfo.AmountKilled = amountKilled;
        }
        catch (UnityException e)
        {
            // Add to text file?
            Debug.Log("EnemyInfo (" + name + ") data error: " + e);

            return false;
        }

        return true;
    }
}