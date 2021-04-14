using UnityEngine;

public class NPCDialogues : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Dialogues that the NPC can say, ordered by importance")] private Dialogue[] dialogues;

    [Header("References")]
    [SerializeField] private NPC npc = null;

    private void Start()
    {
        try
        {
            npc = transform.GetComponent<NPC>();
        }
        catch (UnityException e)
        {
            Debug.Log("NPCDialogues references not found. Disabling script. Error: " + e);
            enabled = false;
        }
    }

    public void StartTalking()
    {
        // Get dialogue still WIP
        DialogueUI.Instance.StartDialogue(dialogues[0], this);
        npc.SetState(NPCState.TALK);
    }

    public void StopTalking()
    {
        npc.ResumeState();
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 3.5f) return;

        StartTalking();
    }
}