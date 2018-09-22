using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

    public static Transform[] waypoints;

    private void Awake()
    {
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++) //Save all of the waypoints in an array for the enemies to find
        {
            waypoints[i] = transform.GetChild(i);
        }
    }



}
