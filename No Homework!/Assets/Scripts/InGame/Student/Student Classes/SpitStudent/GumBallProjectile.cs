using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumBallProjectile : ProjectileParent {

    [Header("Make the collider a trigger")]
    [SerializeField]
    private GameObject flyingMesh;
    [SerializeField]
    private GameObject gumballGroundMesh;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool isOnGround = false;


    protected override void Update()
    {
        if (isOnGround)
            return;

        base.Update();
    }

    protected override void HitTarget()
    {
        //Damage teachers in aoe
        DamageAOE();

        //Spawn gumball on the ground
        flyingMesh.SetActive(false);
        Vector3 _spawnPoint = transform.position;
        GameObject _ground = Instantiate(gumballGroundMesh, this.transform, true);
        _spawnPoint.y = _ground.transform.position.y;
        _ground.transform.position = _spawnPoint;
        isOnGround = true;
        Destroy(this, duration);
    }

    protected override void DamageAOE()
    {
        GameObject _effect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effect, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

        Collider[] _collided = Physics.OverlapSphere(transform.position, currentStat.AOERadius, TeacherLayer);

        foreach (Collider _enemy in _collided)
        {
            TeacherStats _tStat = _enemy.GetComponent<TeacherStats>();
            DamageEnemy(_tStat);
            SlowTeacher(_tStat);
        }
    }

    private void SlowTeacher(TeacherStats _teacher)
    {
        _teacher.SlowTeacher(currentStat.slowAmount / 100f, currentStat.slowDuration);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (isOnGround)
        {
            if (other.CompareTag(GameConstants.TEACHER_TAG))
                SlowTeacher(other.GetComponent<TeacherStats>());
        }
        else
        {
            base.OnTriggerEnter(other);
        }
    }
}
