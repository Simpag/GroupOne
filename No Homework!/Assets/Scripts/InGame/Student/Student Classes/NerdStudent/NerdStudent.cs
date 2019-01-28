using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
[System.Serializable]
public class NerdStudent : StudentParent {

    [SerializeField]
    private LayerMask StudentLayer;

    public void BuffTowers()
    {
        Collider[] _collided = Physics.OverlapSphere(transform.position, stat.CurrentStat.AOERadius, StudentLayer);

        foreach (Collider _enemy in _collided)
        {
            DamageEnemy(_enemy.GetComponent<TeacherStats>());
        }
    }

}
