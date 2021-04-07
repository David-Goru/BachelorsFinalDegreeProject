using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("Dialogues that the NPC can say, ordered by importance")] private Dialogue[] dialogues;

    public void StartTalking()
    {
        DialogueUI.Instance.StartDialogue(dialogues[0]);
    }

    private void OnMouseDown()
    {
        StartTalking();
    }
}