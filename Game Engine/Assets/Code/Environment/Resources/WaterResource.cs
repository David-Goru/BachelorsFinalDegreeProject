using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterResource : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("If the resource is infinite, visuals are not needed")] private bool isInfinite = false;
    [SerializeField] [Tooltip("Amount of water this resource has")] private int waterAmount = 0;

    [Header("References")]
    [SerializeField] private GameObject waterVisuals;

    private void Start()
    {
        if (!isInfinite && waterVisuals == null) Debug.Log("Water visuals not set to water source at " + transform.position);
    }

    public bool CheckWater(int amount)
    {
        return isInfinite || amount <= waterAmount;
    }

    public bool ConsumeWater(int amount)
    {
        if (isInfinite) return true;

        if (amount > waterAmount) return false;

        waterAmount -= amount;
        if (waterAmount == 0 && waterVisuals != null) waterVisuals.SetActive(false);
        return true;
    }
}