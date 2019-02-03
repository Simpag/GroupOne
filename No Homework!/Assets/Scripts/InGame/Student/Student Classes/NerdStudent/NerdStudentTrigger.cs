using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerdStudentTrigger : MonoBehaviour
{
    NerdStudent parent;

    private void Start()
    {
        parent = GetComponentInParent<NerdStudent>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(GameConstants.STUDENT_BUFF_AREA))
        {
            other.GetComponentInParent<StudentStats>().BuffStudent(parent.buffAmount / 100f);
        }
    }
}
