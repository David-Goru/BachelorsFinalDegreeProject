using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "NPCs/Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Lines of the dialogue. One per box")] [TextArea(1, 4)] private string[] lines;
    [SerializeField] [Tooltip("If can be displayed several times")] private bool recurrent = false;
    [SerializeField] [Tooltip("If non recurrent. True for achievements, quests, etc when available")] private bool available = false;

    public string[] Lines { get => lines; }
    public bool Recurrent { get => recurrent; }
    public bool Available { get => available; set => available = value; }
}