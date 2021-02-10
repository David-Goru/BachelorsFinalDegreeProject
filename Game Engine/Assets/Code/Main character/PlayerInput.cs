using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInput
{
    [SerializeField] private string buttonName;
    [SerializeField] private string animationName;
    [SerializeField] private bool requiresTime;
    [SerializeField] private float timeAmount;
    [SerializeField] private List<ICondition> conditions;
    [SerializeField] private List<IAction> actions;

    public string ButtonName { get => buttonName; set => buttonName = value; }
    public string AnimationName { get => animationName; set => animationName = value; }
    public bool RequiresTime { get => requiresTime; set => requiresTime = value; }
    public float TimeAmount { get => timeAmount; set => timeAmount = value; }
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