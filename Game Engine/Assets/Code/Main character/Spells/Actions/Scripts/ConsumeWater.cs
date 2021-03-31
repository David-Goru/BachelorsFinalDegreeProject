using UnityEngine;

[CreateAssetMenu(fileName = "ConsumeWater", menuName = "Spells/Actions/ConsumeWater", order = 0)]
public class ConsumeWater : IAction
{
    [SerializeField] private int waterAmount = 0;
    [SerializeField] private float areaRange = 0.0f;

    public override void DoAction()
    {
        foreach (GameObject waterResource in GameObject.FindGameObjectsWithTag("WaterResource"))
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, waterResource.transform.position) <= areaRange
                && waterResource.GetComponent<WaterResource>().ConsumeWater(waterAmount)) break;
        }
    }
}