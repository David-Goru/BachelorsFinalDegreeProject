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

    private void OnMouseDown()
    {
        UI.Instance.ReadAchievement(achievement.name);
    }
}