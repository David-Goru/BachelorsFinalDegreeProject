using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInput
{
    [SerializeField] private string buttonName;
    [SerializeField] private int animationID;
    [SerializeField] private bool requiresTime;
    [SerializeField] private float timeAmount;
    [SerializeField] private bool spawnProjectile;
    [SerializeField] private bool nextProjectileState;
    [SerializeField] private bool detonateProjectile;
    [SerializeField] private List<ICondition> conditions;
    [SerializeField] private List<IAction> actions;

    public string ButtonName { get => buttonName; set => buttonName = value; }
    public int AnimationID { get => animationID; set => animationID = value; }
    public bool RequiresTime { get => requiresTime; set => requiresTime = value; }
    public float TimeAmount { get => timeAmount; set => timeAmount = value; }
    public bool SpawnProjectile { get => spawnProjectile; set => spawnProjectile = value; }
    public bool NextProjectileState { get => nextProjectileState; set => nextProjectileState = value; }
    public bool DetonateProjectile { get => detonateProjectile; set => detonateProjectile = value; }
    public List<ICondition> Conditions { get => conditions; set => conditions = value; }
    public List<IAction> Actions { get => actions; set => actions = value; }

    public bool MeetsAllConditions()
    {
        foreach (ICondition condition in conditions)
        {
            if (!condition.MeetsCondition()) return false;
        }
        return true;
    }

    public void DoAllActions()
    {
        foreach (IAction action in actions)
        {
            action.DoAction();
        }
    }
}