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
    protected StudentStats student;

    public virtual void Setup(StudentStats.StudentStat _stat, StudentStats _stats)
    {
        currentStat = _stat;
        student = _stats;
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

        dir.Set(dir.x, dir.y + 0.5f, dir.z);
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    protected virtual void HitTarget()
    {
        if (System.Math.Abs(currentStat.AOERadius) > Mathf.Epsilon)
        {
            DamageAOE();
        } 
        else
        {
            DamageEnemy(teacherTarget);
        }

        Destroy(gameObject);
    }

    protected virtual void DamageEnemy(TeacherStats _teacher)
    {
        GameObject _effect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effect, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);
        
        _teacher.TakeDamage(currentStat.damage, student);
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG)) 
        {
            HitTarget();
            return;
        }
    }
}
