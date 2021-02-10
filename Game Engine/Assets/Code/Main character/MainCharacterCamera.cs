using UnityEngine;
using Cinemachine;

public class MainCharacterCamera : MonoBehaviour
{
    [Header("Current values")]
    [Range(0, 500)]
    [SerializeField] private float rotationSpeed = 0f;
    [Range(0, 25)]
    [SerializeField] private float zoomSpeed = 0f;

    [Header("References")]
    [SerializeField] private CinemachineFreeLook characterCameraComponent;

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