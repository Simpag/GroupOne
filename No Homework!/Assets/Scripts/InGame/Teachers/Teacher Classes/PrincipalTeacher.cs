using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PrincipalTeacher : TeacherParent {

    [Header("How much money the death spawner can spend")]
    [SerializeField]
    private float money = 100;

    
}
