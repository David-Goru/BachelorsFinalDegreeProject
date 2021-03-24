using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement", order = 0)]
public class Achievement : ScriptableObject
{
    [Header("Attributes")]
    [SerializeField] private GameObject books;

    [Header("Debug")]
    [SerializeField] private bool completed = false;

    // Getters
    public bool Completed { get => completed; }

    public void AssignBooks(GameObject books)
    {
        this.books = books;
    }

    public void SetState(bool state)
    {
        completed = state;

        if (books != null) UpdateBooksState();
    }

    public void UpdateBooksState()
    {
        books.SetActive(completed);
    }
}