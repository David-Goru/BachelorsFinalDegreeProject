using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("UI object with dialogue panel")] private GameObject dialoguePanel;

    [Header("Debug")]
    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private int currentDialogueLine = 0;

    public static DialogueUI Instance;

    private void Start()
    {
        Instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogueLine = -1;
        currentDialogue = dialogue;
        ContinueTalking();
    }

    public void DisplayLine(string line)
    {
        if (!dialoguePanel.activeSelf) dialoguePanel.SetActive(true);
        dialoguePanel.transform.Find("Line").GetComponent<Text>().text = line;
    }

    public void ContinueTalking()
    {
        currentDialogueLine++;
        if (currentDialogue.Lines.Length == currentDialogueLine) StopTalking();
        else DisplayLine(currentDialogue.Lines[currentDialogueLine]);
    }

    public void StopTalking()
    {
        dialoguePanel.SetActive(false);
    }
}