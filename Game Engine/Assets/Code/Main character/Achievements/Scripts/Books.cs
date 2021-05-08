using UnityEngine;

public class Books : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Achievement achievement;

    private void Start()
    {
        achievement.AssignBooks(gameObject);
        achievement.UpdateBooksState();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerInteraction") && Input.GetButton("R")) UI.Instance.ReadAchievement(achievement.name);
    }
}