using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
[System.Serializable]
public class NerdStudent : StudentParent {

    [SerializeField]
    private LayerMask StudentLayer;
    [SerializeField]
    private float buffAmount;

    public void BuffTowers()
    {
        Collider[] _collided = Physics.OverlapSphere(transform.position, stat.CurrentStat.AOERadius, StudentLayer);

        foreach (Collider _student in _collided)
        {
            _student.GetComponent<StudentStats>().BuffStudent(buffAmount);
        }
    }
}
