using UnityEngine;

public class NPCDialogues : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Dialogues that the NPC can say, ordered by importance")] private Dialogue[] dialogues;

    [Header("References")]
    [SerializeField] private NPC npc = null;
    [SerializeField] private NPCQuests npcQuests = null;

    [Header("Debug")]
    [SerializeField] private Transform listener = null;

    private void Start()
    {
        try
        {
            npc = transform.GetComponent<NPC>();
            if (transform.GetComponent<NPCQuests>() != null) npcQuests = transform.GetComponent<NPCQuests>();
        }
        catch (UnityException e)
        {
            Debug.Log("NPCDialogues references not found. Disabling script. Error: " + e);
            enabled = false;
        }
    }

    public void StartTalking()
    {
        Dialogue nextDialogue = getNextDialogue();

        if (nextDialogue != null) DialogueUI.Instance.StartDialogue(nextDialogue, this);
    }

    public void ContinueTalking()
    {
        if (listener != null) transform.LookAt(listener.position);
        npc.SetState(NPCState.TALK);
    }

    public void StopTalking()
    {
        listener = null;
        npc.ResumeState();
    }

    private Dialogue getNextDialogue()
    {
        if (npcQuests != null)
        {
            Dialogue nextDialogue = npcQuests.GetQuestDialogue();

            if (nextDialogue != null) return nextDialogue;
        }

        for (int i = 0; i < dialogues.Length; i++)
        {
            if (dialogues[i].Available)
            {
                if (!dialogues[i].Recurrent) dialogues[i].Available = false;
                return dialogues[i];
            }
        }

        return null;
    }

    private void OnMouseDown()
    {
        listener = GameObject.FindGameObjectWithTag("Player").transform;

        if (Vector3.Distance(transform.position, listener.position) > 3.5f) return;

        StartTalking();
    }
}