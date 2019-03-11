using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherParent : MonoBehaviour {

    protected TeacherStats stats;

    [SerializeField]
    protected GameObject deathEffect;

    protected virtual void Awake()
    {
        stats = GetComponent<TeacherStats>();
    }

    public virtual void Died(bool _killed)
    {
        if (_killed)
        {
            AudioManager.Instance.Play("EnemyKnockSound");
            PlayerStats.AddCandyCurrency(stats.Worth);

            GameObject _effect = Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(_effect, deathEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

            Debug.Log("Normal Death");
        }

        WaveSpawner.Instance.TeachersOnScreen--;
        Destroy(stats.gameObject);
    }
}
