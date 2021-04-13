using UnityEngine;

[System.Serializable]
public class Waypoint
{
    [Header("Attributes")]
    [Tooltip("Minimum time that the NPC should stay in this point")] public float MinTime = 0.0f;
    [Tooltip("Maximum time that the NPC should stay in this point")] public float MaxTime = 0.0f;

    [Header("References")]
    public Transform PointInWorld = null;
}