using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParent : MonoBehaviour {

	[Header("General Info")]
	[SerializeField]
    protected LayerMask TeacherLayer;
    [SerializeField]
    protected GameObject impactEffect;

	[Header("In-game Info")]
	[SerializeField]
    protected Transform target;

    protected TeacherStats teacherTarget;
    protected StudentStats.StudentStat currentStat;

    public virtual void Setup(StudentStats.StudentStat _stat)
    {
        currentStat = _stat;
    }

    public virtual void Seek (Transform _target)
    {
        target = _target;
        teacherTarget = _target.GetComponent<TeacherStats>();
    }

    protected virtual void Update()
    {
        if (target == null) //Target died
        {            
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = currentStat.bulletSpeed * Time.deltaTime;
        
        if (dir.magnitude <= distanceThisFrame) //If the distance between the bullet and target is less than the distance the bullet will travel
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    protected virtual void HitTarget()
    {
        if (currentStat.AOERadius != 0)
        {
            DamageAOE();
        } else
        {
            DamageEnemy(teacherTarget);
        }

        Destroy(gameObject);
    }

    protected virtual void DamageEnemy(TeacherStats _teacher)
    {
        GameObject _effect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effect, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);
        
        _teacher.TakeDamage(currentStat.damage);
    }

    protected virtual void DamageAOE()
    {
        Debug.Log("Base");
        GameObject _effect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effect, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

        Collider[] _collided = Physics.OverlapSphere(transform.position, currentStat.AOERadius, TeacherLayer);

        foreach (Collider _enemy in _collided)
        {
            DamageEnemy(_enemy.GetComponent<TeacherStats>());
        }
    }
}
