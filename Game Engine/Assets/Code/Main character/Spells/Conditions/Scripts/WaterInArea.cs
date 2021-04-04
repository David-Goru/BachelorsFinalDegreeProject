using UnityEngine;

[CreateAssetMenu(fileName = "WaterInArea", menuName = "Spells/Conditions/WaterInArea", order = 0)]
public class WaterInArea : ICondition
{
    [SerializeField] private int waterAmount = 0;
    [SerializeField] private float areaRange = 0.0f;

    public override bool MeetsCondition()
    {
        foreach (GameObject waterResource in GameObject.FindGameObjectsWithTag("WaterResource"))
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, waterResource.transform.position) <= areaRange 
                && waterResource.GetComponent<WaterResource>().CheckWater(waterAmount)) return true;
        }
        return false;
    }
}