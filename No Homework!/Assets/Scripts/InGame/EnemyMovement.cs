using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float waypointMargin = 0.4f;

    private Transform target;
    private int waypointIndex;

    private void Start()
    {
        waypointIndex = 0;
        target = Waypoints.waypoints[waypointIndex];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= waypointMargin)
        {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.waypoints.Length - 1) //If there are no more waypoints
        {
            WaveSpawner.KillEnemy(this.gameObject);
            return;
        }

        waypointIndex++;
        target = Waypoints.waypoints[waypointIndex];
    }
}
