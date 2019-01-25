using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBallProjectile : ProjectileParent {

	protected override void DamageAOE()
    {
        GameObject _effect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effect, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

        Collider[] _collided = Physics.OverlapSphere(transform.position, AOE, EnemyLayer);

        foreach (Collider _enemy in _collided)
        {
            DamageEnemy(_enemy.GetComponent<TeacherStats>());
        }
    }

    private void SlowEnemy(TeacherStats _teacher)
    {

    }

}
