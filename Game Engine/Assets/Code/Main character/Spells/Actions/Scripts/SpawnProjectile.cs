using UnityEngine;

[CreateAssetMenu(fileName = "SpawnProjectile", menuName = "Spells/Actions/SpawnProjectile", order = 0)]
public class SpawnProjectile : IAction
{
    [SerializeField] private GameObject projectile = null;

    public override void DoAction()
    {
        Debug.Log("Spawning " + projectile.name);
    }
}