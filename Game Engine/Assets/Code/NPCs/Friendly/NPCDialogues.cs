using UnityEngine;

public class NPCDialogues : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Dialogues that the NPC can say, ordered by importance")] private Dialogue[] dialogues;
    [SerializeField] [Tooltip("Events that will occur when the dialogue selected is finished")] private IEvent[] events;
    [SerializeField] [Tooltip("Dialogue that will trigger the events")] private int dialogueWithEvents = 0;

    [Header("References")]
    [SerializeField] private NPC npc = null;
    [SerializeField] private NPCQuests npcQuests = null;

    [Header("Debug")]
    [SerializeField] private Transform listener = null;
    [SerializeField] private int dialogueSelected = 0;

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
        if (dialogueSelected == dialogueWithEvents) RunEvents();

        listener = null;
        npc.ResumeState();
    }

    public void EnableDialogue(int dialogueId)
    {
        dialogues[dialogueId].Available = true;
    }

    public void RunEvents()
    {
        foreach (IEvent ev in events)
        {
            ev.Run();
        }
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
                dialogueSelected = i;
                return dialogues[i];
            }
        }

        return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerInteraction") && Input.GetButton("R") && listener == null)
        {
            listener = GameObject.FindGameObjectWithTag("Player").transform;

            if (Vector3.Distance(transform.position, listener.position) > 3.5f) return;

            StartTalking();
        }
    }
}