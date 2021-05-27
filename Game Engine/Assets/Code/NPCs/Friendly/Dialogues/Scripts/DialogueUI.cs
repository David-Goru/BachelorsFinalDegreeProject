using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("UI object with dialogue panel")] private GameObject dialoguePanel;

    [Header("References")]
    [SerializeField] private Transform mainCharacterTransform;

    [Header("Debug")]
    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private int currentDialogueLine = 0;
    [SerializeField] private NPCDialogues npcDialogues = null;

    public NPCDialogues NpcDialogues { get => npcDialogues; }

    public static DialogueUI Instance;

    private void Start()
    {
        Instance = this;
        mainCharacterTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enabled = false;
    }

    private void Update()
    {
        if (Vector3.Distance(mainCharacterTransform.position, npcDialogues.transform.position) > 4) StopTalking();
    }

    public void StartDialogue(Dialogue dialogue, NPCDialogues npcDialogues)
    {
        UI.Instance.UnlockMouse();
        mainCharacterTransform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(false);
        currentDialogueLine = -1;
        currentDialogue = dialogue;
        this.npcDialogues = npcDialogues;
        ContinueTalking();
        enabled = true;
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
        else
        {
            DisplayLine(currentDialogue.Lines[currentDialogueLine]);
            npcDialogues.ContinueTalking();
        }
    }

    public void StopTalking()
    {
        mainCharacterTransform.GetComponent<MainCharacter>().CharacterCamera.ChangeState(true);
        dialoguePanel.SetActive(false);
        npcDialogues.StopTalking();
        npcDialogues = null;
        enabled = false;
        UI.Instance.LockMouse();
    }
}