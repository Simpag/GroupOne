using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationShoot : MonoBehaviour
{
    [SerializeField]
    private StudentParent student;

    public void Shoot()
    {
        student.Shoot();
    }
}
