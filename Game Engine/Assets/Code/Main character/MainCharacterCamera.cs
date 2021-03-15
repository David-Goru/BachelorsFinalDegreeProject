using UnityEngine;
using Cinemachine;

public class MainCharacterCamera : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Speed for camera rotation")] [Range(0, 500)] private float rotationSpeed = 0f;    
    [SerializeField] [Tooltip("Speed for camera zoom")] [Range(0, 25)] private float zoomSpeed = 0f;

    [Header("References")]
    [SerializeField] private CinemachineFreeLook characterCameraComponent = null;

    private void Awake()
    {
        try
        {
            characterCameraComponent = GameObject.FindGameObjectWithTag("FreeLookCamera").GetComponent<CinemachineFreeLook>();
        }
        catch (UnityException e)
        {
            Debug.Log("MainCharacterCamera references not found. Disabling script. Error: " + e);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        characterCameraComponent.m_XAxis.m_MaxSpeed = rotationSpeed;
    }

    private void OnDisable()
    {
        characterCameraComponent.m_XAxis.m_MaxSpeed = 0;
    }

    private void Update()
    {
        // Zoom
        if (Input.mouseScrollDelta.y != 0) characterCameraComponent.m_YAxis.m_MaxSpeed = zoomSpeed;
    }
}