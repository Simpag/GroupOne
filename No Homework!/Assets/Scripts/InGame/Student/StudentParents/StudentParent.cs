using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StudentStats))]
public class StudentParent : MonoBehaviour {

    /*Quick fix*/
    [SerializeField]
    private GameObject fireEffect;

    private GameObject fireEffectInstance;
    private Vector3 farAway = new Vector3(10000f, 10000f, 10000f);
    private Transform followTransform;

    private void Start()
    {
        fireEffectInstance = Instantiate(fireEffect, farAway, Quaternion.identity, transform);
    }
    /*Quick Fix*/

    protected StudentStats stat;
    protected float fireCountdown = 0;
    protected bool firing = false;
    protected float stopTimer = 0f;

    protected virtual void Awake()
    {
        stat = GetComponent<StudentStats>();
    }

    protected virtual void Update()
    {
        stopTimer -= Time.deltaTime;
        if (followTransform != null)
        {
            fireEffectInstance.transform.position = followTransform.position;
            fireEffectInstance.transform.rotation = followTransform.rotation;
        }
        else
        {
            fireEffectInstance.transform.position = farAway;
        }

        if (stat.target == null || !stat.IsActive)
        {
            if (stopTimer <= 0 && firing)
            {
                stat.UpdateState(StudentStats.State.idle);
                firing = false;
            }
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

        stopTimer = 0.1f;
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
        if (System.Math.Abs(stat.CurrentStat.rotationSpeed) < Mathf.Epsilon)
            return;

        Vector3 dir = stat.target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(stat.PivotPoint.rotation, lookRotation, Time.deltaTime * stat.CurrentStat.rotationSpeed).eulerAngles;
        stat.PivotPoint.rotation = Quaternion.Euler(stat.PivotPoint.rotation.x, rotation.y, stat.PivotPoint.rotation.z);
    }

    public virtual void MaxUpgrade(int _row)
    {
        return;
    }

    public virtual void FireFollow(Transform _follow)
    {
        followTransform = _follow;
    }
}
