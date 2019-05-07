using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
public class StudentParent : MonoBehaviour {

    protected StudentStats stat;
    protected float fireCountdown = 0;
    protected bool firing = false;

    protected virtual void Awake()
    {
        stat = GetComponent<StudentStats>();
    }

    protected virtual void Update()
    {
        if (stat.target == null || !stat.IsActive)
        {
            stat.UpdateState(StudentStats.State.idle);
            firing = false;
            return;
        }

        //Change student state
        if (!firing)
        {
            stat.UpdateState(StudentStats.State.fire);
            firing = true;
        }

        //Look onto target
        LockOn();

        fireCountdown -= Time.deltaTime;
    }

    public virtual void Shoot()
    {
        if (fireCountdown <= 0)
        {
            AudioManager.Instance.Play("TowerThrowSound");

            GameObject _bulletGO = Instantiate(stat.CurrentStat.bulletPrefab, stat.FirePoint.position, stat.FirePoint.rotation);
            ProjectileParent _bullet = _bulletGO.GetComponent<ProjectileParent>();

            if (_bullet != null)
            {
                _bullet.Setup(stat.CurrentStat, stat);
                _bullet.Seek(stat.target);
            }

            fireCountdown = 1 / stat.CurrentStat.firerate;
        }
    }

    protected virtual void LockOn()
    {
        Vector3 dir = stat.target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(stat.PivotPoint.rotation, lookRotation, Time.deltaTime * stat.CurrentStat.rotationSpeed).eulerAngles;
        stat.PivotPoint.rotation = Quaternion.Euler(stat.PivotPoint.rotation.x, rotation.y, stat.PivotPoint.rotation.z);
    }

    public virtual void MaxUpgrade(int _row)
    {
        return;
    }

}
