using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChemistryTeacher : TeacherParent {

    [Header("Dont forget to add a trigger collider")]
    [Header("Slow amount (%), time in s")]
    [SerializeField]
    private float slowNerf = 10f;
    [SerializeField]
    private float slowTime = 1f;

    protected override void Awake()
    {
        stats = GetComponentInParent<TeacherStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.STUDENT_AREA_TAG))
        {
            StudentStats _student = other.GetComponentInParent<StudentStats>();

            _student.SlowStudent((slowNerf / 100f), slowTime);
        }
    }
}
