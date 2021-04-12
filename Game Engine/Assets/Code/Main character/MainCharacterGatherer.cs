using UnityEngine;

public class MainCharacterGatherer : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Range the main character has for picking up items from the floor")] private float pickUpRange = 0.0f;

    [Header("References")]
    [SerializeField] private CapsuleCollider itemGatherArea = null;

    private void Start()
    {
        // Get components
        try
        {
            itemGatherArea = transform.GetComponent<CapsuleCollider>();
        }
        catch (UnityException e)
        {
            Debug.Log("MainCharacterGatherer references not found. Disabling script. Error: " + e);
            enabled = false;
        }

        if (pickUpRange == 0.0f) Debug.Log("Main character gatherer pick up range has not been defined.");

        // Set base info
        itemGatherArea.radius = pickUpRange;
    }
}
