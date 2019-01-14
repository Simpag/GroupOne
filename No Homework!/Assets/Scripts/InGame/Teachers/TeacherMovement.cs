using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TeacherStats))]
public class TeacherMovement : MonoBehaviour {

    private TeacherStats stats;

    [SerializeField]
    private float waypointMargin = 0.4f;
    [SerializeField]
    private Transform meshTransform;

    [Header("0:Right 1:Left 2:Up 3:Down")]
    [SerializeField]
    private Vector3[] meshDirections;

    private float speed;
    private Transform target;
    private int waypointIndex;

    private float distanceTraveled;
    public float DistanceTraveled { get { return distanceTraveled; } }
    private Vector3 lastDir;

    private void Start()
    {
        stats = GetComponent<TeacherStats>();

        speed = stats.Speed;
        waypointIndex = 0;
        target = Waypoints.waypoints[waypointIndex];
        distanceTraveled = 0f;
    }

    private void Update()
    {
        Vector3 _dir = (target.position - transform.position).normalized;
        transform.Translate(_dir * speed * Time.deltaTime, Space.World);
        distanceTraveled += speed * Time.deltaTime;

        if (lastDir != _dir)
        {
            lastDir = _dir;

            RotateEnemy(_dir);
        }

        if (Vector3.Distance(transform.position, target.position) <= waypointMargin)
        {
            GetNextWaypoint();
        }
    }

    private void RotateEnemy(Vector3 _dir)
    {
        if (Mathf.Round(_dir.x) == 1) //Right
        {
            meshTransform.rotation = Quaternion.Euler(meshDirections[0]);
        }
        else if (Mathf.Round(_dir.x) == -1) //Left
        {
            meshTransform.rotation = Quaternion.Euler(meshDirections[1]);
        }
        else if (Mathf.Round(_dir.z) == 1) //Up
        {
            meshTransform.rotation = Quaternion.Euler(meshDirections[2]);
        }
        else //Down
        {
            meshTransform.rotation = Quaternion.Euler(meshDirections[3]);
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
        stats.Teacher.Died(_killed);
    }
}
