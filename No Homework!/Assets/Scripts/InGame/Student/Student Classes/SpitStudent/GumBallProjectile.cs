using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GumBallProjectile : ProjectileParent {

    [Header("Make the collider a trigger")]
    [SerializeField]
    private GameObject flyingMesh;
    [SerializeField]
    private GameObject gumballGroundMesh;
    [SerializeField]
    private float duration;

    private bool isOnGround;

    private void Awake()
    {
        GetComponent<BoxCollider>().enabled = false;
        isOnGround = false;
    }

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
        Vector3 _spawnPoint = Vector3.zero;
        _spawnPoint.y -= transform.position.y;
        Instantiate(gumballGroundMesh, _spawnPoint, Quaternion.identity, this.transform);
        GetComponent<BoxCollider>().enabled = true;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConstants.TEACHER_TAG))
        {
            other.GetComponent<TeacherStats>().SlowTeacher(currentStat.slowAmount / 100f, currentStat.slowDuration);
        }
    }
}
