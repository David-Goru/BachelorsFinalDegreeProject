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

    public void SetState(bool state)
    {
        completed = state;

        if (state)
        {
            if (books == null) Debug.Log("Achievement books have not been defined.");
            //else books.SetActive(true);
        }
    }
}