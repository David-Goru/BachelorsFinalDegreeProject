using System;
using UnityEngine;

public class NPCAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NPC npc = null;
    [SerializeField] private Animator animator = null;

    private void Awake()
    {
        try
        {
            npc = transform.GetComponent<NPC>();
            animator = transform.Find("Model").GetComponent<Animator>();
        }
        catch (UnityException e)
        {
            Debug.Log("NPCAnimations references not found. Disabling script. Error: " + e);
            enabled = false;
        }
    }

    public void UpdateAnimation()
    {
        ResetAllTriggers();
        animator.SetInteger("Random", UnityEngine.Random.Range(0, 101));
        animator.SetTrigger(npc.CurrentState.ToString());
    }

    public void ResetAllTriggers()
    {
        foreach (NPCState state in (NPCState[])Enum.GetValues(typeof(NPCState)))
        {
            animator.ResetTrigger(state.ToString());
        }
    }
}