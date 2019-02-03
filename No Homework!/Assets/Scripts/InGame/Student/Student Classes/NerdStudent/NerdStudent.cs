using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
[System.Serializable]
public class NerdStudent : StudentParent {

    [Header("Needs some work")]

    [Header("Dont forget to add a trigger collider")]
  
    public float buffAmount;

    private bool placed = false;

    protected override void Update()
    {
        if (stat.IsActive && !placed)
        {
            placed = true;
            stat.rangeView.gameObject.AddComponent<NerdStudentTrigger>();
        }

        return;
    }
}
