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
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float animSpeedOffset = 1f;

    private Transform target;
    private int waypointIndex;

    private float distanceTraveled;
    public float DistanceTraveled { get { return distanceTraveled; } }
    private Vector3 lastDir;
    private float newSpeed, lastSpeed, clipLength;

    private void Start()
    {
        stats = GetComponent<TeacherStats>();

        waypointIndex = 0;
        target = Waypoints.waypoints[waypointIndex];
        distanceTraveled = 0f;

        clipLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        newSpeed = (stats.Speed / clipLength) * animSpeedOffset;
        anim.speed = newSpeed;
        lastSpeed = newSpeed;
    }

    private void Update()
    {
        Vector3 _dir = (target.position - transform.position).normalized;
        transform.Translate(_dir * stats.Speed * Time.deltaTime, Space.World);
        distanceTraveled += stats.Speed * Time.deltaTime;

        if (lastDir != _dir)
        {
            lastDir = _dir;

            RotateEnemy(_dir);
        }

        if (Vector3.Distance(transform.position, target.position) <= waypointMargin)
        {
            GetNextWaypoint();
        }

        clipLength = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        newSpeed = (stats.Speed / clipLength) * animSpeedOffset;
        if (Math.Abs(lastSpeed - newSpeed) > Mathf.Epsilon)
        {
            anim.speed = newSpeed;
            lastSpeed = newSpeed;
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
