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

    public virtual void Died(bool _killed, StudentStats _killedBy)
    {
        if (_killed)
        {
            if (GameManager.IsMultiplayer)
            {
                if (_killedBy.isYours)
                {
                    PlayerStats.AddCandyCurrency(stats.Worth * 0.75f);
                }
                else
                {
                    PlayerStats.AddCandyCurrency(stats.Worth * 0.25f);
                }
            }
            else
            {
                PlayerStats.AddCandyCurrency(stats.Worth);
            }

            AudioManager.Instance.Play("EnemyKnockSound");

            GameObject _effect = Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(_effect, deathEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

            //Debug.Log("Normal Death");
        }

        WaveSpawner.Instance.TeachersOnScreen--;
        Destroy(stats.gameObject);
    }
}
