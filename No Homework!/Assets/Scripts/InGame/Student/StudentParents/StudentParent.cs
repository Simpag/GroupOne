using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
public class StudentParent : MonoBehaviour {

    protected StudentStats stat;
    protected float fireCountdown = 0;

    protected virtual void Awake()
    {
        stat = GetComponent<StudentStats>();
    }

    protected virtual void Update()
    {
        if (stat.target == null || !stat.IsActive)
            return;

        //Look onto target
        LockOn();

        //Shot target
        Shoot();
    }

    protected virtual void Shoot()
    {
        if (fireCountdown <= 0)
        {
            AudioManager.Instance.Play("TowerThrowSound");

            GameObject _bulletGO = Instantiate(stat.CurrentStat.bulletPrefab, stat.FirePoint.position, stat.FirePoint.rotation);
            ProjectileParent _bullet = _bulletGO.GetComponent<ProjectileParent>();

            if (_bullet != null)
            {
                _bullet.Setup(stat.CurrentStat);
                _bullet.Seek(stat.target);
            }

            fireCountdown = 1 / stat.CurrentStat.firerate;
        }

        fireCountdown -= Time.deltaTime;
    }

    protected virtual void LockOn()
    {
        Vector3 dir = stat.target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(stat.PivotPoint.rotation, lookRotation, Time.deltaTime * stat.CurrentStat.rotationSpeed).eulerAngles;
        stat.PivotPoint.rotation = Quaternion.Euler(stat.PivotPoint.rotation.x, rotation.y, stat.PivotPoint.rotation.z);
    }

}
