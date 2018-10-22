using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[Header("General Info")]
	[SerializeField]
    private LayerMask EnemyLayer;
    [SerializeField]
    private GameObject impactEffect;

	[Header("In-game Info")]
	[SerializeField]
    private Transform target;

    public float speed = 70f;
    public float damage = 50f;
    public float AOE = -1f; //Area of effect

    public void Seek (Transform _target, float _speed, float _damage, float _AOE)
    {
        target = _target;
        speed = _speed;
        damage = _damage;
        AOE = _AOE;
    }

    private void Update()
    {
        if (target == null) //Target died
        {            
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        if (dir.magnitude <= distanceThisFrame) //If the distance between the bullet and target is less than the distance the bullet will travel
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        GameObject _effect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effect, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

        if (AOE > 0)
        {
            DamageAOE();
        } else
        {
            DamageEnemy(target.gameObject);
        }

        Destroy(gameObject);
    }

    private void DamageEnemy (GameObject _enemy)
    {
        _enemy.GetComponent<EnemyStats>().TakeDamage(damage);
    }

    private void DamageAOE ()
    {
        Collider[] _collided = Physics.OverlapSphere(transform.position, AOE, EnemyLayer);

        foreach (Collider _enemy in _collided)
        {
            DamageEnemy(_enemy.gameObject);
        }
    }
}
