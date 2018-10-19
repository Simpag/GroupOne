using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyMovement : MonoBehaviour {

    private EnemyStats stats;

    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float waypointMargin = 0.4f;

    private Transform target;
    private int waypointIndex;

    private void Start()
    {
        stats = GetComponent<EnemyStats>();

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
        if (waypointIndex >= Waypoints.waypoints.Length - 1) //If there are no more waypoints, enemy reached the end
        {
            ReachedEnd();
            return;
        }

        waypointIndex++;
        target = Waypoints.waypoints[waypointIndex];
    }

    private void ReachedEnd()
    {
        PlayerStats.AddHomework(stats.Homework);
        Died(false);
    }

    public void Died(bool _killed)
    {
        if (_killed)
            PlayerStats.AddCandyCurrency(stats.Worth);

        foreach (Tower _tower in stats.SeenByTower)
        {
            if (_tower != null)
                _tower.rangeView.GetComponent<TowerRange>().RemoveEnemyFromEnemiesInRange(this.gameObject);
        }

        Destroy(this.gameObject);
    }
}
