using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IProjectileBehaviour : MonoBehaviour
{
    public virtual void StartProjectile() { }
    public virtual void NextState() { }
    public virtual void Detonate() { }
    public virtual void Stop() { }
}