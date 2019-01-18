using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PrincipalTeacher : TeacherParent {

    [Header("How much money the death spawner can spend")]
    [SerializeField]
    private int spawnBudget = 100;

    public override void Died(bool _killed)
    {
        StartCoroutine(WaveSpawner.Instance.SpawnTeachers(spawnBudget, transform.root));
        base.Died(_killed);
    }
}
