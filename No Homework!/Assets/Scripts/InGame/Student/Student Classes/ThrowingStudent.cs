using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
[System.Serializable]
public class ThrowingStudent : StudentParent {

    private StudentStats student;

    private void Awake()
    {
        student = GetComponent<StudentStats>();
    }

}
