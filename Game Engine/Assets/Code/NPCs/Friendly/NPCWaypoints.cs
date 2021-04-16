using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoints : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("List of waypoints ordered of the NPC route")] private Waypoint[] waypoints;
    [SerializeField] [Tooltip("Speed at which the NPC moves")] private float speed = 0.0f;

    [Header("References")]
    [SerializeField] private NPC npc = null;

    [Header("Debug")]
    [SerializeField] private int currentWaypoint = 0;

    private void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.Log("NPCWaypoints not defined for " + name);
            enabled = false;
            return;
        }

        try
        {
            npc = transform.GetComponent<NPC>();
            npc.SetState(NPCState.WALK);
            StartCoroutine(moveToWaypoint());
        }
        catch (UnityException e)
        {
            Debug.Log("NPCWaypoints references not found. Disabling script. Error: " + e);
            enabled = false;
            return;
        }
    }

    private IEnumerator moveToWaypoint()
    {
        yield return new WaitUntil(() => move());

        if (waypoints[currentWaypoint].MaxTime > 0)
        {
            npc.SetState(NPCState.IDLE);

            float randomTime = Random.Range(waypoints[currentWaypoint].MinTime, waypoints[currentWaypoint].MaxTime);            
            yield return new WaitForSeconds(randomTime);
            yield return new WaitUntil(() => npc.CurrentState == NPCState.IDLE);

            npc.SetState(NPCState.WALK);
        }

        currentWaypoint++;
        if (currentWaypoint == waypoints.Length) currentWaypoint = 0;
        StartCoroutine(moveToWaypoint());
    }

    private bool move()
    {
        if (npc.CurrentState != NPCState.WALK) return false;

        transform.LookAt(waypoints[currentWaypoint].PointInWorld.position);
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].PointInWorld.position, Time.deltaTime * speed);
        return Vector3.Distance(transform.position, waypoints[currentWaypoint].PointInWorld.position) < 0.25f;
    }
}
