using UnityEngine;

[CreateAssetMenu(fileName = "FloraInArea", menuName = "Spells/Conditions/FloraInArea", order = 0)]
public class FloraInArea : ICondition
{
    [SerializeField] private float areaRange = 0.0f;

    public override bool MeetsCondition()
    {
        return Physics.OverlapSphere(GameObject.FindGameObjectWithTag("Player").transform.position, areaRange, 1 << LayerMask.NameToLayer("Flora")).Length > 0;        
    }
}