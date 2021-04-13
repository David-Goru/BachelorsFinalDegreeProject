using UnityEngine;

public class NPCDialogues : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Dialogues that the NPC can say, ordered by importance")] private Dialogue[] dialogues;

    public void StartTalking()
    {
        DialogueUI.Instance.StartDialogue(dialogues[0]);
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 3.5f) return;

        StartTalking();
    }
}