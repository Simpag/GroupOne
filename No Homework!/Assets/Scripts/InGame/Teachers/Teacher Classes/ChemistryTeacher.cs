using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChemistryTeacher : TeacherParent {

    [Header("Dont forget to add a trigger collider")]
    [Header("Buff amount (%)")]
    [SerializeField]
    private float slowNerf = 100f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            Tower _tower = other.GetComponent<Tower>();

            _tower.Slow(slowNerf / 100f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            Tower _tower = other.GetComponent<Tower>();

            _tower.RemoveSlow();
        }
    }
}
