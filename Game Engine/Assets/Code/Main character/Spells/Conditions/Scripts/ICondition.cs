using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICondition : ScriptableObject
{
    public virtual bool MeetsCondition() { return true; }
}