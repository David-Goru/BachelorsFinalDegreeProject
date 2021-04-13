using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoints : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] [Tooltip("List of waypoints ordered of the NPC route")] private Waypoint[] waypoints;
    [SerializeField] [Tooltip("Speed at which the NPC moves")] private float speed = 0.0f;

    [Header("Debug")]
    [SerializeField] private int currentWaypoint = 0;

    private void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.Log("NPCWaypoints not defined for " + name);
            enabled = false;
        }
        else
        {
            // Set walk animation

            StartCoroutine(moveToWaypoint());
        }
    }

    private IEnumerator moveToWaypoint()
    {
        transform.LookAt(waypoints[currentWaypoint].PointInWorld.position);
        yield return new WaitUntil(() => move());

        if (waypoints[currentWaypoint].MaxTime > 0)
        {
            // Set idle animation

            float randomTime = Random.Range(waypoints[currentWaypoint].MinTime, waypoints[currentWaypoint].MaxTime);            
            yield return new WaitForSeconds(randomTime);

            // Set walk animation
        }

        currentWaypoint++;
        if (currentWaypoint == waypoints.Length) currentWaypoint = 0;
        StartCoroutine(moveToWaypoint());
    }

    private bool move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].PointInWorld.position, Time.deltaTime * speed);
        return Vector3.Distance(transform.position, waypoints[currentWaypoint].PointInWorld.position) < 0.25f;
    }
}
