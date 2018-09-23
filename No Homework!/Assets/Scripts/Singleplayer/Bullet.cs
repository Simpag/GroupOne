using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed = 70f;

    [SerializeField]
    private GameObject impactEffect;

    public void Seek (Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
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
    }

    private void HitTarget()
    {
        GameObject _effect = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(_effect, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

        Destroy(gameObject);
    }

}
