using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
public class StudentFiring : MonoBehaviour {

    private StudentStats stat;
    private float fireCountdown = 0;

    private void Awake()
    {
        stat = GetComponent<StudentStats>();
    }

    private void Update()
    {
        if (stat.target == null || !stat.IsActive)
            return;

        //Look onto target
        LockOn();

        //Shot target
        Shoot();
    }

    private void Shoot()
    {
        if (fireCountdown <= 0)
        {
            AudioManager.Instance.Play("TowerThrowSound");

            GameObject _bulletGO = Instantiate(stat.CurrentStat.bulletPrefab, stat.FirePoint.position, stat.FirePoint.rotation);
            ProjectileParent _bullet = _bulletGO.GetComponent<ProjectileParent>();

            if (_bullet != null)
            {
                _bullet.Seek(stat.target);
            }

            fireCountdown = 1 / stat.CurrentStat.fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void LockOn()
    {
        Vector3 dir = stat.target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(stat.PivotPoint.rotation, lookRotation, Time.deltaTime * stat.CurrentStat.rotationSpeed).eulerAngles;
        stat.PivotPoint.rotation = Quaternion.Euler(stat.PivotPoint.rotation.x, rotation.y, stat.PivotPoint.rotation.z);
    }
}
